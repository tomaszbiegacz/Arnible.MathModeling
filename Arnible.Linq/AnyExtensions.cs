using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class AnyExtensions
  {
    public static bool Any(this IEnumerable<bool> source)
    {
      foreach (bool item in source)
      {
        if (item)
        {
          return true;
        }
      }
      return false;
    }

    public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach (T item in source)
      {
        if (predicate(item))
        {
          return true;
        }
      }
      return false;
    }
  }
}