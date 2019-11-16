using System;

namespace Arnible.MathModeling
{
  public struct PolynomialDivision : IEquatable<PolynomialDivision>
  {
    public Polynomial Numerator { get; }
    public Polynomial Denominator { get; }

    public PolynomialDivision(Polynomial numerator, Polynomial denominator)
    {
      if (denominator.IsZero)
      {
        throw new DivideByZeroException();
      }

      Numerator = numerator;
      Denominator = denominator;
    }

    public bool Equals(PolynomialDivision other) => IsZero ? other.IsZero : other.Numerator == Numerator && other.Denominator == Denominator;

    public override int GetHashCode() => IsZero ? 0 : Numerator.GetHashCode() * Denominator.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj is PolynomialDivision v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override string ToString()
    {
      if (IsZero)
        return "0";
      if (IsNaN)
        return "NaN";

      string numerator = Numerator.HasOneTerm ? Numerator.ToString() : $"({Numerator})";
      string denominator = Denominator.HasOneTerm ? Denominator.ToString() : $"({Denominator})";
      return $"{numerator}/{denominator}";
    }

    public static bool operator ==(PolynomialDivision a, PolynomialDivision b) => a.Equals(b);
    public static bool operator !=(PolynomialDivision a, PolynomialDivision b) => !a.Equals(b);

    /*
     * Properties
     */

    public bool IsNaN => Numerator.IsZero && Denominator.IsZero;

    public bool IsZero => Numerator.IsZero && !Denominator.IsZero;

    public bool IsPolynomial => Denominator.IsConstant && !Denominator.IsZero;

    /*
     * Operators
     */

    public static explicit operator Polynomial(PolynomialDivision v) => v.Numerator / (double)v.Denominator;    

    public static PolynomialDivision operator *(PolynomialDivision a, double numerator)
    {
      return new PolynomialDivision(numerator * a.Numerator, a.Denominator);
    }

    public static PolynomialDivision operator *(double numerator, PolynomialDivision a) => a * numerator;

    public PolynomialDivision DerivativeBy(char name)
    {
      var denominatorDerivative = Denominator.DerivativeBy(name);
      var numeratorDerivative = Numerator.DerivativeBy(name);
      if (denominatorDerivative.IsZero)
      {
        return new PolynomialDivision(numeratorDerivative, Denominator);
      }
      else
      {
        var numerator = numeratorDerivative * Denominator - Numerator * denominatorDerivative;
        var denominator = Denominator * Denominator;
        return new PolynomialDivision(numerator, denominator);
      }
    }
  }
}
