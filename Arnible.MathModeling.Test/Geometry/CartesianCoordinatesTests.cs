using Arnible.MathModeling.Algebra;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CartesianCoordinatesTests
  {
    [Fact]
    public void Cast_RectangularCoordinates()
    {
      var rc = new RectangularCoordianate(3, 4);
      CartesianCoordinate cc = rc;

      AreEqual(2u, cc.DimensionsCount);
      AreEqual(2u, cc.Coordinates.Count());
      AreExactlyEqual(3, cc.Coordinates[0]);
      AreExactlyEqual(4, cc.Coordinates[1]);
    }

    [Fact]
    public void Constructor_3d()
    {
      var cc = new CartesianCoordinate(new NumberVector(2, 3, 4));

      AreEqual(3u, cc.DimensionsCount);
      AreExactlyEqual(2, cc.Coordinates[0]);
      AreExactlyEqual(3, cc.Coordinates[1]);
      AreExactlyEqual(4, cc.Coordinates[2]);
    }

    [Fact]
    public void Equal_Rounding()
    {
      var v1 = new CartesianCoordinate(new NumberVector(1, 1, 0));
      var v2 = new CartesianCoordinate(new NumberVector(1, 1, 8.65956056235496E-17));
      AreEqual(v1, v2);
    }
  }
}
