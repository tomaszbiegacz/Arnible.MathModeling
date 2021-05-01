using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WithMinimumExtensions
  {
    /// <summary>
    /// Finds record with minimum value
    /// </summary>
    public static T WithMinimum<T, TResult>(this IEnumerable<T> x, Func<T, TResult> func)
      where T: notnull
      where TResult: notnull, IComparable<TResult>
    {
      bool isResultKnown = false;
#pragma warning disable CS8600
      T result = default;
      TResult resultMinimum = default;
#pragma warning restore CS8600
      foreach (T v in x)
      {
        if (isResultKnown)
        {
          TResult value = func(v);
          if (resultMinimum!.CompareTo(value) > 0)
          {
            result = v;
            resultMinimum = value;
          }
        }
        else
        {
          result = v;
          resultMinimum = func(v);
          isResultKnown = true;
        }
      }
      
      if(isResultKnown)
      {
        return result!;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
  }
}