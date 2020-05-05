using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialTests
  {
    private readonly PolynomialTerm x = 'x';
    private readonly PolynomialTerm y = 'y';

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
      Polynomial poly = (x + 1) * (x - 1);

      Assert.Equal(x * x - 1, poly);
      Assert.Equal(2 * x, poly.DerivativeBy(x));
      Assert.Equal(0, poly.DerivativeBy('y'));
    }

    [Fact]
    public void Square_1p()
    {
      var expr = x + 1;
      Assert.Equal(x * x + 2 * x + 1, expr * expr);
    }

    [Fact]
    public void Square_2p()
    {
      var expr = x + y;
      Assert.Equal(x * x + 2 * x * y + y * y, expr * expr);
    }

    [Fact]
    public void Square_2p_value()
    {
      var expr = x + y + 1;
      Assert.Equal(x * x + y * y + 2 * x * y + 2 * x + 2 * y + 1, expr * expr);
    }

    [Fact]
    public void Square_2p_xy()
    {
      var expr = x + x * y;
      Assert.Equal(x * x + 2 * x * x * y + x * x * y * y, expr * expr);
    }

    [Fact]
    public void Square_2p_xy_3()
    {
      var expr = x + y + x * y;
      Assert.Equal(x * x * y * y + 2 * x * x * y + 2 * x * y * y + x * x + y * y + 2 * x * y, expr * expr);
    }

    [Fact]
    public void Multiply_by0()
    {
      Polynomial poly = (x + 1) * (x - 1);

      Assert.Equal(0, 0 * poly);
    }

    [Fact]
    public void Composition_Square()
    {
      var entry = 1 + x * x - y * y;
      Assert.Equal(2 + 2 * y, entry.Composition(x, y + 1));
    }

    [Fact]
    public void Composition_InPlace()
    {
      var entry = 1 + x * x - y * y;
      Assert.Equal(x * x + 2 * x + 2 - y * y, entry.Composition(x, x + 1));
    }

    [Fact]
    public void Composition_Constant()
    {
      var entry = 1 + x * x - y * y;
      Assert.Equal(1 - y * y, entry.Composition(x, 0));
    }

    [Fact]
    public void Power_ByZero()
    {      
      Assert.Equal(1, (x + 1).ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      Assert.Equal(x + 1, (x + 1).ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      Assert.Equal(x * x + 2 * x + 1, (x + 1).ToPower(2));
    }

    [Fact]
    public void Power_ByThree()
    {
      Assert.Equal(x * x * x + 3 * x * x + 3 * x + 1, (x + 1).ToPower(3));
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {      
      Assert.Equal(2 * (x + 1), (x + 1).ReduceBy(0.5));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {     
      Assert.Equal(x * x + 3 * x + 9, (x * x * x - 27).ReduceBy(x - 3));
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {     
      Assert.Equal(x * x + 3 * x + 9, (x * x * x - 25).ReduceBy(x - 3, out Polynomial remainder));
      Assert.Equal(2, remainder);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {     
      Polynomial toDivide = 5 * (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      Assert.Equal(x - 1, toDivide.ReduceBy(5 * (x * x - 1), out Polynomial remainder));
      Assert.Equal(2 * x + 3, remainder);
    }

    [Fact]
    public void Remainder_ByExpression()
    {     
      var toDivide = (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      Assert.Equal(2 * x + 3, toDivide % (x * x - 1));
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {      
      Polynomial zero = 0;
      Assert.Equal(0, zero.ReduceBy(x - 3));
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {      
      Assert.Equal(x + y, (x * x - y * y).ReduceBy(x - y));
    }
  }
}
