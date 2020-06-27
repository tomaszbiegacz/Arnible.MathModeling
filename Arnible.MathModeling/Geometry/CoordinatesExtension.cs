using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static double Sqrt(Number x)
    {
      return Math.Sqrt((double)x);
    }

    private static double Asin(Number x)
    {
      return Math.Asin((double)x);
    }

    private static double GetFirstAngle(Number x, Number y)
    {
      return Math.Atan2((double)y, (double)x);
    }

    public static PolarCoordinate ToPolar(this RectangularCoordianate p)
    {
      return new PolarCoordinate(
        r: Sqrt(p.X * p.X + p.Y * p.Y),
        φ: GetFirstAngle(p.X, p.Y));
    }

    public static HypersphericalCoordinate ToSpherical(this CartesianCoordinate p)
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

        return new HypersphericalCoordinate(r, angles.ToAngleVector());
      }
      else
      {
        return default;
      }
    }

    public static HypersphericalCoordinateOnAxisView ToSphericalView(this CartesianCoordinate p)
    {
      return new HypersphericalCoordinateOnAxisView(p);
    }

    public static Number VectorLength(this CartesianCoordinate point)
    {
      return Sqrt(point.Coordinates.Select(d => d * d).SumDefensive());
    }
  }
}
