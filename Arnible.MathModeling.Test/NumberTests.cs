using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    [Fact]
    public void Basic()
    {
      IsEqualToExtensions.AssertIsEqualTo<Number>(default, 0);
      IsEqualToExtensions.AssertIsEqualTo<Number>(2, 2);
      IsEqualToExtensions.AssertIsEqualTo<Number>(-2, -2);

      ConditionExtensions.AssertIsFalse(2 == 0);
      ConditionExtensions.AssertIsFalse(0 == -2);
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0, 1.2246467991473532E-16)]
    [InlineData(0, -1.2246467991473532E-16)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      ConditionExtensions.AssertIsFalse(first.Equals(second));
      IsEqualToExtensions.AssertIsEqualTo<Number>(first, second);
    }
    
    [Fact]
    public void Rounding_Array()
    {
      (new Number[] { 8.65956056235496E-17 }).AssertSequenceEqualsTo(new Number[] { 0 });
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]        
    public void Less_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first < second);
      ConditionExtensions.AssertIsFalse(nf < ns);
    }

    [Theory]
    [InlineData(0, 1)]
    public void Less_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first < second);
      ConditionExtensions.AssertIsTrue(nf < ns);
    }

    [Theory]
    [InlineData(0, -1)]
    public void Less_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first < second);
      ConditionExtensions.AssertIsFalse(nf < ns);
    }

    [Theory]
    [InlineData(0, -8.65956056235496E-17)]
    public void LessOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first <= second);
      ConditionExtensions.AssertIsTrue(nf <= ns);
    }

    [Theory]
    [InlineData(0, 1)]
    public void LessOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first <= second);
      ConditionExtensions.AssertIsTrue(nf <= ns);
    }

    [Theory]
    [InlineData(0, -1)]
    public void LessOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first <= second);
      ConditionExtensions.AssertIsFalse(nf <= ns);
    }

    [Theory]
    [InlineData(8.65956056235496E-17, 0)]
    public void Greater_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first > second);
      ConditionExtensions.AssertIsFalse(nf > ns);
    }

    [Theory]
    [InlineData(1, 0)]
    public void Greater_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first > second);
      ConditionExtensions.AssertIsTrue(nf > ns);
    }

    [Theory]
    [InlineData(-1, 0)]
    public void Greater_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first > second);
      ConditionExtensions.AssertIsFalse(nf > ns);
    }

    [Theory]
    [InlineData(-8.65956056235496E-17, 0)]
    public void GreaterOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first >= second);
      ConditionExtensions.AssertIsTrue(nf >= ns);
    }

    [Theory]
    [InlineData(1, 0)]
    public void GreaterOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsTrue(first >= second);
      ConditionExtensions.AssertIsTrue(nf >= ns);
    }

    [Theory]
    [InlineData(-1, 0)]
    public void GreaterOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      ConditionExtensions.AssertIsFalse(first >= second);
      ConditionExtensions.AssertIsFalse(nf >= ns);
    }

    [Fact]
    public void IntegralSigned()
    {
      Number v = -1;
      ConditionExtensions.AssertIsTrue(v == -1);
      ConditionExtensions.AssertIsFalse(v == 0);
    }

    [Fact]
    public void IntegralUnsigned()
    {
      Number v = 1;
      ConditionExtensions.AssertIsTrue(v == 1U);
      ConditionExtensions.AssertIsFalse(v == 0U);
    }

    [Fact]
    public void IntegralLong()
    {
      Number v = 1;
      ConditionExtensions.AssertIsTrue(v == 1L);
      ConditionExtensions.AssertIsFalse(v == 0L);
    }
  }
}
