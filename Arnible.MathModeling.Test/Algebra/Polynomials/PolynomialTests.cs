using Arnible.Assertions;
using Arnible.MathModeling.Test;
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

      ConditionExtensions.AssertIsTrue(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsSingleTerm);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      EqualExtensions.AssertEqualTo("0", v.ToString());

      EqualExtensions.AssertEqualTo(0, v);
      EqualExtensions.AssertEqualTo<double>(0, (double)v);
      ConditionExtensions.AssertIsFalse(1 == v);

      EqualExtensions.AssertEqualTo(0, v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(0, 2 * v);
      EqualExtensions.AssertEqualTo(0, v / 2);

      EqualExtensions.AssertEqualTo<double>(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      Polynomial v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsSingleTerm);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      EqualExtensions.AssertEqualTo("2", v.ToString());

      EqualExtensions.AssertEqualTo(2, v);
      EqualExtensions.AssertEqualTo<double>(2, (double)v);
      ConditionExtensions.AssertIsFalse(1 == v);
      ConditionExtensions.AssertIsFalse(0 == v);

      EqualExtensions.AssertEqualTo(0, v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(4, 2 * v);
      EqualExtensions.AssertEqualTo(1, v / 2);

      EqualExtensions.AssertEqualTo<double>(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      Polynomial v = 'a';
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsSingleTerm);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      EqualExtensions.AssertEqualTo("a", v.ToString());

      EqualExtensions.AssertEqualTo('a', (PolynomialTerm)v);

      EqualExtensions.AssertEqualTo<double>(1, (double)v.DerivativeBy('a'));
      EqualExtensions.AssertEqualTo<double>(0, (double)v.DerivativeBy('b'));

      EqualExtensions.AssertEqualTo(2 * Term.a, 2 * v);
      EqualExtensions.AssertEqualTo(0.5 * Term.a, v / 2);

      EqualExtensions.AssertEqualTo<double>(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Multiply_xp1_xm1()
    {
      Polynomial poly = (_x + 1) * (_x - 1);

      EqualExtensions.AssertEqualTo(_x * _x - 1, poly);
      EqualExtensions.AssertEqualTo(2 * _x, poly.DerivativeBy(_x));
      EqualExtensions.AssertEqualTo<double>(0, (double)poly.DerivativeBy('y'));
    }

    [Fact]
    public void Square_1p()
    {
      var expr = _x + 1;
      EqualExtensions.AssertEqualTo(_x * _x + 2 * _x + 1, expr * expr);
    }

    [Fact]
    public void Square_2p()
    {
      var expr = _x + _y;
      EqualExtensions.AssertEqualTo(_x * _x + 2 * _x * _y + _y * _y, expr * expr);
    }

    [Fact]
    public void Square_2p_value()
    {
      var expr = _x + _y + 1;
      EqualExtensions.AssertEqualTo(_x * _x + _y * _y + 2 * _x * _y + 2 * _x + 2 * _y + 1, expr * expr);
    }

    [Fact]
    public void Square_2p_xy()
    {
      var expr = _x + _x * _y;
      EqualExtensions.AssertEqualTo(_x * _x + 2 * _x * _x * _y + _x * _x * _y * _y, expr * expr);
    }

    [Fact]
    public void Square_2p_xy_3()
    {
      var expr = _x + _y + _x * _y;
      EqualExtensions.AssertEqualTo(_x * _x * _y * _y + 2 * _x * _x * _y + 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y, expr * expr);
    }

    [Fact]
    public void Multiply_by0()
    {
      Polynomial poly = (_x + 1) * (_x - 1);
      EqualExtensions.AssertEqualTo(0, 0 * poly);
    }

    [Fact]
    public void Composition_Square()
    {
      var entry = 1 + _x * _x - _y * _y;
      EqualExtensions.AssertEqualTo(2 + 2 * _y, entry.Composition(_x, _y + 1));
    }

    [Fact]
    public void Composition_InPlace()
    {
      var entry = 1 + _x * _x - _y * _y;
      EqualExtensions.AssertEqualTo(_x * _x + 2 * _x + 2 - _y * _y, entry.Composition(_x, _x + 1));
    }

    [Fact]
    public void Composition_Constant()
    {
      var entry = 1 + _x * _x - _y * _y;
      EqualExtensions.AssertEqualTo(1 - _y * _y, entry.Composition(_x, 0));
    }

    [Fact]
    public void Power_ByZero()
    {      
      EqualExtensions.AssertEqualTo(1, (_x + 1).ToPower(0));
    }

    [Fact]
    public void Power_ByOne()
    {
      EqualExtensions.AssertEqualTo(_x + 1, (_x + 1).ToPower(1));
    }

    [Fact]
    public void Power_ByTwo()
    {
      EqualExtensions.AssertEqualTo(_x * _x + 2 * _x + 1, (_x + 1).ToPower(2));
    }

    [Fact]
    public void Power_ByThree()
    {
      EqualExtensions.AssertEqualTo(_x * _x * _x + 3 * _x * _x + 3 * _x + 1, (_x + 1).ToPower(3));
    }

    [Fact]
    public void ReduceBy_ByConstant()
    {      
      EqualExtensions.AssertEqualTo(2 * (_x + 1), (_x + 1).DivideBy(0.5));
    }

    [Fact]
    public void ReduceBy_ByExpression()
    {     
      EqualExtensions.AssertEqualTo(_x * _x + 3 * _x + 9, (_x * _x * _x - 27).DivideBy(_x - 3));
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderConstant()
    {     
      EqualExtensions.AssertEqualTo(_x * _x + 3 * _x + 9, (_x * _x * _x - 25).DivideBy(_x - 3, out Polynomial remainder));
      EqualExtensions.AssertEqualTo(2, remainder);
    }

    [Fact]
    public void ReduceBy_ByExpression_RemainderExpression()
    {     
      Polynomial toDivide = 5 * (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      EqualExtensions.AssertEqualTo(_x - 1, toDivide.DivideBy(5 * (_x * _x - 1), out Polynomial remainder));
      EqualExtensions.AssertEqualTo(2 * _x + 3, remainder);
    }

    [Fact]
    public void Remainder_ByExpression()
    {     
      var toDivide = (_x - 1) * (_x - 1) * (_x + 1) + 2 * _x + 3;
      EqualExtensions.AssertEqualTo(2 * _x + 3, toDivide % (_x * _x - 1));
    }

    [Fact]
    public void ReduceBy_0ByExpression()
    {      
      Polynomial zero = 0;
      EqualExtensions.AssertEqualTo(0, zero.DivideBy(_x - 3));
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {      
      EqualExtensions.AssertEqualTo(_x + _y, (_x * _x - _y * _y).DivideBy(_x - _y));
    }
  }
}
