using System;

namespace Arnible.MathModeling.Geometry
{
  public interface IHypersphericalCoordinateOnRectangularView : IRectangularCoordianate, IHypersphericalDirectionOnRectangularView
  {
    Number R { get; }
  }

  public readonly struct HypersphericalCoordinateOnRectangularView :
    IEquatable<HypersphericalCoordinateOnRectangularView>,
    IHypersphericalCoordinateOnRectangularView
  {
    public static HypersphericalCoordinateOnRectangularView FromCartesian(Number r, Number x, Number y)
    {
      if (r == 0)
      {
        if (x != 0 || y != 0)
        {
          throw new ArgumentException("when r is zero, cartesian coordiantes should be too");
        }
        return default;
      }
      else
      {
        return new HypersphericalCoordinateOnRectangularView(
          r: r,
          ratioX: x / r,
          x: x,
          ratioY: y / r,
          y: y
          );
      }
    }

    public static HypersphericalCoordinateOnRectangularView FromRatios(Number r, Number ratioX, Number ratioY)
    {
      if (r == 0)
      {
        if (ratioX != 0 || ratioX != 0)
        {
          throw new ArgumentException("when r is zero, cartesian coordiantes should be too");
        }
        return default;
      }
      else
      {
        return new HypersphericalCoordinateOnRectangularView(
          r: r,
          ratioX: ratioX,
          x: r * ratioX,
          ratioY: ratioY,
          y: r * ratioY
          );
      }
    }

    private HypersphericalCoordinateOnRectangularView(
      Number r,
      Number ratioX,
      Number x,
      Number ratioY,
      Number y)
    {
      if (r < 0)
      {
        throw new ArgumentException(nameof(r));
      }
      R = r;

      X = x;
      Y = y;
      RatioX = ratioX;
      RatioY = ratioY;

      if (r == 0 && (X != 0 || Y != 0))
      {
        throw new ArgumentException("r cannot be zero with non-zero ratios");
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalCoordinateOnRectangularView casted)
      {
        return Equals(casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(HypersphericalCoordinateOnRectangularView other)
    {
      return R == other.R && X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
      int hash = 17;
      hash = hash * 23 + R.GetHashCode();
      hash = hash * 23 + X.GetHashCode();
      hash = hash * 23 + Y.GetHashCode();
      return hash;
    }

    public override string ToString()
    {
      return $"({X}, {Y})";
    }

    //
    // Properties
    //

    public Number R { get; }

    public Number RatioX { get; }

    public Number RatioY { get; }

    //
    // IRectangularCoordianate
    //

    public Number X { get; }

    public Number Y { get; }
  }
}
