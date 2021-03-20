using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ReverseExtensions
  {
    public static IEnumerable<T> Reverse<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.Reverse(source);
    }
    
    public static IEnumerable<T> Reverse<T>(this IReadOnlyList<T> source)
    {
      for(int i=source.Count - 1; i>=0; --i)
      {
        yield return source[i];
      }
    }
    
    public static IEnumerable<T> Reverse<T>(this IList<T> source)
    {
      for(int i=source.Count - 1; i>=0; --i)
      {
        yield return source[i];
      }
    }
  }
}