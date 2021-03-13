using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using Arnible.Linq;

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
      return angles.GetCartesianAxisViewsRatios().GetInternalEnumerable().Select(v => new Derivative1Value(v));
    }

    /// <summary>
    /// Get identity vector coefficients
    /// </summary>
    public static NumberVector GetIdentityVector(uint dimensionsCount)
    {
      switch(dimensionsCount)
      {
        case 0:
          throw new ArgumentException(nameof(dimensionsCount));
        case 1:
          return new NumberVector(1);
      }

      return HypersphericalAngleVector.GetIdentityVector(dimensionsCount).GetCartesianAxisViewsRatios();
    }

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
      double lastAngle = (double)angles[angles.Length - 1];
      return Math.Sin(lastAngle);
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      _p = p;
      Coordinates = p.Angles.GetCartesianAxisViewsRatios().GetInternalEnumerable().Select(v => p.R * v).ToVector();
    }

    public HypersphericalCoordinateOnAxisView(CartesianCoordinate p)
    {
      Coordinates = p.Coordinates;
      _p = p.ToSpherical();
    }

    public static implicit operator CartesianCoordinate(HypersphericalCoordinateOnAxisView rc)
    {
      return rc.Coordinates;
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
      return Coordinates.GetInternalEnumerable().Select(c => c / r);
    }
  }
}
