using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalDirectionOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalDirectionOnRectangularView(ratioX: 0.3, ratioY: -0.4);

      AreExactlyEqual(0.3, p.RatioX);
      AreExactlyEqual(-0.4, p.RatioY);
    }
  }
}
