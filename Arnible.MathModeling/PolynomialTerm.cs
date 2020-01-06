using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Arnible.MathModeling
{
  public struct PolynomialTerm : IEquatable<PolynomialTerm>, IPolynomialOperation
  {
    private readonly double _coefficient;
    private readonly IEnumerable<IndeterminateExpression> _indeterminates;

    private readonly string _indeterminatesSignature;
    private IndeterminateExpression _greatestPower;

    private PolynomialTerm(double coefficient, params IndeterminateExpression[] indeterminates)
      : this(coefficient, (IEnumerable<IndeterminateExpression>)indeterminates)
    {
      // intentionally empty
    }

    private PolynomialTerm(double coefficient, IEnumerable<IndeterminateExpression> indeterminates)
      : this(coefficient, indeterminates.Where(i => !i.IsOne), GetIntermediateSignature(indeterminates))
    {
      // intentionally empty
    }

    private PolynomialTerm(double coefficient, IEnumerable<IndeterminateExpression> indeterminates, string signature)
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
      _greatestPower = default;
    }

    public static implicit operator PolynomialTerm(char name) => new PolynomialTerm(1, (IndeterminateExpression)name);
    public static implicit operator PolynomialTerm(IndeterminateExpression name) => new PolynomialTerm(1, name);
    public static implicit operator PolynomialTerm(double value) => new PolynomialTerm(value);

    private IEnumerable<IndeterminateExpression> Indeterminates => _indeterminates ?? Enumerable.Empty<IndeterminateExpression>();

    private string IndeterminatesSignature => _indeterminatesSignature ?? String.Empty;

    public long PowerSum => Indeterminates.Sum(kv => kv.Power);

    public IndeterminateExpression GreatestPowerIndeterminate
    {
      get
      {
        if (_greatestPower == default)
        {
          // build cache
          foreach (var kv in Indeterminates)
          {
            if (kv.Power > _greatestPower.Power)
            {
              _greatestPower = kv;
            }
          }
        }

        // return last fetched value
        return _greatestPower;
      }
    }

    private static string GetIntermediateSignature(IEnumerable<IndeterminateExpression> indeterminates) => string.Join(string.Empty, indeterminates.Where(k => !k.IsOne).Order());

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

      return $"{_coefficient.ToString(CultureInfo.InvariantCulture)}{IndeterminatesSignature}";
    }

    public bool Equals(PolynomialTerm other)
    {
      return IndeterminatesSignature == other.IndeterminatesSignature && _coefficient.NumericEquals(other._coefficient);
    }

    public override int GetHashCode()
    {
      return _coefficient.GetHashCode() ^ IndeterminatesSignature.GetHashCode();
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
        return (char)v.Indeterminates.Single();
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



    public static PolynomialTerm operator *(PolynomialTerm a, PolynomialTerm b)
    {
      double coefficient = a._coefficient * b._coefficient;
      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        return new PolynomialTerm(coefficient, IndeterminateExpression.Multiply(a.Indeterminates, b.Indeterminates));
      }
    }

    public PolynomialTerm ToPower(uint power)
    {
      switch (power)
      {
        case 0:
          return 1;
        case 1:
          return this;
        default:
          return new PolynomialTerm(
            coefficient: _coefficient.ToPower(power),
            indeterminates: Indeterminates.Select(kv => kv.ToPower(power)));
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

      var bIndeterminates = b.Indeterminates.ToDictionary(kv => kv.Signature, kv => kv);
      var resultIndeterminates = new List<IndeterminateExpression>();
      foreach (var aInt in Indeterminates)
      {
        if (bIndeterminates.TryGetValue(aInt.Signature, out IndeterminateExpression bInt))
        {
          if (!aInt.TryDivide(bInt, out IndeterminateExpression reminder))
          {
            // result is not polynomial
            result = default;
            return false;
          }

          bIndeterminates.Remove(aInt.Signature);
          if (!reminder.IsOne)
          {
            resultIndeterminates.Add(reminder);
          }
        }
        else
        {
          resultIndeterminates.Add(aInt);
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

    public IEnumerable<PolynomialTerm> DerivativeBy(char name)
    {
      var notConstant = Indeterminates.Where(kv => kv.Variable == name).ToArray();
      if (notConstant.Length > 0)
      {
        var constantIndeterminates = Indeterminates.Where(kv => kv.Variable != name).ToArray();
        foreach (IndeterminateExpression toDerivative in notConstant)
        {
          yield return (new PolynomialTerm(_coefficient, notConstant.Where(nc => nc != toDerivative).Concat(constantIndeterminates))) * toDerivative.DerivativeBy(name);
        }
      }
    }

    /*
     * IEnumerable operators
     */

    public static IEnumerable<PolynomialTerm> Simplify(IEnumerable<PolynomialTerm> variables)
    {
      var vars = variables.Where(v => !v.IsZero).ToArray();
      switch(vars.Length)
      {
        case 0:
        case 1:
          return vars;
        default:
          return variables.GroupBy(v => v.IndeterminatesSignature)
            .Select(g => Add(g)).Where(v => !v.IsZero)
            .OrderByDescending(v => v.PowerSum).ThenByDescending(v => v.GreatestPowerIndeterminate.Power).ThenBy(v => v.GreatestPowerIndeterminate.Signature);
      }
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
      var remaining = new PolynomialTerm(_coefficient, _indeterminates.Where(kv => kv.Variable != variable));

      var toReplace = _indeterminates.Where(kv => kv.Variable == variable).ToArray();
      Polynomial inPlace = 1;
      if (toReplace.Length > 0)
      {
        if (toReplace.Any(i => i.HasUnaryModifier))
        {
          if(replacement.IsConstant)
          {
            double constantReplacement = (double)replacement;
            inPlace = toReplace.Select(r => r.SimplifyForConstant(constantReplacement)).Product();
          }
          else
          {
            throw new NotSupportedException("Composition of variables used in modifiers is not supported");
          }          
        }
        else
        {
          if(toReplace.Length > 1)
          {
            throw new InvalidOperationException("Something is wrong, got two terms with the same variable...");
          }

          uint power = toReplace.Single().Power;
          for (uint i = 0; i < power; ++i)
          {
            inPlace *= replacement;
          }
        }        
      }

      return remaining * inPlace;
    }

    public PolynomialDivision Composition(char variable, PolynomialDivision replacement)
    {
      var remaining = new PolynomialTerm(_coefficient, _indeterminates.Where(kv => kv.Variable != variable));

      var toReplace = _indeterminates.Where(kv => kv.Variable == variable).ToArray();
      PolynomialDivision inPlace = PolynomialDivision.One;
      if (toReplace.Length > 0)
      {
        if(toReplace.Length > 1 && toReplace.Any(i => i.HasUnaryModifier))
        {
          throw new NotSupportedException("Composition of variables used in modifiers is not supported");
        }

        uint power = toReplace.Single().Power;
        for (uint i = 0; i < power; ++i)
        {
          inPlace *= replacement;
        }
      }          

      return remaining * inPlace;
    }

    /*
     * IPolynomialOperation
     */

    public IEnumerable<char> Variables => Indeterminates.Select(kv => kv.Variable).Distinct();

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (IsZero)
      {
        return 0;
      }
      else
      {
        return _coefficient * _indeterminates.Select(kv => kv.Value(x)).Product();
      }
    }
  }
}
