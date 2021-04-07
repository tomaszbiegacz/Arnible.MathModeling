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
  }
}