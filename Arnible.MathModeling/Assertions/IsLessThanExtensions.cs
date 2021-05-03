using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsLessThanExtensions
  {
    public static void AssertIsLessThan(in this Number currentValue, in Number baseValue)
    {
      if(baseValue <= currentValue)
      {
        throw new AssertException($"Expected lower than {baseValue} got {currentValue}");
      }
    }
  }
}