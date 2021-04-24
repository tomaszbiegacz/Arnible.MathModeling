using System;
using System.Collections.Generic;

namespace Arnible.Linq.Algebra
{
  public static class AverageDefensiveExtensions
  {
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static double AverageDefensive(this IReadOnlyList<double> items)
    {
      if (items.Count == 0)
      {
        throw new ArgumentException(nameof(items));
      }
      return items.SumDefensive() / items.Count;
    }
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static double AverageDefensive(this IEnumerable<double> x)
    {
      return AverageDefensive(x.ToArray());
    }
  }
}