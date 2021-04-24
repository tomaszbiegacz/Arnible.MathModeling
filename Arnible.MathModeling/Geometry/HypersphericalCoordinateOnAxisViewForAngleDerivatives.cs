using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.MathModeling.Analysis;

namespace Arnible.MathModeling.Geometry
{
  public class HypersphericalCoordinateOnAxisViewForAngleDerivatives
  {
    private static ReadOnlyArray<Number> GetCartesianAxisViewsRatiosDerivativesByAngle(
      in Number r, 
      in HypersphericalAngleVector anglesVector, 
      in uint anglesCount, 
      in uint pos)
    {
      if (anglesCount < anglesVector.Length)
      {
        throw new ArgumentException(nameof(anglesCount));
      }
      if (pos >= anglesCount)
      {
        throw new ArgumentException(nameof(pos));
      }

      IReadOnlyCollection<Number> angles = anglesVector
        .GetInternalEnumerable()
        .Concat(LinqEnumerable.Repeat<Number>(0, anglesCount - anglesVector.Length))
        .ToArray();

      var cartesianDimensions = new List<Number>();
      Number replacement = r;
      uint currentAnglePos = (uint)angles.Count;
      foreach (var angle in angles.Reverse())
      {
        currentAnglePos--;
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

      return cartesianDimensions.Concat(LinqEnumerable.Repeat<Number>(0, (uint)(cartesianDimensions.Count - anglesCount - 1))).ToArray();
    }

    private readonly ReadOnlyArray<Number> _angleDerivatives;

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives(
      in HypersphericalCoordinateOnAxisView view, 
      in uint anglesCount, 
      in uint anglePos)
    {
      View = view;
      AnglePos = anglePos;
      _angleDerivatives = GetCartesianAxisViewsRatiosDerivativesByAngle(view.R, view.Angles, anglesCount: in anglesCount, pos: in anglePos);

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

    public IEnumerable<Derivative1Value> CartesianAxisViewsRatiosDerivatives => _angleDerivatives.AsList().Select(v => new Derivative1Value(v));

    //
    // Operations
    //

    public HypersphericalCoordinateOnRectangularViewWithDerivative GetRectangularViewDerivativeByAngle(ushort axisA, ushort axisB)
    {
      return new HypersphericalCoordinateOnRectangularViewWithDerivative(
        view: View.GetRectangularView(axisA: axisA, axisB: axisB),
        xDerivative: new Derivative1Value(_angleDerivatives[axisA]),
        yDerivative: new Derivative1Value(_angleDerivatives[axisB])
        );
    }
  }
}
