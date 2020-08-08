using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Polynomials.Tests
{
  public class PolynomialTests
  {
    private readonly PolynomialTerm x = 'x';
    private readonly PolynomialTerm y = 'y';

    [Fact]
    public void Constructor_Default()
    {
      Polynomial v = default;

      IsTrue(v == 0);
      IsTrue(v.IsSingleTerm);
      IsTrue(v.IsConstant);
      AreEqual("0", v.ToString());

      AreEqual(0, v);
      AreExactlyEqual(0, (double)v);
      AreNotEqual(1, v);

      AreEqual(0, v.DerivativeBy('a'));

      AreEqual(0, 2 * v);
      AreEqual(0, v / 2);

      AreExactlyEqual(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      Polynomial v = 2;
      IsFalse(v == 0);
      IsTrue(v.IsSingleTerm);
      IsTrue(v.IsConstant);
      AreEqual("2", v.ToString());

      AreEqual(2, v);
      AreExactlyEqual(2, (double)v);
      AreNotEqual(1, v);
      AreNotEqual(0, v);

      AreEqual(0, v.DerivativeBy('a'));

      AreEqual(4, 2 * v);
      AreEqual(1, v / 2);

      AreExactlyEqual(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      Polynomial v = 'a';
      IsFalse(v == 0);
      IsTrue(v.IsSingleTerm);
      IsFalse(v.IsConstant);
      AreEqual("a", v.ToString());

      AreEqual('a', (PolynomialTerm)v);

      AreExactlyEqual(1, (double)v.DerivativeBy('a'));
      AreExactlyEqual(0, (double)v.DerivativeBy('b'));

      AreEqual(2 * Term.a, 2 * v);
      AreEqual(0.5 * Term.a, v / 2);

      AreExactlyEqual(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Multiply_xp1_xm1()
    {
      Polynomial poly = (x + 1) * (x - 1);

      AreEqual(x * x - 1, poly);
      AreEqual(2 * x, poly.DerivativeBy(x));
      AreExactlyEqual(0, (double)poly.DerivativeBy('y'));
    }

    [Fact]
    public void Square_1p()
    {
      var expr = x + 1;
      AreEqual(x * x + 2 * x + 1, expr * expr);
    }

    [Fact]
    public void Square_2p()
    {
      var expr = x + y;
      AreEqual(x * x + 2 * x * y + y * y, expr * expr);
    }

    [Fact]
    public void Square_2p_value()
    {
      var expr = x + y + 1;
      AreEqual(x * x + y * y + 2 * x * y + 2 * x + 2 * y + 1, expr * expr);
    }

    [Fact]
    public void Square_2p_xy()
    {
      var expr = x + x * y;
      AreEqual(x * x + 2 * x * x * y + x * x * y * y, expr * expr);
    }

    [Fact]
    public void Square_2p_xy_3()
    {
      var expr = x + y + x * y;
      AreEqual(x * x * y * y + 2 * x * x * y + 2 * x * y * y + x * x + y * y + 2 * x * y, expr * expr);
    }

    [Fact]
    public void Multiply_by0()
    {
      Polynomial poly = (x + 1) * (x - 1);
      AreEqual(0, 0 * poly);
    }

    [Fact]
    public void Composition_Square()
    {
      var entry = 1 + x * x - y * y;
      AreEqual(2 + 2 * y, entry.Composition(x, y + 1));
    }

    [Fact]
    public void Composition_InPlace()
    {
      var entry = 1 + x * x - y * y;
      AreEqual(x * x + 2 * x + 2 - y * y, entry.Composition(x, x + 1));
    }

    [Fact]
    public void Composition_Constant()
    {
      var entry = 1 + x * x - y * y;
      AreEqual(1 - y * y, entry.Composition(x, 0));
    }

    [Fact]
    public void Power_ByZero()
    {      
      AreEqual(1, (x + 1).ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      AreEqual(x + 1, (x + 1).ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      AreEqual(x * x + 2 * x + 1, (x + 1).ToPower(2));
    }

    [Fact]
    public void Power_ByThree()
    {
      AreEqual(x * x * x + 3 * x * x + 3 * x + 1, (x + 1).ToPower(3));
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {      
      AreEqual(2 * (x + 1), (x + 1).DivideBy(0.5));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {     
      AreEqual(x * x + 3 * x + 9, (x * x * x - 27).DivideBy(x - 3));
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {     
      AreEqual(x * x + 3 * x + 9, (x * x * x - 25).DivideBy(x - 3, out Polynomial remainder));
      AreEqual(2, remainder);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {     
      Polynomial toDivide = 5 * (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      AreEqual(x - 1, toDivide.DivideBy(5 * (x * x - 1), out Polynomial remainder));
      AreEqual(2 * x + 3, remainder);
    }

    [Fact]
    public void Remainder_ByExpression()
    {     
      var toDivide = (x - 1) * (x - 1) * (x + 1) + 2 * x + 3;
      AreEqual(2 * x + 3, toDivide % (x * x - 1));
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {      
      Polynomial zero = 0;
      AreEqual(0, zero.DivideBy(x - 3));
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {      
      AreEqual(x + y, (x * x - y * y).DivideBy(x - y));
    }
  }
}
