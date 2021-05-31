using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Geometry
{
  public static class GetDirectionDerivativeRatiosExtensions
  {
    /// <summary>
    /// Calculate derivative ratios by moving along the array vector
    /// </summary>
    public static void GetDirectionDerivativeRatios(
      in this ReadOnlySpan<Number> direction,
      in Span<Number> result)
    {
      direction.Length.AssertIsGreaterEqualThan(2);
      direction.Length.AssertIsEqualTo(result.Length);
      
      Span<Number> buffer = stackalloc Number[result.Length - 1];
      direction
        .ToSpherical(in buffer)
        .Angles
        .GetCartesianAxisViewsRatios(in result);
    }
    
    /// <summary>
    /// Calculate derivative ratios by moving along the array vector
    /// </summary>
    public static void GetDirectionDerivativeRatios(
      in this Span<Number> direction,
      in Span<Number> result)
    {
      GetDirectionDerivativeRatios((ReadOnlySpan<Number>)direction, in result);
    }
  }
}