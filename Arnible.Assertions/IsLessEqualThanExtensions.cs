using System;

namespace Arnible.Assertions
{
  public static class IsLessEqualThanExtensions
  {
    public static void AssertIsLessEqualThan<T>(this T currentValue, in T baseValue) where T: IComparable<T>
    {
      if(baseValue.CompareTo(currentValue) < 0)
      {
        throw new AssertException($"Expected lower or equal than {baseValue} got {currentValue}");
      }
    }
    
    public static void AssertIsLessEqualThan(this ushort currentValue, int baseValue)
    {
      if(baseValue < currentValue)
      {
        throw new AssertException($"Expected lower or equal than {baseValue} got {currentValue}");
      }
    }
  }
}