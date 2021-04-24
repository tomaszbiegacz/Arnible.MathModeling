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
  }
}