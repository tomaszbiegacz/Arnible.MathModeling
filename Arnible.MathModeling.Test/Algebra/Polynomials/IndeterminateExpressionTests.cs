using System;
using Arnible.Assertions;
using Arnible.MathModeling.Test;
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

      IsEqualToExtensions.AssertIsEqualTo("1", v.ToString());
      IsEqualToExtensions.AssertIsEqualTo(v, v.ToPower(2));

      IsEqualToExtensions.AssertIsEqualTo<double>(1, v.GetOperation().Value());

      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = v.DerivativeBy('a');
      });

      IsEqualToExtensions.AssertIsEqualTo<double>(12, v.SimplifyForConstant(12));
    }

    [Fact]
    public void Constructor_Identity()
    {
      IndeterminateExpression v = 'v';
      ConditionExtensions.AssertIsFalse(v.IsOne);
      ConditionExtensions.AssertIsFalse(v.HasUnaryModifier);
      IsEqualToExtensions.AssertIsEqualTo('v', (char)v);

      IsEqualToExtensions.AssertIsEqualTo("v", v.ToString());
      IsEqualToExtensions.AssertIsEqualTo("v²", v.ToPower(2).ToString());

      IsEqualToExtensions.AssertIsEqualTo<double>(5, v.GetOperation('v').Value(5));

      IsEqualToExtensions.AssertIsEqualTo(2 * (PolynomialTerm)v, v.ToPower(2).DerivativeBy('v'));

      IsEqualToExtensions.AssertIsEqualTo<double>(23, v.SimplifyForConstant(23));
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

      IsEqualToExtensions.AssertIsEqualTo("Sin(v)", v.ToString());
      IsEqualToExtensions.AssertIsEqualTo("Sin²(v)", v.ToPower(2).ToString());

      IsEqualToExtensions.AssertIsEqualTo<double>(0, v.GetOperation('v').Value(0));
      IsEqualToExtensions.AssertIsEqualTo<double>(1, v.GetOperation('v').Value(Math.PI / 2));

      IsEqualToExtensions.AssertIsEqualTo(2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      IsEqualToExtensions.AssertIsEqualTo<double>(0, v.SimplifyForConstant(0));
      IsEqualToExtensions.AssertIsEqualTo<double>(1, v.SimplifyForConstant(Math.PI / 2));
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

      IsEqualToExtensions.AssertIsEqualTo("Cos(v)", v.ToString());
      IsEqualToExtensions.AssertIsEqualTo("Cos²(v)", v.ToPower(2).ToString());

      IsEqualToExtensions.AssertIsEqualTo<double>(1, v.GetOperation('v').Value(0));
      IsEqualToExtensions.AssertIsEqualTo<double>(0, v.GetOperation('v').Value(Math.PI / 2));

      IsEqualToExtensions.AssertIsEqualTo(-2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      IsEqualToExtensions.AssertIsEqualTo<double>(1, v.SimplifyForConstant(0));
      IsEqualToExtensions.AssertIsEqualTo<double>(0, v.SimplifyForConstant(Math.PI / 2));
    }

    [Fact]
    public void Comparer()
    {
      IndeterminateExpression a = 'a';
      IndeterminateExpression b = 'b';

      IndeterminateExpression sinA = IndeterminateExpression.Sin('a');
      IndeterminateExpression sinB = IndeterminateExpression.Sin('b');

      IsEqualToExtensions.AssertIsEqualTo(-1, a.CompareTo(b));
      IsEqualToExtensions.AssertIsEqualTo(0, a.CompareTo(a));
      IsEqualToExtensions.AssertIsEqualTo(1, b.CompareTo(a));

      IsEqualToExtensions.AssertIsEqualTo(-1, sinA.CompareTo(sinB));
      IsEqualToExtensions.AssertIsEqualTo(0, sinA.CompareTo(sinA));
      IsEqualToExtensions.AssertIsEqualTo(1, sinB.CompareTo(sinA));

      IsEqualToExtensions.AssertIsEqualTo(-1, a.CompareTo(sinA));
      IsEqualToExtensions.AssertIsEqualTo(-1, a.CompareTo(sinB));

      IsEqualToExtensions.AssertIsEqualTo(1, sinA.CompareTo(a));
      IsEqualToExtensions.AssertIsEqualTo(1, sinB.CompareTo(a));
    }
  }
}
