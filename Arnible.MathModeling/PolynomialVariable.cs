using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Arnible.MathModeling
{
  public struct PolynomialVariable
  {
    private readonly double _coefficient;
    private readonly IEnumerable<KeyValuePair<char, uint>> _indeterminates;
    private readonly string _indeterminatesSignature;    

    public PolynomialVariable(double coefficient, params (char, uint)[] indeterminates)
      : this(coefficient, Normalize(indeterminates.ToDictionary(i => i.Item1, i => i.Item2)))
    {
      // intentionally empty
    }

    private PolynomialVariable(double coefficient, IEnumerable<KeyValuePair<char, uint>> indeterminates)
      : this(coefficient, indeterminates, GetIntermediateSignature(indeterminates))
    {
      // intentionally empty
    }

    private PolynomialVariable(double coefficient, IEnumerable<KeyValuePair<char, uint>> indeterminates, string signature)
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

    public static implicit operator PolynomialVariable(char name) => new PolynomialVariable(1, (name, 1));
    public static implicit operator PolynomialVariable(double value) => new PolynomialVariable(value);
    public static implicit operator PolynomialVariable((char, uint) indeterminate) => new PolynomialVariable(1, indeterminate);

    private IEnumerable<KeyValuePair<char, uint>> Indeterminates => _indeterminates ?? Enumerable.Empty<KeyValuePair<char, uint>>();

    private string IndeterminatesSignature => _indeterminatesSignature ?? String.Empty;
    
    private static IEnumerable<KeyValuePair<char, uint>> Normalize(IEnumerable<KeyValuePair<char, uint>> source)
    {
      return source.Where(kv => kv.Value > 0).OrderBy(kv => kv.Key).ToArray();
    }


    private static string GetIntermediateSignature(IEnumerable<KeyValuePair<char, uint>> indeterminates)
    {
      var builder = new StringBuilder();
      foreach (var kv in indeterminates)
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

      return $"{_coefficient.ToString(CultureInfo.InvariantCulture)}*{IndeterminatesSignature}";
    }

    public bool Equals(PolynomialVariable other)
    {
      return IndeterminatesSignature == other.IndeterminatesSignature && NumericOperator.Equals(_coefficient, other._coefficient);
    }

    public override int GetHashCode()
    {
      return _coefficient.GetHashCode() * IndeterminatesSignature.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is PolynomialVariable v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public static bool operator ==(PolynomialVariable a, PolynomialVariable b) => a.Equals(b);
    public static bool operator !=(PolynomialVariable a, PolynomialVariable b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsZero => _coefficient == 0;
    public bool IsConstant => IndeterminatesSignature.Length == 0;
    public bool IsPositive => _coefficient > 0;

    /*
     * Operators
     */

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

    public static PolynomialVariable operator *(PolynomialVariable a, PolynomialVariable b)
    {
      double coefficient = a._coefficient * b._coefficient;
      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        return new PolynomialVariable(coefficient, Multiply(a.Indeterminates, b.Indeterminates));
      }
    }    

    public PolynomialVariable DerivativeBy(char name)
    {
      uint power = Indeterminates.Where(kv => kv.Key == name).Select(kv => kv.Value).SingleOrDefault();
      if (power > 0)
      {
        var indeterminates = Indeterminates.Where(kv => kv.Key != name).Append(new KeyValuePair<char, uint>(name, power - 1));
        return new PolynomialVariable(_coefficient * power, Normalize(indeterminates));
      }
      else
      {
        return 0;
      }
    }

    /*
     * IEnumerable operators
     */

    public static IEnumerable<PolynomialVariable> Simplify(IEnumerable<PolynomialVariable> variables)
    {
      return variables.GroupBy(v => v.IndeterminatesSignature).Select(g => Add(g)).Where(v => !v.IsZero);
    }

    private static PolynomialVariable Add(IEnumerable<PolynomialVariable> variables)
    {
      var coefficient = variables.Select(v => v._coefficient).OrderBy(k => k).Sum();

      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        var result = variables.First();
        return new PolynomialVariable(coefficient, result.Indeterminates, result.IndeterminatesSignature);
      }
    }        
  }
}
