using Xunit;

namespace Arnible.MathModeling.Test
{  
  public class PolynomialTautology
  {
    [Fact]
    public void Factoring_a2_minus_b2()
    {
      Assert.Equal(a2 - b2, (a - b) * (a + b));
    }

    [Fact]
    public void Factoring_a3_minus_b3()
    {
      Assert.Equal(a3 - b3, (a - b) * (a2 + b2 + a * b));
    }

    private readonly PolynomialTerm a = 'a';
    private readonly PolynomialTerm a2;
    private readonly PolynomialTerm a3;

    private readonly PolynomialTerm b = 'b';
    private readonly PolynomialTerm b2;
    private readonly PolynomialTerm b3;

    public PolynomialTautology()
    {
      a2 = a.Power(2);
      a3 = a.Power(3);

      b2 = b.Power(2);
      b3 = b.Power(3);
    }    
  }
}
