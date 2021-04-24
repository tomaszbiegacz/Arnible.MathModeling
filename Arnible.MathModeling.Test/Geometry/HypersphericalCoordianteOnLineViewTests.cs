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

      p.R.AssertIsEqualTo(2);
      p.RatioX.AssertIsEqualTo(0.3);

      p.X.AssertIsEqualTo(0.6);
    }
  }
}
