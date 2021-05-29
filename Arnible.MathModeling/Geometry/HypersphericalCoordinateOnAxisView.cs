using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Analysis;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct HypersphericalCoordinateOnAxisView : IEquatable<HypersphericalCoordinateOnAxisView>
  {
    private readonly HypersphericalCoordinate _p;

    /// <summary>
    /// Return ratio of an identity vector. All coefficients should be equal to it
    /// </summary>
    public static Number GetIdentityVectorRatio(uint dimensionsCount)
    {
      switch (dimensionsCount)
      {
        case 0:
          throw new ArgumentException(nameof(dimensionsCount));
        case 1:
          return 1;
      }

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
        HypersphericalAngleVector.GetIdentityVector((uint)result.Length).GetCartesianAxisViewsRatios(in result);
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

    public override bool Equals(object? obj)
    {
      if (obj is HypersphericalCoordinateOnAxisView casted)
      {
        return Equals(casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(HypersphericalCoordinateOnAxisView other)
    {
      return Coordinates == other.Coordinates;
    }

    public override int GetHashCode()
    {
      return Coordinates.GetHashCode();
    }

    public override string ToString()
    {
      return Coordinates.ToString();
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

    public IEnumerable<HypersphericalAngleVector> CartesianCoordinatesAngles()
    {
      uint anglesCount = _p.Angles.Length;
      List<Number> x = new(LinqEnumerable.Repeat<Number>(0, anglesCount));

      yield return x.ToAngleVector();
      for (int i = 0; i < anglesCount; ++i)
      {
        x[i] = Angle.RightAngle;
        yield return x.ToAngleVector();
        x[i] = 0;
      }
    }

    public void DerivativeByR(in Span<Number> derivativeOnAxis)
    {
      Angles.GetCartesianAxisViewsRatios(in derivativeOnAxis);
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

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives GetAngleDerivativesView(uint anglesCount, uint anglePos)
    {
      return new HypersphericalCoordinateOnAxisViewForAngleDerivatives(view: this, anglesCount: anglesCount, anglePos: anglePos);
    }

    public IEnumerable<Derivative1Value> GetCartesianAxisViewsRatiosDerivativesByAngle(uint anglesCount, uint anglePos)
    {
      return GetAngleDerivativesView(anglesCount: anglesCount, anglePos: anglePos).CartesianAxisViewsRatiosDerivatives;
    }

    public IEnumerable<Number> GetCoordinatesRatios()
    {
      Number r = R;
      return Coordinates.AsList().Select(c => c / r);
    }
  }
}
