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

      CartesianCoordinate cc = new NumberVector(x, y);
      var hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(φ));
      var actual = expression.ToSpherical(cc, hc);

      EqualExtensions.AssertEqualTo(expected, actual);
    }
  }
}
