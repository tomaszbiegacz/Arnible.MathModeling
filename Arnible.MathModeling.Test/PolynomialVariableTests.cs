using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialVariableTests
  {
    [Fact]
    public void Constructor_Default()
    {
      var v = new PolynomialVariable();
      Assert.True(v.IsZero);
      Assert.True(v.IsConstant);
      Assert.Equal("0", v.ToString());

      Assert.Equal(0, v);
      Assert.NotEqual(1, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialVariable v = 2;
      Assert.False(v.IsZero);
      Assert.True(v.IsConstant);
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
      PolynomialVariable v = 'a';
      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("a", v.ToString());

      Assert.Equal(1, v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));

      Assert.Equal(new PolynomialVariable(2, ('a', 1)), 2 * v);
    }

    [Fact]
    public void Constructor_PolynomialVariable()
    {
      var v = new PolynomialVariable(2.1, ('a', 1), ('c', 3));
      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("2.1*a*c3", v.ToString());

      Assert.Equal(new PolynomialVariable(2.1, ('c', 3)), v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));
      Assert.Equal(new PolynomialVariable(6.3, ('c', 2), ('a', 1)), v.DerivativeBy('c'));

      Assert.Equal(v.DerivativeBy('a').DerivativeBy('c'), new PolynomialVariable(6.3, ('c', 2)));

      Assert.Equal(new PolynomialVariable(-4.2, ('a', 1), ('c', 3)), -2 * v);
    }

    [Fact]
    public void Multiply_Variables()
    {
      var v1 = new PolynomialVariable(2, ('a', 1), ('c', 3));
      var v2 = new PolynomialVariable(3, ('a', 2), ('d', 2));
      Assert.Equal(new PolynomialVariable(6, ('a', 3), ('d', 2), ('c', 3)), v1 * v2);
    }

    [Fact]
    public void Simplify_To_Zero()
    {
      var v1 = new PolynomialVariable(2, ('a', 1), ('c', 3));
      var v2 = new PolynomialVariable(-2, ('a', 1), ('c', 3));
      Assert.Empty(PolynomialVariable.Simplify(new[] { v1, v2 }));
    }

    [Fact]
    public void Simplify_To_Sum()
    {
      Assert.Equal(
        new PolynomialVariable[] { 3, new PolynomialVariable(2, ('a', 1)), 'b' },
        PolynomialVariable.Simplify(new PolynomialVariable[] { 1, 'a', 2, 'b', 'a' })
      );
    }
  }
}
