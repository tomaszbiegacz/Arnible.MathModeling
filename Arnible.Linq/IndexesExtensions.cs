using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class IndexesExtensions
  {
    public static IEnumerable<ushort> Indexes<T>(this IReadOnlyList<T> arg)
    {
      return LinqEnumerable.RangeUshort((ushort)arg.Count);
    }
  }
}