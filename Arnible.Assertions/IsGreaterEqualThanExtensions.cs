using System;

namespace Arnible.Assertions
{
  public static class IsGreaterEqualThanExtensions
  {
    public static void AssertIsGreaterEqualThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) > 0)
      {
        throw new AssertException($"Expected greater or equal than {baseValue} got {currentValue}");
      }
    }
  }
}