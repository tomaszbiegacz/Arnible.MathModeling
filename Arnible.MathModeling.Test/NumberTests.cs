using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    [Fact]
    public void Basic()
    {
      ((Number)0).AssertIsEqualTo(default);
      ((Number)2).AssertIsEqualTo(2);
      ((Number)(-2)).AssertIsEqualTo(-2);

      (2 == 0).AssertIsFalse();
      (0 == -2).AssertIsFalse();
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0, 1.2246467991473532E-16)]
    [InlineData(0, -1.2246467991473532E-16)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      first.Equals(second).AssertIsFalse();
      ((Number)first).AssertIsEqualTo(second);
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

      (first < second).AssertIsTrue();
      (nf < ns).AssertIsFalse();
    }

    [Theory]
    [InlineData(0, 1)]
    public void Less_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first < second).AssertIsTrue();
      (nf < ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(0, -1)]
    public void Less_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first < second).AssertIsFalse();
      (nf < ns).AssertIsFalse();
    }

    [Theory]
    [InlineData(0, -8.65956056235496E-17)]
    public void LessOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first <= second).AssertIsFalse();
      (nf <= ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(0, 1)]
    public void LessOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first <= second).AssertIsTrue();
      (nf <= ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(0, -1)]
    public void LessOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first <= second).AssertIsFalse();
      (nf <= ns).AssertIsFalse();
    }

    [Theory]
    [InlineData(8.65956056235496E-17, 0)]
    public void Greater_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first > second).AssertIsTrue();
      (nf > ns).AssertIsFalse();
    }

    [Theory]
    [InlineData(1, 0)]
    public void Greater_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first > second).AssertIsTrue();
      (nf > ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(-1, 0)]
    public void Greater_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first > second).AssertIsFalse();
      (nf > ns).AssertIsFalse();
    }

    [Theory]
    [InlineData(-8.65956056235496E-17, 0)]
    public void GreaterOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first >= second).AssertIsFalse();
      (nf >= ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(1, 0)]
    public void GreaterOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first >= second).AssertIsTrue();
      (nf >= ns).AssertIsTrue();
    }

    [Theory]
    [InlineData(-1, 0)]
    public void GreaterOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      (first >= second).AssertIsFalse();
      (nf >= ns).AssertIsFalse();
    }

    [Fact]
    public void IntegralSigned()
    {
      Number v = -1;
      (v == -1).AssertIsTrue();
      (v == 0).AssertIsFalse();
    }

    [Fact]
    public void IntegralUnsigned()
    {
      Number v = 1;
      (v == 1U).AssertIsTrue();
      (v == 0U).AssertIsFalse();
    }

    [Fact]
    public void IntegralLong()
    {
      Number v = 1;
      (v == 1L).AssertIsTrue();
      (v == 0L).AssertIsFalse();
    }
  }
}
