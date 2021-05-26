using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
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
      ReadOnlyArray<Number> pc = p.Coordinates;
      ReadOnlyArray<Number> pc2 = pc.AsList().Select(c => c * c).ToArray();

      Number r = Sqrt(pc2.AsList().SumDefensive());
      if (r > 0)
      {
        var angles = new List<Number>();
        for (ushort i = p.DimensionsCount; i > 2; i--)
        {
          Number radius2 = pc2.AsList().TakeExactly(i).SumDefensive();
          if (radius2 == 0)
          {
            angles.Add(0);
          }
          else
          {
            Number angleSin = pc.GetOrDefault((ushort)(i - 1)) / Sqrt(radius2);
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
        return new HypersphericalCoordinate(0, new HypersphericalAngleVector(0));
      }
    }

    public static HypersphericalCoordinateOnAxisView ToSphericalView(in this CartesianCoordinate p)
    {
      return new HypersphericalCoordinateOnAxisView(p);
    }

    public static Number VectorLength(in this CartesianCoordinate point)
    {
      return Sqrt(point.Coordinates.AsList().Select(d => d * d).SumDefensive());
    }

    /// <summary>
    /// Calculate derivative ratios by moving along the array vector
    /// </summary>
    public static ReadOnlyArray<Number> GetDirectionDerivativeRatios(in this ReadOnlyArray<Number> direction)
    {
      CartesianCoordinate point = direction;
      Number[] result = point.ToSpherical().Angles.GetCartesianAxisViewsRatios();
      result.Length.AssertIsEqualTo(direction.Length);
      return result;
    }
  }
}
