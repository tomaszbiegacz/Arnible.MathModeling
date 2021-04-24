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
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_IsValidTranslation_false()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfRightAngle);
      _range.GetValidTranslation(hc, expected).AssertIsEqualTo(expected);
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_fullRadius_cutAngle()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfRightAngle);
      _range.GetValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)).AssertIsEqualTo(expected);
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      HypersphericalCoordinate hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfCycle / 2 / 6);
      _range.GetValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)).AssertIsEqualTo(expected);
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)).AssertIsEqualTo(new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_fullRadius_cutAngle()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)).AssertIsEqualTo(new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      HypersphericalCoordinate hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfRightAngle));
      _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)).AssertIsEqualTo(new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfCycle / 3)));
    }
  }
}
