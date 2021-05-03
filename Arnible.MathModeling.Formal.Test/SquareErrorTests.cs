using Arnible.Assertions;
using Arnible.MathModeling.Geometry;
using Arnible.MathModeling.Algebra.Polynomials;
using Arnible.MathModeling.Analysis;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;

namespace Arnible.MathModeling.Test
{
  public class SquareErrorTests
  {
    private readonly SquareError _error = new SquareError();

    static void AreDerivativesEqual(in Number value, in Number term, in Derivative1Value actual)
    {
      AreDerivativesEqual((PolynomialDivision)value, in term, in actual);
    }

    static void AreDerivativesEqual(in PolynomialDivision value, in Number term, in Derivative1Value actual)
    {
      PolynomialTerm termSingle = (PolynomialTerm)term;
      Number firstDerivative = value.DerivativeBy(termSingle);
      firstDerivative.AssertIsEqualTo(actual.First);
    }

    [Fact]
    public void Value()
    {
      _error.Value(x, y).AssertIsEqualTo((x - y).ToPower(2));
    }

    [Fact]
    public void DerivativeByX()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual(_error.Value(x, y), x, _error.DerivativeByX(p));
    }

    [Fact]
    public void DerivativeByY()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual(_error.Value(x, y), y, _error.DerivativeByY(p));
    }

    [Fact]
    public void DerivativeByR()
    {
      var cartesianPoint = new RectangularCoordinate(x, y);
      var polarPoint = new PolarCoordinate(r, φ);
      var valueInPolar = _error.Value(x, y).ToPolar(cartesianPoint, polarPoint);
      AreDerivativesEqual(valueInPolar, r, _error.DerivativeByR(polarPoint));
    }

    [Fact]
    public void DerivativeByΦ()
    {
      var cartesianPoint = new RectangularCoordinate(x, y);
      var polarPoint = new PolarCoordinate(r, φ);
      var valueInPolar = _error.Value(x, y).ToPolar(cartesianPoint, polarPoint);
      AreDerivativesEqual(valueInPolar, φ, _error.DerivativeByΦ(polarPoint));
    }
  }
}
