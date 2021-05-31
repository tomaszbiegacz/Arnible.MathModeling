using System;

namespace Arnible.Assertions
{
  public static class IsBetweenExtensions
  {
    public static void AssertIsBetween<T>(this T value, in T bottom, in T up) where T: IComparable<T>
    {
      if(value.CompareTo(bottom) < 0 || value.CompareTo(up) > 0)
      {
        throw new AssertException($"Expected value between {bottom} and {up} but got {value}");
      }
    }
    
    public static void AssertIsBetween<T>(in this ReadOnlySpan<T> values, in T bottom, in T up) where T: IComparable<T>
    {
      foreach(ref readonly T item in values)
      {
        item.AssertIsBetween(in bottom, in up);
      }
    }
    
    public static void AssertIsBetween<T>(in this Span<T> values, T bottom, T up) where T: IComparable<T>
    {
      AssertIsBetween((ReadOnlySpan<T>)values, in bottom, in up);
    }
  }
}