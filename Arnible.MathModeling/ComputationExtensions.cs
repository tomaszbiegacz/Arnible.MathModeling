using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class ComputationExtensions
  {
    public static double Product(this IEnumerable<double> x)
    {
      double current = 1;
      foreach (double v in x)
      {
        current *= v;
      }
      return current;
    }

    public static Number Product(this IEnumerable<Number> x)
    {
      Number current = 1;
      foreach (Number v in x)
      {
        current *= v;
      }
      return current;
    }

    public static double Sum(this IEnumerable<double> x)
    {
      double current = 0;
      foreach (double v in x)
      {
        current += v;
      }
      return current;
    }

    public static Number Sum(this IEnumerable<Number> x)
    {
      Number current = 0;
      foreach (Number v in x)
      {
        current += v;
      }
      return current;
    }

    public static Number Sum(this IEnumerable<Number> x, Func<Number, Number> cast)
    {
      Number current = 0;
      foreach (Number v in x)
      {
        current += cast(v);
      }
      return current;
    }

    private static IEnumerable<T> AggregateCombinations<T>(T[] x, uint i, uint groupCount, Func<IEnumerable<T>, T> aggregator, Stack<T> combination)
    {
      if (groupCount == combination.Count)
      {
        yield return aggregator(combination);
      }
      else
      {
        uint combinationLength = (uint)combination.Count;
        for (uint j = i; j < x.Length; ++j)
        {
          combination.Push(x[j]);
          foreach (T v in AggregateCombinations(x, j + 1, groupCount, aggregator, combination))
          {
            yield return v;
          }

          combination.Pop();
          if (combination.Count != combinationLength)
          {
            throw new InvalidOperationException($"Got {combination.Count} values, expected {combinationLength}.");
          }
        }
      }
    }

    public static IEnumerable<T> AggregateCombinations<T>(this IEnumerable<T> items, uint groupCount, Func<IEnumerable<T>, T> aggregator)
    {
      if (groupCount < 1)
      {
        throw new ArgumentException(nameof(groupCount));
      }

      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToArray();
      if (x.Length < groupCount)
      {
        throw new ArgumentException($"x.Length: {x.Length} where groupCount: {groupCount}");
      }
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<T>();
      return AggregateCombinations(x, 0, groupCount, aggregator, combination);
    }

    public static IEnumerable<T> AggregateAllCombinations<T>(this IEnumerable<T> items, Func<IEnumerable<T>, T> aggregator)
    {
      if (items == null)
      {
        throw new ArgumentException(nameof(items));
      }
      var x = items.ToArray();
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      var combination = new Stack<T>();
      for (uint groupCount = 1; groupCount <= x.Length; ++groupCount)
      {
        foreach (T item in AggregateCombinations(x, 0, groupCount, aggregator, combination))
        {
          yield return item;
        }
      }
    }    
  }
}
