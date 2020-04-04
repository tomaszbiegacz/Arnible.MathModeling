using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class NumberVectorExtensions
  {
    public static NumberVector ToVector(this IEnumerable<Number> numbers)
    {
      return new NumberVector(numbers);
    }

    public static NumberVector ToVector(this IEnumerable<double> numbers)
    {
      return new NumberVector(numbers.Select(v => (Number)v));
    }

    private static (NumberVector, uint) SumWithCount(IEnumerable<NumberVector> vectors)
    {
      Number[] result = null;
      uint itemsCount = 0;
      foreach (var item in vectors)
      {
        itemsCount++;
        if (result == null)
        {
          result = item.ToArray();
        }
        else
        {
          if (item.Count != result.Length)
          {
            throw new ArgumentException(nameof(vectors));
          }

          using (var itemEnumerator = item.GetEnumerator())
          {
            for (int i = 0; i < result.Length; ++i)
            {
              if (!itemEnumerator.MoveNext())
              {
                throw new InvalidOperationException();
              }
              result[i] += itemEnumerator.Current;
            }
          }            
        }
      }
      return (result != null ? new NumberVector(result) : default, itemsCount);
    }

    public static NumberVector Sum(this IEnumerable<NumberVector> vectors)
    {
      var (sum, _) = SumWithCount(vectors);
      return sum;
    }

    public static NumberVector Average(this IEnumerable<NumberVector> vectors)
    {
      var (sum, count) = SumWithCount(vectors);
      return sum / count;
    }
  }
}
