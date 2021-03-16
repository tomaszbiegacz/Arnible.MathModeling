using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SelectExtensions
  {
    public static IEnumerable<TResult> Select<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, TResult> selector)
    {
      foreach(TSource item in source)
      {
        yield return selector(item);
      }
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<uint, TSource, TResult> selector)
    {
      uint i=0;
      foreach(TSource item in source)
      {
        yield return selector(i, item);
        i++;
      }
    }

    public static IEnumerable<TResult> SelectMany<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      foreach(TSource item in source)
      {
        foreach (TResult item2 in selector(item))
        {
          yield return item2;
        }
      }
    }
  }
}