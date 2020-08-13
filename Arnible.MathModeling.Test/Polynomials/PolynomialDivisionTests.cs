using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Polynomials.Tests
{
  public class PolynomialDivisionTests
  {
    private readonly PolynomialTerm _x = 'x';
    private readonly PolynomialTerm _y = 'y';

    [Fact]
    public void Constructor_Default()
    {
      PolynomialDivision v = default;
      AreEqual(0, v);
      IsTrue(v == 0);
      IsTrue(v.IsPolynomial);
      IsTrue(v.IsConstant);
      AreEqual("0", v.ToString());

      AreExactlyEqual(0, (double)v.DerivativeBy('a'));

      AreEqual(0, 2 * v);
      AreEqual(0, v / 2);

      AreExactlyEqual(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialDivision v = 2;
      IsFalse(v == 0);
      IsTrue(v.IsPolynomial);
      IsTrue(v.IsConstant);
      AreEqual("2", v.ToString());

      AreEqual(2, v);
      AreEqual(2, (double)v);
      AreNotEqual(1, v);
      AreNotEqual(0, v);

      AreExactlyEqual(0, (double)v.DerivativeBy('a'));

      AreEqual(4, 2 * v);
      AreEqual(1, v / 2);

      AreExactlyEqual(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialDivision v = 'a';
      IsFalse(v == 0);
      IsFalse(v.IsConstant);
      IsTrue(v.IsPolynomial);
      AreEqual("a", v.ToString());

      AreEqual('a', (PolynomialTerm)v);

      AreExactlyEqual(1, (double)v.DerivativeBy('a'));
      AreExactlyEqual(0, (double)v.DerivativeBy('b'));

      AreEqual(2 * Term.a, 2 * v);
      AreEqual(0.5 * Term.a, v / 2);

      AreExactlyEqual(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Constructor_Polynomial()
    {
      Polynomial constant = 1;
      PolynomialDivision v = _x / constant;
      IsTrue(v.IsPolynomial);
      IsFalse(v.IsConstant);
      AreEqual("x", v.ToString());
    }

    [Fact]
    public void Constructor_PolynomialDivision()
    {
      PolynomialDivision v = _x / _y;
      IsFalse(v == 0);
      IsFalse(v.IsPolynomial);
      IsFalse(v.IsConstant);

      AreEqual(5, v.GetOperation('x', 'y').Value(10, 2));
    }    

    [Fact]
    public void Equality_Equals()
    {
      AreEqual(_x / _y, 'x' / (PolynomialTerm)'y');
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      IsExactlyZero((double)(zero / _x));
      AreEqual(zero / _x, zero / _y);
      AreEqual(0 * (_x / _y), zero / _y);
    }

    [Fact]
    public void Equality_NumeratorNotEqual()
    {
      AreNotEqual((_x + 1) / _y, _x / _y);
    }

    [Fact]
    public void Equality_DenominatorNotEqual()
    {
      AreNotEqual(_x / (_y + 1), _x / _y);
    }

    [Fact]
    public void Value_Simple()
    {
      var v = (_x + 1) / (_x - 3);
      AreEqual(3, v.Value(new Dictionary<char, double>
      {
        { 'x', 5 }
      }));
    }

    [Fact]
    public void Derivative_Simple()
    {
      var expression = 1 + (_y + _x + 1) / (2 * _x + 1);
      AreEqual(-1 * (2 * _y + 1) / (4 * _x * _x + 4 * _x + 1), expression.DerivativeBy(_x));
      AreEqual(1 / (2 * _x + 1), expression.DerivativeBy(_y));

      IsExactlyZero((double)expression.DerivativeBy(_y).DerivativeBy(_y));
    }

    [Fact]
    public void Derivative2_Simple()
    {
      var expression = (_x * _x * _y) / (2 * _x + 1);
      AreEqual((2 * _x * _y * (_x + 1)) / (4 * _x * _x + 4 * _x + 1), expression.DerivativeBy('x'));
      AreEqual((2 * _y) / (8 * _x * _x * _x + 12 * _x * _x + 6 * _x + 1), expression.Derivative2By('x'));
      IsExactlyZero((double)expression.Derivative2By('y'));
    }

    [Fact]
    public void Derivative2_NonZero()
    {
      var denominator = (_x + _y - _x * _y);
      var denominatorExpected = _x * _x * _y * _y - 2 * _x * _x * _y - 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y;
      AreEqual(denominatorExpected, denominator * denominator);

      var expression = (_x * _y) / denominator;
      AreEqual((_y * _y) / denominatorExpected, expression.DerivativeBy('x'));
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialDivision entry = (_x + 1) / (_x - 1);
      AreEqual((_y + 2) / _y, entry.Composition((char)_x, _y + 1));
    }

    [Fact]
    public void Composition_Division()
    {
      PolynomialDivision entry = _x / (_x + 1);
      AreEqual(_y / (_x + _y), entry.Composition(_x, _y / _x));
    }

    [Fact]
    public void Simplify_Polynomial_Division_x2_minus_1()
    {
      var expression = (_x * _x - _y * _y) / (_x - _y);
      AreEqual(_x + _y, (Polynomial)expression);
    }

    [Fact]
    public void Simplify_Polynomial_Division_PolynomialDivision_Polynomial()
    {
      var expression = _x * _x - _y * _y;
      var polymialDivision = (_x - _y) / (_x + 3);
      AreEqual((_x + 3) * (_x + _y), expression / polymialDivision);
    }

    [Fact]
    public void Simplify_Multiplication_x_plus_1()
    {
      var expression = 1 / (_x * _x - 1);
      AreEqual(1 / (_x - 1), expression * (_x + 1));
    }

    [Fact]
    public void Simplify_Division_x_plus_1()
    {
      var expression = (_x * _x - 1) / (_x + 3);
      AreEqual((_x - 1) / (_x + 3), expression / (_x + 1));
    }

    [Fact]
    public void Simplify_CommonTerm()
    {
      var numerator = _x * _x;
      var denominator = 2 * _x - _x * _x;
      AreEqual(_x / (2 - _x), numerator / denominator);
    }

    [Fact]
    public void Operator_Minus_Polynomial()
    {
      AreEqual(_x / (_x + 1), 1 - 1 / (_x + 1));
    }

    [Fact]
    public void Operator_Minus_Long()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x + 2);
      AreEqual((_x * _x + _x + 2) / (_x * _x - 4), a - b);
    }

    [Fact]
    public void Operator_Minus_Short()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x - 2);
      AreEqual((_x - 1) / (_x - 2), a - b);
    }

    [Fact]
    public void Operator_Multiply_Polynomial()
    {
      AreEqual((1 - 2 * _x + _x * _x) / (1 + _x), (1 - _x) / (1 + _x) * (1 - _x));
    }

    [Fact]
    public void Operator_Multiply_Long()
    {
      var a = (_x - 1) / (_x - 2);
      var b = (_x + 1) / (_x + 2);
      AreEqual((_x * _x - 1) / (_x * _x - 4), a * b);
    }

    [Fact]
    public void Operator_Divide_Polynomial()
    {
      AreEqual(1 / (1 - _x * _x), 1 / (1 - _x) / (1 + _x));
    }

    [Fact]
    public void Operator_Divide_Short()
    {
      var a = (_x - 1) / (2 * _x + 3);
      var b = (_x - 2) / (2 * _x + 3);
      AreEqual((_x - 1) / (_x - 2), a / b);
    }

    [Fact]
    public void Operator_Divide_Long()
    {
      var a = (_x - 1) / (_x + 2);
      var b = (_x - 2) / (_x + 1);
      AreEqual((_x * _x - 1) / (_x * _x - 4), a / b);
    }

    [Fact]
    public void ReduceBy()
    {
      var numerator = (_x - 1) * (_x + 1);
      var denominator = (_x - 1) * (_x + 2);
      var expr = numerator / denominator;

      // shortcommings of current library
      AreEqual((_x * _x - 1) / (_x * _x + _x - 2), expr);

      // test
      AreEqual((_x + 1) / (_x + 2), expr.ReduceBy(_x - 1));
    }

    [Fact]
    public void TryDivideBy_Positive()
    {
      var numerator = _x - 1;
      var denominator = _x + 2;
      var expr = numerator / denominator;

      // test
      PolynomialDivision result;
      IsTrue(expr.TryDivideBy(_x - 1, out result));
      AreEqual(1 / (_x + 2), result);
    }

    [Fact]
    public void TryDivideBy_Negative()
    {
      var numerator = _x - 1;
      var denominator = _x + 2;
      var expr = numerator / denominator + 1;

      // test
      IsFalse(expr.TryDivideBy(_x - 1, out _));
    }

    [Fact]
    public void TryDivideBy_Polynomial()
    {
      PolynomialDivision numerator = (_x - 1) * (_x + 2);

      // test
      PolynomialDivision result;
      IsTrue(numerator.TryDivideBy(_x - 1, out result));
      AreEqual(_x + 2, result);
    }

    [Fact]
    public void Power_By_2()
    {
      var expr = (_x + 1) / (_x - 3);
      AreEqual((_x * _x + 2 * _x + 1) / (_x * _x - 6 * _x + 9), expr.ToPower(2));
    }
  }
}
