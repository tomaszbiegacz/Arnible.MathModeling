using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public static class CartesianCoordinate
  {
    /// <summary>
    /// Return ratio of an identity vector. All coefficients should be equal to it
    /// </summary>
    public static Number GetIdentityVectorRatio(ushort dimensionsCount)
    {
      return Math.Sqrt(1.0 / dimensionsCount);
    }
    
    /// <summary>
    /// Get identity vector coefficients
    /// </summary>
    public static void GetIdentityVector(in Span<Number> result)
    {
      if (result.Length > 0)
      {
        Number ratio = GetIdentityVectorRatio((ushort)result.Length);
        result.Fill(ratio);
      }
    }
    
    private static Number Sqr(in Number x)
    {
      return x*x;
    }

    private static Number Asin(in Number x)
    {
      return Math.Asin((double)x);
    }

    private static Number GetFirstAngle(in Number x, in Number y)
    {
      return Math.Atan2((double)y, (double)x);
    }

    public static PolarCoordinate ToPolar(in this RectangularCoordinate p)
    {
      return new PolarCoordinate(
        r: NumberMath.Sqrt(p.X * p.X + p.Y * p.Y),
        φ: GetFirstAngle(p.X, p.Y));
    }
    
    public static Number GetVectorLength(in this ReadOnlySpan<Number> point)
    {
      return NumberMath.Sqrt(point.SumDefensive(Sqr));
    }
    
    public static Number GetVectorLength(in this Span<Number> point)
    {
      return GetVectorLength((ReadOnlySpan<Number>)point);
    }

    public static HypersphericalCoordinate ToSpherical(
      in this ReadOnlySpan<Number> cartesianPoint,
      in Span<Number> buffer)
    {
      cartesianPoint.Length.AssertIsGreaterEqualThan(2);
      cartesianPoint.Length.AssertIsEqualTo(buffer.Length + 1);
      
      Number r2 = Sqr(in cartesianPoint[0]) + Sqr(in cartesianPoint[1]);
      buffer[0] = GetFirstAngle(in cartesianPoint[0], in cartesianPoint[1]);
      for (ushort i = 1; i < buffer.Length; i++)
      {
        ref readonly Number coordinate = ref cartesianPoint[1 + i]; 
        r2 += Sqr(in coordinate);
        if (r2 == 0)
        {
          buffer[i] = 0;
        }
        else
        {
          buffer[i] = Asin(coordinate / NumberMath.Sqrt(r2));
        }
      }
      
      return new HypersphericalCoordinate(NumberMath.Sqrt(in r2), in buffer);
    }
    
    public static HypersphericalCoordinate ToSpherical(
      in this Span<Number> cartesianPoint,
      in Span<Number> buffer)
    {
      return ToSpherical((ReadOnlySpan<Number>)cartesianPoint, in buffer);
    }
    
    public static bool IsOrthogonal(in this Span<Number> cartesianVector)
    {
      return cartesianVector.Count((in Number v) => v != 0) == 1;
    }
  }
}