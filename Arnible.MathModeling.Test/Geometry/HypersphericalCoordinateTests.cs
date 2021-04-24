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

      hc.DimensionsCount.AssertIsEqualTo(2);
      hc.R.AssertIsEqualTo(3);
      hc.Angles.Single().AssertIsEqualTo(1);
    }

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, new HypersphericalAngleVector(1, 0.5));

      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(3d);
      hc.Angles.GetInternalEnumerable().AssertSequenceEqualsTo(new Number[] { 1, 0.5 });
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 8.65956056235496E-17)).AssertIsEqualTo(
        new HypersphericalCoordinate(2, new HypersphericalAngleVector(1, 0))
        );
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      hc.DimensionsCount.AssertIsEqualTo(2);
      hc.R.AssertIsEqualTo(2d);

      const double φ = Math.PI / 3;                           // x to r
      hc.Angles.Single().AssertIsEqualTo(φ);

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      derrivatives.Length.AssertIsEqualTo(2);
      derrivatives[0].First.AssertIsEqualTo(0.5);                 // x
      derrivatives[1].First.AssertIsEqualTo(Math.Sqrt(3) / 2);    // y

      ((CartesianCoordinate)hc.ToCartesianView()).AssertIsEqualTo(cc);
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    private static void VerifyCartesianCoordinateAngle(HypersphericalCoordinate hc, CartesianCoordinate cc)
    {
      var cartesianCoordinatesAngles = hc.ToCartesianView().CartesianCoordinatesAngles().ToArray();
      cc.DimensionsCount.AssertIsEqualTo(cartesianCoordinatesAngles.Length);

      for (ushort pos = 0; pos < cc.DimensionsCount; ++pos)
      {
        var axisCc = new HypersphericalCoordinate(hc.R, cartesianCoordinatesAngles[pos]).ToCartesianView();
        axisCc.Coordinates[pos].AssertIsEqualTo(hc.R);
        axisCc.Coordinates.GetInternalEnumerable().Count(v => v != 0).AssertIsEqualTo(1u);
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      var cc = new CartesianCoordinate(1, Math.Sqrt(2), 2 * Math.Sqrt(3));

      HypersphericalCoordinate hc = cc.ToSphericalView();
      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(cc.VectorLength());

      double φ = (double)hc.Angles[0];    // r to y
      double θ = (double)hc.Angles[1];    // r to xy

      var derrivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      derrivatives.Length.AssertIsEqualTo(3);
      derrivatives[0].First.AssertIsEqualTo(Math.Cos(θ) * Math.Cos(φ));   // x
      derrivatives[1].First.AssertIsEqualTo(Math.Cos(θ) * Math.Sin(φ));   // y
      derrivatives[2].First.AssertIsEqualTo(Math.Sin(θ));                 // z

      ((CartesianCoordinate)hc.ToCartesianView()).AssertIsEqualTo(cc);
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      var cc = new CartesianCoordinate(Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3));

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      HypersphericalCoordinate hc = cc.ToSphericalView();
      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(4d);
      hc.Angles[0].AssertIsEqualTo(φ);
      hc.Angles[1].AssertIsEqualTo(θ);

      Derivative1Value[] derivatives = hc.ToCartesianView().DerivativeByR().ToArray();
      derivatives.Length.AssertIsEqualTo(3);
      derivatives[0].First.AssertIsEqualTo(Math.Sqrt(2) / 4);      // x
      derivatives[1].First.AssertIsEqualTo(Math.Sqrt(2) / 4);      // y
      derivatives[2].First.AssertIsEqualTo(Math.Sqrt(3) / 2);      // z

      ((CartesianCoordinate)hc.ToCartesianView()).AssertIsEqualTo(cc);
    }

    [Fact]
    public void TranslateByAngle()
    {
      HypersphericalCoordinate coordinate = new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1, -1));
      coordinate.TranslateByAngle(1, 0.5).AssertIsEqualTo(new HypersphericalCoordinate(2, new HypersphericalAngleVector(2, 1.5, -1)));
    }
  }
}
