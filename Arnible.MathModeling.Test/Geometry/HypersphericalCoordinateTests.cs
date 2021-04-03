using System;
using Arnible.Linq;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateTests
  {
    [Fact]
    public void Cast_PolarCoordinates()
    {
      var pc = new PolarCoordinate(3, 1);
      HypersphericalCoordinate hc = pc;

      AreEqual(2u, hc.DimensionsCount);
      AreExactlyEqual(3, hc.R);
      AreExactlyEqual(1, hc.Angles.Single());
    }

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, new HypersphericalAngleVector(1, 0.5));

      AreEqual(3u, hc.DimensionsCount);
      AreExactlyEqual(3d, hc.R);
      AreEquals(new Number[] { 1, 0.5 }, hc.Angles);
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      AreEqual(
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 0)),
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 8.65956056235496E-17)));
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      AreEqual(2u, hc.DimensionsCount);
      AreEqual(2d, hc.R);

      const double φ = Math.PI / 3;                           // x to r
      AreEqual(φ, hc.Angles.Single());

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToValueArray();
      AreEqual(2, derrivatives.Length);
      AreEqual(0.5, derrivatives[0].First);                 // x
      AreEqual(Math.Sqrt(3) / 2, derrivatives[1].First);    // y

      AreEqual(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    private static void VerifyCartesianCoordinateAngle(HypersphericalCoordinate hc, CartesianCoordinate cc)
    {
      var cartesianCoordinatesAngles = hc.ToCartesianView().CartesianCoordinatesAngles().ToValueArray();
      AreEqual(cartesianCoordinatesAngles.Length, cc.DimensionsCount);

      for (uint pos = 0; pos < cc.DimensionsCount; ++pos)
      {
        var axisCc = new HypersphericalCoordinate(hc.R, cartesianCoordinatesAngles[pos]).ToCartesianView();
        AreEqual(hc.R, axisCc.Coordinates[pos]);
        // ReSharper disable once HeapView.BoxingAllocation
        AreEqual(1u, axisCc.Coordinates.Count(v => v != 0));
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(2), 2 * Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      AreEqual(3u, hc.DimensionsCount);
      AreEqual(cc.VectorLength(), hc.R);

      double φ = (double)hc.Angles[0];    // r to y
      double θ = (double)hc.Angles[1];    // r to xy

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToValueArray();
      AreEqual(3, derrivatives.Length);
      AreEqual(Math.Cos(θ) * Math.Cos(φ), derrivatives[0].First);   // x
      AreEqual(Math.Cos(θ) * Math.Sin(φ), derrivatives[1].First);   // y
      AreEqual(Math.Sin(θ), derrivatives[2].First);                 // z

      AreEqual(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      HypersphericalCoordinate hc = cc.ToSphericalView();
      AreEqual(3u, hc.DimensionsCount);
      AreEqual(4d, hc.R);
      AreEqual(φ, hc.Angles[0]);
      AreEqual(θ, hc.Angles[1]);

      ValueArray<Derivative1Value> derrivatives = hc.ToCartesianView().DerivativeByR().ToValueArray();
      AreEqual(3, derrivatives.Length);
      AreEqual(Math.Sqrt(2) / 4, derrivatives[0].First);      // x
      AreEqual(Math.Sqrt(2) / 4, derrivatives[1].First);      // y
      AreEqual(Math.Sqrt(3) / 2, derrivatives[2].First);      // z

      AreEqual(cc, hc.ToCartesianView());
    }

    [Fact]
    public void TranslateByAngle()
    {
      HypersphericalCoordinate coordinate = new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1, -1));
      AreEqual(new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1.5, -1)), coordinate.TranslateByAngle(1, 0.5));
    }
  }
}
