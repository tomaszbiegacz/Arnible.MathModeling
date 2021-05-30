using Arnible.Assertions;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CoordinatesExtensionTests
  {
    [Fact]
    public void ToSpherical_Generalizes_ToPolar()
    {
      Number expression = (x - y).ToPower(2);

      var rc = new RectangularCoordinate(x, y);
      var pc = new PolarCoordinate(r, φ);
      var expected = expression.ToPolar(rc, pc);

      var cc = new Number[] {x, y};
      var hc = new HypersphericalCoordinate(r, new Number[] {φ});
      var actual = expression.ToSpherical(cc, hc);

      IsEqualToExtensions.AssertIsEqualTo(expected, actual);
    }
  }
}
