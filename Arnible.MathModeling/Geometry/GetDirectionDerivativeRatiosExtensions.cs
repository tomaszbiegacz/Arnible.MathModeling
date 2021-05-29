using System;

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
      Number[] d = direction.ToArray();
      d.ToSpherical().Angles.GetCartesianAxisViewsRatios(in result);
    }
    
    /// <summary>
    /// Calculate derivative ratios by moving along the array vector
    /// </summary>
    public static void GetDirectionDerivativeRatios(
      this Number[] direction,
      in Span<Number> result)
    {
      GetDirectionDerivativeRatios((ReadOnlySpan<Number>)direction, in result);
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