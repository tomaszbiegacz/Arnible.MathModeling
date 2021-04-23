using System;

namespace Arnible.Assertions
{
  public static class IsGreaterThanExtensions
  {
    public static void AssertIsGreaterThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) >= 0)
      {
        throw new AssertException($"Expected greater than {baseValue} got {currentValue}");
      }
    }
    
    public static void AssertIsGreaterEqualThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) > 0)
      {
        throw new AssertException($"Expected greater or equal than {baseValue} got {currentValue}");
      }
    }
    
    public static void AssertIsGreaterThan(this ushort actual, int baseValue)
    {
      if(baseValue >= actual)
      {
        throw new AssertException($"Expected greater than {baseValue}, got {actual}");
      }
    }
  }
}