using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.xunit;
using System;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CoordinatesExtensionTests
  {
    const double Sqrt2 = 1.4142135623731;
    const double Sqrt3 = 1.7320508075689;

    const double one_Sqrt2 = 1 / Sqrt2;

    /// <summary>
    /// 45 degrees
    /// </summary>
    const double π_4 = Math.PI / 4;

    /// <summary>
    /// 60 degres
    /// </summary>
    const double π_3 = Math.PI / 3;

    /// <summary>
    /// 90 degrees
    /// </summary>
    const double π_2 = Math.PI / 2;

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
      HypersphericalCoordinate sc = cc.ToSphericalView();

      Assert.Equal(2u, sc.DimensionsCount);
      AssertNumber.Equal(pc.R, sc.R);
      AssertNumber.Equal(pc.Φ, sc.Angles.Single());
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void Cast_ToHypersphericalView(double[] cartesian, double r, double[] angles)
    {
      var cc = new CartesianCoordinate(cartesian.ToVector());

      HypersphericalCoordinate sc = cc.ToSphericalView();
      AssertNumber.Equal(r, sc.R);
      AssertNumber.Equal(angles, sc.Angles);
    }

    [Theory]
    [InlineData(new[] { 0d }, 0, new[] { 0d })]
    [InlineData(new[] { 0d, 1d }, 1, new[] { π_2 })]
    [InlineData(new[] { 0d, 0d, 1d }, 1, new[] { 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 1d }, 1, new[] { 0, 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 0, 1d }, 1, new[] { 0, 0, 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 0, -1d }, 1, new[] { 0, 0, 0, -1 * π_2 })]
    public void Cast_ToHyperspherical(double[] cartesian, double r, double[] angles)
    {
      var cc = new CartesianCoordinate(cartesian.ToVector());

      HypersphericalCoordinate sc = cc.ToSpherical();
      AssertNumber.Equal(r, sc.R);
      AssertNumber.Equal(angles, sc.Angles);
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void Cast_ToCartesian(double[] cartesian, double r, double[] angles)
    {
      var sc = new HypersphericalCoordinate(r, angles.ToAngleVector());

      var cc = sc.ToCartesianView();
      AssertNumber.Equal(cartesian, cc.Coordinates);
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void AddDimension(double[] cartesian, double r, double[] angles)
    {
      var cc = new CartesianCoordinate(cartesian.ToVector());
      var sc = new HypersphericalCoordinate(r, angles.ToAngleVector());

      Assert.Equal(cc.AddDimension(), sc.AddDimension().ToCartesianView());
      Assert.Equal(sc.AddDimension(), cc.AddDimension().ToSphericalView());
    }
  }
}
