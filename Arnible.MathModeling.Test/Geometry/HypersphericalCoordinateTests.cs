using Arnible.MathModeling.Geometry;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class HypersphericalCoordinateTests
  {
    [Fact]
    public void Cast_PolarCoordinates()
    {
      var pc = new PolarCoordinate(3, 1);
      HypersphericalCoordinate hc = pc;

      Assert.Equal(2u, hc.DimensionsCount);
      AssertNumber.Equal(3, hc.R);
      AssertNumber.Equal(1, hc.Angles.Single());
    }

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, new Number[] { 1, 2 });

      Assert.Equal(3u, hc.DimensionsCount);
      AssertNumber.Equal(3, hc.R);
      AssertNumber.Equal(new[] { 1, 2 }, hc.Angles);
    }    
  }
}
