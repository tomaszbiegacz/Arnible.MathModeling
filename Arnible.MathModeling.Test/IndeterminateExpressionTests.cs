using System;
using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Test
{
  public class IndeterminateExpressionTests
  {
    [Fact]
    public void Constructor_Default()
    {
      IndeterminateExpression v = default;

      Assert.True(v.IsOne);
      Assert.False(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() => (char)v);

      Assert.Equal("1", v.ToString());
      Assert.Equal(v, v.ToPower(2));

      Assert.Equal(1, v.GetOperation().Value());

      Assert.Throws<InvalidOperationException>(() => v.DerivativeBy('a'));
    }

    [Fact]
    public void Constructor_Identity()
    {
      IndeterminateExpression v = 'v';
      Assert.False(v.IsOne);
      Assert.False(v.HasUnaryModifier);
      Assert.Equal('v', (char)v);

      Assert.Equal("v", v.ToString());
      Assert.Equal("v²", v.ToPower(2).ToString());

      Assert.Equal(5, v.GetOperation('v').Value(5));

      Assert.Equal(2 * (PolynomialTerm)v, v.ToPower(2).DerivativeBy('v'));
    }

    [Fact]
    public void Constructor_Sin()
    {
      IndeterminateExpression v = IndeterminateExpression.Sin('v');
      Assert.False(v.IsOne);
      Assert.True(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() => (char)v);

      Assert.Equal("Sin(v)", v.ToString());
      Assert.Equal("Sin²(v)", v.ToPower(2).ToString());

      Assert.Equal(0, v.GetOperation('v').Value(0));
      Assert.Equal(1, v.GetOperation('v').Value(Math.PI / 2));
      
      Assert.Equal(2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));
    }

    [Fact]
    public void Constructor_Cos()
    {
      IndeterminateExpression v = IndeterminateExpression.Cos('v');
      Assert.False(v.IsOne);
      Assert.True(v.HasUnaryModifier);
      Assert.Throws<InvalidOperationException>(() => (char)v);

      Assert.Equal("Cos(v)", v.ToString());
      Assert.Equal("Cos²(v)", v.ToPower(2).ToString());

      Assert.Equal(1, v.GetOperation('v').Value(0));
      Assert.Equal(0, v.GetOperation('v').Value(Math.PI / 2), 15);

      Assert.Equal(-2 * Cos(Term.v) * Sin(Term.v), v.ToPower(2).DerivativeBy('v'));
    }

    [Fact]
    public void Comparer()
    {
      IndeterminateExpression a = 'a';
      IndeterminateExpression b = 'b';

      IndeterminateExpression sinA = IndeterminateExpression.Sin('a');
      IndeterminateExpression sinB = IndeterminateExpression.Sin('b');

      Assert.Equal(-1, a.CompareTo(b));
      Assert.Equal(0, a.CompareTo(a));
      Assert.Equal(1, b.CompareTo(a));

      Assert.Equal(-1, sinA.CompareTo(sinB));
      Assert.Equal(0, sinA.CompareTo(sinA));
      Assert.Equal(1, sinB.CompareTo(sinA));

      Assert.Equal(-1, a.CompareTo(sinA));
      Assert.Equal(-1, a.CompareTo(sinB));
      
      Assert.Equal(1, sinA.CompareTo(a));
      Assert.Equal(1, sinB.CompareTo(a));
    }
  }
}
