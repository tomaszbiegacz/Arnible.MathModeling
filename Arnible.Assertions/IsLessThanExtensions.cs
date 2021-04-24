using System;

namespace Arnible.Assertions
{
  public static class IsLessThanExtensions2
  {
    public static void AssertIsLessThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) <= 0)
      {
        throw new AssertException($"Expected lower than {baseValue} got {currentValue}");
      }
    }
  }
}