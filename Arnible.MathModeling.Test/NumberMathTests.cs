using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberMathTests
  {
    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, 1)]
    public void AbsTests(double value, double expected)
    {
      Number v = value;
      v.Abs().AssertIsEqualTo(expected);
    }
  }
}