using Arnible.MathModeling.Algebra;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public static class HypersphericalAngleVectorExtension
  {
    public static HypersphericalAngleVector Sum(this IEnumerable<HypersphericalAngleVector> x)
    {
      HypersphericalAngleVector? current = null;
      foreach (var v in x)
      {
        if (current.HasValue)
        {
          current = current.Value + v;
        }
        else
        {
          current = v;
        }
      }
      return current.Value;
    }

    public static HypersphericalAngleVector Average(this IEnumerable<HypersphericalAngleVector> angles)
    {
      return new HypersphericalAngleVector(angles.Select(v => (NumberVector)v).Average());
    }
  }
}
