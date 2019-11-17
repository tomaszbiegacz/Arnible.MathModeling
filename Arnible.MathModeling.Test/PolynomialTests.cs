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
      Assert.Equal(2 * x, poly.DerivativeBy('x'));
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

      Polynomial entry = 1 + x * x - y * y;
      Polynomial replacement = y + 1;

      Assert.Equal(2 + 2 * y, entry.Composition('x', replacement));
    }
  }
}
