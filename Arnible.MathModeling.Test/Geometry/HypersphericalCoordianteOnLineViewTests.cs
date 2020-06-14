using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordianteOnLineViewTests
  {
    [Fact]
    public void FromCartesian()
    {
      var p = new HypersphericalCoordianteOnLineView(r: 2, ratioX: 0.3);

      AreExactlyEqual(2, p.R);
      AreExactlyEqual(0.3, p.RatioX);

      AreEqual(0.6, p.X);
    }
  }
}
