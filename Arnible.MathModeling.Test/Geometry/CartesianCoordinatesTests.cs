using Arnible.MathModeling.Geometry;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class CartesianCoordinatesTests
  {
    [Fact]
    public void Cast_RectangularCoordinates()
    {
      var rc = new RectangularCoordianate(3, 4);
      CartesianCoordinate cc = rc;

      Assert.Equal(2u, cc.DimensionsCount);
      Assert.Equal(2, cc.Coordinates.Count());
      Assert.Equal(3, cc.Coordinates.ElementAt(0));
      Assert.Equal(4, cc.Coordinates.ElementAt(1));
    }

    [Fact]
    public void Constructor_3d()
    {
      var cc = new CartesianCoordinate(2, 3, 4);       

      Assert.Equal(3u, cc.DimensionsCount);      
      Assert.Equal(2, cc.Coordinates.ElementAt(0));
      Assert.Equal(3, cc.Coordinates.ElementAt(1));
      Assert.Equal(4, cc.Coordinates.ElementAt(2));
    }
  }
}
