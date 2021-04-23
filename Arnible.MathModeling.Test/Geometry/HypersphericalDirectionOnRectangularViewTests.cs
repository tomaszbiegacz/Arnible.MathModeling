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

      EqualExtensions.AssertEqualTo<double>(0.3, (double)p.RatioX);
      EqualExtensions.AssertEqualTo<double>(-0.4, (double)p.RatioY);
    }
  }
}
