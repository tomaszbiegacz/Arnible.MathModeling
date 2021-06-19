using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WithMinimumAtExtensions
  {
    /// <summary>
    /// Finds position of record with minimum value
    /// </summary>
    public static ushort WithMinimumAt<T, TResult>(this IReadOnlyList<T> x, Func<T, TResult> func)
      where TResult: IComparable<TResult>
    {
      if(x.Count == 0)
      {
        throw new ArgumentException(nameof(x));
      }
      
      ushort resultMinimumAt = 0;
      TResult resultMinimum = func(x[resultMinimumAt]);
      for(ushort i=1; i<x.Count; ++i)
      {
        TResult value = func(x[i]);
        if (resultMinimum.CompareTo(value) > 0)
        {
          resultMinimum = value;
          resultMinimumAt = i;
        }
      }
      return resultMinimumAt;
    }
    
    /// <summary>
    /// Finds position of record with minimum value
    /// </summary>
    public static ushort WithMinimumAt<T>(in this ReadOnlySpan<T> x) where T: IComparable<T>
    {
      if(x.Length == 0)
      {
        throw new ArgumentException(nameof(x));
      }
      
      ushort resultMinimumAt = 0;
      ref readonly T resultMinimum = ref x[resultMinimumAt];
      for(ushort i=1; i<x.Length; ++i)
      {
        if (resultMinimum.CompareTo(x[i]) > 0)
        {
          resultMinimum = ref x[i];
          resultMinimumAt = i;
        }
      }
      return resultMinimumAt;
    }
    
    public static ushort WithMinimumAt<T>(in this Span<T> x) where T: IComparable<T>
    {
      return WithMinimumAt((ReadOnlySpan<T>)x);
    }
  }
}