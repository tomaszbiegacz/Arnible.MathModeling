using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ZipCommonExtensions
  {
    /// <summary>
    /// Applies a specified function to the elements matched by common keys from both dictionaries
    /// </summary>
    public static IDictionary<TKey, TMergeResult> ZipCommon<TKey, TResult, TMergeResult>(
      this IReadOnlyDictionary<TKey, TResult> source,
      IReadOnlyDictionary<TKey, TResult> other,
      Func<TResult, TResult, TMergeResult> merge) 
      where TKey: notnull
    {
      var result = new Dictionary<TKey, TMergeResult>();
      foreach (TKey key in source.Keys)
      {
        if (other.TryGetValue(key, out TResult? otherValue))
        {
          result.Add(key, merge(source[key], otherValue));
        }
      }
      return result;
    }
  }
}