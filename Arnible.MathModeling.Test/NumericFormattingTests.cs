using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumericFormattingTests
  {
    [Theory]
    [InlineData(0, "⁰")]
    [InlineData(1, "¹")]
    [InlineData(123, "¹²³")]
    public void ToSuperscriptString_Uint(uint number, string expected)
    {
      AreEqual(expected, number.ToSuperscriptString());
    }
  }
}
