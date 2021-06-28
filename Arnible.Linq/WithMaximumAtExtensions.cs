using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WithMaximumAtExtensions
  {
    /// <summary>
    /// Finds position of record with maximum value
    /// </summary>
    public static ushort WithMaximumAt<T>(in this ReadOnlySpan<T> x) where T: IComparable<T>
    {
      if(x.Length == 0)
      {
        throw new ArgumentException(nameof(x));
      }
      
      ushort resultMaximumAt = 0;
      ref readonly T resultMaximum = ref x[resultMaximumAt];
      for(ushort i=1; i<x.Length; ++i)
      {
        if (resultMaximum.CompareTo(x[i]) < 0)
        {
          resultMaximum = ref x[i];
          resultMaximumAt = i;
        }
      }
      return resultMaximumAt;
    }
    
    public static ushort WithMaximumAt<T>(in this Span<T> x) where T: IComparable<T>
    {
      return WithMaximumAt((ReadOnlySpan<T>)x);
    }
    
    /// <summary>
    /// Finds position of record with maximum value
    /// </summary>
    public static ushort WithMaximumAt<T, TResult>(in this ReadOnlySpan<T> x, FuncIn<T, TResult> func)
      where TResult: IComparable<TResult>
    {
      if(x.IsEmpty)
      {
        throw new ArgumentException(nameof(x));
      }
      
      ushort resultMaximumAt = 0;
      TResult resultMaximum = func(x[resultMaximumAt]);
      for(ushort i=1; i<x.Length; ++i)
      {
        TResult value = func(in x[i]);
        if (resultMaximum.CompareTo(value) < 0)
        {
          resultMaximum = value;
          resultMaximumAt = i;
        }
      }
      return resultMaximumAt;
    }
    
    /// <summary>
    /// Finds position of record with maximum value
    /// </summary>
    public static ushort WithMaximumAt<T, TResult>(in this Span<T> x, FuncIn<T, TResult> func)
      where TResult: IComparable<TResult>
    {
      return WithMaximumAt<T, TResult>((ReadOnlySpan<T>)x, func);
    }
  }
}