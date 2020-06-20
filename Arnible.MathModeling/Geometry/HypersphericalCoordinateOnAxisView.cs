using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct HypersphericalCoordinateOnAxisView :
    IEquatable<HypersphericalCoordinateOnAxisView>,
    IEquatable<CartesianCoordinate>,
    IEquatable<HypersphericalCoordinate>,
    ICartesianCoordinate,
    IHypersphericalCoordinate
  {
    private readonly HypersphericalCoordinate _p;

    public static IEnumerable<Derivative1Value> DerivativeByR(HypersphericalAngleVector angles)
    {
      return angles.GetCartesianAxisViewsRatios().Select(v => new Derivative1Value(v));
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      _p = p;
      Coordinates = p.Angles.GetCartesianAxisViewsRatios().Select(v => p.R * v).ToVector();
    }

    public HypersphericalCoordinateOnAxisView(CartesianCoordinate p)
    {
      Coordinates = p.Coordinates;
      _p = p.ToSpherical();
    }

    public static implicit operator CartesianCoordinate(HypersphericalCoordinateOnAxisView rc)
    {
      return new CartesianCoordinate(rc.Coordinates);
    }

    public static implicit operator HypersphericalCoordinate(HypersphericalCoordinateOnAxisView rc)
    {
      return rc._p;
    }

    public override bool Equals(object obj)
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

    public bool Equals(CartesianCoordinate other)
    {
      return other.Equals(this);
    }

    public bool Equals(HypersphericalCoordinate other)
    {
      return other.Equals(this);
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

    public NumberVector Coordinates { get; }

    public uint DimensionsCount => Coordinates.Length;

    //
    // Operators
    //        

    public IEnumerable<HypersphericalAngleVector> CartesianCoordinatesAngles()
    {
      uint anglesCount = _p.Angles.Length;
      Number[] x = LinqEnumerable.Repeat<Number>(0, anglesCount).ToArray();

      yield return x.ToAngleVector();
      for (uint i = 0; i < anglesCount; ++i)
      {
        x[i] = Angle.RightAngle;
        yield return x.ToAngleVector();
        x[i] = 0;
      }
    }

    public IEnumerable<Derivative1Value> DerivativeByR() => DerivativeByR(_p.Angles);

    public HypersphericalCoordinateOnRectangularView GetRectangularView(uint axisA, uint axisB)
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
          x: Coordinates.GetOrDefault(axisA),
          y: Coordinates.GetOrDefault(axisB));
      }
    }

    public HypersphericalCoordianteOnLineView GetLineView(uint axis)
    {
      if (R == 0)
      {
        return default;
      }
      else
      {
        return new HypersphericalCoordianteOnLineView(r: R, ratioX: Coordinates.GetOrDefault(axis) / R);
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
      return Coordinates.Select(c => c / r);
    }
  }
}
