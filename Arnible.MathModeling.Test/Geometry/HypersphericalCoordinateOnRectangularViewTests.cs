using Arnible.Assertions;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnRectangularViewTests
  {
    [Fact]
    public void FromCartesian()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromCartesian(r: 2, x: 0.6, y: -0.8);

      EqualExtensions.AssertEqualTo<double>(2, (double)p.R);
      EqualExtensions.AssertEqualTo(0.3, p.RatioX);
      EqualExtensions.AssertEqualTo(-0.4, p.RatioY);

      EqualExtensions.AssertEqualTo<double>(0.6, (double)p.X);
      EqualExtensions.AssertEqualTo<double>(-0.8, (double)p.Y);
    }

    [Fact]
    public void FromRatios()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromRatios(r: 2, ratioX: 0.6, ratioY: -0.8);

      EqualExtensions.AssertEqualTo<double>(2, (double)p.R);
      EqualExtensions.AssertEqualTo<double>(0.6, (double)p.RatioX);
      EqualExtensions.AssertEqualTo<double>(-0.8, (double)p.RatioY);

      EqualExtensions.AssertEqualTo(1.2, p.X);
      EqualExtensions.AssertEqualTo(-1.6, p.Y);
    }
  }
}
