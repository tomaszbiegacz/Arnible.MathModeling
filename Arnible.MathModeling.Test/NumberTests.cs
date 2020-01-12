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
    }

    [Theory]
    [InlineData(0, 8.65956056235496E-17)]
    [InlineData(0.8660254037844386, 0.86602540378443871)]
    public void Equal_Rounding(double first, double second)
    {
      Assert.NotEqual(first, second);
      Assert.Equal<Number>(first, second);
    }

    [Theory]    
    [InlineData(double.NegativeInfinity, double.PositiveInfinity)]
    public void NotEqual_Both(double first, double second)
    {
      Assert.NotEqual(first, second);
      Assert.NotEqual<Number>(first, second);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void NotEqual_Different(double first)
    {
      Assert.Equal(first, first);
      Assert.NotEqual<Number>(first, first);
    }
  }
}
