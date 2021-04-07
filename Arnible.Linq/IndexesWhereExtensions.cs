using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class IndexesWhereExtensions
  {
    public static IEnumerable<ushort> IndexesWhere<T>(this IReadOnlyList<T> arg, Func<T, bool> predicate)
    {
      for (ushort i = 0; i < arg.Count; ++i)
      {
        if (predicate(arg[i]))
        {
          yield return i;
        }
      }
    }
  }
}