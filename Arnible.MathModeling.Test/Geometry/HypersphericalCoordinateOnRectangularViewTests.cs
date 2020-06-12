using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnRectangularViewTests
  {
    [Fact]
    public void FromCartesian()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromCartesian(r: 2, x: 0.6, y: -0.8);

      AreExactlyEqual(2, p.R);
      AreEqual(0.3, p.RatioX);
      AreEqual(-0.4, p.RatioY);

      AreExactlyEqual(0.6, p.X);
      AreExactlyEqual(-0.8, p.Y);
    }

    [Fact]
    public void FromRatios()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromRatios(r: 2, ratioX: 0.6, ratioY: -0.8);

      AreExactlyEqual(2, p.R);
      AreExactlyEqual(0.6, p.RatioX);
      AreExactlyEqual(-0.8, p.RatioY);

      AreEqual(1.2, p.X);
      AreEqual(-1.6, p.Y);
    }
  }
}
