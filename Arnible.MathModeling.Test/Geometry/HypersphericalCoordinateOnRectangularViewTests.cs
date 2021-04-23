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

      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)p.R);
      IsEqualToExtensions.AssertIsEqualTo(0.3, p.RatioX);
      IsEqualToExtensions.AssertIsEqualTo(-0.4, p.RatioY);

      IsEqualToExtensions.AssertIsEqualTo<double>(0.6, (double)p.X);
      IsEqualToExtensions.AssertIsEqualTo<double>(-0.8, (double)p.Y);
    }

    [Fact]
    public void FromRatios()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromRatios(r: 2, ratioX: 0.6, ratioY: -0.8);

      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)p.R);
      IsEqualToExtensions.AssertIsEqualTo<double>(0.6, (double)p.RatioX);
      IsEqualToExtensions.AssertIsEqualTo<double>(-0.8, (double)p.RatioY);

      IsEqualToExtensions.AssertIsEqualTo(1.2, p.X);
      IsEqualToExtensions.AssertIsEqualTo(-1.6, p.Y);
    }
  }
}
