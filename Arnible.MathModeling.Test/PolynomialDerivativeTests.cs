using Xunit;
using static Arnible.MathModeling.Term;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Test
{
  public class PolynomialDerivativeTests
  {
    [Fact]
    public void CosMinusSin()
    {
      var p = Cos(φ) - Sin(φ);
      Assert.Equal(-1 * (Sin(φ) + Cos(φ)), p.DerivativeBy(φ));
    }
  }
}
