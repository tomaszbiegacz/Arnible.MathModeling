using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class PrependExtensions
  {
    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> src, T item)
    {
      yield return item;
      foreach (var srcItem in src)
      {
        yield return srcItem;
      }
    }
  }
}