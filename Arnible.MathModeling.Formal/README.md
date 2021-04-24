# Arnible.MathModeling.Formal

Library reuses most of the code from [Arnible.MathModeling](./../Arnible.MathModeling) but replaces definition of [Number](./Number.cs) to allow symbolic rather than numeric modeling.

Assume that square error is defined as class
```C#
public class SquareError : IBinaryOperation<Number>
{    
    public Number Value(in Number x, in Number y)
    {
      return (x - y).ToPower(2);
    }

    public Derivative2Value DerivativeByX(RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByY(RectangularCoordinate p)
    {
      return new Derivative2Value(
        first: -2 * (p.X - p.Y),
        second: 2);
    }

    public Derivative2Value DerivativeByR(PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2),
        second: 2 * (Cos(p.Φ) - Sin(p.Φ)).ToPower(2)
        );
    }     

    public Derivative2Value DerivativeByΦ(PolarCoordinate p)
    {
      return new Derivative2Value(
        first: 2 * p.R.ToPower(2) * (Sin(p.Φ).ToPower(2) - Cos(p.Φ).ToPower(2)),
        second: 8 * p.R.ToPower(2) * Cos(p.Φ) * Sin(p.Φ)
        );
    }
}
```

Based on this class the following tests can be written to verify above class correctness
```C#
public class SquareErrorTests
{
    private readonly SquareError _error = new SquareError();
    
    static void AreDerivativesEqual(in PolynomialDivision value, in Number term, in Derivative1Value actual)
    {
      PolynomialTerm termSingle = (PolynomialTerm)term;
      PolynomialDivision firstDerivative = value.DerivativeBy(termSingle);
      IsEqualToExtensions.AssertIsEqualTo<Number>(firstDerivative, actual.First);
    }

    [Fact]
    public void Value()
    {
      IsEqualToExtensions.AssertIsEqualTo((x - y).ToPower(2), _error.Value(x, y));
    }

    [Fact]
    public void DerivativeByX()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual((PolynomialDivision)_error.Value(x, y), x, _error.DerivativeByX(p));
    }
}
```