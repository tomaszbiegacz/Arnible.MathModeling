using Arnible.MathModelling.xunit;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    [Fact]
    public void Basic()
    {
      AssertNumber.Equal(default, 0);
      AssertNumber.Equal(2, 2);
      AssertNumber.Equal(-2, -2);

      AssertNumber.NotEqual(2, 0);
      AssertNumber.NotEqual(0, -2);
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0, 1.2246467991473532E-16)]
    [InlineData(0, -1.2246467991473532E-16)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      AssertNumber.NotEqualExact(first, second);
      AssertNumber.Equal(first, second);
    }

    [Fact]
    public void IntegralSigned()
    {
      Number v = -1;
      Assert.True(v == -1);
      Assert.False(v == 0);
    }

    [Fact]
    public void IntegralUnsigned()
    {
      Number v = 1;
      Assert.True(v == 1U);
      Assert.False(v == 0U);
    }

    [Fact]
    public void IntegralLong()
    {
      Number v = 1;
      Assert.True(v == 1L);
      Assert.False(v == 0L);
    }
  }
}
