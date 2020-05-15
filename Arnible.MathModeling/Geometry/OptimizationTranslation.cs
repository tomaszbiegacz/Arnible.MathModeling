using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Geometry
{
  public static class OptimizationTranslation
  {
    /// <summary>
    /// Estimated change to reach minimum in 1 dimentional case
    /// </summary>
    public static Number ForMinimumEquals0(Number value, IDerivative1 derivative)
    {
      if (derivative.First != 0 && value != 0)
      {
        return -1 * value / derivative.First;
      }
      else
      {
        if (derivative.First == 0 && value == 0)
        {
          return 0;
        }
        else
        {
          throw new InvalidOperationException($"Value {value}, derivative {derivative}");
        }
      }
    }

    /// <summary>
    /// Estimated change to reach minimum in given direction
    /// </summary>
    public static NumberTranslationVector ForMinimumEquals0(
      Number value,
      uint cartesiaxAxisNumber,
      IDerivative1 derivative)
    {
      Number rDelta = ForMinimumEquals0(value, derivative);
      NumberVector direction = NumberVector.FirstNonZeroValueAt(pos: cartesiaxAxisNumber, value: 1);
      return new NumberTranslationVector(rDelta * direction);
    }

    /// <summary>
    /// Estimated change to reach minimum in given direction
    /// </summary>
    public static NumberTranslationVector ForMinimumEquals0(
      Number value,
      HypersphericalAngleVector direction,
      IDerivative1 derivative)
    {
      HypersphericalCoordinate hc;
      Number rDelta = ForMinimumEquals0(value, derivative);

      if (rDelta > 0)
      {
        hc = new HypersphericalCoordinate(rDelta, direction);
      }
      else
      {
        hc = new HypersphericalCoordinate(-1 * rDelta, direction.Mirror);
      }

      return new NumberTranslationVector(hc.ToCartesianView().Coordinates);
    }    
  }
}
