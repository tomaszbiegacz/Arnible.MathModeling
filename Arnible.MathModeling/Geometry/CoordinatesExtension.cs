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
      var angle = Atan2(y, x);
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

      Number[] pc = p.Coordinates.ToArray();
      var pc2 = pc.Select(c => c * c).ToArray();

      Number r = Sqrt(pc2.Sum());
      if (r > 0)
      {
        var angles = new List<Number>();
        for (uint i = p.DimensionsCount; i > 2; i--)
        {
          var radius2 = pc2.Take((int)i).Sum();
          var angleCos = pc[i - 1] / Sqrt(radius2);
          var angle = Acos(angleCos);
          angles.Add(angle);
        }
        angles.Add(GetFirstAngle(pc[0], pc[1]));
        angles.Reverse();

        return new HypersphericalCoordinate(r, angles);
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
      var ad = hypersphericalPoint.Angles.Reverse().ToArray();
      for (int i = 0; i < ad.Length; ++i)
      {
        var angle = ad[i];
        cartesianDimensions.Add(replacement * Cos(angle));
        replacement *= Sin(angle);
      }
      cartesianDimensions.Add(replacement);

      cartesianDimensions.Reverse();
      return new CartesianCoordinate(cartesianDimensions);
    }
  }
}
