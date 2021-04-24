using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling.Analysis;
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
      v.AssertIsEqualTo(0);
      (v == 0).AssertIsTrue();
      v.IsPolynomial.AssertIsTrue();
      v.IsConstant.AssertIsTrue();
      v.ToString().AssertIsEqualTo("0");

      v.DerivativeBy('a').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(0);
      (v / 2).AssertIsEqualTo(0);

      v.GetOperation().Value().AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialDivision v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      ConditionExtensions.AssertIsTrue(v.IsConstant);
      v.ToString().AssertIsEqualTo("2");

      v.AssertIsEqualTo(2);
      ConditionExtensions.AssertIsFalse(1 == v);
      ConditionExtensions.AssertIsFalse(0 == v);

      v.DerivativeBy('a').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(4);
      (v / 2).AssertIsEqualTo(1);

      v.GetOperation().Value().AssertIsEqualTo(2);
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialDivision v = 'a';
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      v.ToString().AssertIsEqualTo("a");

      ((PolynomialTerm)v).AssertIsEqualTo('a');

      v.DerivativeBy('a').AssertIsEqualTo(1);
      v.DerivativeBy('b').AssertIsEqualTo(0);

      (2 * v).AssertIsEqualTo(2 * Term.a);
      (v / 2).AssertIsEqualTo(0.5 * Term.a);

      v.GetOperation('a').Value(5).AssertIsEqualTo(5);
    }

    [Fact]
    public void Constructor_Polynomial()
    {
      Polynomial constant = 1;
      PolynomialDivision v = _x / constant;
      ConditionExtensions.AssertIsTrue(v.IsPolynomial);
      ConditionExtensions.AssertIsFalse(v.IsConstant);
      v.ToString().AssertIsEqualTo("x");
    }

    [Fact]
    public void Constructor_PolynomialDivision()
    {
      PolynomialDivision v = _x / _y;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsFalse(v.IsPolynomial);
      ConditionExtensions.AssertIsFalse(v.IsConstant);

      v.GetOperation('x', 'y').Value(10, 2).AssertIsEqualTo(5);
    }    

    [Fact]
    public void Equality_Equals()
    {
      ('x' / (PolynomialTerm)'y').AssertIsEqualTo(_x / _y);
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      (zero / _x).AssertIsEqualTo(0);
      (zero / _y).AssertIsEqualTo(zero / _x);
      (zero / _y).AssertIsEqualTo(0 * (_x / _y));
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
      v.Value(new Dictionary<char, double>
      {
        { 'x', 5 }
      }).AssertIsEqualTo(3);
    }

    [Fact]
    public void Derivative_Simple()
    {
      var expression = 1 + (_y + _x + 1) / (2 * _x + 1);
      expression.DerivativeBy(_x).AssertIsEqualTo(-1 * (2 * _y + 1) / (4 * _x * _x + 4 * _x + 1));
      expression.DerivativeBy(_y).AssertIsEqualTo(1 / (2 * _x + 1));

      expression.DerivativeBy(_y).DerivativeBy(_y).AssertIsEqualTo(0);
    }

    [Fact]
    public void Derivative2_Simple()
    {
      var expression = (_x * _x * _y) / (2 * _x + 1);
      expression.DerivativeBy('x').AssertIsEqualTo((2 * _x * _y * (_x + 1)) / (4 * _x * _x + 4 * _x + 1));
      expression.Derivative2By('x').AssertIsEqualTo((2 * _y) / (8 * _x * _x * _x + 12 * _x * _x + 6 * _x + 1));
      expression.Derivative2By('y').AssertIsEqualTo(0);
    }

    [Fact]
    public void Derivative2_NonZero()
    {
      var denominator = (_x + _y - _x * _y);
      var denominatorExpected = _x * _x * _y * _y - 2 * _x * _x * _y - 2 * _x * _y * _y + _x * _x + _y * _y + 2 * _x * _y;
      (denominator * denominator).AssertIsEqualTo(denominatorExpected);

      var expression = (_x * _y) / denominator;
      expression.DerivativeBy('x').AssertIsEqualTo((_y * _y) / denominatorExpected);
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialDivision entry = (_x + 1) / (_x - 1);
      entry.Composition((char)_x, _y + 1).AssertIsEqualTo((_y + 2) / _y);
    }

    [Fact]
    public void Composition_Division()
    {
      PolynomialDivision entry = _x / (_x + 1);
      entry.Composition(_x, _y / _x).AssertIsEqualTo(_y / (_x + _y));
    }

    [Fact]
    public void Simplify_Polynomial_Division_x2_minus_1()
    {
      var expression = (_x * _x - _y * _y) / (_x - _y);
      ((Polynomial)expression).AssertIsEqualTo(_x + _y);
    }

    [Fact]
    public void Simplify_Polynomial_Division_PolynomialDivision_Polynomial()
    {
      var expression = _x * _x - _y * _y;
      var polymialDivision = (_x - _y) / (_x + 3);
      (expression / polymialDivision).AssertIsEqualTo((_x + 3) * (_x + _y));
    }

    [Fact]
    public void Simplify_Multiplication_x_plus_1()
    {
      var expression = 1 / (_x * _x - 1);
      (expression * (_x + 1)).AssertIsEqualTo(1 / (_x - 1));
    }

    [Fact]
    public void Simplify_Division_x_plus_1()
    {
      var expression = (_x * _x - 1) / (_x + 3);
      (expression / (_x + 1)).AssertIsEqualTo((_x - 1) / (_x + 3));
    }

    [Fact]
    public void Simplify_CommonTerm()
    {
      var numerator = _x * _x;
      var denominator = 2 * _x - _x * _x;
      (numerator / denominator).AssertIsEqualTo(_x / (2 - _x));
    }

    [Fact]
    public void Operator_Minus_Polynomial()
    {
      (1 - 1 / (_x + 1)).AssertIsEqualTo(_x / (_x + 1));
    }

    [Fact]
    public void Operator_Minus_Long()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x + 2);
      (a - b).AssertIsEqualTo((_x * _x + _x + 2) / (_x * _x - 4));
    }

    [Fact]
    public void Operator_Minus_Short()
    {
      var a = _x / (_x - 2);
      var b = 1 / (_x - 2);
      (a - b).AssertIsEqualTo((_x - 1) / (_x - 2));
    }

    [Fact]
    public void Operator_Multiply_Polynomial()
    {
      ((1 - _x) / (1 + _x) * (1 - _x)).AssertIsEqualTo((1 - 2 * _x + _x * _x) / (1 + _x));
    }

    [Fact]
    public void Operator_Multiply_Long()
    {
      var a = (_x - 1) / (_x - 2);
      var b = (_x + 1) / (_x + 2);
      (a * b).AssertIsEqualTo((_x * _x - 1) / (_x * _x - 4));
    }

    [Fact]
    public void Operator_Divide_Polynomial()
    {
      (1 / (1 - _x) / (1 + _x)).AssertIsEqualTo(1 / (1 - _x * _x));
    }

    [Fact]
    public void Operator_Divide_Short()
    {
      var a = (_x - 1) / (2 * _x + 3);
      var b = (_x - 2) / (2 * _x + 3);
      (a / b).AssertIsEqualTo((_x - 1) / (_x - 2));
    }

    [Fact]
    public void Operator_Divide_Long()
    {
      var a = (_x - 1) / (_x + 2);
      var b = (_x - 2) / (_x + 1);
      (a / b).AssertIsEqualTo((_x * _x - 1) / (_x * _x - 4));
    }

    [Fact]
    public void ReduceBy()
    {
      var numerator = (_x - 1) * (_x + 1);
      var denominator = (_x - 1) * (_x + 2);
      var expr = numerator / denominator;

      // shortcommings of current library
      expr.AssertIsEqualTo((_x * _x - 1) / (_x * _x + _x - 2));

      // test
      expr.ReduceBy(_x - 1).AssertIsEqualTo((_x + 1) / (_x + 2));
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
      result.AssertIsEqualTo(1 / (_x + 2));
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
      result.AssertIsEqualTo(_x + 2);
    }

    [Fact]
    public void Power_By_2()
    {
      var expr = (_x + 1) / (_x - 3);
      expr.ToPower(2).AssertIsEqualTo((_x * _x + 2 * _x + 1) / (_x * _x - 6 * _x + 9));
    }
  }
}
