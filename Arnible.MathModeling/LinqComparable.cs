using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqComparable
  {
    class BuildinComparerStruct<T> : IComparer<T> where T : struct, IComparable<T>
    {
      public int Compare(T x, T y) => x.CompareTo(y);
    }

    /*
     * Filtering
     */
    
    private static bool IsPresent<T>(in List<T> result, in T item) where T : IEquatable<T>
    {
      foreach (T accepted in result)
      {
        if (accepted.Equals(item))
        {
          return true;
        }
      }
      return false;
    }
    
    public static IReadOnlyCollection<T> Distinct<T>(this IEnumerable<T> collection) where T: IEquatable<T>
    {
      List<T> result = new List<T>();
      foreach (T candidate in collection)
      {
        if (!IsPresent(result, in candidate))
        {
          result.Add(candidate);
        }
      }

      return result;
    }
    
    public static int SequenceCompare<T>(this IEnumerable<T> col1, IEnumerable<T> col2) where T : struct, IComparable<T>
    {
      using var col1Enum = col1.GetEnumerator();
      using var col2Enum = col2.GetEnumerator();
      bool isCol1Valid = col1Enum.MoveNext();
      bool isCol2Valid = col2Enum.MoveNext();
      while (isCol1Valid && isCol2Valid)
      {
        int result = col1Enum.Current.CompareTo(col2Enum.Current);
        if (result != 0)
        {
          // first difference
          return result;
        }

        isCol1Valid = col1Enum.MoveNext();
        isCol2Valid = col2Enum.MoveNext();
      }

      if (isCol1Valid || isCol2Valid)
      {
        throw new InvalidOperationException("Collections are not the same size.");
      }

      // there is no difference
      return 0;
    }

    /*
     * Order
     */

    public static System.Linq.IOrderedEnumerable<TSource> Order<TSource>(
      this IEnumerable<TSource> collection) where TSource : struct, IComparable<TSource>
    {
      return System.Linq.Enumerable.OrderBy(collection, i => i, new BuildinComparerStruct<TSource>());
    }

    public static System.Linq.IOrderedEnumerable<TSource> OrderDescending<TSource>(
      this IEnumerable<TSource> collection) where TSource : struct, IComparable<TSource>
    {
      return System.Linq.Enumerable.OrderByDescending(collection, i => i, new BuildinComparerStruct<TSource>());
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

    public static IEnumerable<T> Reverse<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Reverse(source);
    }
  }
}
