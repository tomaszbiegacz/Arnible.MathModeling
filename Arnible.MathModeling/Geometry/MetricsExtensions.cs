using System;
using System.Collections.Generic;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public static class MetricsExtensions
  {
    private static IEnumerable<Number> AxisDistance(ValueArray<Number> src, ValueArray<Number> dst)
    {
      if (src.Length != dst.Length)
      {
        throw new ArgumentException(nameof(dst));
      }
      
      for (uint i = 0; i < src.Length; ++i)
      {
        yield return Math.Abs((double) (dst[i] - src[i]));
      }
    }
    
    public static Number ManhattanDistance(this in ValueArray<Number> src, in ValueArray<Number> dst)
    {
      return AxisDistance(src, dst).SumDefensive();
    }
    
    public static Number ChebyshevDistance(this in ValueArray<Number> src, in ValueArray<Number> dst)
    {
      return AxisDistance(src, dst).MaxDefensive();
    }
  }
}