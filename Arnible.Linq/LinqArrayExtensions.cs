using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LinqArrayExtensions
  {
    public static T[] ToArray<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToArray(source);
    }
  }
}