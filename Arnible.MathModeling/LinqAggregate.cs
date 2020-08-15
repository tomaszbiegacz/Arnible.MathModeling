using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqAggregate
  {
    /// <summary>
    /// Group items by "keySelector" and return aggregation for each group
    /// </summary>
    public static IDictionary<TKey, TResult> AggregateBy<TSource, TKey, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IEnumerable<TSource>, TResult> aggregator)
    {
      return System.Linq.Enumerable.GroupBy(source, keySelector).ToDictionary(g => g.Key, g => aggregator(g));
    }

    /// <summary>
    /// Group items present in each source sequence by "keySelector" and return aggregation for each group
    /// </summary>
    public static IDictionary<TKey, TResult> AggregateCommonBy<TSource, TKey, TResult>(
      this IEnumerable<IEnumerable<TSource>> source,
      Func<TSource, TKey> keySelector,
      Func<IEnumerable<TSource>, TResult> aggregator)
    {
      Dictionary<TKey, List<TSource>> groupByKey = new Dictionary<TKey, List<TSource>>();

      uint sequenceCount = 0;
      foreach (var sequence in source)
      {
        foreach (TSource item in sequence)
        {
          TKey key = keySelector(item);
          List<TSource> groupedItems;
          if (!groupByKey.TryGetValue(key, out groupedItems))
          {
            groupedItems = new List<TSource>();
            groupByKey.Add(key, groupedItems);
          }
          groupedItems.Add(item);
        }
        sequenceCount++;
      }

      return groupByKey.Where(kv => kv.Value.Count == sequenceCount).ToDictionary(kv => kv.Key, kv => aggregator(kv.Value));
    }

    private static IEnumerable<TOutput> AggregateCombinations<TInput, TOutput>(
      IReadOnlyList<TInput> x,
      uint i,
      uint groupCount,
      Func<IEnumerable<TInput>, TOutput> aggregator, Stack<TInput> combination)
    {
      if (groupCount == combination.Count)
      {
        yield return aggregator(combination);
      }
      else
      {
        uint combinationLength = (uint)combination.Count;
        for (uint j = i; j < x.Count; ++j)
        {
          combination.Push(x[(int)j]);
          foreach (TOutput v in AggregateCombinations(x, j + 1, groupCount, aggregator, combination))
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
    public static IEnumerable<TOutput> AggregateCombinations<TInput, TOutput>(
      this IEnumerable<TInput> items,
      in uint groupSize,
      in Func<IEnumerable<TInput>, TOutput> aggregator)
    {
      if (groupSize < 1)
      {
        throw new ArgumentException(nameof(groupSize));
      }

      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      IReadOnlyList<TInput> x = items.ToReadOnlyList();
      if (x.Count < groupSize)
      {
        throw new ArgumentException($"x.Length: {x.Count} where groupCount: {groupSize}");
      }
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<TInput>();
      return AggregateCombinations(x, 0, groupSize, aggregator, combination);
    }

    /// <summary>
    /// Calculate aggregate for items count from 1 to collection size
    /// </summary>
    public static IEnumerable<T> AggregateCombinationsAll<T>(
      this IEnumerable<T> items,
      Func<IEnumerable<T>, T> aggregator)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      IReadOnlyList<T> x = items.ToReadOnlyList();
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<T>();
      for (uint groupCount = 1; groupCount <= x.Count; ++groupCount)
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
    public static IEnumerable<TResult> Zip<T, TResult>(
      this IEnumerable<T> col1,
      IEnumerable<T> col2,
      Func<T?, T?, TResult> merge) where T : struct
    {
      using (var col1Enum = col1.GetEnumerator())
      using (var col2Enum = col2.GetEnumerator())
      {
        bool isCol1Valid = col1Enum.MoveNext();
        bool isCol2Valid = col2Enum.MoveNext();
        while (isCol1Valid || isCol2Valid)
        {
          T? col1Current = null;
          if (isCol1Valid)
          {
            col1Current = col1Enum.Current;
            isCol1Valid = col1Enum.MoveNext();
          }

          T? col2Current = null;
          if (isCol2Valid)
          {
            col2Current = col2Enum.Current;
            isCol2Valid = col2Enum.MoveNext();
          }

          yield return merge(col1Current, col2Current);
        }
      }
    }

    /// <summary>
    /// Applies a specified function to the corresponding elements of two sequences,
    /// producing a sequence of the results.
    /// </summary>
    /// <remarks>
    /// If validation of equal length is not needed, use Zip instead.
    /// </remarks>
    public static IEnumerable<TResult> ZipDefensive<T, TResult>(
      this IEnumerable<T> col1,
      IEnumerable<T> col2,
      Func<T, T, TResult> merge)
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

    /// <summary>
    /// Applies a specified function to the elements matched by common keys from dictionaries
    /// producing a sequence of the results.
    /// </summary>
    public static IDictionary<TKey, TMergeResult> ZipCommon<TKey, TResult, TMergeResult>(
      this IDictionary<TKey, TResult> source,
      IDictionary<TKey, TResult> other,
      Func<TResult, TResult, TMergeResult> merge)
    {
      var result = new Dictionary<TKey, TMergeResult>();
      foreach (TKey key in source.Keys)
      {
        if (other.TryGetValue(key, out TResult otherValue))
        {
          result.Add(key, merge(source[key], otherValue));
        }
      }
      return result;
    }
  }
}
