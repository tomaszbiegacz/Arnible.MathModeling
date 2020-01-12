using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    private static double GetFirstAngle(double x, double y)
    {
      var angle = Atan2(x, y);
      return angle >= 0 ? angle : 2 * Math.PI + angle;
    }
    public static PolarCoordinate ToPolar(this RectangularCoordianate p)
    {
      return new PolarCoordinate(
        r: Sqrt(p.X * p.X + p.Y * p.Y),
        φ: GetFirstAngle(p.X, p.Y));
    }

    public static HypersphericalCoordinate ToSpherical(this CartesianCoordinate p)
    {
      if (p.DimensionsCount == 1)
      {
        throw new ArgumentException($"Invalid dimensions count");
      }

      NumberVector pc = p.Coordinates;
      NumberVector pc2 = pc.Transform(c => c * c);

      Number r = Sqrt(pc2.Sum());
      if (r > 0)
      {
        var angles = new List<Number>();
        for (uint i = p.DimensionsCount; i > 2; i--)
        {
          double radius2 = pc2.Take((int)i).Sum();
          double angleCos = pc[i - 1] / Sqrt(radius2);
          double angle = Acos(angleCos);
          angles.Add(angle);
        }
        angles.Add(GetFirstAngle(pc[0], pc[1]));
        angles.Reverse();

        return new HypersphericalCoordinate(r, angles.ToVector());
      }
      else
      {
        return default;
      }
    }

    public static CartesianCoordinate ToCartesian(this HypersphericalCoordinate hypersphericalPoint)
    {
      var replacement = hypersphericalPoint.R;

      var cartesianDimensions = new List<Number>();
      NumberVector ad = hypersphericalPoint.Angles.Reverse();
      for (uint i = 0; i < ad.Count; ++i)
      {
        var angle = ad[i];
        cartesianDimensions.Add(replacement * Cos(angle));
        replacement *= Sin(angle);
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return new CartesianCoordinate(cartesianDimensions.ToVector());
    }

    public static Number VectorLength(this CartesianCoordinate point)
    {
      return Math.Sqrt(point.Coordinates.Select(d => d * d).Sum());
    }
  }
}
