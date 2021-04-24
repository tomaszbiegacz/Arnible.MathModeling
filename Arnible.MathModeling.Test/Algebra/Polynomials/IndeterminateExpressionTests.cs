using System;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class IndeterminateExpressionTests
  {
    [Fact]
    public void Constructor_Default()
    {
      IndeterminateExpression v = default;

      ConditionExtensions.AssertIsTrue(v.IsOne);
      ConditionExtensions.AssertIsFalse(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      v.ToString().AssertIsEqualTo("1");
      v.ToPower(2).AssertIsEqualTo(v);

      v.GetOperation().Value().AssertIsEqualTo(1);

      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = v.DerivativeBy('a');
      });

      v.SimplifyForConstant(12).AssertIsEqualTo(12);
    }

    [Fact]
    public void Constructor_Identity()
    {
      IndeterminateExpression v = 'v';
      ConditionExtensions.AssertIsFalse(v.IsOne);
      ConditionExtensions.AssertIsFalse(v.HasUnaryModifier);
      ((char)v).AssertIsEqualTo('v');

      v.ToString().AssertIsEqualTo("v");
      v.ToPower(2).ToString().AssertIsEqualTo("v²");

      v.GetOperation('v').Value(5).AssertIsEqualTo<double>(5);

      v.ToPower(2).DerivativeBy('v').AssertIsEqualTo(2 * (PolynomialTerm)v);

      v.SimplifyForConstant(23).AssertIsEqualTo<double>(23);
    }

    [Fact]
    public void Constructor_Sin()
    {
      IndeterminateExpression v = IndeterminateExpression.Sin('v');
      ConditionExtensions.AssertIsFalse(v.IsOne);
      ConditionExtensions.AssertIsTrue(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      v.ToString().AssertIsEqualTo("Sin(v)");
      v.ToPower(2).ToString().AssertIsEqualTo("Sin²(v)");

      v.GetOperation('v').Value(0).AssertIsEqualTo(0);
      v.GetOperation('v').Value(Math.PI / 2).AssertIsEqualTo(1);

      v.ToPower(2).DerivativeBy('v').AssertIsEqualTo(2 * Cos(Term.v) * Sin(Term.v));

      v.SimplifyForConstant(0).AssertIsEqualTo(0);
      v.SimplifyForConstant(Math.PI / 2).AssertIsEqualTo(1);
    }

    [Fact]
    public void Constructor_Cos()
    {
      IndeterminateExpression v = IndeterminateExpression.Cos('v');
      ConditionExtensions.AssertIsFalse(v.IsOne);
      ConditionExtensions.AssertIsTrue(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      v.ToString().AssertIsEqualTo("Cos(v)");
      v.ToPower(2).ToString().AssertIsEqualTo("Cos²(v)");

      v.GetOperation('v').Value(0).AssertIsEqualTo(1);
      v.GetOperation('v').Value(Math.PI / 2).AssertIsEqualTo(0);

      v.ToPower(2).DerivativeBy('v').AssertIsEqualTo(-2 * Cos(Term.v) * Sin(Term.v));

      v.SimplifyForConstant(0).AssertIsEqualTo(1);
      v.SimplifyForConstant(Math.PI / 2).AssertIsEqualTo(0);
    }

    [Fact]
    public void Comparer()
    {
      IndeterminateExpression a = 'a';
      IndeterminateExpression b = 'b';

      IndeterminateExpression sinA = IndeterminateExpression.Sin('a');
      IndeterminateExpression sinB = IndeterminateExpression.Sin('b');

      a.CompareTo(b).AssertIsEqualTo(-1);
      a.CompareTo(a).AssertIsEqualTo(0);
      b.CompareTo(a).AssertIsEqualTo(1);

      sinA.CompareTo(sinB).AssertIsEqualTo(-1);
      sinA.CompareTo(sinA).AssertIsEqualTo(0);
      sinB.CompareTo(sinA).AssertIsEqualTo(1);

      a.CompareTo(sinA).AssertIsEqualTo(-1);
      a.CompareTo(sinB).AssertIsEqualTo(-1);

      sinA.CompareTo(a).AssertIsEqualTo(1);
      sinB.CompareTo(a).AssertIsEqualTo(1);
    }
  }
}
