using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqPredicate
  {
    public static bool All(this IEnumerable<bool> col)
    {
      foreach (bool item in col)
      {
        if (!item)
        {
          return false;
        }
      }
      return true;
    }

    public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      return System.Linq.Enumerable.All(source, predicate);
    }

    public static bool Any<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Any(source);
    }

    public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      return System.Linq.Enumerable.Any(source, predicate);
    }
    
    public static uint? IndexOf<T>(this IEnumerable<T> src, Func<T, bool> predicate)
    {
      if (src == null)
      {
        throw new ArgumentNullException(nameof(src));
      }
      if (predicate == null)
      {
        throw new ArgumentNullException(nameof(predicate));
      }
      uint pos = 0;
      foreach (T item in src)
      {
        if (predicate(item))
        {
          return pos;
        }
        pos++;
      }

      return null;
    }    
  }
}
