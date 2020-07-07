using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqTransformation
  {
    public static IEnumerable<TResult> Select<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return System.Linq.Enumerable.Select(source, selector);
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<uint, TSource, TResult> selector)
    {
      return System.Linq.Enumerable.Select(source, (v, i) => selector((uint)i, v));
    }

    public static IEnumerable<TResult> SelectMany<TSource, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      return System.Linq.Enumerable.SelectMany(source, selector);
    }
  }
}
