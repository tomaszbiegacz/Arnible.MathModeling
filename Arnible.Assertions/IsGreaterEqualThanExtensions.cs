using System;

namespace Arnible.Assertions
{
  public static class IsGreaterEqualThanExtensions
  {
    public static void AssertIsGreaterEqualThan<T>(this T currentValue, in T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) > 0)
      {
        throw new AssertException($"Expected greater or equal than {baseValue} got {currentValue}");
      }
    }
    
    public static void AssertIsGreaterEqualThan(this ushort currentValue, int baseValue)
    {
      if(baseValue > currentValue)
      {
        throw new AssertException($"Expected greater or equal than {baseValue} got {currentValue}");
      }
    }
  }
}