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
      v.Length.AssertIsEqualTo(1);
      v[0].AssertIsEqualTo(0);
      v.ToString().AssertIsEqualTo("0");

      v.AssertIsEqualTo(default);
      new HypersphericalAngleTranslationVector().AssertIsEqualTo(default);
      new HypersphericalAngleTranslationVector(new Number[0]).AssertIsEqualTo(default);
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleTranslationVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      v[0].AssertIsEqualTo(2);
      v.Length.AssertIsEqualTo(1);
      v.ToString().AssertIsEqualTo("2");
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleTranslationVector v = new HypersphericalAngleTranslationVector(2, 1, -1);
      ConditionExtensions.AssertIsFalse(v == 0);
      v.Length.AssertIsEqualTo(3);
      v.ToString().AssertIsEqualTo("[2 1 -1]");
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      t.Translate(v).AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, 0));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      t.Translate(v).AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, Angle.HalfRightAngle));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1);
      t.Translate(v).AssertIsEqualTo(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, -1 * Angle.HalfRightAngle));
    }
  }
}
