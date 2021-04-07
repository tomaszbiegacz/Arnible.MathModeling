using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ConcatExtensions
  {
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, IEnumerable<T> after)
    {
      foreach (T srcItem in source)
      {
        yield return srcItem;
      }
      foreach (T srcItem in after)
      {
        yield return srcItem;
      }
    }
  }
}