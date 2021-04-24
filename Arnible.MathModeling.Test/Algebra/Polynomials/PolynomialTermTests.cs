using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Analysis;
using Xunit;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialTermTests
  {
    [Fact]
    public void Constructor_Default()
    {
      PolynomialTerm v = default;

      ConditionExtensions.AssertIsTrue(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      v.ToString().AssertIsEqualTo("0");

      v.PowerSum.AssertIsEqualTo(0u);
      v.GreatestPowerIndeterminate.Variable.AssertIsEqualTo((char)0);
      v.GreatestPowerIndeterminate.Power.AssertIsEqualTo(0u);

      v.AssertIsEqualTo(0);
      ConditionExtensions.AssertIsFalse(1 == v);

      IsEmptyExtensions.AssertIsEmpty(v.DerivativeBy('a'));

      (2 * v).AssertIsEqualTo(0);
      (v / 2).AssertIsEqualTo(0);

      v.GetOperation().Value().AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialTerm v = 2;

      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      v.ToString().AssertIsEqualTo("2");

      v.PowerSum.AssertIsEqualTo(0u);
      v.GreatestPowerIndeterminate.Variable.AssertIsEqualTo((char)0);
      v.GreatestPowerIndeterminate.Power.AssertIsEqualTo(0u);

      v.AssertIsEqualTo(2);
      ConditionExtensions.AssertIsFalse(1 == v);
      ConditionExtensions.AssertIsFalse(0 == v);

      IsEmptyExtensions.AssertIsEmpty(v.DerivativeBy('a'));

      (2 * v).AssertIsEqualTo(4);
      (v / 2).AssertIsEqualTo(1);

      v.GetOperation().Value().AssertIsEqualTo(2);
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialTerm v = 'a';
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsConstant);

      v.PowerSum.AssertIsEqualTo(1u);
      v.GreatestPowerIndeterminate.Variable.AssertIsEqualTo('a');
      v.GreatestPowerIndeterminate.Power.AssertIsEqualTo(1u);

      v.ToString().AssertIsEqualTo("a");
      ConditionExtensions.AssertIsFalse('b' == v);

      v.DerivativeBy('a').Single().AssertIsEqualTo(1);
      IsEmptyExtensions.AssertIsEmpty(v.DerivativeBy('b'));

      (2 * v).AssertIsEqualTo(2 * Term.a);
      (v / 2).AssertIsEqualTo(0.5 * Term.a);

      v.GetOperation('a').Value(5).AssertIsEqualTo(5);
    }

    [Fact]
    public void Constructor_PolynomialVariable()
    {
      PolynomialTerm v = 2.1 * Term.a * Term.c.ToPower(3);

      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      v.ToString().AssertIsEqualTo("2.1ac³");

      v.PowerSum.AssertIsEqualTo(4u);
      v.GreatestPowerIndeterminate.Variable.AssertIsEqualTo('c');
      v.GreatestPowerIndeterminate.Power.AssertIsEqualTo(3u);      

      v.DerivativeBy('a').Single().AssertIsEqualTo(2.1 * Term.c.ToPower(3));
      IsEmptyExtensions.AssertIsEmpty(v.DerivativeBy('b'));
      v.DerivativeBy('c').Single().AssertIsEqualTo(6.3 * Term.c.ToPower(2) * Term.a.ToPower(1));

      v.DerivativeBy('a').DerivativeBy('c').Single().AssertIsEqualTo(6.3 * Term.c.ToPower(2));

      (-2 * v).AssertIsEqualTo(-4.2 * Term.a * Term.c.ToPower(3));

      v.GetOperation('a', 'c').Value(5, 2).AssertIsEqualTo(2.1 * 5 * 8);
    }

    [Fact]
    public void Multiply_Inline()
    {
      PolynomialTerm x = 'x';
      (4 * x * x).AssertIsEqualTo(4 * Term.x.ToPower(2));
      (4 * x * x).ToString().AssertIsEqualTo("4x²");

      (x * x).GetOperation('x').Value(4).AssertIsEqualTo(16);
    }

    [Fact]
    public void Multiply_Variables()
    {
      PolynomialTerm v1 = 2 * Term.a * Term.c.ToPower(3);
      PolynomialTerm v2 = 3 * Term.a.ToPower(2) * Term.d.ToPower(2);
      (v1 * v2).AssertIsEqualTo(6 * Term.a.ToPower(3) * Term.d.ToPower(2) * Term.c.ToPower(3));
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
      PolynomialTerm.Simplify(before).AssertIsEqualTo(expected);
    }

    [Fact]
    public void Power_ByZero()
    {
      PolynomialTerm x = 'x';
      x.ToPower(0).AssertIsEqualTo(1);
    }

    [Fact]
    public void Power_ByOne()
    {
      PolynomialTerm x = 'x';
      x.ToPower(1).AssertIsEqualTo(x);
    }

    [Fact]
    public void Power_ByTwo()
    {
      PolynomialTerm x = 'x';
      x.ToPower(2).AssertIsEqualTo(x * x);
    }

    [Fact]
    public void TryDivide_ByZero()
    {
      PolynomialTerm x = 'x';
      ConditionExtensions.AssertIsFalse(x.TryDivide(0, out _));
    }

    [Fact]
    public void TryDivide_ByConstant()
    {
      PolynomialTerm x = 'x';
      ConditionExtensions.AssertIsTrue((2 * x).TryDivide(2, out PolynomialTerm r));
      r.AssertIsEqualTo(x);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_xy_y()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      ConditionExtensions.AssertIsFalse(x.TryDivide(x * y, out _));
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetVariables_9x_x()
    {
      PolynomialTerm x = 'x';
      ConditionExtensions.AssertIsTrue((9 * x).TryDivide(x, out PolynomialTerm r));
      r.AssertIsEqualTo(9);
    }

    [Fact]
    public void TryDivide_ByTerm_BySupersetPowers()
    {
      PolynomialTerm x = 'x';
      ConditionExtensions.AssertIsFalse(x.TryDivide(x * x, out _));
    }

    [Fact]
    public void TryDivide_BySubsetTerm()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';
      ConditionExtensions.AssertIsTrue((2 * x.ToPower(3) * y.ToPower(2)).TryDivide(0.5 * x, out PolynomialTerm r));
      r.AssertIsEqualTo(4 * x.ToPower(2) * y.ToPower(2));
    }

    [Fact]
    public void Composition_SinWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α) + 1;
      sinExpression.Composition(Term.α, 0).AssertIsEqualTo(1);
    }

    [Fact]
    public void Composition_CosWithZero()
    {
      var sinExpression = MetaMath.Cos(Term.α) + 1;
      sinExpression.Composition(Term.α, 0).AssertIsEqualTo(2);
    }

    [Fact]
    public void Composition_SinCosWithZero()
    {
      var sinExpression = MetaMath.Sin(Term.α)* MetaMath.Cos(Term.α) + 1;
      sinExpression.Composition(Term.α, 0).AssertIsEqualTo(1);
    }

    [Fact]
    public void Sin_Variable()
    {
      PolynomialTerm.Sin(Term.a).AssertIsEqualTo(IndeterminateExpression.Sin('a'));
    }

    [Fact]
    public void Sin_Constant()
    {
      PolynomialTerm term = Math.PI / 2;
      PolynomialTerm.Sin(term).AssertIsEqualTo(1);
    }

    [Fact]
    public void Cos_Variable()
    {
      PolynomialTerm.Cos(Term.a).AssertIsEqualTo(IndeterminateExpression.Cos('a'));
    }

    [Fact]
    public void Cos_Constant()
    {
      PolynomialTerm term = 0;
      PolynomialTerm.Cos(term).AssertIsEqualTo(1);
    }
  }
}
