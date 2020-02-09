using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Arnible.MathModeling
{
  public readonly struct PolynomialTerm : IEquatable<PolynomialTerm>, IPolynomialOperation
  {
    private readonly double _coefficient;
    private readonly IImmutableList<IndeterminateExpression> _indeterminates;
    private readonly string _indeterminatesSignature;

    private PolynomialTerm(double coefficient, IEnumerable<IndeterminateExpression> indeterminates)
      : this(coefficient, indeterminates.Where(i => !i.IsOne).Order().ToImmutableList(), default, default)
    {
      // intentionally empty
    }

    private PolynomialTerm(
      double coefficient,
      IImmutableList<IndeterminateExpression> indeterminates,
      string signature,
      IndeterminateExpression greatestPowerExpression)
    {
      bool anyIndeterminates = indeterminates != null && indeterminates.Count > 0;
      bool hasSignature = signature != null;
      if (coefficient == 0 && anyIndeterminates)
      {        
        throw new ArgumentException(nameof(indeterminates));
      }

      _coefficient = coefficient;
      _indeterminates = indeterminates;
      if (anyIndeterminates == hasSignature)
      {
        _indeterminatesSignature = signature;
        GreatestPowerIndeterminate = greatestPowerExpression;
      }
      else
      {
        _indeterminatesSignature = anyIndeterminates ? string.Join(string.Empty, indeterminates) : null;        
         GreatestPowerIndeterminate = anyIndeterminates ? FindGreatestPowerIndeterminate(indeterminates) : default;
      }
    }

    public static implicit operator PolynomialTerm(char name) => new PolynomialTerm(1, ((IndeterminateExpression)name).Yield());
    public static implicit operator PolynomialTerm(IndeterminateExpression name) => new PolynomialTerm(1, name.Yield());
    public static implicit operator PolynomialTerm(double value) => new PolynomialTerm(value, Enumerable.Empty<IndeterminateExpression>());

    private IEnumerable<IndeterminateExpression> Indeterminates => _indeterminates ?? Enumerable.Empty<IndeterminateExpression>();

    private string IndeterminatesSignature => _indeterminatesSignature ?? string.Empty;

    public long PowerSum => Indeterminates.Sum(kv => kv.Power);

    public IndeterminateExpression GreatestPowerIndeterminate { get; }

    private static IndeterminateExpression FindGreatestPowerIndeterminate(IEnumerable<IndeterminateExpression> indeterminates)
    {
      IndeterminateExpression greatestPower = default;
      foreach (var kv in indeterminates)
      {
        if (kv.Power > greatestPower.Power)
        {
          greatestPower = kv;
        }
      }
      return greatestPower;
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      if (IsZero)
        return "0";
      if (IsConstant)
        return _coefficient.ToString(cultureInfo);
      if (_coefficient == 1)
        return IndeterminatesSignature;
      if (_coefficient == -1)
        return $"-{IndeterminatesSignature}";
      else
        return $"{_coefficient.ToString(cultureInfo)}{IndeterminatesSignature}";
    }

    public bool Equals(PolynomialTerm other)
    {
      return _indeterminatesSignature == other._indeterminatesSignature && _coefficient.NumericEquals(other._coefficient);
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

    public static PolynomialTerm Sin(PolynomialTerm name)
    {
      if(name.IsConstant)
      {
        return IndeterminateExpression.Sin((double)name);
      }
      else
      {
        return IndeterminateExpression.Sin((char)name);
      }
    }

    public static PolynomialTerm Cos(PolynomialTerm name)
    {
      if (name.IsConstant)
      {
        return IndeterminateExpression.Cos((double)name);
      }
      else
      {
        return IndeterminateExpression.Cos((char)name);
      }
    }

    /*
     * Properties
     */

    public bool IsZero => _coefficient == 0;
    public bool IsConstant => _indeterminatesSignature == null;
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
        return (char)v._indeterminates.Single();
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
      return new PolynomialTerm(
        a._coefficient / denominator,
        a._indeterminates,
        a._indeterminatesSignature,
        a.GreatestPowerIndeterminate);
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
      switch (vars.Length)
      {
        case 0:
        case 1:
          return vars;
        default:
          return variables.GroupBy(v => v._indeterminatesSignature)
            .Select(g => Add(g)).Where(v => !v.IsZero)
            .OrderByDescending(v => v.PowerSum).ThenByDescending(v => v.GreatestPowerIndeterminate.Power).ThenBy(v => v.GreatestPowerIndeterminate.Signature);
      }
    }

    public static bool IsSimplified(IEnumerable<PolynomialTerm> variables)
    {
      return variables.Select(v => v._indeterminatesSignature).Distinct().Count() == variables.Count();
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
        return new PolynomialTerm(
          coefficient,
          result._indeterminates,
          result._indeterminatesSignature,
          result.GreatestPowerIndeterminate);
      }
    }

    public IEnumerable<PolynomialTerm> Composition(char variable, Polynomial replacement)
    {
      if (!Indeterminates.Any(kv => kv.Variable == variable))
      {
        // nothing to do here
        return this.Yield();
      }
      else
      {
        var remaining = new PolynomialTerm(_coefficient, Indeterminates.Where(kv => kv.Variable != variable));

        var toReplace = Indeterminates.Where(kv => kv.Variable == variable);
        if (toReplace.Any(i => i.HasUnaryModifier))
        {
          if (replacement.IsConstant)
          {
            double constantReplacement = (double)replacement;
            Polynomial inPlace = toReplace.Select(r => r.SimplifyForConstant(constantReplacement)).Product();
            return inPlace * remaining;
          }
          else
          {
            throw new NotSupportedException("Composition of variables used in modifiers is not supported");
          }
        }
        else
        {
          uint power = toReplace.Single().Power;
          if (replacement.HasOneTerm)
          {
            var temp = (PolynomialTerm)replacement;
            return (temp.ToPower(power) * remaining).Yield();
          }
          else
          {
            Polynomial inPlace = replacement.ToPower(power);
            return inPlace * remaining;
          }
        }
      }
    }

    public PolynomialDivision Composition(char variable, PolynomialDivision replacement)
    {
      var remaining = new PolynomialTerm(_coefficient, Indeterminates.Where(kv => kv.Variable != variable));

      var toReplace = Indeterminates.Where(kv => kv.Variable == variable).ToArray();
      PolynomialDivision inPlace = PolynomialDivision.One;
      if (toReplace.Length > 0)
      {
        if (toReplace.Length > 1 && toReplace.Any(i => i.HasUnaryModifier))
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
        return _coefficient * Indeterminates.Select(kv => kv.Value(x)).Product();
      }
    }
  }
}
