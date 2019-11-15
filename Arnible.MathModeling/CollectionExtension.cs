using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class CollectionExtension
  {
    public static IEnumerable<int> Indexes<T>(this T[] arg) => Enumerable.Range(0, arg.Length);

    public static IEnumerable<T> ExcludeAt<T>(this T[] x, uint pos)
    {
      if (pos >= x?.Length)
      {
        throw new ArgumentException($"Max {x?.Length}, got {pos}");
      }
      for (uint i = 0; i < x.Length; ++i)
      {
        if (i != pos)
        {
          yield return x[i];
        }
      }
    }
  }
}
