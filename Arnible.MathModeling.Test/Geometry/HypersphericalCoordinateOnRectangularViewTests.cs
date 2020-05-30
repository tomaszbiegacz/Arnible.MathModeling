using Arnible.MathModeling.xunit;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnRectangularViewTests
  {
    [Fact]
    public void HappyDay()
    {
      var p = new HypersphericalCoordinateOnRectangularView(r: 2, ratioX: 0.3, ratioY: -0.4);

      AssertNumber.EqualExact(2, p.R);
      AssertNumber.EqualExact(0.3, p.RatioX);
      AssertNumber.EqualExact(-0.4, p.RatioY);

      AssertNumber.EqualExact(0.6, p.X);
      AssertNumber.EqualExact(-0.8, p.Y);
    }
  }
}
