using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public static class MetricsExtensions
  {
    private static IEnumerable<Number> AxisDistance(ReadOnlyArray<Number> src, ReadOnlyArray<Number> dst)
    {
      if (src.Length != dst.Length)
      {
        throw new ArgumentException(nameof(dst));
      }
      
      for (ushort i = 0; i < src.Length; ++i)
      {
        yield return Math.Abs((double) (dst[i] - src[i]));
      }
    }
    
    public static Number ManhattanDistance(this ReadOnlyArray<Number> src, ReadOnlyArray<Number> dst)
    {
      return AxisDistance(src, dst).SumDefensive();
    }
    
    public static Number ChebyshevDistance(this ReadOnlyArray<Number> src, ReadOnlyArray<Number> dst)
    {
      return AxisDistance(src, dst).MaxDefensive();
    }
  }
}