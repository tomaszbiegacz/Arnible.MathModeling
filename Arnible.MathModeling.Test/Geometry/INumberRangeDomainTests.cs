using Arnible.MathModeling.Algebra;
using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class INumberRangeDomainTests
  {
    private readonly INumberRangeDomain _range = new NumberRangeDomain(0, 1);

    [Fact]
    public void HypersphericalCoordinate_IsValidTranslation_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new Number[] {Angle.HalfRightAngle});
      _range.IsValidTranslation(hc, new Number[] {Angle.HalfRightAngle}).AssertIsTrue();
    }

    [Fact]
    public void HypersphericalCoordinate_IsValidTranslation_false()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new Number[] {Angle.HalfRightAngle});
      _range.IsValidTranslation(hc, new Number[] {Angle.RightAngle}).AssertIsFalse();
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new Number[] {Angle.HalfRightAngle});
      ReadOnlySpan<Number> expected = new Number[] {Angle.HalfRightAngle};
      
      HypersphericalAngleVector current = new Number[] {Angle.HalfRightAngle};
      _range.GetValidTranslationForLastAngle(hc, in current);
      current.Span.AssertSequenceEqualsTo(expected);
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_fullRadius_cutAngle()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new Number[] {Angle.HalfRightAngle});
      ReadOnlySpan<Number> expected = new Number[] {Angle.HalfRightAngle};
      
      HypersphericalAngleVector current = new Number[] {Angle.RightAngle};
      _range.GetValidTranslationForLastAngle(hc, in current);
      current.Span.AssertSequenceEqualsTo(expected);
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      HypersphericalCoordinate hc = new HypersphericalCoordinate(r, new Number[] {Angle.HalfRightAngle});
      ReadOnlySpan<Number> expected = new Number[] {Angle.HalfCycle / 2 / 6};
      
      HypersphericalAngleVector current = new Number[] {Angle.HalfRightAngle};
      _range.GetValidTranslationForLastAngle(hc, in current);
      current.Span.AssertSequenceEqualsTo(expected);
    }
  }
}
