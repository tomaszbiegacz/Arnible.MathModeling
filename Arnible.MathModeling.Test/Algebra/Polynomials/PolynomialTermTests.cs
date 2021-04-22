using System;
using Arnible.Assertions;
using Arnible.Linq;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialTermTests
  {
    [Fact]
    public void Constructor_Default()
    {
      PolynomialTerm v = default;

      IsTrue(v == 0);
      IsTrue(v.IsConstant);
      AreEqual("0", v.ToString());

      AreEqual(0u, v.PowerSum);
      AreEqual(0, v.GreatestPowerIndeterminate.Variable);
      AreEqual(0u, v.GreatestPowerIndeterminate.Power);

      EqualExtensions.AssertEqualTo(0, v);
      AreExactlyEqual(0, (double)v);
      IsFalse(1 == v);

      IsEmpty(v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(0, 2 * v);
      EqualExtensions.AssertEqualTo(0, v / 2);

      AreExactlyEqual(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialTerm v = 2;

      IsFalse(v == 0);
      IsTrue(v.IsConstant);
      EqualExtensions.AssertEqualTo("2", v.ToString());

      EqualExtensions.AssertEqualTo(0u, v.PowerSum);
      EqualExtensions.AssertEqualTo(0, v.GreatestPowerIndeterminate.Variable);
      EqualExtensions.AssertEqualTo(0u, v.GreatestPowerIndeterminate.Power);

      EqualExtensions.AssertEqualTo(2, v);
      AreExactlyEqual(2, (double)v);
      IsFalse(1 == v);
      IsFalse(0 == v);

      IsEmpty(v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(4, 2 * v);
      EqualExtensions.AssertEqualTo(1, v / 2);

      AreExactlyEqual(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialTerm v = 'a';
      IsFalse(v == 0);
      IsFalse(v.IsConstant);

      EqualExtensions.AssertEqualTo(1u, v.PowerSum);
      EqualExtensions.AssertEqualTo('a', v.GreatestPowerIndeterminate.Variable);
      EqualExtensions.AssertEqualTo(1u, v.GreatestPowerIndeterminate.Power);

      EqualExtensions.AssertEqualTo("a", v.ToString());
      IsFalse('b' == v);

      EqualExtensions.AssertEqualTo(1, v.DerivativeBy('a').Single());
      IsEmpty(v.DerivativeBy('b'));

      EqualExtensions.AssertEqualTo(2 * Term.a, 2 * v);
      EqualExtensions.AssertEqualTo(0.5 * Term.a, v / 2);

      AreExactlyEqual(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Constructor_PolynomialVariable()
    {
      PolynomialTerm v = 2.1 * Term.a * Term.c.ToPower(3);

      IsFalse(v == 0);
      IsFalse(v.IsConstant);
      EqualExtensions.AssertEqualTo("2.1ac³", v.ToString());

      EqualExtensions.AssertEqualTo(4u, v.PowerSum);
      EqualExtensions.AssertEqualTo('c', v.GreatestPowerIndeterminate.Variable);
      EqualExtensions.AssertEqualTo(3u, v.GreatestPowerIndeterminate.Power);      

      EqualExtensions.AssertEqualTo(2.1 * Term.c.ToPower(3), v.DerivativeBy('a').Single());
      IsEmpty(v.DerivativeBy('b'));
      EqualExtensions.AssertEqualTo(6.3 * Term.c.ToPower(2) * Term.a.ToPower(1), v.DerivativeBy('c').Single());

      EqualExtensions.AssertEqualTo(6.3 * Term.c.ToPower(2), v.DerivativeBy('a').DerivativeBy('c').Single());

      EqualExtensions.AssertEqualTo(-4.2 * Term.a * Term.c.ToPower(3), -2 * v);

      EqualExtensions.AssertEqualTo(2.1 * 5 * 8, v.GetOperation('a', 'c').Value(5, 2));
    }

    [Fact]
    public void Multiply_Inline()
    {
      PolynomialTerm x = 'x';
      EqualExtensions.AssertEqualTo(4 * Term.x.ToPower(2), 4 * x * x);
      EqualExtensions.AssertEqualTo("4x²", (4 * x * x).ToString());

      EqualExtensions.AssertEqualTo(16, (x * x).GetOperation('x').Value(4));
    }

    [Fact]
    public void Multiply_Variables()
    {
      PolynomialTerm v1 = 2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm v2 = 3 * Term.a.ToPower(2) * Term.d.ToPower(2);
      EqualExtensions.AssertEqualTo(6 * Term.a.ToPower(3) * Term.d.ToPower(2) * Term.c.ToPower(3), v1 * v2);
    }

    [Fact]
    public void Simplify_To_Zero()
    {
      PolynomialTerm v1 = 2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm v2 = -2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm.Simplify(new[] { v1, v2 }).AsList().AssertIsEmpty();
    }

    [Fact]
    public void Simplify_To_Sum()
    {
      PolynomialTerm a = 'a';
      PolynomialTerm b = 'b';
      PolynomialTerm c = 'c';

      var expected = new PolynomialTerm[] { a * b * c, a * a, b * b, a * b, a, 2 * b, 3 };
      var before   = new PolynomialTerm[] { 1, b, 2, b, a, a * b, a * b * c, a * a, b * b };
      EqualExtensions.AssertEqualTo(expected, PolynomialTerm.Simplify(before));
    }

    [Fact]
    public void Power_ByZero()
    {
      PolynomialTerm x = 'x';
      EqualExtensions.AssertEqualTo(1, x.ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      PolynomialTerm x = 'x';
      EqualExtensions.AssertEqualTo(x, x.ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      PolynomialTerm x = 'x';
      EqualExtensions.AssertEqualTo(x * x, x.ToPower(2));
    }

    [Fact]
    public void TryDivide_ByZero()
    {
      PolynomialTerm x = 'x';
      IsFalse(x.TryDivide(0, out _));
    }

    [Fact]
    public void TryDivide_ByConstant()
    {
      PolynomialTerm x = 'x';
      IsTrue((2 * x).TryDivide(2, out PolynomialTerm r));
      EqualExtensions.AssertEqualTo(x, r);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_xy_y()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      IsFalse(x.TryDivide(x * y, out _));
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_9x_x()
    {
      PolynomialTerm x = 'x';
      IsTrue((9 * x).TryDivide(x, out PolynomialTerm r));
      EqualExtensions.AssertEqualTo(9, r);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetPowers()
    {
      PolynomialTerm x = 'x';
      IsFalse(x.TryDivide(x * x, out _));
    }

    [Fact]
    public void TryDivide_BySubsetTerm()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      IsTrue((2 * x.ToPower(3) * y.ToPower(2)).TryDivide(0.5 * x, out PolynomialTerm r));
      EqualExtensions.AssertEqualTo(4 * x.ToPower(2) * y.ToPower(2), r);
    }

    [Fact]
    public void Composition_SinWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α) + 1;
      EqualExtensions.AssertEqualTo(1, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Composition_CosWithZero()
    {
      var sinExpression = MetaMath.Cos(Term.α) + 1;
      EqualExtensions.AssertEqualTo(2, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Composition_SinCosWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α)* MetaMath.Cos(Term.α) + 1;
      EqualExtensions.AssertEqualTo(1, sinExpression.Composition(Term.α, 0));
    }

    [Fact]
    public void Sin_Variable()
    {
      EqualExtensions.AssertEqualTo(IndeterminateExpression.Sin('a'), PolynomialTerm.Sin(Term.a));
    }

    [Fact]
    public void Sin_Constant()
    {
      PolynomialTerm term = Math.PI / 2;
      EqualExtensions.AssertEqualTo(1, PolynomialTerm.Sin(term));
    }

    [Fact]
    public void Cos_Variable()
    {
      EqualExtensions.AssertEqualTo(IndeterminateExpression.Cos('a'), PolynomialTerm.Cos(Term.a));
    }

    [Fact]
    public void Cos_Constant()
    {
      PolynomialTerm term = 0;
      EqualExtensions.AssertEqualTo(1, PolynomialTerm.Cos(term));
    }
  }
}
