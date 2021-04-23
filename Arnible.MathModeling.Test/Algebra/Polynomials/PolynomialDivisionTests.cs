using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialDivisionTests
  {
    private readonly PolynomialTerm _x = 'x';
    private readonly PolynomialTerm _y = 'y';

    [Fact]
    public void Constructor_Default()
    {
      PolynomialDivision v = default;
      v.AssertEqualTo(0);
      (v == 0).AssertIsTrue();
      v.IsPolynomial.AssertIsTrue();
      v.IsConstant.AssertIsTrue();
      v.ToString().AssertEqualTo("0");

      EqualExtensions.AssertEqualTo<double>(0, (double)v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(0, 2 * v);
      EqualExtensions.AssertEqualTo(0, v / 2);

      EqualExtensions.AssertEqualTo<double>(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialDivision v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      EqualExtensions.AssertEqualTo("2", v.ToString());

      EqualExtensions.AssertEqualTo(2, v);
      EqualExtensions.AssertEqualTo(2, (double)v);
      ConditionExtensions.AssertIsFalse(1 == v);
      ConditionExtensions.AssertIsFalse(0 == v);

      EqualExtensions.AssertEqualTo<double>(0, (double)v.DerivativeBy('a'));

      EqualExtensions.AssertEqualTo(4, 2 * v);
      EqualExtensions.AssertEqualTo(1, v / 2);

      EqualExtensions.AssertEqualTo<double>(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialDivision v = 'a';
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      EqualExtensions.AssertEqualTo("a", v.ToString());

      EqualExtensions.AssertEqualTo('a', (PolynomialTerm)v);

      EqualExtensions.AssertEqualTo<double>(1, (double)v.DerivativeBy('a'));
      EqualExtensions.AssertEqualTo<double>(0, (double)v.DerivativeBy('b'));

      EqualExtensions.AssertEqualTo(2 * Term.a, 2 * v);
      EqualExtensions.AssertEqualTo(0.5 * Term.a, v / 2);

      EqualExtensions.AssertEqualTo<double>(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Constructor_Polynomial()
    {
      Polynomial constant = 1;
      PolynomialDivision v = _x / constant;
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      EqualExtensions.AssertEqualTo("x", v.ToString());
    }

    [Fact]
    public void Constructor_PolynomialDivision()
    {
      PolynomialDivision v = _x / _y;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsPolynomial);
      ConditionExtensions.AssertIsFalse(v.IsConstant);

      EqualExtensions.AssertEqualTo(5, v.GetOperation('x', 'y').Value(10, 2));
    }    

    [Fact]
    public void Equality_Equals()
    {
      EqualExtensions.AssertEqualTo(_x / _y, 'x' / (PolynomialTerm)'y');
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      EqualExtensions.AssertEqualTo(0, (double)(zero / _x));
      EqualExtensions.AssertEqualTo(zero / _x, zero / _y);
      EqualExtensions.AssertEqualTo(0 * (_x / _y), zero / _y);
    }

    [Fact]
    public void Equality_NumeratorNotEqual()
    {
      ConditionExtensions.AssertIsFalse((_x + 1) / _y == _x / _y);
    }

    [Fact]
    public void Equality_DenominatorNotEqual()
    {
      ConditionExtensions.AssertIsFalse(_x / (_y + 1) == _x / _y);
    }

    [Fact]
    public void Value_Simple()
    {
      var v = (_x + 1) / (_x - 3);
      EqualExtensions.AssertEqualTo(3, v.Value(new Dictionary<char, double>
      {
        { 'x', 5 }
      }));
    }

    [Fact]
    public void Derivative_Simple()
    {
      var expression = 1 + (_y + _x + 1) / (2 * _x + 1);
      EqualExtensions.AssertEqualTo(-1 * (2 * _y + 1) / (4 * _x * _x + 4 * _x + 1), expression.DerivativeBy(_x));
      EqualExtensions.AssertEqualTo(1 / (2 * _x + 1), expression.DerivativeBy(_y));

      EqualExtensions.AssertEqualTo<double>(0, (double)expression.DerivativeBy(_y).DerivativeBy(_y));
    }

    [Fact]
    public void Derivative2_Simple()
    {
      var expression = (_x * _x * _y) / (2 * _x + 1);
      EqualExtensions.AssertEqualTo((2 * _x * _y * (_x + 1)) / (4 * _x * _x + 4 * _x + 1), expression.DerivativeBy('x'));
      EqualExtensions.AssertEqualTo((2 * _y) / (8 * _x * _x * _x + 12 * _x * _x + 6 * _x + 1), expression.Derivative2By('x'));
      EqualExtensions.AssertEqualTo<double>(0, (double)expression.Derivative2By('y'));
    }

    [Fact]
    public void Derivative2_NonZero()
    {
      var denominator = (_x + _y - _x * _y);
      var denominatorExpected = _x * _x * _y * _y - 2 * _x * _x * _y - 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y;
      EqualExtensions.AssertEqualTo(denominatorExpected, denominator * denominator);

      var expression = (_x * _y) / denominator;
      EqualExtensions.AssertEqualTo((_y * _y) / denominatorExpected, expression.DerivativeBy('x'));
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialDivision entry = (_x + 1) / (_x - 1);
      EqualExtensions.AssertEqualTo((_y + 2) / _y, entry.Composition((char)_x, _y + 1));
    }

    [Fact]
    public void Composition_Division()
    {
      PolynomialDivision entry = _x / (_x + 1);
      EqualExtensions.AssertEqualTo(_y / (_x + _y), entry.Composition(_x, _y / _x));
    }

    [Fact]
    public void Simplify_Polynomial_Division_x2_minus_1()
    {
      var expression = (_x * _x - _y * _y) / (_x - _y);
      EqualExtensions.AssertEqualTo(_x + _y, (Polynomial)expression);
    }

    [Fact]
    public void Simplify_Polynomial_Division_PolynomialDivision_Polynomial()
    {
      var expression = _x * _x - _y * _y;
      var polymialDivision = (_x - _y) / (_x + 3);
      EqualExtensions.AssertEqualTo((_x + 3) * (_x + _y), expression / polymialDivision);
    }

    [Fact]
    public void Simplify_Multiplication_x_plus_1()
    {
      var expression = 1 / (_x * _x - 1);
      EqualExtensions.AssertEqualTo(1 / (_x - 1), expression * (_x + 1));
    }

    [Fact]
    public void Simplify_Division_x_plus_1()
    {
      var expression = (_x * _x - 1) / (_x + 3);
      EqualExtensions.AssertEqualTo((_x - 1) / (_x + 3), expression / (_x + 1));
    }

    [Fact]
    public void Simplify_CommonTerm()
    {
      var numerator = _x * _x;
      var denominator = 2 * _x - _x * _x;
      EqualExtensions.AssertEqualTo(_x / (2 - _x), numerator / denominator);
    }

    [Fact]
    public void Operator_Minus_Polynomial()
    {
      EqualExtensions.AssertEqualTo(_x / (_x + 1), 1 - 1 / (_x + 1));
    }

    [Fact]
    public void Operator_Minus_Long()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x + 2);
      EqualExtensions.AssertEqualTo((_x * _x + _x + 2) / (_x * _x - 4), a - b);
    }

    [Fact]
    public void Operator_Minus_Short()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x - 2);
      EqualExtensions.AssertEqualTo((_x - 1) / (_x - 2), a - b);
    }

    [Fact]
    public void Operator_Multiply_Polynomial()
    {
      EqualExtensions.AssertEqualTo((1 - 2 * _x + _x * _x) / (1 + _x), (1 - _x) / (1 + _x) * (1 - _x));
    }

    [Fact]
    public void Operator_Multiply_Long()
    {
      var a = (_x - 1) / (_x - 2);
      var b = (_x + 1) / (_x + 2);
      EqualExtensions.AssertEqualTo((_x * _x - 1) / (_x * _x - 4), a * b);
    }

    [Fact]
    public void Operator_Divide_Polynomial()
    {
      EqualExtensions.AssertEqualTo(1 / (1 - _x * _x), 1 / (1 - _x) / (1 + _x));
    }

    [Fact]
    public void Operator_Divide_Short()
    {
      var a = (_x - 1) / (2 * _x + 3);
      var b = (_x - 2) / (2 * _x + 3);
      EqualExtensions.AssertEqualTo((_x - 1) / (_x - 2), a / b);
    }

    [Fact]
    public void Operator_Divide_Long()
    {
      var a = (_x - 1) / (_x + 2);
      var b = (_x - 2) / (_x + 1);
      EqualExtensions.AssertEqualTo((_x * _x - 1) / (_x * _x - 4), a / b);
    }

    [Fact]
    public void ReduceBy()
    {
      var numerator = (_x - 1) * (_x + 1);
      var denominator = (_x - 1) * (_x + 2);
      var expr = numerator / denominator;

      // shortcommings of current library
      EqualExtensions.AssertEqualTo((_x * _x - 1) / (_x * _x + _x - 2), expr);

      // test
      EqualExtensions.AssertEqualTo((_x + 1) / (_x + 2), expr.ReduceBy(_x - 1));
    }

    [Fact]
    public void TryDivideBy_Positive()
    {
      var numerator = _x - 1;
      var denominator = _x + 2;
      var expr = numerator / denominator;

      // test
      PolynomialDivision result;
      ConditionExtensions.AssertIsTrue(expr.TryDivideBy(_x - 1, out result));
      EqualExtensions.AssertEqualTo(1 / (_x + 2), result);
    }

    [Fact]
    public void TryDivideBy_Negative()
    {
      var numerator = _x - 1;
      var denominator = _x + 2;
      var expr = numerator / denominator + 1;

      // test
      ConditionExtensions.AssertIsFalse(expr.TryDivideBy(_x - 1, out _));
    }

    [Fact]
    public void TryDivideBy_Polynomial()
    {
      PolynomialDivision numerator = (_x - 1) * (_x + 2);

      // test
      PolynomialDivision result;
      ConditionExtensions.AssertIsTrue(numerator.TryDivideBy(_x - 1, out result));
      EqualExtensions.AssertEqualTo(_x + 2, result);
    }

    [Fact]
    public void Power_By_2()
    {
      var expr = (_x + 1) / (_x - 3);
      EqualExtensions.AssertEqualTo((_x * _x + 2 * _x + 1) / (_x * _x - 6 * _x + 9), expr.ToPower(2));
    }
  }
}
