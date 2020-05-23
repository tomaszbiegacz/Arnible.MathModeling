using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalDirectionOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalDirectionOnRectangularView(ratioX: 0.3, ratioY: -0.4);
      
      Assert.Equal<double>(0.3, p.RatioX);
      Assert.Equal<double>(-0.4, p.RatioY);
    }
  }
}
