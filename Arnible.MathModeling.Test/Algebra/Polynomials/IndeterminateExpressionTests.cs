using System;
using Arnible.Assertions;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class IndeterminateExpressionTests
  {
    [Fact]
    public void Constructor_Default()
    {
      IndeterminateExpression v = default;

      IsTrue(v.IsOne);
      IsFalse(v.HasUnaryModifier);
      Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      EqualExtensions.AssertEqualTo("1", v.ToString());
      EqualExtensions.AssertEqualTo(v, v.ToPower(2));

      AreExactlyEqual(1, v.GetOperation().Value());

      Throws<InvalidOperationException>(() =>
      {
        var _ = v.DerivativeBy('a');
      });

      AreExactlyEqual(12, v.SimplifyForConstant(12));
    }

    [Fact]
    public void Constructor_Identity()
    {
      IndeterminateExpression v = 'v';
      IsFalse(v.IsOne);
      IsFalse(v.HasUnaryModifier);
      EqualExtensions.AssertEqualTo('v', (char)v);

      EqualExtensions.AssertEqualTo("v", v.ToString());
      EqualExtensions.AssertEqualTo("v²", v.ToPower(2).ToString());

      AreExactlyEqual(5, v.GetOperation('v').Value(5));

      EqualExtensions.AssertEqualTo(2 * (PolynomialTerm)v, v.ToPower(2).DerivativeBy('v'));

      AreExactlyEqual(23, v.SimplifyForConstant(23));
    }

    [Fact]
    public void Constructor_Sin()
    {
      IndeterminateExpression v = IndeterminateExpression.Sin('v');
      IsFalse(v.IsOne);
      IsTrue(v.HasUnaryModifier);
      Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      EqualExtensions.AssertEqualTo("Sin(v)", v.ToString());
      EqualExtensions.AssertEqualTo("Sin²(v)", v.ToPower(2).ToString());

      AreExactlyEqual(0, v.GetOperation('v').Value(0));
      AreExactlyEqual(1, v.GetOperation('v').Value(Math.PI / 2));

      EqualExtensions.AssertEqualTo(2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      AreExactlyEqual(0, v.SimplifyForConstant(0));
      AreExactlyEqual(1, v.SimplifyForConstant(Math.PI / 2));
    }

    [Fact]
    public void Constructor_Cos()
    {
      IndeterminateExpression v = IndeterminateExpression.Cos('v');
      IsFalse(v.IsOne);
      IsTrue(v.HasUnaryModifier);
      Throws<InvalidOperationException>(() =>
      {
        var _ = (char) v;
      });

      EqualExtensions.AssertEqualTo("Cos(v)", v.ToString());
      EqualExtensions.AssertEqualTo("Cos²(v)", v.ToPower(2).ToString());

      AreExactlyEqual(1, v.GetOperation('v').Value(0));
      AreExactlyEqual(0, v.GetOperation('v').Value(Math.PI / 2));

      EqualExtensions.AssertEqualTo(-2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

      AreExactlyEqual(1, v.SimplifyForConstant(0));
      AreExactlyEqual(0, v.SimplifyForConstant(Math.PI / 2));
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
