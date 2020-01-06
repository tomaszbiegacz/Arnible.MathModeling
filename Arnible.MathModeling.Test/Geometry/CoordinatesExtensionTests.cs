using Arnible.MathModeling.Geometry;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class CoordinatesExtensionTests
  {
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    [InlineData(-3, 2)]
    [InlineData(3, -2)]
    public void Cast_HyperspehricalEqualsRectangular(double x, double y)
    {
      var rc = new RectangularCoordianate(x, y);
      CartesianCoordinate cc = rc;

      var pc = rc.ToPolar();
      var sc = cc.ToSpherical();

      Assert.Equal(2u, sc.DimensionsCount);
      AssertNumber.Equal(pc.R, sc.R);
      AssertNumber.Equal(pc.Φ, sc.Angles.Single());
    }

    [Theory]
    [InlineData(new double[] { 1, 1 }, TestsConst.Sqrt2, new[] { TestsConst.π_4 })]
    [InlineData(
      new double[] { TestsConst.one_Sqrt2, TestsConst.one_Sqrt2, 1 },
      TestsConst.Sqrt2, new[] { TestsConst.π_4, TestsConst.π_4 })]
    [InlineData(
      new double[] { TestsConst.Sqrt2, TestsConst.Sqrt2, 2 * TestsConst.Sqrt3 },
      4, new[] { TestsConst.π_4, TestsConst.π_6 })]
    public void Cast_ToHyperspherical(double[] cartesian, double r, double[] angles)
    {
      var cc = new CartesianCoordinate(cartesian.Select(c => (Number)c).ToArray());

      var sc = cc.ToSpherical();
      AssertNumber.Equal(r, sc.R);
      AssertNumber.Equal(angles, sc.Angles);
    }

    [Theory]
    [InlineData(new double[] { 1, 1 }, TestsConst.Sqrt2, new[] { TestsConst.π_4 })]
    [InlineData(
      new double[] { TestsConst.one_Sqrt2, TestsConst.one_Sqrt2, 1 },
      TestsConst.Sqrt2, new[] { TestsConst.π_4, TestsConst.π_4 })]
    [InlineData(
      new double[] { TestsConst.Sqrt2, TestsConst.Sqrt2, 2 * TestsConst.Sqrt3 },
      4, new[] { TestsConst.π_4, TestsConst.π_6 })]
    public void Cast_ToCartesian(double[] cartesian, double r, double[] angles)
    {
      var sc = new HypersphericalCoordinate(r, angles.Select(c => (Number)c).ToArray());

      var cc = sc.ToCartesian();
      AssertNumber.Equal(cartesian, cc.Coordinates);
    }
  }
}
