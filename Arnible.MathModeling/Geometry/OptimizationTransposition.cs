using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Geometry
{
  public static class OptimizationTransposition
  {
    public static Number Minimum(Number value, IDerivative1 derivative)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }
      if (derivative.First == 0)
      {
        throw new ArgumentException(nameof(derivative));
      }
      return -1 * value / derivative.First;
    }

    public static NumberVectorTransposition Minimum(
      Number value,
      HypersphericalAngleVector direction,
      IDerivative1 derivative)
    {
      HypersphericalCoordinate hc;
      Number rDelta = Minimum(value, derivative);
      if(rDelta > 0)
      {
        hc = new HypersphericalCoordinate(rDelta, direction);
      }
      else
      {
        hc = new HypersphericalCoordinate(Math.Abs(rDelta), direction.Mirror);
      }
      return new NumberVectorTransposition(hc.ToCartesian().Coordinates);
    }
  }
}
