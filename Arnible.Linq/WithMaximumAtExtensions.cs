using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WithMaximumAtExtensions
  {
    /// <summary>
    /// Finds position of record with maximum value
    /// </summary>
    public static ushort WithMaximumAt<T, TResult>(this IReadOnlyList<T> x, in Func<T, TResult> func)
      where TResult: IComparable<TResult>
    {
      if(x.Count == 0)
      {
        throw new ArgumentException(nameof(x));
      }
      
      ushort resultMaximumAt = 0;
      TResult resultMaximum = func(x[resultMaximumAt]);
      for(ushort i=1; i<x.Count; ++i)
      {
        TResult value = func(x[i]);
        if (resultMaximum.CompareTo(value) < 0)
        {
          resultMaximum = value;
          resultMaximumAt = i;
        }
      }
      return resultMaximumAt;
    }
  }
}