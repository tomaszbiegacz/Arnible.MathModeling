using Arnible.Assertions;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordianteOnLineViewTests
  {
    [Fact]
    public void FromCartesian()
    {
      var p = new HypersphericalCoordianteOnLineView(r: 2, ratioX: 0.3);

      EqualExtensions.AssertEqualTo<double>(2, (double)p.R);
      EqualExtensions.AssertEqualTo<double>(0.3, (double)p.RatioX);

      EqualExtensions.AssertEqualTo(0.6, p.X);
    }
  }
}
