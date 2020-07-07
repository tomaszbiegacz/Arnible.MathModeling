using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Geometry
{
  public static class OptimizationTranslation
  {
    /// <summary>
    /// Estimated change to reach minimum in 1 dimentional case
    /// </summary>
    public static Number ForMinimumEquals0(in Number value, in Derivative1Value derivative)
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
      in Number value,
      in uint cartesiaxAxisNumber,
      in Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(in value, in derivative);
      return new NumberTranslationVector(NumberVector.NonZeroValueAt(pos: in cartesiaxAxisNumber, value: in rDelta));
    }

    /// <summary>
    /// Estimated change to reach minimum in the selected axis direction
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      in Number value,
      in IUnmanagedArray<Sign> axis,
      in Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(in value, in derivative);
      return new NumberTranslationVector(NumberVector.NonZeroValueAt(pos: in axis, value: rDelta));
    }

    /// <summary>
    /// Estimated change to reach minimum in direction of an angle
    /// </summary>
    public static HypersphericalAngleTranslationVector HypersphericalForMinimumEquals0(
      in Number value,
      in uint anglePos,
      in Derivative1Value derivative)
    {
      Number rDelta = ForMinimumEquals0(in value, in derivative);
      return new HypersphericalAngleTranslationVector(NumberVector.NonZeroValueAt(pos: in anglePos, value: in rDelta).ToAngleVector());
    }

    /// <summary>
    /// Estimated change to reach minimum in the angle vector direction
    /// </summary>
    public static NumberTranslationVector CartesianForMinimumEquals0(
      in Number value,
      in HypersphericalAngleVector direction,
      in Derivative1Value derivative)
    {
      HypersphericalCoordinate hc;
      Number rDelta = ForMinimumEquals0(in value, in derivative);

      if (rDelta > 0)
      {
        hc = new HypersphericalCoordinate(in rDelta, in direction);
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
      in Number value,
      in HypersphericalCoordinateOnAxisView currentHcView,
      in uint anglePos,
      in Derivative1Value derivative)
    {
      Number angleDelta = ForMinimumEquals0(in value, in derivative);

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
