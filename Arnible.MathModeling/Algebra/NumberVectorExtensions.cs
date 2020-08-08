using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class NumberVectorExtensions
  {
    public static NumberVector ToVector(this IEnumerable<Number> numbers)
    {
      return NumberVector.Create(numbers);
    }

    public static NumberVector ToVector(this IEnumerable<double> numbers)
    {
      return numbers.Select(v => (Number)v).ToVector();
    }

    private static (NumberVector, uint) SumWithCount(IEnumerable<NumberVector> vectors)
    {
      List<Number>? result = null;
      uint itemsCount = 0;
      foreach (var item in vectors)
      {
        itemsCount++;
        if (result == null)
        {
          result = item.GetInternalEnumerable().ToList();
        }
        else
        {
          if (item.Length != result.Count)
          {
            throw new ArgumentException(nameof(vectors));
          }

          using (var itemEnumerator = item.GetEnumerator())
          {
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
      }
      return (result != null ? result.ToVector() : default, itemsCount);
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
