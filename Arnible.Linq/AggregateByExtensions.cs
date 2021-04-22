using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class AggregateByExtensions
  {
    /// <summary>
    /// Group items by "keySelector" and return aggregation for each group
    /// </summary>
    public static Dictionary<TKey, TResult> AggregateBy<TSource, TKey, TResult>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IEnumerable<TSource>, TResult> aggregator) where TKey: notnull
    {
      return System.Linq.Enumerable.GroupBy(source, keySelector)
        .ToDictionary(g => g.Key, g => aggregator(g));
    }

    /// <summary>
    /// Group items present in each source sequence by "keySelector"
    /// and return aggregation for each group having items in each sequence
    /// </summary>
    public static Dictionary<TKey, TResult> AggregateCommonBy<TSource, TKey, TResult>(
      this IEnumerable<IEnumerable<TSource>> source,
      Func<TSource, TKey> keySelector,
      Func<IEnumerable<TSource>, TResult> aggregator) where TKey: notnull
    {
      Dictionary<TKey, List<TSource>> groupByKey = new Dictionary<TKey, List<TSource>>();

      uint sequenceCount = 0;
      foreach (var sequence in source)
      {
        foreach (TSource item in sequence)
        {
          TKey key = keySelector(item);
          List<TSource>? groupedItems;
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
  }
}