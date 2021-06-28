using System;
using System.Collections.Generic;
using Arnible.Assertions;

namespace Arnible.MathModeling.Geometry
{
  public readonly ref struct HypersphericalCoordinateOnAxisViewForAngleDerivatives
  {
    private static Number[] GetCartesianAxisViewsRatiosDerivativesByAngle(
      in Number r, 
      in HypersphericalAngleVector angles,
      in ushort pos)
    {
      pos.AssertIsLessEqualThan(angles.Length);

      var cartesianDimensions = new List<Number>();
      Number replacement = r;
      for(int currentAnglePos = angles.Length - 1; currentAnglePos >= 0; --currentAnglePos)
      {
        ref readonly Number angle = ref angles.Span[(ushort)currentAnglePos]; 
        if (currentAnglePos == pos)
        {
          // derivative by angle
          if (currentAnglePos <= pos)
          {
            cartesianDimensions.Add(replacement * NumberMath.Cos(angle));
          }
          else
          {
            cartesianDimensions.Add(0);
          }
          replacement *= -1 * NumberMath.Sin(angle);
        }
        else
        {
          if (currentAnglePos <= pos)
          {
            cartesianDimensions.Add(replacement * NumberMath.Sin(angle));
          }
          else
          {
            cartesianDimensions.Add(0);
          }
          replacement *= NumberMath.Cos(angle);
        }
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return cartesianDimensions.ToArray();
    }

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives(
      in HypersphericalCoordinate view,
      in ushort anglePos)
    {
      Point = view;
      AnglePos = anglePos;
      DerivativesCartesianVector = GetCartesianAxisViewsRatiosDerivativesByAngle(view.R, view.Angles, pos: anglePos);
      DerivativesCartesianVector.Length.AssertIsEqualTo(view.Angles.Length + 1);
    }    

    //
    // Properties
    //

    public HypersphericalCoordinate Point { get; }

    public ushort AnglePos { get; }

    public ReadOnlySpan<Number> DerivativesCartesianVector { get; }
  }
}
