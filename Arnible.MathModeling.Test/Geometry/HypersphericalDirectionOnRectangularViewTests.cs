using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalDirectionOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalDirectionOnRectangularView(ratioX: 0.3, ratioY: -0.4);

      p.RatioX.AssertIsEqualTo(0.3);
      p.RatioY.AssertIsEqualTo(-0.4);
    }
  }
}
