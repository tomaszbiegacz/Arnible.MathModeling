using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
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
      hc.Angles.Span.Single().AssertIsEqualTo(1);
    }

    [Fact]
    public void Constructor_3d()
    {
      var hc = new HypersphericalCoordinate(3, new Number[] {1, 0.5});

      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(3d);
      hc.Angles.Span.AssertSequenceEqualsTo(new Number[] { 1, 0.5 });
    }

    [Fact]
    public void Equal_Rounding_Angle()
    {
      new HypersphericalCoordinate(2, new Number[] {1, 8.65956056235496E-17}).AssertIsEqualTo(
        new HypersphericalCoordinate(2, new Number[] {1, 0})
        );
    }

    [Fact]
    public void RectangularToPolarTransformation()
    {
      Span<Number> cc = new Number[] { 1, Math.Sqrt(3) };

      Span<Number> hcBuffer = new Number[1];
      HypersphericalCoordinate hc = cc.ToSpherical(in hcBuffer);
      hc.DimensionsCount.AssertIsEqualTo(2);
      hc.R.AssertIsEqualTo(2d);

      const double φ = Math.PI / 3;                           // x to r
      hc.Angles.Span.Single().AssertIsEqualTo(φ);

      Span<Number> cv = stackalloc Number[2];
      hc.ToCartesian(in cv); 
      Span<Number> derivatives = stackalloc Number[2];
      hc.DerivativeByR(in derivatives);
      derivatives[0].AssertIsEqualTo(0.5);                 // x
      derivatives[1].AssertIsEqualTo(Math.Sqrt(3) / 2);    // y

      cv.AssertSequenceEqualsTo(cc);
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    private static void VerifyCartesianCoordinateAngle(HypersphericalCoordinate hc, ReadOnlySpan<Number> cc)
    {
      Span<Number> hcBuffer = new Number[hc.Angles.Length];
      Span<Number> axisCc = new Number[cc.Length];

      for (ushort pos = 0; pos < cc.Length; ++pos)
      {
        var cartesianCoordinatesAngles = HypersphericalCoordinate.CartesianCoordinatesAngle(pos, in hcBuffer);
        new HypersphericalCoordinate(hc.R, cartesianCoordinatesAngles).ToCartesian(in axisCc);
        axisCc[pos].AssertIsEqualTo(hc.R);
        axisCc.IsOrthogonal().AssertIsTrue();
      }
    }

    [Fact]
    public void CubeToSphericalTransformation()
    {
      Span<Number> cc = new Number[] { 1, Math.Sqrt(2), 2 * Math.Sqrt(3) };

      Span<Number> hcBuffer = new Number[2];
      HypersphericalCoordinate hc = cc.ToSpherical(hcBuffer);
      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(cc.GetVectorLength());

      double φ = (double)hc.Angles.Span[0];    // r to y
      double θ = (double)hc.Angles.Span[1];    // r to xy

      Span<Number> derivatives = stackalloc Number[3];
      hc.DerivativeByR(in derivatives);
      derivatives[0].AssertIsEqualTo(Math.Cos(θ) * Math.Cos(φ));   // x
      derivatives[1].AssertIsEqualTo(Math.Cos(θ) * Math.Sin(φ));   // y
      derivatives[2].AssertIsEqualTo(Math.Sin(θ));                 // z

      Span<Number> cv = stackalloc Number[3];
      hc.ToCartesian(in cv);
      cv.AssertSequenceEqualsTo(cc);
      VerifyCartesianCoordinateAngle(hc, cc);
    }

    [Fact]
    public void CubeToSphericalTransformation_Known()
    {
      Span<Number> cc = new Number[] { Math.Sqrt(2), Math.Sqrt(2), 2 * Math.Sqrt(3) };

      const double φ = Math.PI / 4;   // x to r(xy)
      const double θ = Math.PI / 3;   // xy to r

      Span<Number> hcBuffer = new Number[2];
      HypersphericalCoordinate hc = cc.ToSpherical(hcBuffer);
      hc.DimensionsCount.AssertIsEqualTo(3);
      hc.R.AssertIsEqualTo(4d);
      hc.Angles.Span[0].AssertIsEqualTo(φ);
      hc.Angles.Span[1].AssertIsEqualTo(θ);

      Span<Number> derivatives = stackalloc Number[3];
      hc.DerivativeByR(in derivatives);
      derivatives[0].AssertIsEqualTo(Math.Sqrt(2) / 4);      // x
      derivatives[1].AssertIsEqualTo(Math.Sqrt(2) / 4);      // y
      derivatives[2].AssertIsEqualTo(Math.Sqrt(3) / 2);      // z

      Span<Number> cv = new Number[3];
      hc.ToCartesian(in cv);
      cv.AssertSequenceEqualsTo(cc);
    }

    [Fact]
    public void TranslateByAngle()
    {
      Span<Number> buffer = new Number[3];
      HypersphericalCoordinate coordinate = new HypersphericalCoordinate(2, new Number[] {2, 1, -1});
      coordinate.TranslateSelf(HypersphericalAngleVector.CreateOrthogonalDirection(1, 0.5, in buffer));
      coordinate.AssertIsEqualTo(new HypersphericalCoordinate(2, new Number[] {2, 1.5, -1}));
    }
    
    [Fact]
    public void ConversationCircle()
    {
      Span<Number> cc = new Number[] {1, 2, 3, 4};

      Span<Number> hcBuffer = stackalloc Number[3];
      HypersphericalCoordinate hc = cc.ToSpherical(in hcBuffer);
      hc.DimensionsCount.AssertIsEqualTo(cc.Length);

      Span<Number> cc2 = stackalloc Number[4];
      hc.ToCartesian(in cc2);
      
      cc2.AssertSequenceEqualsTo(cc);
    }

    [Theory]
    [InlineData(2u)]
    [InlineData(3u)]
    [InlineData(4u)]
    [InlineData(5u)]
    [InlineData(6u)]
    [InlineData(7u)]
    [InlineData(8u)]
    public void GetIdentityVector(ushort dimensionsCount)
    {
      Span<Number> vector = stackalloc Number[dimensionsCount];
      CartesianCoordinate.GetIdentityVector(in vector);
      
      vector.SumDefensive((in Number v) => v * v).AssertIsEqualTo(1d);
      
      Span<Number> h1 = stackalloc Number[dimensionsCount - 1];
      Span<Number> v2 = stackalloc Number[dimensionsCount];
      HypersphericalAngleVector.GetIdentityVector(h1).GetCartesianAxisViewsRatios(in v2);
      
      v2.AssertSequenceEqualsTo(vector);
    }
  }
}
