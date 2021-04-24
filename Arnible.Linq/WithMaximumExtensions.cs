using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WithMaximumExtensions
  {
    /// <summary>
    /// Finds record with minimum value
    /// </summary>
    public static T WithMaximum<T, TResult>(this IEnumerable<T> x, Func<T, TResult> func)
      where T: notnull
      where TResult: notnull, IComparable<TResult>
    {
      bool isResultKnown = false;
      T result = default;
      TResult resultMaximum = default;
      foreach (T v in x)
      {
        if (isResultKnown)
        {
          TResult value = func(v);
          if (resultMaximum!.CompareTo(value) < 0)
          {
            result = v;
            resultMaximum = value;
          }
        }
        else
        {
          result = v;
          resultMaximum = func(v);
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