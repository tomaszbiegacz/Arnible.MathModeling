using System;
using System.Collections.Generic;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public class HypersphericalCoordinateOnAxisViewForAngleDerivatives
  {
    private static NumberArray GetCartesianAxisViewsRatiosDerivativesByAngle(HypersphericalAngleVector anglesVector, uint anglesCount, uint pos)
    {
      if (anglesCount < anglesVector.Length)
      {
        throw new ArgumentException(nameof(anglesCount));
      }
      if (pos >= anglesCount)
      {
        throw new ArgumentException(nameof(pos));
      }

      Number[] angles = anglesVector.Concat(LinqEnumerable.Repeat<Number>(0, anglesCount - anglesVector.Length)).ToArray();

      var cartesianDimensions = new List<Number>();
      Number replacement = 1;
      int currentAnglePos = angles.Length;
      foreach (var angle in angles.Reverse())
      {
        currentAnglePos--;
        if (currentAnglePos == pos)
        {
          // derivative by angle
          if (currentAnglePos <= pos)
          {
            cartesianDimensions.Add(replacement * Cos(angle));
          }
          else
          {
            cartesianDimensions.Add(0);
          }
          replacement *= -1 * Sin(angle);
        }
        else
        {
          if (currentAnglePos <= pos)
          {
            cartesianDimensions.Add(replacement * Sin(angle));
          }
          else
          {
            cartesianDimensions.Add(0);
          }
          replacement *= Cos(angle);
        }
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return cartesianDimensions.Concat(LinqEnumerable.Repeat<Number>(0, (uint)cartesianDimensions.Count - anglesCount - 1)).ToNumberArray();
    }

    private NumberArray _angleDerivatives;

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives(HypersphericalCoordinateOnAxisView view, uint anglesCount, uint anglePos)
    {
      View = view;
      AnglePos = anglePos;
      _angleDerivatives = GetCartesianAxisViewsRatiosDerivativesByAngle(view.Angles, anglesCount: anglesCount, pos: anglePos);

      if(_angleDerivatives.Length != anglesCount + 1)
      {
        throw new InvalidOperationException("Something went wrong");
      }
    }    

    //
    // Properties
    //

    public HypersphericalCoordinateOnAxisView View { get; }

    public uint AnglePos { get; }

    public IEnumerable<IDerivative1> CartesianAxisViewsRatiosDerivatives => _angleDerivatives.Select<Number, IDerivative1>(v => new Derivative1Value(v));

    //
    // Operations
    //

    public HypersphericalCoordinateOnRectangularViewWithDerivative GetRectangularViewDerivativeByAngle(uint axisA, uint axisB)
    {
      return new HypersphericalCoordinateOnRectangularViewWithDerivative(
        view: View.GetRectangularView(axisA: axisA, axisB: axisB),
        xDerivative: new Derivative1Value(_angleDerivatives[axisA]),
        yDerivative: new Derivative1Value(_angleDerivatives[axisB])
        );
    }
  }
}
