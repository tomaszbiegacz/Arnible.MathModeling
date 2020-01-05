using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Arnible.MathModeling.Geometry
{
  public static class CoordinatesExtension
  {
    public static PolarCoordinate ToPolar(this RectangularCoordianate p)
    {
      var angle = Atan2(p.Y, p.X);
      return new PolarCoordinate(
        r: Sqrt(p.X * p.X + p.Y * p.Y),
        φ: angle >= 0 ? angle : 2 * Math.PI + angle);
    }

    public static HypersphericalCoordinate ToSpherical(this CartesianCoordinate p)
    {
      if(p.DimensionsCount == 1)
      {
        throw new ArgumentException($"Invalid dimensions count");
      }

      Number[] pc = p.Coordinates.ToArray();
      var pc2 = p.Coordinates.Select(c => c * c).ToArray();

      Number r = Math.Sqrt(pc2.Sum());
      if (r > 0)
      {
        var angles = new List<Number>();
        for (int i = 0; i < p.DimensionsCount - 1; ++i)
        {
          var angleCos = pc[i] / Math.Sqrt(pc2.Skip(i).Sum());
          var angle = Math.Acos(angleCos);
          angles.Add(angle >= 0 ? angle : 2 * Math.PI + angle);
        }
        return new HypersphericalCoordinate(r, angles.ToArray());
      }
      else
      {
        return default;
      }
    }
  }
}
