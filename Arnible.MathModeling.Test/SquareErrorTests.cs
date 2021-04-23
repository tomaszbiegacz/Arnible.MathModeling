using Arnible.Assertions;
using Arnible.MathModeling.Geometry;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class SquareErrorTests
  {
    private const double Sqrt2 = 1.41421356237;    

    private readonly SquareError _error = new SquareError();

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, -1, 4)]
    [InlineData(-1, 1, 4)]
    public void Value(double x, double y, double expected)
    {
      IsEqualToExtensions.AssertIsEqualTo(expected, _error.Value(x, y));
    }

    [Theory]
    [InlineData(1, 1, 0, 2)]
    [InlineData(1, -1, 4, 2)]
    [InlineData(-1, 1, -4, 2)]
    public void DerivativeByX(double x, double y, double first, double second)
    {
      var derivative = _error.DerivativeByX(new RectangularCoordinate(x, y));
      IsEqualToExtensions.AssertIsEqualTo(first, derivative.First);
      IsEqualToExtensions.AssertIsEqualTo(second, derivative.Second);
    }

    [Theory]
    [InlineData(1, 1, 0, 2)]
    [InlineData(1, -1, -4, 2)]
    [InlineData(-1, 1, 4, 2)]
    public void DerivativeByY(double x, double y, double first, double second)
    {
      var derivative = _error.DerivativeByY(new RectangularCoordinate(x, y));
      IsEqualToExtensions.AssertIsEqualTo(first, derivative.First);
      IsEqualToExtensions.AssertIsEqualTo(second, derivative.Second);
    }    

    [Theory]
    [InlineData(1, 1, 0, 0)]
    [InlineData(1, -1, 4 * Sqrt2, 4)]
    [InlineData(-1, 1, 4 * Sqrt2, 4)]
    public void DerivativeByR(double x, double y, double first, double second)
    {
      PolarCoordinate p = new RectangularCoordinate(x, y).ToPolar();
      var derivative = _error.DerivativeByR(p);
      IsEqualToExtensions.AssertIsEqualTo(first, derivative.First);
      IsEqualToExtensions.AssertIsEqualTo(second, derivative.Second);
    }

    [Theory]
    [InlineData(1, 1, 0, 8)]
    [InlineData(1, -1, 0, -8)]
    [InlineData(-1, 1, 0, -8)]
    public void DerivativeByΦ(double x, double y, double first, double second)
    {
      PolarCoordinate p = new RectangularCoordinate(x, y).ToPolar();
      var derivative = _error.DerivativeByΦ(p);
      IsEqualToExtensions.AssertIsEqualTo(first, derivative.First);
      IsEqualToExtensions.AssertIsEqualTo(second, derivative.Second);
    }
  }
}
