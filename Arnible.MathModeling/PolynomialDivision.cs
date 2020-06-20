using System;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling
{
  public readonly struct PolynomialDivision : IEquatable<PolynomialDivision>, IEquatable<Polynomial>, IPolynomialOperation
  {
    private readonly Polynomial _numerator;
    private readonly Polynomial _denominator;

    private PolynomialDivision(Polynomial numerator)
    {
      // this is polynomial
      _numerator = numerator;
      _denominator = default;
    }

    private PolynomialDivision(Polynomial numerator, Polynomial denominator)
    {
      if (denominator == 0)
      {
        throw new DivideByZeroException();
      }

      if (denominator.IsConstant)
      {
        _numerator = numerator / (double)denominator;
        _denominator = default;
      }
      else if (numerator.IsConstant)
      {
        // denominator is polynomial
        if (numerator == 0)
        {
          _numerator = 0;
          _denominator = default;
        }
        else
        {
          _numerator = numerator;
          _denominator = _numerator == 0 ? default : denominator;
        }
      }
      else
      {
        // this is true polynomial division
        _numerator = numerator;
        _denominator = denominator;
      }
    }

    internal static PolynomialDivision SimplifyPolynomialDivision(double numerator, Polynomial denominator)
    {
      // normalization is handled in the constructor
      return new PolynomialDivision(numerator, denominator);
    }

    internal static PolynomialDivision SimplifyPolynomialDivision(Polynomial numerator, Polynomial denominator)
    {
      if (numerator.IsConstant || denominator.IsConstant)
      {
        // normalization is handled in the constructor
        return new PolynomialDivision(numerator, denominator);
      }
      else
      {
        var commonVariables = GetCommonIdentityVariables(numerator: numerator, denominator: denominator).ToArray();
        if (commonVariables.Length > 0)
        {
          numerator = numerator.ReduceByCommon(commonVariables);
          denominator = denominator.ReduceByCommon(commonVariables);
        }
        return SimplifyByDividingNumeratorByDenominator(numerator: numerator, denominator: denominator);
      }
    }

    private static IEnumerable<VariableTerm> GetCommonIdentityVariables(Polynomial numerator, Polynomial denominator)
    {
      var numeratorVariables = numerator.GetIdentityVariableTerms();
      if (numeratorVariables.Count > 0)
      {
        var denominatorVariables = denominator.GetIdentityVariableTerms();
        if (denominatorVariables.Count > 0)
        {
          var commonVariables = numeratorVariables.ZipCommon(denominatorVariables, Math.Min);
          return commonVariables.Select(kv => new VariableTerm(variable: kv.Key, power: kv.Value));
        }
      }
      return LinqEnumerable.Empty<VariableTerm>();
    }

    private static PolynomialDivision SimplifyByDividingNumeratorByDenominator(Polynomial numerator, Polynomial denominator)
    {
      if (numerator.TryDivideBy(denominator, out Polynomial reducedNumerator))
      {
        // let's simplify it if we can
        return new PolynomialDivision(reducedNumerator);
      }
      else
      {
        return new PolynomialDivision(numerator, denominator);
      }
    }

    public static implicit operator PolynomialDivision(Polynomial v) => new PolynomialDivision(v);
    public static implicit operator PolynomialDivision(PolynomialTerm v) => new PolynomialDivision(v);
    public static implicit operator PolynomialDivision(double v) => new PolynomialDivision(v);
    public static implicit operator PolynomialDivision(char name) => new PolynomialDivision(name);

    public bool Equals(PolynomialDivision other)
    {
      if (IsPolynomial)
      {
        return _numerator == other._numerator;
      }
      else
      {
        return _numerator == other._numerator && _denominator == other._denominator;
      }
    }

    public bool Equals(Polynomial other)
    {
      if (IsPolynomial)
      {
        return _numerator == other;
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode() => _numerator.GetHashCode() ^ _denominator.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj is PolynomialDivision pd)
      {
        return Equals(pd);
      }
      else if (obj is Polynomial p)
      {
        return Equals(p);
      }
      else
      {
        return false;
      }
    }

    public string ToString(CultureInfo cultureInfo)
    {
      if (IsPolynomial)
        return _numerator.ToString(cultureInfo);
      else
      {
        string numerator = _numerator.ToString(cultureInfo);
        string denominator = _denominator.ToString(cultureInfo);
        string separator = new string('-', Math.Max(numerator.Length, denominator.Length));
        return $"{numerator} \n{separator} \n{denominator} ";
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public static bool operator ==(PolynomialDivision a, PolynomialDivision b) => a.Equals(b);
    public static bool operator !=(PolynomialDivision a, PolynomialDivision b) => !a.Equals(b);

    public static bool operator ==(PolynomialDivision a, Polynomial b) => a.Equals(b);
    public static bool operator !=(PolynomialDivision a, Polynomial b) => !a.Equals(b);

    public static bool operator ==(Polynomial a, PolynomialDivision b) => a.Equals(b);
    public static bool operator !=(Polynomial a, PolynomialDivision b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsPolynomial => _denominator == 0;

    public bool IsConstant
    {
      get
      {
        if (IsPolynomial)
          return _numerator.IsConstant;
        else
          return false;
      }
    }

    /*
     * Operators
     */

    public static explicit operator Polynomial(PolynomialDivision v)
    {
      if (!v.IsPolynomial)
      {
        throw new ArgumentException($"Polynomial division is not a polynomial: {v}");
      }
      return v._numerator;
    }

    public static explicit operator PolynomialTerm(PolynomialDivision v)
    {
      var p = (Polynomial)v;
      return (PolynomialTerm)p;
    }

    public static explicit operator double(PolynomialDivision v)
    {
      var p = (Polynomial)v;
      return (double)p;
    }

    // +

    public static PolynomialDivision operator +(PolynomialDivision a, PolynomialDivision b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator + b;
      }
      else if (b.IsPolynomial)
      {
        return a + b._numerator;
      }
      else if (a._denominator == b._denominator)
      {
        return SimplifyPolynomialDivision(a._numerator + b._numerator, a._denominator);
      }
      else
      {
        return SimplifyPolynomialDivision(
          numerator: a._numerator * b._denominator + b._numerator * a._denominator,
          denominator: a._denominator * b._denominator);
      }
    }

    public static PolynomialDivision operator +(PolynomialDivision a, Polynomial b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator + b;
      }
      else
      {
        return SimplifyPolynomialDivision(a._numerator + b * a._denominator, a._denominator);
      }
    }

    public static PolynomialDivision operator +(Polynomial a, PolynomialDivision b) => b + a;

    public static PolynomialDivision operator +(PolynomialDivision a, double b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator + b;
      }
      else
      {
        return SimplifyPolynomialDivision(a._numerator + b * a._denominator, a._denominator);
      }
    }

    public static PolynomialDivision operator +(double a, PolynomialDivision b) => b + a;

    // -

    public static PolynomialDivision operator -(PolynomialDivision a, PolynomialDivision b) => -1 * b + a;

    public static PolynomialDivision operator -(PolynomialDivision a, Polynomial b) => -1 * b + a;

    public static PolynomialDivision operator -(Polynomial a, PolynomialDivision b) => -1 * b + a;

    public static PolynomialDivision operator -(PolynomialDivision a, double b) => -1 * b + a;

    public static PolynomialDivision operator -(double a, PolynomialDivision b) => -1 * b + a;

    // *

    public static PolynomialDivision operator *(PolynomialDivision a, PolynomialDivision b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator * b;
      }
      else if (b.IsPolynomial)
      {
        return a * b._numerator;
      }
      else
      {
        return SimplifyPolynomialDivision(a._numerator * b._numerator, a._denominator * b._denominator);
      }
    }

    public static PolynomialDivision operator *(PolynomialDivision a, Polynomial b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator * b;
      }
      else
      {
        if (a._denominator.TryDivideBy(b, out Polynomial reducedDenominator))
        {
          return SimplifyPolynomialDivision(numerator: a._numerator, denominator: reducedDenominator);
        }
        else
        {
          return SimplifyPolynomialDivision(numerator: b * a._numerator, denominator: a._denominator);
        }
      }
    }

    public static PolynomialDivision operator *(Polynomial b, PolynomialDivision a) => a * b;

    public static PolynomialDivision operator *(PolynomialDivision a, double b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator * b;
      }
      else
      {
        // no need for simplification here
        return new PolynomialDivision(b * a._numerator, a._denominator);
      }
    }

    public static PolynomialDivision operator *(double b, PolynomialDivision a) => a * b;

    // /

    public static PolynomialDivision operator /(PolynomialDivision a, PolynomialDivision b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator / b;
      }
      else if (b.IsPolynomial)
      {
        return a / b._numerator;
      }
      else if (a._denominator == b._denominator)
      {
        return SimplifyPolynomialDivision(numerator: a._numerator, denominator: b._numerator);
      }
      else
      {
        return SimplifyPolynomialDivision(numerator: a._numerator * b._denominator, denominator: a._denominator * b._numerator);
      }
    }

    public static PolynomialDivision operator /(PolynomialDivision a, Polynomial b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator / b;
      }
      else
      {
        if (a._numerator.TryDivideBy(b, out Polynomial reducedNumerator))
        {
          return SimplifyPolynomialDivision(numerator: reducedNumerator, denominator: a._denominator);
        }
        else
        {
          return SimplifyPolynomialDivision(numerator: a._numerator, denominator: a._denominator * b);
        }
      }
    }

    public static PolynomialDivision operator /(Polynomial a, PolynomialDivision b)
    {
      if (b.IsPolynomial)
      {
        return a / b._numerator;
      }
      else
      {
        if (b._numerator.TryDivideBy(a, out Polynomial reducedNumerator))
        {
          return SimplifyPolynomialDivision(numerator: b._denominator, denominator: reducedNumerator);
        }
        else
        {
          return SimplifyPolynomialDivision(numerator: a * b._denominator, denominator: b._numerator);
        }
      }
    }

    public static PolynomialDivision operator /(PolynomialDivision a, double b)
    {
      if (a.IsPolynomial)
      {
        return a._numerator / b;
      }
      else
      {
        // no need for simplification here
        return new PolynomialDivision(a._numerator, a._denominator * b);
      }
    }

    public static PolynomialDivision operator /(double a, PolynomialDivision b)
    {
      if (b.IsPolynomial)
      {
        return a / b._numerator;
      }
      else
      {
        // no need for simplification here
        return new PolynomialDivision(a * b._denominator, b._numerator);
      }
    }

    /*
     * Other operators
     */

    public PolynomialDivision ToPower(uint power)
    {
      if (IsPolynomial)
      {
        return _numerator.ToPower(power);
      }
      else
      {
        // no need for simplification here
        return new PolynomialDivision(
          numerator: _numerator.ToPower(power),
          denominator: _denominator.ToPower(power)
        );
      }
    }

    public PolynomialDivision ReduceBy(Polynomial pol)
    {
      if (IsPolynomial)
      {
        throw new ArgumentException("Cannot reduce polynomial.");
      }

      return SimplifyPolynomialDivision(numerator: _numerator.DivideBy(pol), denominator: _denominator.DivideBy(pol));
    }

    public bool TryDivideBy(Polynomial b, out PolynomialDivision result)
    {
      if (_numerator.TryDivideBy(b, out Polynomial reminder))
      {
        if (IsPolynomial)
        {
          result = reminder;
        }
        else
        {
          result = SimplifyPolynomialDivision(reminder, _denominator);
        }
        return true;
      }
      else
      {
        result = default;
        return false;
      }
    }

    /*
     * Derivative
     */

    public PolynomialDivision DerivativeBy(char name)
    {
      if (IsPolynomial)
      {
        return _numerator.DerivativeBy(name);
      }
      else
      {
        Polynomial denominatorDerivative = _denominator.DerivativeBy(name);
        Polynomial numeratorDerivative = _numerator.DerivativeBy(name);
        if (denominatorDerivative == 0)
        {
          // this is necessary, since library doesn't support yet polynomials reduction/division
          return SimplifyPolynomialDivision(numeratorDerivative, _denominator);
        }
        else
        {
          Polynomial numerator = numeratorDerivative * _denominator - _numerator * denominatorDerivative;
          Polynomial denominator = _denominator * _denominator;
          return SimplifyPolynomialDivision(numerator, denominator);
        }
      }
    }

    public PolynomialDivision DerivativeBy(PolynomialTerm name) => DerivativeBy((char)name);

    public PolynomialDivision Derivative2By(char name)
    {
      if (IsPolynomial)
      {
        return _numerator.DerivativeBy(name).DerivativeBy(name);
      }
      else
      {
        Polynomial denominatorDerivative = _denominator.DerivativeBy(name);
        Polynomial numeratorDerivative = _numerator.DerivativeBy(name);
        if (denominatorDerivative == 0)
        {
          // this is necessary, since library doesn't support yet polynomials reduction/division
          return SimplifyPolynomialDivision(numeratorDerivative, _denominator).DerivativeBy(name);
        }
        else
        {
          Polynomial numerator1 = numeratorDerivative * _denominator - _numerator * denominatorDerivative;
          Polynomial denominator1 = _denominator * _denominator;

          Polynomial numerator2 = numerator1.DerivativeBy(name) * _denominator - 2 * numerator1 * denominatorDerivative;
          Polynomial denominator2 = denominator1 * _denominator;
          return SimplifyPolynomialDivision(numerator2, denominator2);
        }
      }
    }

    public PolynomialDivision Derivative2By(PolynomialTerm name) => Derivative2By((char)name);

    /*
     * Composition
     */

    public PolynomialDivision Composition(char variable, Polynomial replacement)
    {
      if (IsPolynomial)
      {
        return _numerator.Composition(variable, replacement);
      }
      else
      {
        return SimplifyPolynomialDivision(
          numerator: _numerator.Composition(variable, replacement),
          denominator: _denominator.Composition(variable, replacement));
      }

    }

    public PolynomialDivision Composition(char variable, PolynomialDivision replacement)
    {
      if (IsPolynomial)
      {
        if (replacement.IsPolynomial)
        {
          return _numerator.Composition(variable, replacement._numerator);
        }
        else
        {
          return _numerator.Composition(variable, replacement);
        }
      }
      else
      {
        if (replacement.IsPolynomial)
        {
          return _numerator.Composition(variable, replacement._numerator) / _denominator.Composition(variable, replacement._numerator);
        }
        else
        {
          return _numerator.Composition(variable, replacement) / _denominator.Composition(variable, replacement);
        }
      }
    }

    public PolynomialDivision Composition(PolynomialTerm variable, Polynomial replacement)
    {
      return Composition((char)variable, replacement);
    }

    public PolynomialDivision Composition(PolynomialTerm variable, PolynomialDivision replacement)
    {
      return Composition((char)variable, replacement);
    }

    /*
     * IPolynomialOperation
     */

    public IEnumerable<char> Variables => _numerator.Variables.Concat(_denominator.Variables).Distinct();

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (IsPolynomial)
      {
        return _numerator.Value(x);
      }
      else
      {
        return _numerator.Value(x) / _denominator.Value(x);
      }
    }
  }
}
