using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class DistinctExtensions
  {
    public static IReadOnlyCollection<T> Distinct<T>(this IEnumerable<T> collection) where T: IEquatable<T>
    {
      return new HashSet<T>(collection);
    }
    
    public static IReadOnlyCollection<TValue> Distinct<T, TValue>(
      this ReadOnlySpan<T> collection, 
      FuncIn<T, TValue> getValue) where TValue: IEquatable<TValue>
    {
      HashSet<TValue> result = new();
      foreach (ref readonly T src in collection)
      {
        result.Add(getValue(in src));
      }
      return result;
    }
  }
}