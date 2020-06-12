using Arnible.MathModeling.Algebra;
using System;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class INumberRangeDomainTests
  {
    private readonly INumberRangeDomain _range = new NumberRangeDomain(0, 1);

    [Fact]
    public void HypersphericalCoordinate_IsValidTranslation_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      IsTrue(_range.IsValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_IsValidTranslation_false()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      IsFalse(_range.IsValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfRightAngle);
      AreEqual(expected, _range.GetValidTranslation(hc, expected));
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_fullRadius_cutAngle()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfRightAngle);
      AreEqual(expected, _range.GetValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_GetValidTranslation_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      HypersphericalCoordinate hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfRightAngle));
      var expected = new HypersphericalAngleTranslationVector(Angle.HalfCycle / 2 / 6);
      AreEqual(expected, _range.GetValidTranslation(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_ok()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      AreEqual(new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.RightAngle)), _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_fullRadius_cutAngle()
    {
      HypersphericalCoordinate hc = new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.HalfRightAngle));
      AreEqual(new HypersphericalCoordinate(1, new HypersphericalAngleVector(Angle.RightAngle)), _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.RightAngle)));
    }

    [Fact]
    public void HypersphericalCoordinate_Translate_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      HypersphericalCoordinate hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfRightAngle));
      AreEqual(new HypersphericalCoordinate(r, new HypersphericalAngleVector(Angle.HalfCycle / 3)), _range.Translate(hc, new HypersphericalAngleTranslationVector(Angle.HalfRightAngle)));
    }
  }
}
