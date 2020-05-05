using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqAggregate
  {
    /// <summary>
    /// Group items by "keySelector" and return aggregation for each group
    /// </summary>
    public static IEnumerable<TResult> AggregateBy<TSource, TKey, TResult>(
      this IEnumerable<TSource> source, 
      Func<TSource, TKey> keySelector, 
      Func<IEnumerable<TSource>, TResult> aggregator)
    {
      return System.Linq.Enumerable.GroupBy(source, keySelector).Select(g => aggregator(g));
    }

    private static IEnumerable<T> AggregateCombinations<T>(T[] x, uint i, uint groupCount, Func<IEnumerable<T>, T> aggregator, Stack<T> combination)
    {
      if (groupCount == combination.Count)
      {
        yield return aggregator(combination);
      }
      else
      {
        uint combinationLength = (uint)combination.Count;
        for (uint j = i; j < x.Length; ++j)
        {
          combination.Push(x[j]);
          foreach (T v in AggregateCombinations(x, j + 1, groupCount, aggregator, combination))
          {
            yield return v;
          }

          combination.Pop();
          if (combination.Count != combinationLength)
          {
            throw new InvalidOperationException($"Got {combination.Count} values, expected {combinationLength}.");
          }
        }
      }
    }

    /// <summary>
    /// Calculate aggregate for "groupSize" items count combinations from source collection.
    /// </summary>
    public static IEnumerable<T> AggregateCombinations<T>(this IEnumerable<T> items, uint groupSize, Func<IEnumerable<T>, T> aggregator)
    {
      if (groupSize < 1)
      {
        throw new ArgumentException(nameof(groupSize));
      }

      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToArray();
      if (x.Length < groupSize)
      {
        throw new ArgumentException($"x.Length: {x.Length} where groupCount: {groupSize}");
      }
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<T>();
      return AggregateCombinations(x, 0, groupSize, aggregator, combination);
    }

    /// <summary>
    /// Calculate aggregate for items count from 1 to collection size
    /// </summary>
    public static IEnumerable<T> AggregateCombinationsAll<T>(this IEnumerable<T> items, Func<IEnumerable<T>, T> aggregator)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToArray();
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<T>();
      for (uint groupCount = 1; groupCount <= x.Length; ++groupCount)
      {
        foreach (T item in AggregateCombinations(x, 0, groupCount, aggregator, combination))
        {
          yield return item;
        }
      }
    }

    /// <summary>
    /// Returns total items count in the sequence
    /// </summary>
    public static uint Count<T>(this IEnumerable<T> source)
    {
      return (uint)System.Linq.Enumerable.LongCount(source);
    }

    /// <summary>
    /// Applies a specified function to the corresponding elements of two sequences,
    /// producing a sequence of the results.
    /// </summary>
    public static IEnumerable<TResult> Zip<T, TResult>(this IEnumerable<T> col1, IEnumerable<T> col2, Func<T, T, TResult> merge)
    {
      return System.Linq.Enumerable.Zip(col1, col2, merge);
    }

    /// <summary>
    /// Applies a specified function to the corresponding elements of two sequences,
    /// producing a sequence of the results.
    /// </summary>
    /// <remarks>
    /// If validation of equal length is not needed, use Zip instead.
    /// </remarks>
    public static IEnumerable<TResult> ZipDefensive<T, TResult>(this IEnumerable<T> col1, IEnumerable<T> col2, Func<T, T, TResult> merge)
    {
      using (var col1Enum = col1.GetEnumerator())
      using (var col2Enum = col2.GetEnumerator())
      {
        bool isCol1Valid = col1Enum.MoveNext();
        bool isCol2Valid = col2Enum.MoveNext();
        while (isCol1Valid && isCol2Valid)
        {
          yield return merge(col1Enum.Current, col2Enum.Current);

          isCol1Valid = col1Enum.MoveNext();
          isCol2Valid = col2Enum.MoveNext();
        }

        if (isCol1Valid || isCol2Valid)
        {
          throw new InvalidOperationException("Collections are not the same size.");
        }
      }
    }
  }
}
