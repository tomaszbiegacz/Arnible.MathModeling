using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    [Fact]
    public void Basic()
    {
      Assert.Equal<Number>(default, 0);
      Assert.Equal<Number>(2, 2);
      Assert.Equal<Number>(-2, -2);

      Assert.NotEqual<Number>(2, 0);
      Assert.NotEqual<Number>(0, -2);
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0, 1.2246467991473532E-16)]
    [InlineData(0, -1.2246467991473532E-16)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      Assert.NotEqual(first, second);
      Assert.Equal<Number>(first, second);
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
