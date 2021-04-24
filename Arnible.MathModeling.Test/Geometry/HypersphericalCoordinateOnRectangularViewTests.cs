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

      p.R.AssertIsEqualTo(2);
      p.RatioX.AssertIsEqualTo(0.3);
      p.RatioY.AssertIsEqualTo(-0.4);

      p.X.AssertIsEqualTo(0.6);
      p.Y.AssertIsEqualTo(-0.8);
    }

    [Fact]
    public void FromRatios()
    {
      var p = HypersphericalCoordinateOnRectangularView.FromRatios(r: 2, ratioX: 0.6, ratioY: -0.8);

      p.R.AssertIsEqualTo(2);
      p.RatioX.AssertIsEqualTo(0.6);
      p.RatioY.AssertIsEqualTo(-0.8);

      p.X.AssertIsEqualTo(1.2);
      p.Y.AssertIsEqualTo(-1.6);
    }
  }
}
