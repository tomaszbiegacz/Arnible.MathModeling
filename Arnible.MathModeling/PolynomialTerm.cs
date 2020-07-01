using System;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling
{
  public readonly struct PolynomialTerm : IEquatable<PolynomialTerm>, IPolynomialOperation
  {
    private readonly double _coefficient;
    private readonly ValueArray<IndeterminateExpression> _indeterminates;
    private readonly string _indeterminatesSignature;

    private PolynomialTerm(double coefficient)
    {
      _coefficient = coefficient;
      _indeterminates = default;
      _indeterminatesSignature = string.Empty;
      GreatestPowerIndeterminate = default;
    }

    private PolynomialTerm(IndeterminateExpression term)
    {
      _coefficient = 1;
      if (term.IsOne)
      {
        _indeterminates = default;
        _indeterminatesSignature = string.Empty;
        GreatestPowerIndeterminate = default;
      }
      else
      {
        _indeterminates = term;
        _indeterminatesSignature = _indeterminates.Single().ToString();
        GreatestPowerIndeterminate = term;
      }
    }

    private PolynomialTerm(double newCoefficient, PolynomialTerm baseTerm)
    {
      if (newCoefficient.NumericEquals(0))
      {
        throw new ArgumentException(nameof(baseTerm));
      }

      _coefficient = newCoefficient;
      _indeterminates = baseTerm._indeterminates;
      _indeterminatesSignature = baseTerm._indeterminatesSignature;
      GreatestPowerIndeterminate = baseTerm.GreatestPowerIndeterminate;
    }

    private PolynomialTerm(double coefficient, IEnumerable<IndeterminateExpression> indeterminates)
      : this(coefficient, indeterminates.Where(i => !i.IsOne).Order().ToValueArray(), default, default)
    {
      // intentionally empty
    }

    private PolynomialTerm(
      double coefficient,
      ValueArray<IndeterminateExpression> indeterminates,
      string signature,
      IndeterminateExpression greatestPowerExpression)
    {
      bool anyIndeterminates = indeterminates.Length > 0;
      bool hasSignature = signature != null;
      if (coefficient.NumericEquals(0) && anyIndeterminates)
      {
        throw new ArgumentException(nameof(indeterminates));
      }

      _coefficient = coefficient;
      _indeterminates = indeterminates;
      if (anyIndeterminates == hasSignature)
      {
        _indeterminatesSignature = signature ?? string.Empty;
        GreatestPowerIndeterminate = greatestPowerExpression;
      }
      else
      {
        _indeterminatesSignature = anyIndeterminates ? string.Join(string.Empty, indeterminates) : string.Empty;
        GreatestPowerIndeterminate = anyIndeterminates ? FindGreatestPowerIndeterminate(indeterminates) : default;
      }
    }

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

    public static implicit operator PolynomialTerm(char name) => new PolynomialTerm((IndeterminateExpression)name);
    public static implicit operator PolynomialTerm(IndeterminateExpression name) => new PolynomialTerm(name);
    public static implicit operator PolynomialTerm(double value) => new PolynomialTerm(value);

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
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

    public bool IsConstant => string.IsNullOrEmpty(_indeterminatesSignature);
    public bool HasPositiveCoefficient => _coefficient > 0;

    private string IndeterminatesSignature => _indeterminatesSignature ?? string.Empty;

    public ulong PowerSum => _indeterminates.Select(kv => kv.Power).SumWithDefault();

    public IndeterminateExpression GreatestPowerIndeterminate { get; }

    /*
     * Query
     */

    internal IEnumerable<VariableTerm> GetIdentityVariableTerms()
    {
      return _indeterminates.Where(i => !i.HasUnaryModifier).Select(i => new VariableTerm(variable: i.Variable, power: i.Power));
    }

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
      if (v._coefficient == 1 && v._indeterminates.Length == 1)
      {
        return (char)v._indeterminates[0];
      }

      throw new InvalidOperationException("Polynomial term is not a single variable");
    }

    // +

    public static Polynomial operator +(PolynomialTerm a, PolynomialTerm b)
    {
      return new Polynomial(a, b);
    }

    public static Polynomial operator +(PolynomialTerm a, double b)
    {
      if (a.IsConstant)
      {
        return (double)a + b;
      }
      else
      {
        return new Polynomial(a, b);
      }
    }

    public static Polynomial operator +(double a, PolynomialTerm b) => b + a;

    // -

    public static Polynomial operator -(PolynomialTerm a, PolynomialTerm b) => -1 * b + a;

    public static Polynomial operator -(PolynomialTerm a, double b) => -1 * b + a;

    public static Polynomial operator -(double a, PolynomialTerm b) => -1 * b + a;

    // *

    public static PolynomialTerm operator *(PolynomialTerm a, PolynomialTerm b)
    {
      double coefficient = a._coefficient * b._coefficient;
      if (coefficient == 0)
      {
        return 0;
      }
      else if (a.IsConstant)
      {
        return new PolynomialTerm(coefficient, b);
      }
      else if (b.IsConstant)
      {
        return new PolynomialTerm(coefficient, a);
      }
      else
      {
        return new PolynomialTerm(coefficient, IndeterminateExpression.Multiply(a._indeterminates, b._indeterminates));
      }
    }

    public static PolynomialTerm operator *(double a, PolynomialTerm b)
    {
      double coefficient = a * b._coefficient;
      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        return new PolynomialTerm(coefficient, b);
      }
    }

    public static PolynomialTerm operator *(PolynomialTerm a, double b)
    {
      double coefficient = a._coefficient * b;
      if (coefficient == 0)
      {
        return 0;
      }
      else
      {
        return new PolynomialTerm(coefficient, a);
      }
    }

    // /

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
      return PolynomialDivision.SimplifyPolynomialDivision(numerator: a, denominator: b);
    }

    public bool TryDivide(PolynomialTerm b, out PolynomialTerm result)
    {
      if (b == 0)
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

      var bIndeterminates = b._indeterminates.ToDictionary(kv => kv.Signature);
      var resultIndeterminates = new List<IndeterminateExpression>();
      foreach (var aInt in _indeterminates)
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

      if (bIndeterminates.Count > 0)
      {
        // some indeterminates from b hasn't been removed
        // result is not polynomial
        result = default;
        return false;
      }

      result = new PolynomialTerm(_coefficient / b._coefficient, resultIndeterminates);
      return true;
    }

    /*
     * Other operators
     */


    public static PolynomialTerm Sin(PolynomialTerm name)
    {
      if (name.IsConstant)
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
            indeterminates: _indeterminates.Select(kv => kv.ToPower(power)));
      }
    }

    private static IEnumerable<IndeterminateExpression> ReduceByCommon(
      IEnumerable<IndeterminateExpression> source,
      IEnumerable<VariableTerm> toReduce)
    {
      var remainingTerms = toReduce.ToDictionary(t => t.Variable, t => t.Power);
      foreach (var expression in source)
      {
        if (!expression.HasUnaryModifier && remainingTerms.TryGetValue(expression.Variable, out uint reducePowerBy))
        {
          yield return expression.ReducePowerBy(reducePowerBy);
          remainingTerms.Remove(expression.Variable);
        }
        else
        {
          yield return expression;
        }
      }
      if (remainingTerms.Count > 0)
      {
        throw new InvalidOperationException("Not all variables are common.");
      }
    }

    public PolynomialTerm ReduceByCommon(IEnumerable<VariableTerm> terms)
    {
      return new PolynomialTerm(_coefficient, ReduceByCommon(_indeterminates, terms));
    }

    public IEnumerable<PolynomialTerm> DerivativeBy(char name)
    {
      ValueArray<IndeterminateExpression> notConstant = _indeterminates.Where(kv => kv.Variable == name).ToValueArray();
      if (notConstant.Length > 0)
      {
        ValueArray<IndeterminateExpression> constantIndeterminates = _indeterminates.Where(kv => kv.Variable != name).ToValueArray();
        foreach (IndeterminateExpression toDerivative in notConstant)
        {
          yield return (new PolynomialTerm(_coefficient, notConstant.Where(nc => nc != toDerivative).Concat(constantIndeterminates))) * toDerivative.DerivativeBy(name);
        }
      }
    }

    /*
     * IEnumerable operators
     */

    public static ValueArray<PolynomialTerm> Simplify(ValueArray<PolynomialTerm> variables)
    {
      switch (variables.Length)
      {
        case 0:
          return variables;
        case 1:
          if (variables[0] == 0)
            return default;
          else
            return variables;
        default:
          return variables
            .AggregateBy(v => v._indeterminatesSignature, g => Add(g)).Values
            .Where(v => v != 0)
            .OrderByDescending(v => v.PowerSum, v => v.GreatestPowerIndeterminate.Power).ThenOrderBy(v => v.GreatestPowerIndeterminate.Signature)
            .ToValueArray();
      }
    }

    public static bool IsSimplified(IEnumerable<PolynomialTerm> variables)
    {
      return variables.Select(v => v._indeterminatesSignature).Distinct().Count() == variables.Count();
    }

    private static PolynomialTerm Add(IEnumerable<PolynomialTerm> variables)
    {
      if (variables.Count() == 1)
      {
        return variables.Single();
      }
      else
      {
        var coefficient = variables.Select(v => v._coefficient).SumWithDefault();

        if (coefficient.NumericEquals(0))
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
    }

    public IEnumerable<PolynomialTerm> Composition(char variable, Polynomial replacement)
    {
      if (!_indeterminates.Any(kv => kv.Variable == variable))
      {
        // nothing to do here
        return LinqEnumerable.Yield(this);
      }
      else
      {
        var remaining = new PolynomialTerm(_coefficient, _indeterminates.Where(kv => kv.Variable != variable));

        var toReplace = _indeterminates.Where(kv => kv.Variable == variable);
        if (toReplace.Any(i => i.HasUnaryModifier))
        {
          if (replacement.IsConstant)
          {
            double constantReplacement = (double)replacement;
            Polynomial inPlace = toReplace.Select(r => r.SimplifyForConstant(constantReplacement)).ProductDefensive();
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
          if (replacement.IsSingleTerm)
          {
            var temp = (PolynomialTerm)replacement;
            return LinqEnumerable.Yield(temp.ToPower(power) * remaining);
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
      var remaining = new PolynomialTerm(_coefficient, _indeterminates.Where(kv => kv.Variable != variable));

      IReadOnlyList<IndeterminateExpression> toReplace = _indeterminates.Where(kv => kv.Variable == variable).ToReadOnlyList();
      PolynomialDivision inPlace = 1;
      if (toReplace.Count > 0)
      {
        if (toReplace.Count > 1 && toReplace.Any(i => i.HasUnaryModifier))
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

    public IEnumerable<char> Variables => _indeterminates.Select(kv => kv.Variable).Distinct();

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (_coefficient == 0)
      {
        return 0;
      }
      else
      {
        return _coefficient * _indeterminates.Select(kv => kv.Value(x)).ProductWithDefault();
      }
    }
  }
}
