using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsInFuzzyLogicRangeNotSharpExtensions
  {
    public static void AssertIsInFuzzyLogicRangeNotSharp(this in Number value)
    {
      if (value <= 0 || value >= 1)
      {
        throw new AssertException(value.ToString());
      }
    }
  }
}