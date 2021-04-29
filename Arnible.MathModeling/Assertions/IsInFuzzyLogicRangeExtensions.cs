using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsInFuzzyLogicRangeExtensions
  {
    public static void AssertIsInFuzzyLogicRange(this in Number value)
    {
      if (value < 0 || value > 1)
      {
        throw new AssertException(value.ToString());
      }
    }
  }
}