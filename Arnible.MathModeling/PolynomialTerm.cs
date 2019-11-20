using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Arnible.MathModeling
{
  public struct PolynomialTerm : IEquatable<PolynomialTerm>, IPolynomialOperation
  {
    private readonly double _coefficient;
    private readonly IEnumerable<KeyValuePair<char, uint>> _indeterminates;
    private readonly string _indeterminatesSignature;

    private PolynomialTerm(double coefficient, params (char, uint)[] indeterminates)
      : this(coefficient, Normalize(indeterminates.ToDictionary(i => i.Item1, i => i.Item2)))
    {
      // intentionally empty
    }

    private PolynomialTerm(double coefficient, IEnumerable<KeyValuePair<char, uint>> indeterminates)
      : this(coefficient, indeterminates, GetIntermediateSignature(indeterminates))
    {
      // intentionally empty
    }

    private PolynomialTerm(double coefficient, IEnumerable<KeyValuePair<char, uint>> indeterminates, string signature)
    {
      if (coefficient == 0)
      {
        if (indeterminates.Any())
        {
          throw new ArgumentException(nameof(indeterminates));
        }
      }

      _coefficient = coefficient;
      _indeterminates = indeterminates;
      _indeterminatesSignature = signature;
    }

    public static implicit operator PolynomialTerm(char name) => new PolynomialTerm(1, (name, 1));
    public static implicit operator PolynomialTerm(double value) => new PolynomialTerm(value);
    public static implicit operator PolynomialTerm((char, uint) indeterminate) => new PolynomialTerm(1, indeterminate);

    private IEnumerable<KeyValuePair<char, uint>> Indeterminates => _indeterminates ?? Enumerable.Empty<KeyValuePair<char, uint>>();

    private string IndeterminatesSignature => _indeterminatesSignature ?? String.Empty;

    private static IEnumerable<KeyValuePair<char, uint>> Normalize(IEnumerable<KeyValuePair<char, uint>> source)
    {
      return source.Where(kv => kv.Value > 0).OrderBy(kv => kv.Key).ToArray();
    }


    private static string GetIntermediateSignature(IEnumerable<KeyValuePair<char, uint>> indeterminates)
    {
      var builder = new StringBuilder();
      foreach (var kv in indeterminates.OrderBy(i => i.Key).ThenBy(i => i.Value))
      {
        if (kv.Value == 0)
        {
          throw new InvalidOperationException("Power cannot be zero");
        }

        if (builder.Length > 0)
        {
          builder.Append("*");
        }
        builder.Append(kv.Key);
        if (kv.Value > 1)
        {
          builder.Append("^");
          builder.Append(kv.Value);
        }
      }
      return builder.ToString();
    }

    public override string ToString()
    {
      if (IsZero)
        return "0";
      if (IsConstant)
        return _coefficient.ToString();
      if (_coefficient == 1)
        return IndeterminatesSignature;
      if (_coefficient == -1)
        return $"-{IndeterminatesSignature}";

      return $"{_coefficient.ToString(CultureInfo.InvariantCulture)}*{IndeterminatesSignature}";
    }

    public bool Equals(PolynomialTerm other)
    {
      return IndeterminatesSignature == other.IndeterminatesSignature && NumericOperator.Equals(_coefficient, other._coefficient);
    }

    public override int GetHashCode()
    {
      return _coefficient.GetHashCode() * IndeterminatesSignature.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is PolynomialTerm v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public static bool operator ==(PolynomialTerm a, PolynomialTerm b) => a.Equals(b);
    public static bool operator !=(PolynomialTerm a, PolynomialTerm b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsZero => _coefficient == 0;
    public bool IsConstant => IndeterminatesSignature.Length == 0;
    public bool HasPositiveCoefficient => _coefficient > 0;

    /*
     * Operators
     */

    public static explicit operator double(PolynomialTerm v)
    {
      if (v.IsConstant)
        return v._coefficient;
      else
        throw new InvalidOperationException("Polynomial term is not constant");
    }

    public static explicit operator char(PolynomialTerm v)
    {
      if (v._coefficient == 1 && v.Indeterminates.Count() == 1)
      {
        var kv = v.Indeterminates.Single();
        if (kv.Value == 1)
        {
          return kv.Key;
        }
      }

      throw new InvalidOperationException("Polynomial term is not a single variable");
    }

    public static Polynomial operator +(PolynomialTerm a, PolynomialTerm b)
    {
      return new Polynomial(a, b);
    }

    public static Polynomial operator -(PolynomialTerm a, PolynomialTerm b)
    {
      return new Polynomial(a, -1 * b);
    }

    private static IEnumerable<KeyValuePair<char, uint>> Multiply(IEnumerable<KeyValuePair<char, uint>> i1, IEnumerable<KeyValuePair<char, uint>> i2)
    {
      var ei1 = i1.GetEnumerator();
      var ei2 = i2.GetEnumerator();

      bool isEi1Valid = ei1.MoveNext();
      bool isEi2Valid = ei2.MoveNext();
      while (isEi1Valid && isEi2Valid)
      {
        var vi1 = ei1.Current;
        var vi2 = ei2.Current;

        if (vi1.Key < vi2.Key)
        {
          yield return vi1;
          isEi1Valid = ei1.MoveNext();
        }
        else if (vi2.Key < vi1.Key)
        {
          yield return vi2;
          isEi2Valid = ei2.MoveNext();
        }
        else
        {
          yield return new KeyValuePair<char, uint>(vi1.Key, vi1.Value + vi2.Value);
          isEi1Valid = ei1.MoveNext();
          isEi2Valid = ei2.MoveNext();
        }
      }

      while (isEi1Valid)
      {
        yield return ei1.Current;
        isEi1Valid = ei1.MoveNext();
      }

      while (isEi2Valid)
      {
        yield return ei2.Current;
        isEi2Valid = ei2.MoveNext();
      }
    }

    public static PolynomialTerm operator *(PolynomialTerm a, PolynomialTerm b)
    {
      double coefficient = a._coefficient * b._coefficient;
      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        return new PolynomialTerm(coefficient, Multiply(a.Indeterminates, b.Indeterminates));
      }
    }

    public PolynomialTerm Power(uint power)
    {
      switch (power)
      {
        case 0:
          return 1;
        case 1:
          return this;
        default:
          return new PolynomialTerm(
            coefficient: NumericOperator.Power(_coefficient, power),
            indeterminates: Indeterminates.Select(kv => new KeyValuePair<char, uint>(kv.Key, kv.Value * power)));
      }
    }

    public static PolynomialTerm operator /(PolynomialTerm a, double denominator)
    {
      if (!denominator.IsValidNumeric())
      {
        throw new ArgumentOutOfRangeException($"Denominator is not valid: {denominator}");
      }
      if (denominator == 0)
      {
        throw new DivideByZeroException();
      }
      return new PolynomialTerm(a._coefficient / denominator, a.Indeterminates, a.IndeterminatesSignature);
    }

    public static PolynomialDivision operator /(PolynomialTerm a, PolynomialTerm b)
    {
      return new PolynomialDivision(a, b);
    }

    public bool TryDivide(PolynomialTerm b, out PolynomialTerm result)
    {
      if (b.IsZero)
      {
        // division by zero
        result = default;
        return false;
      }
      if (b.IsConstant)
      {
        result = this / (double)b;
        return true;
      }

      var bIndeterminates = b.Indeterminates.ToDictionary(kv => kv.Key, kv => kv.Value);
      var resultIndeterminates = new List<KeyValuePair<char, uint>>();
      foreach (var kv in Indeterminates)
      {
        if (bIndeterminates.TryGetValue(kv.Key, out uint bPower))
        {
          if (kv.Value < bPower)
          {
            // result is not polynomial
            result = default;
            return false;
          }
          {
            if (kv.Value > bPower)
            {
              resultIndeterminates.Add(new KeyValuePair<char, uint>(kv.Key, kv.Value - bPower));
            }
            bIndeterminates.Remove(kv.Key);
          }          
        }
        else
        {
          resultIndeterminates.Add(kv);
        }
      }

      if (bIndeterminates.Any())
      {
        // some indeterminates from b hasn't been removed
        // result is not polynomial
        result = default;
        return false;
      }

      result = new PolynomialTerm(_coefficient / b._coefficient, resultIndeterminates);
      return true;
    }

    public PolynomialTerm DerivativeBy(char name)
    {
      uint power = Indeterminates.Where(kv => kv.Key == name).Select(kv => kv.Value).SingleOrDefault();
      if (power > 0)
      {
        var indeterminates = Indeterminates.Where(kv => kv.Key != name).Append(new KeyValuePair<char, uint>(name, power - 1));
        return new PolynomialTerm(_coefficient * power, Normalize(indeterminates));
      }
      else
      {
        return 0;
      }
    }

    /*
     * IEnumerable operators
     */

    public static IEnumerable<PolynomialTerm> Simplify(IEnumerable<PolynomialTerm> variables)
    {
      return variables.GroupBy(v => v.IndeterminatesSignature).Select(g => Add(g)).Where(v => !v.IsZero).OrderByDescending(v => v.IndeterminatesSignature);
    }

    public static bool IsSimplified(IEnumerable<PolynomialTerm> variables)
    {
      return variables.Select(v => v.IndeterminatesSignature).Distinct().Count() == variables.Count();
    }

    private static PolynomialTerm Add(IEnumerable<PolynomialTerm> variables)
    {
      var coefficient = variables.Select(v => v._coefficient).OrderBy(k => k).Sum();

      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        var result = variables.First();
        return new PolynomialTerm(coefficient, result.Indeterminates, result.IndeterminatesSignature);
      }
    }

    public Polynomial Composition(char variable, Polynomial replacement)
    {
      var remaining = new PolynomialTerm(_coefficient, _indeterminates.Where(kv => kv.Key != variable));

      uint power = _indeterminates.Single(kv => kv.Key == variable).Value;
      Polynomial inPlace = 1;
      for (uint i = 0; i < power; ++i)
      {
        inPlace *= replacement;
      }

      return remaining * inPlace;
    }

    /*
     * IPolynomialOperation
     */

    public IEnumerable<char> Variables => Indeterminates.Select(kv => kv.Key);

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (IsZero)
      {
        return 0;
      }
      else
      {
        return _coefficient * _indeterminates.Select(kv => NumericOperator.Power(x[kv.Key], kv.Value)).Product();
      }
    }
  }
}
