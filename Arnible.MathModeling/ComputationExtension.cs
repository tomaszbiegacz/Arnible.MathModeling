using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class ComputationExtension
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

    public static double Sum(this IEnumerable<double> x)
    {
      double current = 0;
      foreach (double v in x)
      {
        current += v;
      }
      return current;
    }

    private static IEnumerable<double> AggregateCombinations(double[] x, uint i, uint groupCount, Func<IEnumerable<double>, double> aggregator, Stack<double> combination)
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
          foreach (double v in AggregateCombinations(x, j + 1, groupCount, aggregator, combination))
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

    public static IEnumerable<double> AggregateCombinations(this double[] x, uint groupCount, Func<IEnumerable<double>, double> aggregator)
    {
      if (groupCount < 1)
      {
        throw new ArgumentException(nameof(groupCount));
      }
      if (x == null)
      {
        throw new ArgumentException(nameof(x));
      }
      if (x?.Length < groupCount)
      {
        throw new ArgumentException($"x.Length: {x?.Length} where groupCount: {groupCount}");
      }
      if (aggregator == null)
      {
        throw new ArgumentException(nameof(aggregator));
      }

      Stack<double> combination = new Stack<double>();
      return AggregateCombinations(x, 0, groupCount, aggregator, combination);
    }
  }
}
