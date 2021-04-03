using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class OrderByExtensions
  {
    class BuildInComparerStruct<T> : IComparer<T> where T : struct, IComparable<T>
    {
      public int Compare(T x, T y) => x.CompareTo(y);
    }
    
    public static System.Linq.IOrderedEnumerable<TSource> Order<TSource>(
      this IEnumerable<TSource> collection
      ) where TSource : struct, IComparable<TSource>
    {
      return System.Linq.Enumerable.OrderBy(collection, i => i, new BuildInComparerStruct<TSource>());
    }

    public static System.Linq.IOrderedEnumerable<TSource> OrderDescending<TSource>(
      this IEnumerable<TSource> collection
      ) where TSource : struct, IComparable<TSource>
    {
      return System.Linq.Enumerable.OrderByDescending(collection, i => i, new BuildInComparerStruct<TSource>());
    }

    public static System.Linq.IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
      this IEnumerable<TSource> source,
      params Func<TSource, TKey>[] keySelector)
    {
      if (keySelector.Length == 0)
      {
        throw new ArgumentException(nameof(keySelector));
      }

      System.Linq.IOrderedEnumerable<TSource> result = System.Linq.Enumerable.OrderBy(source, keySelector[0]);
      for (uint i = 1; i < keySelector.Length; ++i)
      {
        result = System.Linq.Enumerable.ThenBy(result, keySelector[i]);
      }
      return result;
    }

    public static System.Linq.IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
      this IEnumerable<TSource> source,
      params Func<TSource, TKey>[] keySelector)
    {
      if (keySelector.Length == 0)
      {
        throw new ArgumentException(nameof(keySelector));
      }
      
      System.Linq.IOrderedEnumerable<TSource> result = System.Linq.Enumerable.OrderByDescending(source, keySelector[0]);
      for (uint i = 1; i < keySelector.Length; ++i)
      {
        result = System.Linq.Enumerable.ThenByDescending(result, keySelector[i]);
      }
      return result;      
    }

    public static System.Linq.IOrderedEnumerable<TSource> ThenOrderBy<TSource, TKey>(
      this System.Linq.IOrderedEnumerable<TSource> source,
      params Func<TSource, TKey>[] keySelector)
    {
      if (keySelector.Length == 0)
      {
        throw new ArgumentException(nameof(keySelector));
      }

      System.Linq.IOrderedEnumerable<TSource> result = source;
      foreach (Func<TSource, TKey> ks in keySelector)
      {
        result = System.Linq.Enumerable.ThenBy(result, ks);
      }
      return result;
    }
    
    public static System.Linq.IOrderedEnumerable<TSource> ThenOrderByDescending<TSource, TKey>(
      this System.Linq.IOrderedEnumerable<TSource> source,
      params Func<TSource, TKey>[] keySelector)
    {
      if (keySelector.Length == 0)
      {
        throw new ArgumentException(nameof(keySelector));
      }

      System.Linq.IOrderedEnumerable<TSource> result = source;
      foreach (Func<TSource, TKey> ks in keySelector)
      {
        result = System.Linq.Enumerable.ThenByDescending(result, ks);
      }
      return result;
    }
  }
}