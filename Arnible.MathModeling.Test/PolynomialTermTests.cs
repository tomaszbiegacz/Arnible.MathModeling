using System;
using System.Linq;
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

      Assert.Equal(0, v.PowerSum);
      Assert.Equal(0, v.GreatestPowerIndeterminate.Variable);
      Assert.Equal(0u, v.GreatestPowerIndeterminate.Power);

      Assert.Equal(0, v);
      Assert.Equal(0, (double)v);
      Assert.NotEqual(1, v);

      Assert.Empty(v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
      Assert.Equal(0, v / 2);

      Assert.Equal(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialTerm v = 2;

      Assert.False(v.IsZero);
      Assert.True(v.IsConstant);
      Assert.Equal("2", v.ToString());

      Assert.Equal(0, v.PowerSum);
      Assert.Equal(0, v.GreatestPowerIndeterminate.Variable);
      Assert.Equal(0u, v.GreatestPowerIndeterminate.Power);

      Assert.Equal(2, v);
      Assert.Equal(2, (double)v);
      Assert.NotEqual(1, v);
      Assert.NotEqual(0, v);

      Assert.Empty(v.DerivativeBy('a'));

      Assert.Equal(4, 2 * v);
      Assert.Equal(1, v / 2);

      Assert.Equal(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialTerm v = 'a';
      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);

      Assert.Equal(1, v.PowerSum);
      Assert.Equal('a', v.GreatestPowerIndeterminate.Variable);
      Assert.Equal(1u, v.GreatestPowerIndeterminate.Power);

      Assert.Equal("a", v.ToString());
      Assert.NotEqual('b', v);

      Assert.Equal(1, v.DerivativeBy('a').Single());
      Assert.Empty(v.DerivativeBy('b'));

      Assert.Equal(2 * Term.a, 2 * v);
      Assert.Equal(0.5 * Term.a, v / 2);

      Assert.Equal(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Constructor_PolynomialVariable()
    {
      PolynomialTerm v = 2.1 * Term.a * Term.c.ToPower(3);

      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("2.1ac³", v.ToString());

      Assert.Equal(4, v.PowerSum);
      Assert.Equal('c', v.GreatestPowerIndeterminate.Variable);
      Assert.Equal(3u, v.GreatestPowerIndeterminate.Power);      

      Assert.Equal(2.1 * Term.c.ToPower(3), v.DerivativeBy('a').Single());
      Assert.Empty(v.DerivativeBy('b'));
      Assert.Equal(6.3 * Term.c.ToPower(2) * Term.a.ToPower(1), v.DerivativeBy('c').Single());

      Assert.Equal(6.3 * Term.c.ToPower(2), v.DerivativeBy('a').DerivativeBy('c').Single());

      Assert.Equal(-4.2 * Term.a * Term.c.ToPower(3), -2 * v);

      Assert.Equal(2.1 * 5 * 8, v.GetOperation('a', 'c').Value(5, 2));
    }

    [Fact]
    public void Multiply_Inline()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(4 * Term.x.ToPower(2), 4 * x * x);
      Assert.Equal("4x²", (4 * x * x).ToString());

      Assert.Equal(16, (x * x).GetOperation('x').Value(4));
    }

    [Fact]
    public void Multiply_Variables()
    {
      PolynomialTerm v1 = 2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm v2 = 3 * Term.a.ToPower(2) * Term.d.ToPower(2);
      Assert.Equal(6 * Term.a.ToPower(3) * Term.d.ToPower(2) * Term.c.ToPower(3), v1 * v2);
    }

    [Fact]
    public void Simplify_To_Zero()
    {
      PolynomialTerm v1 = 2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm v2 = -2 * Term.a * Term.c.ToPower(3);
      Assert.Empty(PolynomialTerm.Simplify(new[] { v1, v2 }));
    }

    [Fact]
    public void Simplify_To_Sum()
    {
      PolynomialTerm a = 'a';
      PolynomialTerm b = 'b';
      PolynomialTerm c = 'c';

      var expected = new PolynomialTerm[] { a * b * c, a * a, b * b, a * b, a, 2 * b, 3 };
      var before   = new PolynomialTerm[] { 1, b, 2, b, a, a * b, a * b * c, a * a, b * b };
      Assert.Equal(expected, PolynomialTerm.Simplify(before));
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

    [Fact]
    public void Power_ByZero()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(1, x.ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x, x.ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      PolynomialTerm x = 'x';
      Assert.Equal(x * x, x.ToPower(2));
    }

    [Fact]
    public void TryDivide_ByZero()
    {
      PolynomialTerm x = 'x';
      Assert.False(x.TryDivide(0, out _));
    }

    [Fact]
    public void TryDivide_ByConstant()
    {
      PolynomialTerm x = 'x';
      Assert.True((2 * x).TryDivide(2, out PolynomialTerm r));
      Assert.Equal(x, r);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_xy_y()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      Assert.False(x.TryDivide(x * y, out _));
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_9x_x()
    {
      PolynomialTerm x = 'x';
      Assert.True((9 * x).TryDivide(x, out PolynomialTerm r));
      Assert.Equal(9, r);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetPowers()
    {
      PolynomialTerm x = 'x';
      Assert.False(x.TryDivide(x * x, out _));
    }

    [Fact]
    public void TryDivide_BySubsetTerm()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      Assert.True((2 * x.ToPower(3) * y.ToPower(2)).TryDivide(0.5 * x, out PolynomialTerm r));
      Assert.Equal(4 * x.ToPower(2) * y.ToPower(2), r);
    }

    [Fact]
    public void Composition_SinWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α) + 1;
      Assert.Equal(1, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Composition_CosWithZero()
    {
      var sinExpression = MetaMath.Cos(Term.α) + 1;
      Assert.Equal(2, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Composition_SinCosWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α)* MetaMath.Cos(Term.α) + 1;
      Assert.Equal(1, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Sin_Variable()
    {
      Assert.Equal(IndeterminateExpression.Sin('a'), PolynomialTerm.Sin(Term.a));
    }

    [Fact]
    public void Sin_Constant()
    {
      PolynomialTerm term = Math.PI / 2;
      Assert.Equal(1, PolynomialTerm.Sin(term));
    }

    [Fact]
    public void Cos_Variable()
    {
      Assert.Equal(IndeterminateExpression.Cos('a'), PolynomialTerm.Cos(Term.a));
    }

    [Fact]
    public void Cos_Constant()
    {
      PolynomialTerm term = 0;
      Assert.Equal(1, PolynomialTerm.Cos(term));
    }
  }
}
