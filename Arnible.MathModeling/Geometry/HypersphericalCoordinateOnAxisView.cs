using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using static Arnible.MathModeling.MetaMath;

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
      return GetCartesianAxisViewsRatios(angles).Select(v => new Derivative1Value(v));
    }

    private static IEnumerable<Number> GetCartesianAxisViewsRatios(HypersphericalAngleVector angles)
    {
      var cartesianDimensions = new List<Number>();
      Number replacement = 1;
      foreach (var angle in angles.Reverse())
      {
        cartesianDimensions.Add(replacement * Sin(angle));
        replacement *= Cos(angle);
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return cartesianDimensions;
    }

    private static IEnumerable<IDerivative1> GetCartesianAxisViewsRatiosDerivativesByAngle(uint pos, HypersphericalAngleVector angles)
    {
      if (pos >= angles.Length)
      {
        throw new ArgumentException(nameof(pos));
      }

      var cartesianDimensions = new List<Number>();
      Number replacement = 1;
      uint currentAnglePos = angles.Length;
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

      return cartesianDimensions.Select(v => new Derivative1Value(v));
    }

    public HypersphericalCoordinateOnAxisView(HypersphericalCoordinate p)
    {
      _p = p;
      _viewRatio = GetCartesianAxisViewsRatios(p.Angles).ToVector();
      Coordinates = _viewRatio.Select(v => p.R * v).ToVector();
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

      yield return new HypersphericalAngleVector(x.ToVector());
      for (uint i = 0; i < anglesCount; ++i)
      {
        x[i] = Angle.RightAngle;
        yield return new HypersphericalAngleVector(x.ToVector());
        x[i] = 0;
      }
    }

    public IEnumerable<IDerivative1> DerivativeByR() => DerivativeByR(_p.Angles);

    public IEnumerable<IDerivative1> GetCartesianAxisViewsRatiosDerivativesByAngle(uint anglePos)
    {
      return GetCartesianAxisViewsRatiosDerivativesByAngle(anglePos, Angles);
    }

    public HypersphericalCoordinateOnRectangularView GetRectangularView(uint axisA, uint axisB)
    {
      if (axisA >= _viewRatio.Length)
      {
        throw new ArgumentException(nameof(axisA));
      }
      if (axisB >= _viewRatio.Length || axisA == axisB)
      {
        throw new ArgumentException(nameof(axisB));
      }      

      return new HypersphericalCoordinateOnRectangularView(
        r: R,
        ratioX: _viewRatio[axisA],        
        ratioY: _viewRatio[axisB]);
    }

    public HypersphericalCoordinateOnRectangularViewWithDerivative GetRectangularViewDerivativeByAngle(uint axisA, uint axisB, uint anglePos)
    {      
      IDerivative1[] derivatives = GetCartesianAxisViewsRatiosDerivativesByAngle(anglePos).ToArray();

      return new HypersphericalCoordinateOnRectangularViewWithDerivative(
        view: GetRectangularView(axisA: axisA, axisB: axisB),
        xDerivative: derivatives[axisA],
        yDerivative: derivatives[axisB]);
    }
  }
}
