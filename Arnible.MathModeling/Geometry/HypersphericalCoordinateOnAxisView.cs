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
    private readonly NumberVector _viewRatio;

    public static IEnumerable<IDerivative1> DerivativeByR(HypersphericalAngleVector angles)
    {
      return angles.GetCartesianAxisViewsRatios().Select<Number, IDerivative1>(v => new Derivative1Value(v));
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      _p = p;
      _viewRatio = p.Angles.GetCartesianAxisViewsRatios();
      Coordinates = _viewRatio.Select(v => p.R * v).ToVector();

      if (_viewRatio.Length != Coordinates.Length)
      {
        throw new InvalidOperationException($"Something is wrong: view ration card {_viewRatio.Length}, coordinates card {Coordinates.Length}");
      }
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
      return R == other.R && _viewRatio == other._viewRatio;
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
      return R.GetHashCode() ^ _viewRatio.GetHashCode();
    }

    public override string ToString()
    {
      return $"{_viewRatio} on {R}";
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

    public IEnumerable<IDerivative1> DerivativeByR() => DerivativeByR(_p.Angles);

    public HypersphericalCoordinateOnRectangularView GetRectangularView(uint axisA, uint axisB)
    {
      if (axisA == axisB)
      {
        throw new ArgumentException("axis a and b are the same.");
      }

      if (R == 0)
      {
        return new HypersphericalCoordinateOnRectangularView(0, 0, 0);
      }
      else
      {
        return new HypersphericalCoordinateOnRectangularView(
          r: R,
          ratioX: _viewRatio.GetOrDefault(axisA),
          ratioY: _viewRatio.GetOrDefault(axisB));
      }
    }

    public HypersphericalCoordinateOnAxisViewForAngleDerivatives GetAngleDerivativesView(uint anglesCount, uint anglePos)
    {
      return new HypersphericalCoordinateOnAxisViewForAngleDerivatives(view: this, anglesCount: anglesCount, anglePos: anglePos);
    }

    public IEnumerable<IDerivative1> GetCartesianAxisViewsRatiosDerivativesByAngle(uint anglesCount, uint anglePos)
    {
      return GetAngleDerivativesView(anglesCount: anglesCount, anglePos: anglePos).CartesianAxisViewsRatiosDerivatives;
    }
  }
}
