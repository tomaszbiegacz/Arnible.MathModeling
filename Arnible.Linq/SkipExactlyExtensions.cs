using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SkipExactlyExtensions
  {
    public static IEnumerable<T> SkipExactly<T>(this IEnumerable<T> x, uint length)
    {
      uint i = 0;
      foreach (T item in x)
      {
        if (i >= length)
        {
          yield return item;
        }
        i++;
      }
      if (i < length)
      {
        throw new ArgumentException($"Enumerator length {i}, hence I can't skip {length}");
      }
    }
  }
}