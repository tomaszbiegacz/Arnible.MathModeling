using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalAngleTranslationVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      HypersphericalAngleTranslationVector v = default;
      Assert.True(v == 0);
      Assert.Equal(1u, v.Length);
      Assert.Equal(0, v[0]);
      Assert.Equal("0", v.ToString());

      Assert.Equal(default, v);
      Assert.Equal(default, new HypersphericalAngleTranslationVector());
      Assert.Equal(default, new HypersphericalAngleTranslationVector(new Number[0]));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleTranslationVector v = 2;
      Assert.False(v == 0);
      Assert.True(v == 2);
      Assert.False(v != 2);
      Assert.Equal(2, v[0]);
      Assert.Equal(1u, v.Length);
      Assert.Equal("2", v.ToString());
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleTranslationVector v = new HypersphericalAngleTranslationVector(2, 1, -1);
      Assert.False(v == 0);
      Assert.Equal(v, new Number[] { 2, 1, -1 });
      Assert.Equal(3u, v.Length);
      Assert.Equal("[2 1 -1]", v.ToString());
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      Assert.Equal(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, 0), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1, Angle.HalfRightAngle);
      Assert.Equal(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, Angle.HalfRightAngle), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new HypersphericalAngleTranslationVector(Angle.HalfCycle, 0, -1 * Angle.HalfRightAngle);
      var v = new HypersphericalAngleVector(Angle.RightAngle, 1);
      Assert.Equal(new HypersphericalAngleVector(-1 * Angle.RightAngle, 1, -1 * Angle.HalfRightAngle), t.Translate(v));
    }
  }
}
