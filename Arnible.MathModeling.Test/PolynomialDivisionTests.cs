using System.Collections.Generic;
using Xunit;

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
      Assert.Equal(0, v);
      Assert.True(v.IsZero);
      Assert.True(v.IsPolynomial);
      Assert.True(v.IsConstant);
      Assert.Equal("0", v.ToString());

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(0, 2 * v);
      Assert.Equal(0, v / 2);

      Assert.Equal(0, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Constant()
    {
      PolynomialDivision v = 2;
      Assert.False(v.IsZero);
      Assert.True(v.IsPolynomial);
      Assert.True(v.IsConstant);
      Assert.Equal("2", v.ToString());

      Assert.Equal(2, v);
      Assert.Equal(2, (double)v);
      Assert.NotEqual(1, v);
      Assert.NotEqual(0, v);

      Assert.Equal(0, v.DerivativeBy('a'));

      Assert.Equal(4, 2 * v);
      Assert.Equal(1, v / 2);

      Assert.Equal(2, v.GetOperation().Value());
    }

    [Fact]
    public void Constructor_Variable()
    {
      PolynomialDivision v = 'a';
      Assert.False(v.IsZero);
      Assert.False(v.IsConstant);
      Assert.Equal("a", v.ToString());

      Assert.Equal('a', (PolynomialTerm)v);

      Assert.Equal(1, v.DerivativeBy('a'));
      Assert.Equal(0, v.DerivativeBy('b'));

      Assert.Equal(2 * Term.a, 2 * v);
      Assert.Equal(0.5 * Term.a, v / 2);

      Assert.Equal(5, v.GetOperation('a').Value(5));
    }

    [Fact]
    public void Constructor_Polynomial()
    {
      Polynomial x = 'x';
      Polynomial constant = 1;
      PolynomialDivision v = x / constant;
      Assert.True(v.IsPolynomial);
      Assert.False(v.IsConstant);
      Assert.Equal("x", v.ToString());
    }

    [Fact]
    public void Constructor_PolynomialDivision()
    {
      Polynomial x = 'x';
      Polynomial y = 'y';
      PolynomialDivision v = x / y;
      Assert.False(v.IsZero);
      Assert.False(v.IsPolynomial);
      Assert.False(v.IsConstant);
      Assert.Equal("x/y", v.ToString());

      Assert.Equal(5, v.GetOperation('x', 'y').Value(10, 2));
    }

    [Fact]
    public void ToString_Common()
    {
      Assert.Equal("(x+1)/y", ((x + 1) / y).ToString());
      Assert.Equal("x/(y+1)", (x / (y + 1)).ToString());
    }

    [Fact]
    public void Equality_Equals()
    {
      Assert.Equal(x / y, 'x' / (PolynomialTerm)'y');
    }

    [Fact]
    public void Equality_Zero()
    {
      PolynomialTerm zero = 0;

      Assert.True((zero / x).IsZero);
      Assert.Equal(zero / x, zero / y);
      Assert.Equal(0 * (x / y), zero / y);
    }

    [Fact]
    public void Equality_NumeratorNotEqual()
    {
      Assert.NotEqual((x + 1) / y, x / y);
    }

    [Fact]
    public void Equality_DenominatorNotEqual()
    {
      Assert.NotEqual(x / (y + 1), x / y);
    }

    [Fact]
    public void Value_Simple()
    {
      PolynomialTerm x = 'x';
      var v = (x + 1) / (x - 3);
      Assert.Equal(3, v.Value(new Dictionary<char, double>
      {
        { 'x', 5 }
      }));
    }

    [Fact]
    public void Derivative_Simple()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      var expression = 1 + (y + x + 1) / (2 * x + 1);
      Assert.Equal(-1 * (2 * y + 1) / (4 * x * x + 4 * x + 1), expression.DerivativeBy(x));
      Assert.Equal(1 / (2 * x + 1), expression.DerivativeBy(y));

      Assert.True(expression.DerivativeBy(y).DerivativeBy(y).IsZero);
    }

    [Fact]
    public void Derivative2_Simple()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      var expression = (x * x * y) / (2 * x + 1);
      Assert.Equal((2 * x * y * (x + 1)) / (4 * x * x + 4 * x + 1), expression.DerivativeBy('x'));
      Assert.Equal((2 * y) / (8 * x * x * x + 12 * x * x + 6 * x + 1), expression.Derivative2By('x'));
      Assert.True(expression.Derivative2By('y').IsZero);
    }

    [Fact]
    public void Derivative2_NonZero()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';      

      var denominator = (x + y - x * y);
      var denominatorExpected = x * x * y * y - 2 * x * x * y - 2 * x * y * y + x * x + y * y + 2 * x * y;
      Assert.Equal(denominatorExpected, denominator * denominator);

      var expression = (x * y) / denominator;
      Assert.Equal((y * y) / denominatorExpected, expression.DerivativeBy('x'));
    }

    [Fact]
    public void Composition_Square()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      PolynomialDivision entry = (x + 1) / (x - 1);
      Assert.Equal((y + 2) / y, entry.Composition((char)x, y + 1));
    }

    [Fact]
    public void Composition_Division()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      PolynomialDivision entry = x / (x + 1);
      Assert.Equal(y / (x + y), entry.Composition(x, y / x));
    }

    [Fact]
    public void Division_Simplification_x2_minus_1()
    {
      PolynomialTerm x = 'x';
      PolynomialTerm y = 'y';

      var expression = (x * x - y * y) / (x - y);
      Assert.Equal(x + y, (Polynomial)expression);
    }

    [Fact]
    public void Operator_Minus_Polynomial()
    {
      PolynomialTerm x = 'x';

      Assert.Equal(x / (x + 1), 1 - 1 / (x + 1));
    }

    [Fact]
    public void Operator_Minus_Long()
    {
      PolynomialTerm x = 'x';

      var a = x / (x - 2);
      var b = 1 / (x + 2);
      Assert.Equal((x * x + x + 2) / (x * x - 4), a - b);
    }

    [Fact]
    public void Operator_Minus_Short()
    {
      PolynomialTerm x = 'x';

      var a = x / (x - 2);
      var b = 1 / (x - 2);
      Assert.Equal((x - 1) / (x - 2), a - b);
    }

    [Fact]
    public void Operator_Multiply_Polynomial()
    {
      PolynomialTerm x = 'x';

      Assert.Equal((1 - 2 * x + x * x) / (1 + x), (1 - x) / (1 + x) * (1 - x));
    }

    [Fact]
    public void Operator_Multiply_Long()
    {
      PolynomialTerm x = 'x';

      var a = (x - 1) / (x - 2);
      var b = (x + 1) / (x + 2);
      Assert.Equal((x * x - 1) / (x * x - 4), a * b);
    }

    [Fact]
    public void Operator_Divide_Polynomial()
    {
      PolynomialTerm x = 'x';

      Assert.Equal(1 / (1 - x * x), 1 / (1 - x) / (1 + x));
    }

    [Fact]
    public void Operator_Divide_Short()
    {
      PolynomialTerm x = 'x';

      var a = (x - 1) / (2 * x + 3);
      var b = (x - 2) / (2 * x + 3);
      Assert.Equal((x - 1) / (x - 2), a / b);
    }

    [Fact]
    public void Operator_Divide_Long()
    {
      PolynomialTerm x = 'x';

      var a = (x - 1) / (x + 2);
      var b = (x - 2) / (x + 1);
      Assert.Equal((x * x - 1) / (x * x - 4), a / b);
    }
  }
}
