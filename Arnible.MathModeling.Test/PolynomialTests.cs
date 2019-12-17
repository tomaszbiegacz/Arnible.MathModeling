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

      Assert.Equal(2 * Term.a, 2 * v);
      Assert.Equal(0.5 * Term.a, v / 2);

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
      Assert.Equal(1, (x + 1).ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x + 1, (x + 1).ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x * x + 2 * x + 1, (x + 1).ToPower(2));
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(2 * (x + 1), (x + 1).ReduceBy(0.5));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x * x + 3 * x + 9, (x * x * x - 27).ReduceBy(x - 3));
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x * x + 3 * x + 9, (x * x * x - 25).ReduceBy(x - 3, out Polynomial remainder));
      Assert.Equal(2, remainder);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {
      PolynomialTerm x = 'x';
      Polynomial toDivide = 5 * (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      Assert.Equal(x - 1, toDivide.ReduceBy(5 * (x * x - 1), out Polynomial remainder));
      Assert.Equal(2 * x + 3, remainder);
    }

    [Fact]
    public void Remainder_ByExpression()
    {
      PolynomialTerm x = 'x';
      var toDivide = (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      Assert.Equal(2 * x + 3, toDivide % (x * x - 1));
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {
      PolynomialTerm x = 'x';
      Polynomial zero = 0;
      Assert.Equal(0, zero.ReduceBy(x - 3));
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
