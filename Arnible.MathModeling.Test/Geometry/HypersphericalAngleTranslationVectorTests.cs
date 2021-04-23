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
      EqualExtensions.AssertEqualTo(1u, v.Length);
      EqualExtensions.AssertEqualTo<double>(0, (double)v[0]);
      EqualExtensions.AssertEqualTo("0", v.ToString());

      EqualExtensions.AssertEqualTo(default, v);
      EqualExtensions.AssertEqualTo(default, new HypersphericalAngleTranslationVector());
      EqualExtensions.AssertEqualTo(default, new HypersphericalAngleTranslationVector(new Number[0]));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleTranslationVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      EqualExtensions.AssertEqualTo<double>(2, (double)v[0]);
      EqualExtensions.AssertEqualTo(1u, v.Length);
      EqualExtensions.AssertEqualTo("2", v.ToString());
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleTranslationVector v = new HypersphericalAngleTranslationVector(2, 1, -1);
      ConditionExtensions.AssertIsFalse(v == 0);
      EqualExtensions.AssertEqualTo(3u, v.Length);
      EqualExtensions.AssertEqualTo("[2 1 -1]", v.ToString());
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, 0), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, Angle.HalfRightAngle), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, -1 * Angle.HalfRightAngle), t.Translate(v));
    }
  }
}
