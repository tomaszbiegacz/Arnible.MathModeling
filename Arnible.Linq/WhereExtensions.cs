using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class WhereExtensions
  {
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach(T item in source)
      {
        if(predicate(item))
        {
          yield return item;
        }
      }
    }
  }
}