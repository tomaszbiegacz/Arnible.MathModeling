using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Analysis;
using Arnible.MathModeling.Test;
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

      EqualExtensions.AssertEqualTo(2u, hc.DimensionsCount);
      EqualExtensions.AssertEqualTo<double>(3, (double)hc.R);
      EqualExtensions.AssertEqualTo<double>(1, (double)hc.Angles.Single());
    }

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, new HypersphericalAngleVector(1, 0.5));

      EqualExtensions.AssertEqualTo(3u, hc.DimensionsCount);
      EqualExtensions.AssertEqualTo<double>(3d, (double)hc.R);
      hc.Angles.GetInternalEnumerable().AssertSequenceEqual(new Number[] { 1, 0.5 });
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      EqualExtensions.AssertEqualTo(
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 0)),
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 8.65956056235496E-17)));
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      EqualExtensions.AssertEqualTo(2u, hc.DimensionsCount);
      EqualExtensions.AssertEqualTo(2d, hc.R);

      const double φ = Math.PI / 3;                           // x to r
      EqualExtensions.AssertEqualTo(φ, hc.Angles.Single());

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      EqualExtensions.AssertEqualTo(2, derrivatives.Length);
      EqualExtensions.AssertEqualTo(0.5, derrivatives[0].First);                 // x
      EqualExtensions.AssertEqualTo(Math.Sqrt(3) / 2, derrivatives[1].First);    // y

      EqualExtensions.AssertEqualTo(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    private static void VerifyCartesianCoordinateAngle(HypersphericalCoordinate hc, CartesianCoordinate cc)
    {
      var cartesianCoordinatesAngles = hc.ToCartesianView().CartesianCoordinatesAngles().ToArray();
      EqualExtensions.AssertEqualTo(cartesianCoordinatesAngles.Length, cc.DimensionsCount);

      for (ushort pos = 0; pos < cc.DimensionsCount; ++pos)
      {
        var axisCc = new HypersphericalCoordinate(hc.R, cartesianCoordinatesAngles[pos]).ToCartesianView();
        EqualExtensions.AssertEqualTo(hc.R, axisCc.Coordinates[pos]);
        // ReSharper disable once HeapView.BoxingAllocation
        EqualExtensions.AssertEqualTo(1u, axisCc.Coordinates.GetInternalEnumerable().Count(v => v != 0));
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(2), 2 * Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      EqualExtensions.AssertEqualTo(3u, hc.DimensionsCount);
      EqualExtensions.AssertEqualTo(cc.VectorLength(), hc.R);

      double φ = (double)hc.Angles[0];    // r to y
      double θ = (double)hc.Angles[1];    // r to xy

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      EqualExtensions.AssertEqualTo(3, derrivatives.Length);
      EqualExtensions.AssertEqualTo(Math.Cos(θ) * Math.Cos(φ), derrivatives[0].First);   // x
      EqualExtensions.AssertEqualTo(Math.Cos(θ) * Math.Sin(φ), derrivatives[1].First);   // y
      EqualExtensions.AssertEqualTo(Math.Sin(θ), derrivatives[2].First);                 // z

      EqualExtensions.AssertEqualTo(cc, hc.ToCartesianView());
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      HypersphericalCoordinate hc = cc.ToSphericalView();
      EqualExtensions.AssertEqualTo(3u, hc.DimensionsCount);
      EqualExtensions.AssertEqualTo(4d, hc.R);
      EqualExtensions.AssertEqualTo(φ, hc.Angles[0]);
      EqualExtensions.AssertEqualTo(θ, hc.Angles[1]);

      Derivative1Value[] derivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      EqualExtensions.AssertEqualTo(3, derivatives.Length);
      EqualExtensions.AssertEqualTo(Math.Sqrt(2) / 4, derivatives[0].First);      // x
      EqualExtensions.AssertEqualTo(Math.Sqrt(2) / 4, derivatives[1].First);      // y
      EqualExtensions.AssertEqualTo(Math.Sqrt(3) / 2, derivatives[2].First);      // z

      EqualExtensions.AssertEqualTo(cc, hc.ToCartesianView());
    }

    [Fact]
    public void TranslateByAngle()
    {
      HypersphericalCoordinate coordinate = new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1, -1));
      EqualExtensions.AssertEqualTo(new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1.5, -1)), coordinate.TranslateByAngle(1, 0.5));
    }
  }
}
