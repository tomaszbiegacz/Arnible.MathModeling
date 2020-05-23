using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalCoordinateOnRectangularView(r: 2, ratioX: 0.3, ratioY: -0.4);
      
      Assert.Equal<double>(2, p.R);
      Assert.Equal<double>(0.3, p.RatioX);
      Assert.Equal<double>(-0.4, p.RatioY);

      Assert.Equal<double>(0.6, p.X);
      Assert.Equal<double>(-0.8, p.Y);
    }
  }
}
