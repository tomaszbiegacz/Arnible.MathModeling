using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericFormattingTests
  {
    [Theory]
    [InlineData(0, "⁰")]
    [InlineData(1, "¹")]
    [InlineData(10, "¹⁰")]
    public void ToSuperscriptString(uint value, string result)
    {
      value.ToSuperscriptString().AssertIsEqualTo(result);
    }
  }
}