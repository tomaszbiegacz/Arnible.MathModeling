using System;
using Xunit;
using static Arnible.MathModeling.Polynomials.MetaMath;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Polynomials.Tests
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

      AreEqual("1", v.ToString());
      AreEqual(v, v.ToPower(2));

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
      AreEqual('v', (char)v);

      AreEqual("v", v.ToString());
      AreEqual("v²", v.ToPower(2).ToString());

      AreExactlyEqual(5, v.GetOperation('v').Value(5));

      AreEqual(2 * (PolynomialTerm)v, v.ToPower(2).DerivativeBy('v'));

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

      AreEqual("Sin(v)", v.ToString());
      AreEqual("Sin²(v)", v.ToPower(2).ToString());

      AreExactlyEqual(0, v.GetOperation('v').Value(0));
      AreExactlyEqual(1, v.GetOperation('v').Value(Math.PI / 2));

      AreEqual(2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

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

      AreEqual("Cos(v)", v.ToString());
      AreEqual("Cos²(v)", v.ToPower(2).ToString());

      AreExactlyEqual(1, v.GetOperation('v').Value(0));
      AreExactlyEqual(0, v.GetOperation('v').Value(Math.PI / 2));

      AreEqual(-2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));

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

      AreEqual(-1, a.CompareTo(b));
      AreEqual(0, a.CompareTo(a));
      AreEqual(1, b.CompareTo(a));

      AreEqual(-1, sinA.CompareTo(sinB));
      AreEqual(0, sinA.CompareTo(sinA));
      AreEqual(1, sinB.CompareTo(sinA));

      AreEqual(-1, a.CompareTo(sinA));
      AreEqual(-1, a.CompareTo(sinB));

      AreEqual(1, sinA.CompareTo(a));
      AreEqual(1, sinB.CompareTo(a));
    }
  }
}
