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
      Assert.Equal(3, hc.R);
      Assert.Equal(1, hc.Angles.Single());      
    }    

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, 1, 2);       

      Assert.Equal(3u, hc.DimensionsCount);      
      Assert.Equal(3, hc.R);
      Assert.Equal(1, hc.Angles.ElementAt(0));
      Assert.Equal(2, hc.Angles.ElementAt(1));
    }

    [Fact]
    public void Cast_FromCartesian()
    {
      var rc = new RectangularCoordianate(2, 3);
      CartesianCoordinate cc = rc;

      var pc = rc.ToPolar();
      var sc = cc.ToSpherical();

      Assert.Equal(2u, sc.DimensionsCount);
      Assert.Equal(pc.R, sc.R);
      Assert.Equal(pc.Φ, sc.Angles.Single());
    }
  }
}
