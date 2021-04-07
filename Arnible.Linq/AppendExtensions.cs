using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class AppendExtensions
  {
    public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item)
    {
      return Append(src, item, 1);
    }
    
    public static IEnumerable<T> Append<T>(this IEnumerable<T> src, T item, uint count)
    {
      foreach (T srcItem in src)
      {
        yield return srcItem;
      }
      for (uint i = 0; i < count; i++)
      {
        yield return item;
      }
    }
  }
}