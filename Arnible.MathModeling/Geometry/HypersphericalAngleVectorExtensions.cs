using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  public static class HypersphericalAngleVectorExtensions
  {
    public static HypersphericalAngleVector ToAngleVector(this IEnumerable<Number> numbers)
    {
      return HypersphericalAngleVector.Create(numbers);
    }
    
    public static HypersphericalAngleVector ToAngleVector(this NumberVector numbers)
    {
      return numbers.GetInternalEnumerable().ToAngleVector();
    }

    public static HypersphericalAngleVector ToAngleVector(this IEnumerable<double> numbers)
    {
      return HypersphericalAngleVector.Create(numbers.Select(n => (Number)n));
    }

    public static HypersphericalAngleVector SumDefensive(this IEnumerable<HypersphericalAngleVector> x)
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
      return current ?? throw new ArgumentException(nameof(x));
    }

    public static HypersphericalAngleVector SumWithDefault(this IEnumerable<HypersphericalAngleVector> x)
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
      return current ?? default;
    }

    public static HypersphericalAngleVector Average(this IEnumerable<HypersphericalAngleVector> angles)
    {
      return angles.Select(v => (NumberVector)v).Average().GetInternalEnumerable().ToAngleVector();
    }
    
    //
    // IEnumerable implementation (to avoid boxing)
    //
    
    public static Number Single(in this HypersphericalAngleVector vector)
    {
      return vector.GetInternalEnumerable().Single();
    }
  }
}
