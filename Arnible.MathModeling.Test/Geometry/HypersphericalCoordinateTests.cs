using Arnible.MathModeling.xunit;
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
      var hc = new HypersphericalCoordinate(3, new HypersphericalAngleVector(1, 0.5));

      Assert.Equal(3u, hc.DimensionsCount);
      AssertNumber.Equal(3, hc.R);
      AssertNumber.Equal(new[] { 1, 0.5 }, hc.Angles);
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      Assert.Equal(
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 0)),
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 8.65956056235496E-17)));
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      Assert.Equal(2u, hc.DimensionsCount);
      Assert.Equal(2, hc.R);

      const double φ = Math.PI / 3;                           // x to r
      AssertNumber.Equal(φ, hc.Angles.Single());

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(2, derrivatives.Length);
      AssertNumber.Equal(0.5, derrivatives[0].First);                 // x
      AssertNumber.Equal(Math.Sqrt(3) / 2, derrivatives[1].First);    // y

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
        Assert.Equal(hc.R, axisCc.Coordinates[pos]);
        Assert.Equal(1u, axisCc.Coordinates.Where(v => v != 0).Count());
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(2), 2 * Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      Assert.Equal(3u, hc.DimensionsCount);
      Assert.Equal(cc.VectorLength(), hc.R);

      double φ = hc.Angles[0];    // r to y
      double θ = hc.Angles[1];    // r to xy

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(3, derrivatives.Length);
      AssertNumber.Equal(Math.Cos(θ) * Math.Cos(φ), derrivatives[0].First);   // x
      AssertNumber.Equal(Math.Cos(θ) * Math.Sin(φ), derrivatives[1].First);   // y
      AssertNumber.Equal(Math.Sin(θ), derrivatives[2].First);                 // z

      Assert.Equal(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      HypersphericalCoordinate hc = cc.ToSphericalView();
      Assert.Equal(3u, hc.DimensionsCount);
      Assert.Equal(4, hc.R);
      AssertNumber.Equal(φ, hc.Angles[0]);
      AssertNumber.Equal(θ, hc.Angles[1]);

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      Assert.Equal(3, derrivatives.Length);
      AssertNumber.Equal(Math.Sqrt(2) / 4, derrivatives[0].First);      // x
      AssertNumber.Equal(Math.Sqrt(2) / 4, derrivatives[1].First);      // y
      AssertNumber.Equal(Math.Sqrt(3) / 2, derrivatives[2].First);      // z

      Assert.Equal(cc, hc.ToCartesianView());
    }

    [Fact]
    public void TranslateByAngle()
    {
      HypersphericalCoordinate coordinate = new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1, -1));
      Assert.Equal(new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1.5, -1)), coordinate.TranslateByAngle(1, 0.5));
    }
  }
}
