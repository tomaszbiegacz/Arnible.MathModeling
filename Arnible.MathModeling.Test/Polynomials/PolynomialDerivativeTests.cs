using Xunit;
using static Arnible.MathModeling.Polynomials.Term;
using static Arnible.MathModeling.Polynomials.MetaMath;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Polynomials.Tests
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
