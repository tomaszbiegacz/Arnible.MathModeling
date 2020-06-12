using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    [Fact]
    public void Basic()
    {
      AreEqual<Number>(default, 0);
      AreEqual<Number>(2, 2);
      AreEqual<Number>(-2, -2);

      AreNotEqual<Number>(2, 0);
      AreNotEqual<Number>(0, -2);
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0, 1.2246467991473532E-16)]
    [InlineData(0, -1.2246467991473532E-16)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      AreExactlyNotEqual(first, second);
      AreEqual<Number>(first, second);
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]        
    public void Less_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first < second);
      IsFalse(nf < ns);
    }

    [Theory]
    [InlineData(0, 1)]
    public void Less_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first < second);
      IsTrue(nf < ns);
    }

    [Theory]
    [InlineData(0, -1)]
    public void Less_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first < second);
      IsFalse(nf < ns);
    }

    [Theory]
    [InlineData(0, -8.65956056235496E-17)]
    public void LessOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first <= second);
      IsTrue(nf <= ns);
    }

    [Theory]
    [InlineData(0, 1)]
    public void LessOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first <= second);
      IsTrue(nf <= ns);
    }

    [Theory]
    [InlineData(0, -1)]
    public void LessOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first <= second);
      IsFalse(nf <= ns);
    }

    [Theory]
    [InlineData(8.65956056235496E-17, 0)]
    public void Greater_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first > second);
      IsFalse(nf > ns);
    }

    [Theory]
    [InlineData(1, 0)]
    public void Greater_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first > second);
      IsTrue(nf > ns);
    }

    [Theory]
    [InlineData(-1, 0)]
    public void Greater_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first > second);
      IsFalse(nf > ns);
    }

    [Theory]
    [InlineData(-8.65956056235496E-17, 0)]
    public void GreaterOrEqual_Rounding_Different(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first >= second);
      IsTrue(nf >= ns);
    }

    [Theory]
    [InlineData(1, 0)]
    public void GreaterOrEqual_Rounding_BothTrue(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsTrue(first >= second);
      IsTrue(nf >= ns);
    }

    [Theory]
    [InlineData(-1, 0)]
    public void GreaterOrEqual_Rounding_BothFalse(double first, double second)
    {
      Number nf = first;
      Number ns = second;

      IsFalse(first >= second);
      IsFalse(nf >= ns);
    }

    [Fact]
    public void IntegralSigned()
    {
      Number v = -1;
      IsTrue(v == -1);
      IsFalse(v == 0);
    }

    [Fact]
    public void IntegralUnsigned()
    {
      Number v = 1;
      IsTrue(v == 1U);
      IsFalse(v == 0U);
    }

    [Fact]
    public void IntegralLong()
    {
      Number v = 1;
      IsTrue(v == 1L);
      IsFalse(v == 0L);
    }
  }
}
