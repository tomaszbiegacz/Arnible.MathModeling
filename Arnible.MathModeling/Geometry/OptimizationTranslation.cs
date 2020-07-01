using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Logic;
using System;

namespace Arnible.MathModeling.Geometry
{
  public static class OptimizationTranslation
  {
    /// <summary>
    /// Estimated change to reach minimum in 1 dimentional case
    /// </summary>
    public static Number ForMinimumEquals0(Number value, Derivative1Value derivative)
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
    /// Estimated change to reach minimum in the axis direction
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      Number value,
      uint cartesiaxAxisNumber,
      Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(value, derivative);
      return new NumberTranslationVector(NumberVector.NonZeroValueAt(pos: cartesiaxAxisNumber, value: rDelta));
    }

    /// <summary>
    /// Estimated change to reach minimum in the selected axis direction
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      Number value,
      IArray<Sign> axis,
      Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(value, derivative);
      return new NumberTranslationVector(NumberVector.NonZeroValueAt(pos: axis, value: rDelta));
    }

    /// <summary>
    /// Estimated change to reach minimum in direction of an angle
    /// </summary>
    public static HypersphericalAngleTranslationVector HypersphericalForMinimumEquals0(
      Number value,
      uint anglePos,
      Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(value, derivative);
      return new HypersphericalAngleTranslationVector(NumberVector.NonZeroValueAt(pos: anglePos, value: rDelta).ToAngleVector());
    }

    /// <summary>
    /// Estimated change to reach minimum in the angle vector direction
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      Number value,
      HypersphericalAngleVector direction,
      Derivative1Value derivative)
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


    /// <summary>
    /// Estimated change to reach minimum in direction of an angle
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      Number value,
      HypersphericalCoordinateOnAxisView currentHcView,
      uint anglePos,
      Derivative1Value derivative)
    {
      Number angleDelta = ForMinimumEquals0(value, derivative);

      if (angleDelta < -1 * Angle.RightAngle)
      {
        angleDelta = -1 * Angle.RightAngle;
      }
      else if (angleDelta > Angle.RightAngle)
      {
        angleDelta = Angle.RightAngle;
      }

      HypersphericalCoordinate currentHc = currentHcView;
      HypersphericalCoordinate newHc = currentHc.TranslateByAngle(anglePos, angleDelta);
      CartesianCoordinate newCartesian = newHc.ToCartesianView();

      return new NumberTranslationVector(newCartesian.Coordinates - currentHcView.Coordinates);
    }
  }
}
