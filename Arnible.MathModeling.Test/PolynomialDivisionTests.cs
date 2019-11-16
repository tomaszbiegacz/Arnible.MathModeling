using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialDivisionTests
  {
    private readonly PolynomialTerm x = 'x';
    private readonly PolynomialTerm y = 'y';

    [Fact]
    public void Constructor_Polynomial()
    {
      Polynomial x = 'x';
      Polynomial constant = 1;
      PolynomialDivision pol = x / constant;
      Assert.True(pol.IsPolynomial);
      Assert.Equal('x', (Polynomial)pol);
    }

    [Fact]
    public void ToString_Special()
    {
      Assert.Equal("0", (0 / (PolynomialTerm)'x').ToString());
      Assert.Equal("NaN", (new PolynomialDivision()).ToString());
    }

    [Fact]
    public void ToString_Common()
    {
      Assert.Equal("(x+1)/y", ((x + 1) / y).ToString());
      Assert.Equal("x/(y+1)", (x / (y + 1)).ToString());
    }

    [Fact]
    public void Equality_Equals()
    {
      Assert.Equal(x / y, 'x' / (PolynomialTerm)'y');
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      Assert.True((zero / x).IsZero);
      Assert.Equal(zero / x, zero / y);
      Assert.Equal(0 * (x / y), zero / y);
    }

    [Fact]
    public void Equality_NumeratorNotEqual()
    {
      Assert.NotEqual((x + 1) / y, x / y);
    }

    [Fact]
    public void Equality_DenominatorNotEqual()
    {
      Assert.NotEqual(x / (y + 1), x / y);
    }

    [Fact]
    public void Derivative_Simple()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      var expression = (y + x + 1) / (2 * x + 1);
      Assert.Equal(-1 * (2 * y + 1) / (4 * x * x + 4 * x + 1), expression.DerivativeBy('x'));
      Assert.Equal(1 / (2 * x + 1), expression.DerivativeBy('y'));

      Assert.True(expression.DerivativeBy('y').DerivativeBy('y').IsZero);
    }
  }
}
