using Xunit;
using static Arnible.MathModeling.Term;
using static Arnible.MathModeling.MetaMath;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class PolynomialDerivativeTests
  {
    [Fact]
    public void CosMinusSin()
    {
      var p = Cos(φ) - Sin(φ);
      AreEqual(-1 * (Sin(φ) + Cos(φ)), p.DerivativeBy(φ));
    }
  }
}
