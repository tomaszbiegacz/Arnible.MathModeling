using Arnible.MathModelling.xunit;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalDirectionOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalDirectionOnRectangularView(ratioX: 0.3, ratioY: -0.4);
      
      AssertNumber.EqualExact(0.3, p.RatioX);
      AssertNumber.EqualExact(-0.4, p.RatioY);
    }
  }
}
