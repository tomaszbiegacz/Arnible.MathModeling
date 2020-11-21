using Arnible.MathModeling.Geometry;
using Arnible.MathModeling.Polynomials;
using Xunit;
using static Arnible.MathModeling.Polynomials.Term;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class SquareErrorTests
  {
    private readonly SquareError _error = new SquareError();

    [Fact]
    public void Value()
    {
      AreEqual((x - y).ToPower(2), _error.Value(x, y));
    }

    [Fact]
    public void DerivativeByX()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual((PolynomialDivision)_error.Value(x, y), x, _error.DerivativeByX(p));
    }

    [Fact]
    public void DerivativeByY()
    {
      var p = new RectangularCoordinate(x, y);
      AreDerivativesEqual((PolynomialDivision)_error.Value(x, y), y, _error.DerivativeByY(p));
    }

    [Fact]
    public void DerivativeByR()
    {
      var cartesianPoint = new RectangularCoordinate(x, y);
      var polarPoint = new PolarCoordinate(r, φ);
      var valueInPolar = _error.Value(x, y).ToPolar(cartesianPoint, polarPoint);
      AreDerivativesEqual((PolynomialDivision)valueInPolar, r, _error.DerivativeByR(polarPoint));
    }

    [Fact]
    public void DerivativeByΦ()
    {
      var cartesianPoint = new RectangularCoordinate(x, y);
      var polarPoint = new PolarCoordinate(r, φ);
      var valueInPolar = _error.Value(x, y).ToPolar(cartesianPoint, polarPoint);
      AreDerivativesEqual((PolynomialDivision)valueInPolar, φ, _error.DerivativeByΦ(polarPoint));
    }
  }
}
