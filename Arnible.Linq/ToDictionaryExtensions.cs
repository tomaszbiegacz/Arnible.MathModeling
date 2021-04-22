using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LinqDictionaryExtensions
  {
    public static Dictionary<TKey, TSource> ToDictionary<TKey, TSource>(
      this IEnumerable<TSource> source, 
      Func<TSource, TKey> keySelector) where TKey: notnull
    {
      return System.Linq.Enumerable.ToDictionary(source, keySelector);
    }

    public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TValue> valueSelector) where TKey: notnull
    {
      return System.Linq.Enumerable.ToDictionary(source, keySelector, valueSelector);
    }
  }
}