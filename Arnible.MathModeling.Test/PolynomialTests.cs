using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialTests
  {
    [Fact]
    public void Constructor_Default()
    {
      var v = new Polynomial();
      Assert.True(v.IsZero);      
      Assert.Equal("0", v.ToString());

      Assert.Equal(0, v);
      Assert.NotEqual(1, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
    }

    [Fact]
    public void Constructor_Constant()
    {
      Polynomial v = 2;
      Assert.False(v.IsZero);
      Assert.Equal("2", v.ToString());

      Assert.Equal(2, v);
      Assert.NotEqual(1, v);
      Assert.NotEqual(0, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(4, 2 * v);
    }

    [Fact]
    public void Constructor_Variable()
    {
      Polynomial v = 'a';
      Assert.False(v.IsZero);
      Assert.Equal("a", v.ToString());

      Assert.Equal(1, v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));

      Assert.Equal(new PolynomialVariable(2, ('a', 1)), 2 * v);
    }

    [Fact]
    public void Multiply_xp1_xm1()
    {
      var v1 = new Polynomial('x', +1);
      var v2 = new Polynomial('x', -1);

      var v = v1 * v2;
      Assert.Equal(new Polynomial(('x', 2), -1), v);
      Assert.Equal(new PolynomialVariable(2) * 'x', v.DerivativeBy('x'));
      Assert.Equal(0, v.DerivativeBy('y'));
    }
  }
}
