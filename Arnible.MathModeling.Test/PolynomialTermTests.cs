using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class PolynomialTermTests
  {
    [Fact]
    public void Constructor_Default()
    {
      PolynomialTerm v = default;

      Assert.True(v.IsZero);
      Assert.True(v.IsConstant);
      Assert.Equal("0", v.ToString());

      Assert.Equal(0, v);
      Assert.Equal(0, (double)v);
      Assert.NotEqual(1, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
      Assert.Equal(0, v / 2);

      Assert.Equal(0, v.Value(new Dictionary<char, double>()));
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialTerm v = 2;

      Assert.False(v.IsZero);
      Assert.True(v.IsConstant);
      Assert.Equal("2", v.ToString());

      Assert.Equal(2, v);
      Assert.Equal(2, (double)v);
      Assert.NotEqual(1, v);
      Assert.NotEqual(0, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(4, 2 * v);
      Assert.Equal(1, v / 2);

      Assert.Equal(2, v.Value(new Dictionary<char, double>()));
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialTerm v = 'a';
      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("a", v.ToString());
      Assert.NotEqual('b', v);

      Assert.Equal(1, v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));

      Assert.Equal(2 * (PolynomialTerm)('a', 1), 2 * v);
      Assert.Equal(0.5 * (PolynomialTerm)('a', 1), v / 2);

      Assert.Equal(5, v.Value(new Dictionary<char, double>
      {
        { 'a', 5 }
      }));
    }

    [Fact]
    public void Constructor_PolynomialVariable()
    {
      PolynomialTerm v = 2.1 * (PolynomialTerm)('a', 1) * ('c', 3);

      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("2.1*a*c3", v.ToString());

      Assert.Equal(2.1 * (PolynomialTerm)('c', 3), v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));
      Assert.Equal(6.3 * (PolynomialTerm)('c', 2) * ('a', 1), v.DerivativeBy('c'));

      Assert.Equal(v.DerivativeBy('a').DerivativeBy('c'), 6.3 * (PolynomialTerm)('c', 2));

      Assert.Equal(-4.2 * (PolynomialTerm)('a', 1) * ('c', 3), -2 * v);

      Assert.Equal(2.1 * 5 * 8, v.Value(new Dictionary<char, double>
      {
        { 'a', 5 },
        { 'c', 2 }
      }));
    }

    [Fact]
    public void Multiply_Inline()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(4 * (PolynomialTerm)('x', 2), 4 * x * x);      
      Assert.Equal("4*x2", (4 * x * x).ToString());

      Assert.Equal(16, (x * x).Value(new Dictionary<char, double> { { 'x', 4 } }));
    }

    [Fact]
    public void Multiply_Variables()
    {
      PolynomialTerm v1 = 2 * (PolynomialTerm)('a', 1) * ('c', 3);
      PolynomialTerm v2 = 3 * (PolynomialTerm)('a', 2) * ('d', 2);
      Assert.Equal(6 * (PolynomialTerm)('a', 3) * ('d', 2) * ('c', 3), v1 * v2);
    }

    [Fact]
    public void Simplify_To_Zero()
    {
      PolynomialTerm v1 = 2 * (PolynomialTerm)('a', 1) * ('c', 3);
      PolynomialTerm v2 = -2 * (PolynomialTerm)('a', 1) * ('c', 3);
      Assert.Empty(PolynomialTerm.Simplify(new[] { v1, v2 }));
    }

    [Fact]
    public void Simplify_To_Sum()
    {
      Assert.Equal(
        new PolynomialTerm[] { 3, 2 * (PolynomialTerm)('a', 1), 'b' },
        PolynomialTerm.Simplify(new PolynomialTerm[] { 1, 'a', 2, 'b', 'a' })
      );
    }

    [Fact]
    public void IsSimplified_Empty()
    {
      Assert.True(PolynomialTerm.IsSimplified(new PolynomialTerm[] { }));
    }

    [Fact]
    public void IsSimplified_True()
    {
      Assert.True(PolynomialTerm.IsSimplified(new PolynomialTerm[] { 'a', 'b', 1 }));
    }

    [Fact]
    public void IsSimplified_False()
    {
      Assert.False(PolynomialTerm.IsSimplified(new PolynomialTerm[] { 1, 'a', 2, 'b', 'a' }));
    }
  }
}
