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
