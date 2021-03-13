using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ReverseExtensions
  {
    public static IEnumerable<T> Reverse<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Reverse(source);
    }
  }
}