using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class PolynomialDivisionTests
  {
    private readonly PolynomialTerm x = 'x';
    private readonly PolynomialTerm y = 'y';

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
      PolynomialDivision v = x / constant;
      IsTrue(v.IsPolynomial);
      IsFalse(v.IsConstant);
      AreEqual("x", v.ToString());
    }

    [Fact]
    public void Constructor_PolynomialDivision()
    {
      PolynomialDivision v = x / y;
      IsFalse(v == 0);
      IsFalse(v.IsPolynomial);
      IsFalse(v.IsConstant);
      AreEqual("x/y", v.ToString());

      AreEqual(5, v.GetOperation('x', 'y').Value(10, 2));
    }

    [Fact]
    public void ToString_Common()
    {
      AreEqual("(x+1)/y", ((x + 1) / y).ToString());
      AreEqual("x/(y+1)", (x / (y + 1)).ToString());
    }

    [Fact]
    public void Equality_Equals()
    {
      AreEqual(x / y, 'x' / (PolynomialTerm)'y');
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      IsExactlyZero((double)(zero / x));
      AreEqual(zero / x, zero / y);
      AreEqual(0 * (x / y), zero / y);
    }

    [Fact]
    public void Equality_NumeratorNotEqual()
    {
      AreNotEqual((x + 1) / y, x / y);
    }

    [Fact]
    public void Equality_DenominatorNotEqual()
    {
      AreNotEqual(x / (y + 1), x / y);
    }

    [Fact]
    public void Value_Simple()
    {
      var v = (x + 1) / (x - 3);
      AreEqual(3, v.Value(new Dictionary<char, double>
      {
        { 'x', 5 }
      }));
    }

    [Fact]
    public void Derivative_Simple()
    {
      var expression = 1 + (y + x + 1) / (2 * x + 1);
      AreEqual(-1 * (2 * y + 1) / (4 * x * x + 4 * x + 1), expression.DerivativeBy(x));
      AreEqual(1 / (2 * x + 1), expression.DerivativeBy(y));

      IsExactlyZero((double)expression.DerivativeBy(y).DerivativeBy(y));
    }

    [Fact]
    public void Derivative2_Simple()
    {
      var expression = (x * x * y) / (2 * x + 1);
      AreEqual((2 * x * y * (x + 1)) / (4 * x * x + 4 * x + 1), expression.DerivativeBy('x'));
      AreEqual((2 * y) / (8 * x * x * x + 12 * x * x + 6 * x + 1), expression.Derivative2By('x'));
      IsExactlyZero((double)expression.Derivative2By('y'));
    }

    [Fact]
    public void Derivative2_NonZero()
    {
      var denominator = (x + y - x * y);
      var denominatorExpected = x * x * y * y - 2 * x * x * y - 2 * x * y * y + x * x + y * y + 2 * x * y;
      AreEqual(denominatorExpected, denominator * denominator);

      var expression = (x * y) / denominator;
      AreEqual((y * y) / denominatorExpected, expression.DerivativeBy('x'));
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialDivision entry = (x + 1) / (x - 1);
      AreEqual((y + 2) / y, entry.Composition((char)x, y + 1));
    }

    [Fact]
    public void Composition_Division()
    {
      PolynomialDivision entry = x / (x + 1);
      AreEqual(y / (x + y), entry.Composition(x, y / x));
    }

    [Fact]
    public void Simplify_Polynomial_Division_x2_minus_1()
    {
      var expression = (x * x - y * y) / (x - y);
      AreEqual(x + y, (Polynomial)expression);
    }

    [Fact]
    public void Simplify_Polynomial_Division_PolynomialDivision_Polynomial()
    {
      var expression = x * x - y * y;
      var polymialDivision = (x - y) / (x + 3);
      AreEqual((x + 3) * (x + y), expression / polymialDivision);
    }

    [Fact]
    public void Simplify_Multiplication_x_plus_1()
    {
      var expression = 1 / (x * x - 1);
      AreEqual(1 / (x - 1), expression * (x + 1));
    }

    [Fact]
    public void Simplify_Division_x_plus_1()
    {
      var expression = (x * x - 1) / (x + 3);
      AreEqual((x - 1) / (x + 3), expression / (x + 1));
    }

    [Fact]
    public void Simplify_CommonTerm()
    {
      var numerator = x * x;
      var denominator = 2 * x - x * x;
      AreEqual(x / (2 - x), numerator / denominator);
    }

    [Fact]
    public void Operator_Minus_Polynomial()
    {
      AreEqual(x / (x + 1), 1 - 1 / (x + 1));
    }

    [Fact]
    public void Operator_Minus_Long()
    {
      var a = x / (x - 2);
      var b = 1 / (x + 2);
      AreEqual((x * x + x + 2) / (x * x - 4), a - b);
    }

    [Fact]
    public void Operator_Minus_Short()
    {
      var a = x / (x - 2);
      var b = 1 / (x - 2);
      AreEqual((x - 1) / (x - 2), a - b);
    }

    [Fact]
    public void Operator_Multiply_Polynomial()
    {
      AreEqual((1 - 2 * x + x * x) / (1 + x), (1 - x) / (1 + x) * (1 - x));
    }

    [Fact]
    public void Operator_Multiply_Long()
    {
      var a = (x - 1) / (x - 2);
      var b = (x + 1) / (x + 2);
      AreEqual((x * x - 1) / (x * x - 4), a * b);
    }

    [Fact]
    public void Operator_Divide_Polynomial()
    {
      AreEqual(1 / (1 - x * x), 1 / (1 - x) / (1 + x));
    }

    [Fact]
    public void Operator_Divide_Short()
    {
      var a = (x - 1) / (2 * x + 3);
      var b = (x - 2) / (2 * x + 3);
      AreEqual((x - 1) / (x - 2), a / b);
    }

    [Fact]
    public void Operator_Divide_Long()
    {
      var a = (x - 1) / (x + 2);
      var b = (x - 2) / (x + 1);
      AreEqual((x * x - 1) / (x * x - 4), a / b);
    }

    [Fact]
    public void ReduceBy()
    {
      var numerator = (x - 1) * (x + 1);
      var denominator = (x - 1) * (x + 2);
      var expr = numerator / denominator;

      // shortcommings of current library
      AreEqual((x * x - 1) / (x * x + x - 2), expr);

      // test
      AreEqual((x + 1) / (x + 2), expr.ReduceBy(x - 1));
    }

    [Fact]
    public void TryDivideBy_Positive()
    {
      var numerator = x - 1;
      var denominator = x + 2;
      var expr = numerator / denominator;

      // test
      PolynomialDivision result;
      IsTrue(expr.TryDivideBy(x - 1, out result));
      AreEqual(1 / (x + 2), result);
    }

    [Fact]
    public void TryDivideBy_Negative()
    {
      var numerator = x - 1;
      var denominator = x + 2;
      var expr = numerator / denominator + 1;

      // test
      IsFalse(expr.TryDivideBy(x - 1, out _));
    }

    [Fact]
    public void TryDivideBy_Polynomial()
    {
      PolynomialDivision numerator = (x - 1) * (x + 2);

      // test
      PolynomialDivision result;
      IsTrue(numerator.TryDivideBy(x - 1, out result));
      AreEqual(x + 2, result);
    }

    [Fact]
    public void Power_By_2()
    {
      var expr = (x + 1) / (x - 3);
      AreEqual((x * x + 2 * x + 1) / (x * x - 6 * x + 9), expr.ToPower(2));
    }
  }
}
