using System;
using System.Collections.Generic;

namespace Arnible.Linq.Combinatorics
{
  public static class AggregateCombinationsExtensions
  {
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
      IReadOnlyList<TInput> x = items.ToArray();
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
      IReadOnlyList<T> x = items.ToArray();
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
  }
}