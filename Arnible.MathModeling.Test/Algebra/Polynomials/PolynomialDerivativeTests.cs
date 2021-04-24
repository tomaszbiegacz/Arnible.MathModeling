using Arnible.Assertions;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.Term;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialDerivativeTests
  {
    [Fact]
    public void CosMinusSin()
    {
      var p = Cos(φ) - Sin(φ);
      p.DerivativeBy(φ).AssertIsEqualTo(-1 * (Sin(φ) + Cos(φ)));
    }
  }
}
