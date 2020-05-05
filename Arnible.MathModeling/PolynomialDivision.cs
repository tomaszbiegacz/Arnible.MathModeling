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
      _numerator = numerator;
      _denominator = default;
    }

    internal PolynomialDivision(Polynomial numerator, Polynomial denominator)
    {
      if (denominator.IsZero)
      {
        throw new DivideByZeroException();
      }

      Polynomial reducedNumerator = numerator.ReduceBy(denominator, out Polynomial reducedDenominator);
      if (reducedDenominator == 0)
      {
        // let's simplify it if we can
        _numerator = reducedNumerator;
        _denominator = default;
      }
      else
      {
        _numerator = numerator;
        _denominator = _numerator.IsZero ? default : denominator;
      }
    }

    internal PolynomialDivision(double numerator, Polynomial denominator)
    {
      if (denominator.IsZero)
      {
        throw new DivideByZeroException();
      }
      _numerator = numerator;
      _denominator = _numerator.IsZero ? default : denominator;
    }    

    public static implicit operator PolynomialDivision(Polynomial v) => new PolynomialDivision(numerator: v, denominator: 1);
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
        string numerator = _numerator.HasOneTerm ? _numerator.ToString(cultureInfo) : $"({ _numerator.ToString(cultureInfo) })";
        string denominator = _denominator.HasOneTerm ? _denominator.ToString(cultureInfo) : $"({ _denominator.ToString(cultureInfo) })";
        return $"{numerator}/{denominator}";
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

    public bool IsZero => IsPolynomial && _numerator.IsZero;

    public bool IsPolynomial => _denominator.IsZero;

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
        return new PolynomialDivision(a._numerator + b._numerator, a._denominator);
      }
      else
      {
        return new PolynomialDivision(
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
        return new PolynomialDivision(a._numerator + b * a._denominator, a._denominator);
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
        return new PolynomialDivision(a._numerator + b * a._denominator, a._denominator);
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
        return new PolynomialDivision(a._numerator * b._numerator, a._denominator * b._denominator);
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
        return new PolynomialDivision(b * a._numerator, a._denominator);
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
        return new PolynomialDivision(numerator: a._numerator, denominator: b._numerator);
      }
      else
      {
        return new PolynomialDivision(numerator: a._numerator * b._denominator, denominator: a._denominator * b._numerator);
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
        return new PolynomialDivision(a._numerator, a._denominator * b);
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
        return new PolynomialDivision(a * b._denominator, b._numerator);
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
        return new PolynomialDivision(
          numerator: _numerator.ToPower(power),
          denominator: _denominator.ToPower(power)
        );
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
        if (denominatorDerivative.IsZero)
        {
          // this is necessary, since library doesn't support yet polynomials reduction/division
          return new PolynomialDivision(numeratorDerivative, _denominator);
        }
        else
        {
          Polynomial numerator = numeratorDerivative * _denominator - _numerator * denominatorDerivative;
          Polynomial denominator = _denominator * _denominator;
          return new PolynomialDivision(numerator, denominator);
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
        if (denominatorDerivative.IsZero)
        {
          // this is necessary, since library doesn't support yet polynomials reduction/division
          return new PolynomialDivision(numeratorDerivative, _denominator).DerivativeBy(name);
        }
        else
        {
          Polynomial numerator1 = numeratorDerivative * _denominator - _numerator * denominatorDerivative;
          Polynomial denominator1 = _denominator * _denominator;

          Polynomial numerator2 = numerator1.DerivativeBy(name) * _denominator - 2 * numerator1 * denominatorDerivative;
          Polynomial denominator2 = denominator1 * _denominator;
          return new PolynomialDivision(numerator2, denominator2);
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
        return new PolynomialDivision(
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
