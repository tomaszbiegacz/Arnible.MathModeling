using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Test;
using System;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
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
      var hc = new HypersphericalCoordinate(3, new NumberVector(1, 0.5));

      Assert.Equal(3u, hc.DimensionsCount);
      AssertNumber.Equal(3, hc.R);
      AssertNumber.Equal(new[] { 1, 0.5 }, hc.Angles);
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      Assert.Equal(
        new HypersphericalCoordinate(2, new NumberVector(1, 0)),
        new HypersphericalCoordinate(2, new NumberVector(1, 8.65956056235496E-17)));
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(3));

      var hc = cc.ToSpherical();
      Assert.Equal(2u, hc.DimensionsCount);
      Assert.Equal(2, hc.R);

      const double φ = Math.PI / 3;                           // x to r
      Assert.Equal<Number>(φ, hc.Angles.Single());

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(2, derrivatives.Length);
      Assert.Equal<Number>(0.5, derrivatives[0].First);                 // x
      Assert.Equal<Number>(Math.Sqrt(3) / 2, derrivatives[1].First);    // y

      Assert.Equal(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    private static void VerifyCartesianCoordinateAngle(HypersphericalCoordinate hc, CartesianCoordinate cc)
    {
      var cartesianCoordinatesAngles = hc.ToCartesianView().CartesianCoordinatesAngles().ToArray();
      Assert.Equal((uint)cartesianCoordinatesAngles.Length, cc.DimensionsCount);

      for (uint pos = 0; pos < cc.DimensionsCount; ++pos)
      {
        var axisCc = new HypersphericalCoordinate(hc.R, cartesianCoordinatesAngles[pos]).ToCartesianView();
        Assert.Equal(cc.DimensionsCount, axisCc.DimensionsCount);
        Assert.Equal(hc.R, axisCc.Coordinates[pos]);
        Assert.Equal(1u, axisCc.Coordinates.Where(v => v != 0).Count());
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(2), 2 * Math.Sqrt(3));

      var hc = cc.ToSpherical();
      Assert.Equal(3u, hc.DimensionsCount);
      Assert.Equal(cc.VectorLength(), hc.R);

      double φ = hc.Angles[0];    // r to y
      double θ = hc.Angles[1];    // r to xy

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(3, derrivatives.Length);
      Assert.Equal<Number>(Math.Cos(θ) * Math.Cos(φ), derrivatives[0].First);   // x
      Assert.Equal<Number>(Math.Cos(θ) * Math.Sin(φ), derrivatives[1].First);   // y
      Assert.Equal<Number>(Math.Sin(θ), derrivatives[2].First);                 // z

      Assert.Equal(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      var hc = cc.ToSpherical();
      Assert.Equal(3u, hc.DimensionsCount);
      Assert.Equal(4, hc.R);
      Assert.Equal<Number>(φ, hc.Angles[0]);
      Assert.Equal<Number>(θ, hc.Angles[1]);

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(3, derrivatives.Length);
      Assert.Equal<Number>(Math.Sqrt(2) / 4, derrivatives[0].First);      // x
      Assert.Equal<Number>(Math.Sqrt(2) / 4, derrivatives[1].First);      // y
      Assert.Equal<Number>(Math.Sqrt(3) / 2, derrivatives[2].First);      // z

      Assert.Equal(cc, hc.ToCartesianView());
    }
  }
}
