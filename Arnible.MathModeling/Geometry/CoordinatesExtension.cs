using System;
using System.Collections.Generic;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static double Sqrt(in Number x)
    {
      return Math.Sqrt((double)x);
    }

    private static double Asin(in Number x)
    {
      return Math.Asin((double)x);
    }

    private static double GetFirstAngle(in Number x, in Number y)
    {
      return Math.Atan2((double)y, (double)x);
    }

    public static PolarCoordinate ToPolar(in this RectangularCoordinate p)
    {
      return new PolarCoordinate(
        r: Sqrt(p.X * p.X + p.Y * p.Y),
        φ: GetFirstAngle(p.X, p.Y));
    }

    public static HypersphericalCoordinate ToSpherical(in this CartesianCoordinate p)
    {
      NumberVector pc = p.Coordinates;
      NumberVector pc2 = pc.Transform(c => c * c);

      Number r = Sqrt(pc2.SumDefensive());
      if (r > 0)
      {
        var angles = new List<Number>();
        for (uint i = p.DimensionsCount; i > 2; i--)
        {
          Number radius2 = pc2.TakeExactly(i).SumDefensive();
          if (radius2 == 0)
          {
            angles.Add(0);
          }
          else
          {
            Number angleSin = pc.GetOrDefault(i - 1) / Sqrt(radius2);
            double angle = Asin(angleSin);
            angles.Add(angle);
          }
        }
        angles.Add(GetFirstAngle(pc.GetOrDefault(0), pc.GetOrDefault(1)));
        angles.Reverse();

        return new HypersphericalCoordinate(in r, angles.ToAngleVector());
      }
      else
      {
        return default;
      }
    }

    public static HypersphericalCoordinateOnAxisView ToSphericalView(in this CartesianCoordinate p)
    {
      return new HypersphericalCoordinateOnAxisView(p);
    }

    public static Number VectorLength(in this CartesianCoordinate point)
    {
      return Sqrt(point.Coordinates.Select(d => d * d).SumDefensive());
    }

    /// <summary>
    /// Calculate derivative ratios by moving along the vector
    /// </summary>
    public static NumberVector GetDirectionDerivativeRatios(in this NumberVector direction)
    {
      CartesianCoordinate point = direction;
      return point.ToSpherical().Angles.GetCartesianAxisViewsRatios();
    }
    
    /// <summary>
    /// Calculate derivative ratios by moving along the array vector
    /// </summary>
    public static ValueArray<Number> GetDirectionDerivativeRatios(in this ValueArray<Number> direction)
    {
      CartesianCoordinate point = direction;
      return point.ToSpherical().Angles.GetCartesianAxisViewsRatios().ToValueArray(direction.Length);
    }
  }
}
