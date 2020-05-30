using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.xunit;
using Xunit;
using static Arnible.MathModeling.Term;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CoordinatesExtensionTests
  {
    [Fact]
    public void ToSpherical_Generalizes_ToPolar()
    {
      Number expression = (x - y).ToPower(2);

      var rc = new RectangularCoordianate(x, y);
      var pc = new PolarCoordinate(r, φ);
      var expected = expression.ToPolar(rc, pc);

      var cc = new CartesianCoordinate(new NumberVector(x, y));
      var hc = new HypersphericalCoordinate(r, new HypersphericalAngleVector(φ));
      var actual = expression.ToSpherical(cc, hc);

      AssertNumber.Equal(expected, actual);
    }
  }
}
