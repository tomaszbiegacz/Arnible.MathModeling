using Xunit;
using static Arnible.MathModeling.Term;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Test
{  
  public class PolynomialTautology
  {
    [Fact]
    public void Factoring_a2_minus_b2()
    {
      Assert.Equal(a.ToPower(2) - b.ToPower(2), (a - b) * (a + b));
    }

    [Fact]
    public void Factoring_a3_minus_b3()
    {
      Assert.Equal(a.ToPower(3) - b.ToPower(3), (a - b) * (a.ToPower(2) + b.ToPower(2) + a * b));
    }

    [Fact]
    public void Trigonometric_one()
    {
      Assert.Equal(0, (Sin(a).ToPower(2) + Cos(a).ToPower(2)).DerivativeBy(a));
    }
  }
}
