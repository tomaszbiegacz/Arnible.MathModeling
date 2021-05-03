using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsGreaterEqualThanExtensions
  {
    public static void AssertIsGreaterEqualThan(in this Number currentValue, in Number baseValue)
    {
      if(baseValue > currentValue)
      {
        throw new AssertException($"Expected greater or equal than {baseValue} got {currentValue}");
      }
    }
  }
}