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

    public static IEnumerable<Derivative1Value> DerivativeByR(in HypersphericalAngleVector angles)
    {
      return angles.GetCartesianAxisViewsRatios().Select(v => new Derivative1Value(v));
    }

    public static NumberVector GetIdentityVector(in uint dimensionsCount)
    {
      switch(dimensionsCount)
      {
        case 0:
          throw new ArgumentException(nameof(dimensionsCount));
        case 1:
          return new NumberVector(1);
      }

      return HypersphericalAngleVector.GetIdentityVector(in dimensionsCount).GetCartesianAxisViewsRatios();
    }

    public static Number GetIdentityVectorRatio(in uint dimensionsCount)
    {
      switch (dimensionsCount)
      {
        case 0:
          throw new ArgumentException(nameof(dimensionsCount));
        case 1:
          return 1;
      }

      var angles = HypersphericalAngleVector.GetIdentityVector(in dimensionsCount);
      double lastAngle = (double)angles[angles.Length - 1];
      return Math.Sin(lastAngle);
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      _p = p;
      Coordinates = p.Angles.GetCartesianAxisViewsRatios().Select(v => p.R * v).ToVector();
    }

    public HypersphericalCoordinateOnAxisView(in CartesianCoordinate p)
    {
      Coordinates = p.Coordinates;
      _p = p.ToSpherical();
    }

    public static implicit operator CartesianCoordinate(in HypersphericalCoordinateOnAxisView rc)
    {
      return rc.Coordinates;
    }

    public static implicit operator HypersphericalCoordinate(in HypersphericalCoordinateOnAxisView rc)
    {
      return rc._p;
    }

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalCoordinateOnAxisView casted)
      {
        return Equals(in casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(in HypersphericalCoordinateOnAxisView other)
    {
      return Coordinates == other.Coordinates;
    }

    public bool Equals(HypersphericalCoordinateOnAxisView other) => Equals(in other);

    public bool Equals(in CartesianCoordinate other)
    {
      return other.Equals(this);
    }

    public bool Equals(CartesianCoordinate other) => Equals(in other);

    public bool Equals(in HypersphericalCoordinate other)
    {
      return other.Equals(this);
    }

    public bool Equals(HypersphericalCoordinate other) => Equals(in other);

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
      List<Number> x = LinqEnumerable.Repeat<Number>(0, anglesCount).ToList();

      yield return x.ToAngleVector();
      for (int i = 0; i < anglesCount; ++i)
      {
        x[i] = Angle.RightAngle;
        yield return x.ToAngleVector();
        x[i] = 0;
      }
    }

    public IEnumerable<Derivative1Value> DerivativeByR() => DerivativeByR(_p.Angles);

    public HypersphericalCoordinateOnRectangularView GetRectangularView(in uint axisA, in uint axisB)
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

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives GetAngleDerivativesView(in uint anglesCount, in uint anglePos)
    {
      return new HypersphericalCoordinateOnAxisViewForAngleDerivatives(view: this, anglesCount: in anglesCount, anglePos: in anglePos);
    }

    public IEnumerable<Derivative1Value> GetCartesianAxisViewsRatiosDerivativesByAngle(in uint anglesCount, in uint anglePos)
    {
      return GetAngleDerivativesView(anglesCount: in anglesCount, anglePos: in anglePos).CartesianAxisViewsRatiosDerivatives;
    }

    public IEnumerable<Number> GetCoordinatesRatios()
    {
      Number r = R;
      return Coordinates.Select(c => c / r);
    }
  }
}
