using System;

namespace Arnible.Assertions
{
  public static class IsLessThanExtensions
  {
    public static void AssertIsLessThan<T>(this T currentValue, in T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) <= 0)
      {
        throw new AssertException($"Expected lower than {baseValue} got {currentValue}");
      }
    }
  }
}