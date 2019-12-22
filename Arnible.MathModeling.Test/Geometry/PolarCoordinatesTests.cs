using Arnible.MathModeling.Geometry;
using System;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class PolarCoordinatesTests
  {
    [Fact]
    public void Constructor_Default()
    {
      RectangularCoordianate rc = default;
      PolarCoordinate pc = rc.ToPolar();

      Assert.Equal(0, pc.R);
      Assert.Equal(0, pc.Φ);
    }

    [Theory]
    [InlineData(1, 0, 1, 0)]
    [InlineData(0, 1, 1, Math.PI / 2)]
    [InlineData(1, 1, 1.4142135623731, Math.PI / 4)]
    public void Constructor_Cases(double x, double y, double r, double φ)
    {      
      PolarCoordinate pc = (new RectangularCoordianate(x, y)).ToPolar();

      Assert.Equal(r, pc.R, 10);
      Assert.Equal(φ, pc.Φ, 10);
    }    
  }
}
