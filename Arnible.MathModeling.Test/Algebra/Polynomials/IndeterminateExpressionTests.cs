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

      EqualExtensions.AssertEqualTo("1", v.ToString());
      EqualExtensions.AssertEqualTo(v, v.ToPower(2));

      EqualExtensions.AssertEqualTo<double>(1, v.GetOperation().Value());

      Assert.Throws<InvalidOperationException>(() =>
      {
        var _ = v.DerivativeBy('a');
      });

      EqualExtensions.AssertEqualTo<double>(12, v.SimplifyForConstant(12));
    }

    [Fact]
    public void Constructor_Identity()
    {
      IndeterminateExpression v = 'v';
      ConditionExtensions.AssertIsFalse(v.IsOne);
      ConditionExtensions.AssertIsFalse(v.HasUnaryModifier);
      EqualExtensions.AssertEqualTo('v', (char)v);

      EqualExtensions.AssertEqualTo("v", v.ToString());
      EqualExtensions.AssertEqualTo("v²", v.ToPower(2).ToString());

      EqualExtensions.AssertEqualTo<double>(5, v.GetOperation('v').Value(5));

      EqualExtensions.AssertEqualTo(2 * (PolynomialTerm)v, v.ToPower(2).DerivativeBy('v'));

      EqualExtensions.AssertEqualTo<double>(23, v.SimplifyForConstant(23));
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

      EqualExtensions.AssertEqualTo("Sin(v)", v.ToString());
      EqualExtensions.AssertEqualTo("Sin²(v)", v.ToPower(2).ToString());

      EqualExtensions.AssertEqualTo<double>(0, v.GetOperation('v').Value(0));
      EqualExtensions.AssertEqualTo<double>(1, v.GetOperation('v').Value(Math.PI / 2));

      EqualExtensions.AssertEqualTo(2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      EqualExtensions.AssertEqualTo<double>(0, v.SimplifyForConstant(0));
      EqualExtensions.AssertEqualTo<double>(1, v.SimplifyForConstant(Math.PI / 2));
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

      EqualExtensions.AssertEqualTo("Cos(v)", v.ToString());
      EqualExtensions.AssertEqualTo("Cos²(v)", v.ToPower(2).ToString());

      EqualExtensions.AssertEqualTo<double>(1, v.GetOperation('v').Value(0));
      EqualExtensions.AssertEqualTo<double>(0, v.GetOperation('v').Value(Math.PI / 2));

      EqualExtensions.AssertEqualTo(-2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      EqualExtensions.AssertEqualTo<double>(1, v.SimplifyForConstant(0));
      EqualExtensions.AssertEqualTo<double>(0, v.SimplifyForConstant(Math.PI / 2));
    }

    [Fact]
    public void Comparer()
    {
      IndeterminateExpression a = 'a';
      IndeterminateExpression b = 'b';

      IndeterminateExpression sinA = IndeterminateExpression.Sin('a');
      IndeterminateExpression sinB = IndeterminateExpression.Sin('b');

      EqualExtensions.AssertEqualTo(-1, a.CompareTo(b));
      EqualExtensions.AssertEqualTo(0, a.CompareTo(a));
      EqualExtensions.AssertEqualTo(1, b.CompareTo(a));

      EqualExtensions.AssertEqualTo(-1, sinA.CompareTo(sinB));
      EqualExtensions.AssertEqualTo(0, sinA.CompareTo(sinA));
      EqualExtensions.AssertEqualTo(1, sinB.CompareTo(sinA));

      EqualExtensions.AssertEqualTo(-1, a.CompareTo(sinA));
      EqualExtensions.AssertEqualTo(-1, a.CompareTo(sinB));

      EqualExtensions.AssertEqualTo(1, sinA.CompareTo(a));
      EqualExtensions.AssertEqualTo(1, sinB.CompareTo(a));
    }
  }
}
