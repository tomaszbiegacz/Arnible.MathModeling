using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialTests
  {
    [Fact]
    public void Constructor_Default()
    {
      Polynomial v = default;

      Assert.True(v.IsZero);
      Assert.True(v.HasOneTerm);
      Assert.True(v.IsConstant);
      Assert.Equal("0", v.ToString());

      Assert.Equal(0, v);
      Assert.Equal(0, (double)v);
      Assert.NotEqual(1, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
      Assert.Equal(0, v / 2);

      Assert.Equal(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      Polynomial v = 2;
      Assert.False(v.IsZero);
      Assert.True(v.HasOneTerm);
      Assert.True(v.IsConstant);
      Assert.Equal("2", v.ToString());

      Assert.Equal(2, v);
      Assert.Equal(2, (double)v);
      Assert.NotEqual(1, v);
      Assert.NotEqual(0, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(4, 2 * v);
      Assert.Equal(1, v / 2);

      Assert.Equal(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      Polynomial v = 'a';
      Assert.False(v.IsZero);
      Assert.True(v.HasOneTerm);
      Assert.False(v.IsConstant);
      Assert.Equal("a", v.ToString());

      Assert.Equal('a', (PolynomialTerm)v);

      Assert.Equal(1, v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));

      Assert.Equal(2 * (PolynomialTerm)('a', 1), 2 * v);
      Assert.Equal(0.5 * (PolynomialTerm)('a', 1), v / 2);

      Assert.Equal(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Multiply_xp1_xm1()
    {
      PolynomialTerm x = 'x';
      Polynomial poly = (x + 1) * (x - 1);

      Assert.Equal(x * x - 1, poly);
      Assert.Equal(2 * x, poly.DerivativeBy(x));
      Assert.Equal(0, poly.DerivativeBy('y'));
    }

    [Fact]
    public void Multiply_by0()
    {
      PolynomialTerm x = 'x';
      Polynomial poly = (x + 1) * (x - 1);

      Assert.Equal(0, 0 * poly);
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      var entry = 1 + x * x - y * y;
      Assert.Equal(2 + 2 * y, entry.Composition(x, y + 1));
    }

    [Fact]
    public void Power_ByZero()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(1, (x + 1).Power(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x + 1, (x + 1).Power(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x * x + 2 * x + 1, (x + 1).Power(2));
    }

    [Fact]
    public void TryDivide_ByConstant()
    {
      PolynomialTerm x = 'x';
      Assert.True((x + 1).TryDivide(0.5, out Polynomial result));
      Assert.Equal(2 * (x + 1), result);
    }

    [Fact]
    public void TryDivide_ByExpression()
    {
      PolynomialTerm x = 'x';
      Assert.True((x * x * x - 27).TryDivide(x - 3, out Polynomial result));
      Assert.Equal(x * x + 3 * x + 9, result);
    }

    [Fact]
    public void TryDivide_ByExpression_False()
    {
      PolynomialTerm x = 'x';
      Assert.False((x * x * x - 26).TryDivide(x - 3, out _));
    }

    [Fact]
    public void TryDivide_0ByExpression()
    {
      PolynomialTerm x = 'x';
      Polynomial zero = 0;
      Assert.True(zero.TryDivide(x - 3, out Polynomial result));
      Assert.Equal(0, result);
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      
      Assert.Equal(x + y, (x * x - y * y).ReduceBy(x - y));
    }
  }
}
