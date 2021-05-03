using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsBetweenExtensions
  {
    public static void AssertIsBetween(in this Number value, in Number bottom, in Number up)
    {
      if(value < bottom || value > up)
      {
        throw new AssertException($"Expected value between {bottom} and {up} but got {value}");
      }
    }
  }
}