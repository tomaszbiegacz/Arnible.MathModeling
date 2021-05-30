using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Analysis;

namespace Arnible.MathModeling.Geometry
{
  public readonly ref struct HypersphericalCoordinateOnAxisView
  {
    private readonly HypersphericalCoordinate _p;

    /// <summary>
    /// Return ratio of an identity vector. All coefficients should be equal to it
    /// </summary>
    public static Number GetIdentityVectorRatio(ushort dimensionsCount)
    {
      var angles = HypersphericalAngleVector.GetIdentityVector(dimensionsCount);
      double lastAngle = (double)angles[(ushort)(angles.Length - 1)];
      return Math.Sin(lastAngle);
    }
    
    /// <summary>
    /// Get identity vector coefficients
    /// </summary>
    public static void GetIdentityVector(in Span<Number> result)
    {
      if (result.Length > 0 )
      {
        HypersphericalAngleVector.GetIdentityVector((ushort)result.Length).GetCartesianAxisViewsRatios(in result);
      }
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      Number[] perAxisRatios = new Number[p.DimensionsCount];
      p.Angles.GetCartesianAxisViewsRatios(perAxisRatios);
      
      _p = p;
      Coordinates = perAxisRatios.Multiply(p.R);
    }

    public HypersphericalCoordinateOnAxisView(IReadOnlyList<Number> p)
    {
      Coordinates = p.ToArray();
      _p = p.ToSpherical();
    }

    public static implicit operator HypersphericalCoordinate(HypersphericalCoordinateOnAxisView rc)
    {
      return rc._p;
    }

    //
    // Properties
    //

    public Number R => _p.R;

    public HypersphericalAngleVector Angles => _p.Angles;

    public ReadOnlyArray<Number> Coordinates { get; }

    public ushort DimensionsCount => Coordinates.Length;

    //
    // Operators
    //        

    public void DerivativeByR(in Span<Number> derivativeOnAxis)
    {
      Angles.GetCartesianAxisViewsRatios(in derivativeOnAxis);
    }
    
    public HypersphericalAngleVector CartesianCoordinatesAngles(ushort dimensionPos)
    {
      uint anglesCount = _p.Angles.Length;
      Span<Number> x = LinqEnumerable.Repeat<Number>(0, anglesCount).ToArray();

      if(dimensionPos == 0)
      {
        // on 'X' axis all angles are O
        return new HypersphericalAngleVector(x);
      }
      else
      {
        ushort anglePos = (ushort)(dimensionPos - 1);
        x[anglePos] = Angle.RightAngle;
        return new HypersphericalAngleVector(x);
      }
    }

    public HypersphericalCoordinateOnRectangularView GetRectangularView(ushort axisA, ushort axisB)
    {
      if (axisA == axisB)
      {
        throw new ArgumentException("axis a and b are the same.");
      }

      if (R == 0)
      {
        return default;
      }
      else
      {
        return HypersphericalCoordinateOnRectangularView.FromCartesian(
          r: R,
          x: Coordinates.AsList().GetOrDefault(axisA),
          y: Coordinates.AsList().GetOrDefault(axisB));
      }
    }

    public HypersphericalCoordianteOnLineView GetLineView(ushort axis)
    {
      if (R == 0)
      {
        return default;
      }
      else
      {
        return new HypersphericalCoordianteOnLineView(r: R, ratioX: Coordinates.AsList().GetOrDefault(axis) / R);
      }
    }

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives GetAngleDerivativesView(ushort anglePos)
    {
      return new HypersphericalCoordinateOnAxisViewForAngleDerivatives(view: this, anglePos: anglePos);
    }

    public IEnumerable<Number> GetCoordinatesRatios()
    {
      Number r = R;
      return Coordinates.AsList().Select(c => c / r);
    }
  }
}
