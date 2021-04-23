using Arnible.Assertions;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalAngleTranslationVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      HypersphericalAngleTranslationVector v = default;
      ConditionExtensions.AssertIsTrue(v == 0);
      IsEqualToExtensions.AssertIsEqualTo(1u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)v[0]);
      IsEqualToExtensions.AssertIsEqualTo("0", v.ToString());

      IsEqualToExtensions.AssertIsEqualTo(default, v);
      IsEqualToExtensions.AssertIsEqualTo(default, new HypersphericalAngleTranslationVector());
      IsEqualToExtensions.AssertIsEqualTo(default, new HypersphericalAngleTranslationVector(new Number[0]));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleTranslationVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)v[0]);
      IsEqualToExtensions.AssertIsEqualTo(1u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo("2", v.ToString());
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleTranslationVector v = new HypersphericalAngleTranslationVector(2, 1, -1);
      ConditionExtensions.AssertIsFalse(v == 0);
      IsEqualToExtensions.AssertIsEqualTo(3u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo("[2 1 -1]", v.ToString());
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, 0), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, Angle.HalfRightAngle), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, -1 * Angle.HalfRightAngle), t.Translate(v));
    }
  }
}
