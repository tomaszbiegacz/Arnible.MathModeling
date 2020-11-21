using System;

namespace Arnible.MathModeling.Geometry
{
  interface IHypersphericalCoordinateOnRectangularView : IRectangularCoordinate, IHypersphericalDirectionOnRectangularView
  {
    Number R { get; }
  }

  public readonly struct HypersphericalCoordinateOnRectangularView :
    IEquatable<HypersphericalCoordinateOnRectangularView>,
    IHypersphericalCoordinateOnRectangularView
  {
    public static HypersphericalCoordinateOnRectangularView FromCartesian(in Number r, in Number x, in Number y)
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
          r: in r,
          ratioX: x / r,
          x: in x,
          ratioY: y / r,
          y: in y
          );
      }
    }

    public static HypersphericalCoordinateOnRectangularView FromRatios(in Number r, in Number ratioX, in Number ratioY)
    {
      if (ratioX < -1 || ratioX > 1)
      {
        throw new ArgumentException(nameof(ratioX));
      }
      if (ratioY < -1 || ratioY > 1)
      {
        throw new ArgumentException(nameof(ratioY));
      }

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
          r: in r,
          ratioX: in ratioX,
          x: r * ratioX,
          ratioY: in ratioY,
          y: r * ratioY
          );
      }
    }

    private HypersphericalCoordinateOnRectangularView(
      in Number r,
      in Number ratioX,
      in Number x,
      in Number ratioY,
      in Number y)
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
        return Equals(in casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(in HypersphericalCoordinateOnRectangularView other)
    {
      return R == other.R && X == other.X && Y == other.Y;
    }

    public bool Equals(HypersphericalCoordinateOnRectangularView other) => Equals(in other);

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
      return $"({X.ToString()}, {Y.ToString()})";
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
