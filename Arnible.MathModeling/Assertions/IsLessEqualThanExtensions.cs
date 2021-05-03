using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsLessEqualThanExtensions
  {
    public static void AssertIsLessEqualThan(in this Number currentValue, in Number baseValue)
    {
      if(baseValue < currentValue)
      {
        throw new AssertException($"Expected lower or equal than {baseValue} got {currentValue}");
      }
    }
  }
}