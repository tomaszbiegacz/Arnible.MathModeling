using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class AllWithDefaultExtensions
  {
    public static bool AllWithDefault(this IEnumerable<bool> col)
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

    public static bool AllWithDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach(T item in source)
      {
        if (!predicate(item))
        {
          return false;
        }
      }
      return true;
    }
  }
}