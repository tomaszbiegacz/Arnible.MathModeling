using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using static System.Math;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static double GetFirstAngle(double x, double y) => Atan2(y, x);

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
          double radius2 = pc2.TakeExactly(i).SumDefensive();
          if(radius2.NumericEquals(0))
          {
            angles.Add(0);
          }
          else
          {
            double angleSin = pc.GetOrDefault(i - 1) / Sqrt(radius2);
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
      return new HypersphericalCoordinateOnAxisView(ToSpherical(p));
    }

    public static Number VectorLength(this CartesianCoordinate point)
    {
      return Sqrt(point.Coordinates.Select(d => d * d).SumDefensive());
    }
  }
}
