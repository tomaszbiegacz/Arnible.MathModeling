using Arnible.Assertions;
using Arnible.MathModeling.Analysis;
using Xunit;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialTests
  {
    private readonly PolynomialTerm _x = 'x';
    private readonly PolynomialTerm _y = 'y';

    [Fact]
    public void Constructor_Default()
    {
      Polynomial v = default;

      (v == 0).AssertIsTrue();
      v.IsSingleTerm.AssertIsTrue();
      v.IsConstant.AssertIsTrue();
      v.ToString().AssertIsEqualTo("0");

      v.AssertIsEqualTo(0);
      v.AssertIsEqualTo(0);
      (1 == v).AssertIsFalse();

      v.DerivativeBy('a').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(0);
      (v / 2).AssertIsEqualTo(0);

      v.GetOperation().Value().AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Constant()
    {
      Polynomial v = 2;
      (v == 0).AssertIsFalse();
      v.IsSingleTerm.AssertIsTrue();
      v.IsConstant.AssertIsTrue();
      v.ToString().AssertIsEqualTo("2");

      v.AssertIsEqualTo(2);
      v.AssertIsEqualTo(2);
      (1 == v).AssertIsFalse();
      (0 == v).AssertIsFalse();

      v.DerivativeBy('a').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(4);
      (v / 2).AssertIsEqualTo(1);

      v.GetOperation().Value().AssertIsEqualTo(2);
    }

    [Fact]
    public void Constructor_Variable()
    {
      Polynomial v = 'a';
      (v == 0).AssertIsFalse();
      v.IsSingleTerm.AssertIsTrue();
      v.IsConstant.AssertIsFalse();
      v.ToString().AssertIsEqualTo("a");

      ((PolynomialTerm)v).AssertIsEqualTo('a');

      v.DerivativeBy('a').AssertIsEqualTo(1);
      v.DerivativeBy('b').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(2 * Term.a);
      (v / 2).AssertIsEqualTo(0.5 * Term.a);

      v.GetOperation('a').Value(5).AssertIsEqualTo(5);
    }

    [Fact]
    public void Multiply_xp1_xm1()
    {
      Polynomial poly = (_x + 1) * (_x - 1);

      poly.AssertIsEqualTo(_x * _x - 1);
      poly.DerivativeBy(_x).AssertIsEqualTo(2 * _x);
      poly.DerivativeBy('y').AssertIsEqualTo(0);
    }

    [Fact]
    public void Square_1p()
    {
      var expr = _x + 1;
      (expr * expr).AssertIsEqualTo(_x * _x + 2 * _x + 1);
    }

    [Fact]
    public void Square_2p()
    {
      var expr = _x + _y;
      (expr * expr).AssertIsEqualTo(_x * _x + 2 * _x * _y + _y * _y);
    }

    [Fact]
    public void Square_2p_value()
    {
      var expr = _x + _y + 1;
      (expr * expr).AssertIsEqualTo(_x * _x + _y * _y + 2 * _x * _y + 2 * _x + 2 * _y + 1);
    }

    [Fact]
    public void Square_2p_xy()
    {
      var expr = _x + _x * _y;
      (expr * expr).AssertIsEqualTo(_x * _x + 2 * _x * _x * _y + _x * _x * _y * _y);
    }

    [Fact]
    public void Square_2p_xy_3()
    {
      var expr = _x + _y + _x * _y;
      (expr * expr).AssertIsEqualTo(_x * _x * _y * _y + 2 * _x * _x * _y + 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y);
    }

    [Fact]
    public void Multiply_by0()
    {
      Polynomial poly = (_x + 1) * (_x - 1);
      (0 * poly).AssertIsEqualTo(0);
    }

    [Fact]
    public void Composition_Square()
    {
      var entry = 1 + _x * _x - _y * _y;
      (entry.Composition(_x, _y + 1)).AssertIsEqualTo(2 + 2 * _y);
    }

    [Fact]
    public void Composition_InPlace()
    {
      var entry = 1 + _x * _x - _y * _y;
      entry.Composition(_x, _x + 1).AssertIsEqualTo(_x * _x + 2 * _x + 2 - _y * _y);
    }

    [Fact]
    public void Composition_Constant()
    {
      var entry = 1 + _x * _x - _y * _y;
      entry.Composition(_x, 0).AssertIsEqualTo(1 - _y * _y);
    }

    [Fact]
    public void Power_ByZero()
    {      
      (_x + 1).ToPower(0).AssertIsEqualTo(1);
    }

    [Fact]
    public void Power_ByOne()
    {
      (_x + 1).ToPower(1).AssertIsEqualTo(_x + 1);
    }

    [Fact]
    public void Power_ByTwo()
    {
      (_x + 1).ToPower(2).AssertIsEqualTo(_x * _x + 2 * _x + 1);
    }

    [Fact]
    public void Power_ByThree()
    {
      (_x + 1).ToPower(3).AssertIsEqualTo(_x * _x * _x + 3 * _x * _x + 3 * _x + 1);
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {      
      (_x + 1).DivideBy(0.5).AssertIsEqualTo(2 * (_x + 1));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {     
      (_x * _x * _x - 27).DivideBy(_x - 3).AssertIsEqualTo(_x * _x + 3 * _x + 9);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {     
      (_x * _x * _x - 25).DivideBy(_x - 3, out Polynomial remainder).AssertIsEqualTo(_x * _x + 3 * _x + 9);
      remainder.AssertIsEqualTo(2);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {     
      Polynomial toDivide = 5 * (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      toDivide.DivideBy(5 * (_x * _x - 1), out Polynomial remainder).AssertIsEqualTo(_x - 1);
      remainder.AssertIsEqualTo(2 * _x + 3);
    }

    [Fact]
    public void Remainder_ByExpression()
    {     
      var toDivide = (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      (toDivide % (_x * _x - 1)).AssertIsEqualTo(2 * _x + 3);
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {      
      Polynomial zero = 0;
      zero.DivideBy(_x - 3).AssertIsEqualTo(0);
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {      
      (_x * _x - _y * _y).DivideBy(_x - _y).AssertIsEqualTo(_x + _y);
    }
  }
}
