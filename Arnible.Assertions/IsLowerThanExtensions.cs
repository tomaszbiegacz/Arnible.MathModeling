using System;

namespace Arnible.Assertions
{
  public static class IsLowerThanExtensions
  {
    public static void AssertIsLowerThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) <= 0)
      {
        throw new AssertException($"Expected lower than {baseValue} got {currentValue}");
      }
    }
    
    public static void AssertIsLowerEqualThan<T>(this T currentValue, T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) < 0)
      {
        throw new AssertException($"Expected lower or equal than {baseValue} got {currentValue}");
      }
    }
  }
}