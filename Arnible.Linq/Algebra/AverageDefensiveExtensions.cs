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
      return items.SumDefensive() / items.Count;
    }
  }
}