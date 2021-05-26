using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  public static class NumberVectorExtensions
  {
    private static (Number[], uint) SumWithCount(IEnumerable<ReadOnlyArray<Number>> vectors)
    {
      List<Number>? result = null;
      uint itemsCount = 0;
      foreach (ReadOnlyArray<Number> item in vectors)
      {
        itemsCount++;
        if (result == null)
        {
          result = new List<Number>(item.AsList());
        }
        else
        {
          if (item.Length != result.Count)
          {
            throw new ArgumentException(nameof(vectors));
          }

          using var itemEnumerator = item.GetEnumerator();
          for (int i = 0; i < result.Count; ++i)
          {
            if (!itemEnumerator.MoveNext())
            {
              throw new InvalidOperationException();
            }
            result[i] += itemEnumerator.Current;
          }
        }
      }
      return (result?.ToArray() ?? new Number[0], itemsCount);
    }

    public static ReadOnlyArray<Number> Sum(this IEnumerable<ReadOnlyArray<Number>> vectors)
    {
      (Number[] sum, _) = SumWithCount(vectors);
      return sum;
    }

    public static ReadOnlyArray<Number> Average(this IEnumerable<ReadOnlyArray<Number>> vectors)
    {
      (Number[] sum, var count) = SumWithCount(vectors);
      return sum.Select(v => v / count).ToArray();
    }
    
    public static Number GetOrDefault(this ReadOnlyArray<Number> src, ushort pos)
    {
      if (pos >= src.Length)
      {
        return 0;
      }
      else
      {
        return src[pos];
      }
    }
  }
}
