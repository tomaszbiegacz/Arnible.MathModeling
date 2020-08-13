using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Polynomials.Tests
{
  public class PolynomialTests
  {
    private readonly PolynomialTerm _x = 'x';
    private readonly PolynomialTerm _y = 'y';

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
      Polynomial poly = (_x + 1) * (_x - 1);

      AreEqual(_x * _x - 1, poly);
      AreEqual(2 * _x, poly.DerivativeBy(_x));
      AreExactlyEqual(0, (double)poly.DerivativeBy('y'));
    }

    [Fact]
    public void Square_1p()
    {
      var expr = _x + 1;
      AreEqual(_x * _x + 2 * _x + 1, expr * expr);
    }

    [Fact]
    public void Square_2p()
    {
      var expr = _x + _y;
      AreEqual(_x * _x + 2 * _x * _y + _y * _y, expr * expr);
    }

    [Fact]
    public void Square_2p_value()
    {
      var expr = _x + _y + 1;
      AreEqual(_x * _x + _y * _y + 2 * _x * _y + 2 * _x + 2 * _y + 1, expr * expr);
    }

    [Fact]
    public void Square_2p_xy()
    {
      var expr = _x + _x * _y;
      AreEqual(_x * _x + 2 * _x * _x * _y + _x * _x * _y * _y, expr * expr);
    }

    [Fact]
    public void Square_2p_xy_3()
    {
      var expr = _x + _y + _x * _y;
      AreEqual(_x * _x * _y * _y + 2 * _x * _x * _y + 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y, expr * expr);
    }

    [Fact]
    public void Multiply_by0()
    {
      Polynomial poly = (_x + 1) * (_x - 1);
      AreEqual(0, 0 * poly);
    }

    [Fact]
    public void Composition_Square()
    {
      var entry = 1 + _x * _x - _y * _y;
      AreEqual(2 + 2 * _y, entry.Composition(_x, _y + 1));
    }

    [Fact]
    public void Composition_InPlace()
    {
      var entry = 1 + _x * _x - _y * _y;
      AreEqual(_x * _x + 2 * _x + 2 - _y * _y, entry.Composition(_x, _x + 1));
    }

    [Fact]
    public void Composition_Constant()
    {
      var entry = 1 + _x * _x - _y * _y;
      AreEqual(1 - _y * _y, entry.Composition(_x, 0));
    }

    [Fact]
    public void Power_ByZero()
    {      
      AreEqual(1, (_x + 1).ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      AreEqual(_x + 1, (_x + 1).ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      AreEqual(_x * _x + 2 * _x + 1, (_x + 1).ToPower(2));
    }

    [Fact]
    public void Power_ByThree()
    {
      AreEqual(_x * _x * _x + 3 * _x * _x + 3 * _x + 1, (_x + 1).ToPower(3));
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {      
      AreEqual(2 * (_x + 1), (_x + 1).DivideBy(0.5));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {     
      AreEqual(_x * _x + 3 * _x + 9, (_x * _x * _x - 27).DivideBy(_x - 3));
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {     
      AreEqual(_x * _x + 3 * _x + 9, (_x * _x * _x - 25).DivideBy(_x - 3, out Polynomial remainder));
      AreEqual(2, remainder);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {     
      Polynomial toDivide = 5 * (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      AreEqual(_x - 1, toDivide.DivideBy(5 * (_x * _x - 1), out Polynomial remainder));
      AreEqual(2 * _x + 3, remainder);
    }

    [Fact]
    public void Remainder_ByExpression()
    {     
      var toDivide = (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      AreEqual(2 * _x + 3, toDivide % (_x * _x - 1));
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {      
      Polynomial zero = 0;
      AreEqual(0, zero.DivideBy(_x - 3));
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {      
      AreEqual(_x + _y, (_x * _x - _y * _y).DivideBy(_x - _y));
    }
  }
}
