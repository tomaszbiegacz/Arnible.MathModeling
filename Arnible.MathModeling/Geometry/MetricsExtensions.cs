using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public static class MetricsExtensions
  {
    private static void AxisDistance(
      in ReadOnlySpan<Number> src, 
      in ReadOnlySpan<Number> dst,
      in Span<Number> result)
    {
      src.Length.AssertIsEqualTo(dst.Length);
      for (ushort i = 0; i < src.Length; ++i)
      {
        result[i] = NumberMath.Abs(dst[i] - src[i]);
      }
    }
    
    public static Number ManhattanDistance(this ReadOnlySpan<Number> src, ReadOnlySpan<Number> dst)
    {
      Span<Number> distances = stackalloc Number[src.Length];
      AxisDistance(in src, in dst, in distances);
      return distances.SumDefensive();
    }
    
    public static Number ChebyshevDistance(this ReadOnlySpan<Number> src, ReadOnlySpan<Number> dst)
    {
      Span<Number> distances = stackalloc Number[src.Length];
      AxisDistance(in src, in dst, in distances);
      return distances.MaxDefensive();
    }
  }
}