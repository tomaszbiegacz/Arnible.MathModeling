using System;

namespace Arnible.Assertions
{
  public static class IsBetweenExtensions
  {
    public static void AssertIsBetween<T>(this T value, T bottom, T up) where T: IComparable<T>
    {
      if(value.CompareTo(bottom) < 0 || value.CompareTo(up) > 0)
      {
        throw new AssertException($"Expected value between {bottom} and {up} but got {value}");
      }
    }
  }
}