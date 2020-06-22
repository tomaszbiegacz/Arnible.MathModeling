using System;

namespace Arnible.MathModeling.Geometry
{
  interface IHypersphericalCoordinateOnLineView
  {
    Number R { get; }

    Number RatioX { get; }

    Number X { get; }
  }

  public readonly struct HypersphericalCoordianteOnLineView :
    IEquatable<HypersphericalCoordianteOnLineView>,
    IHypersphericalCoordinateOnLineView
  {
    public HypersphericalCoordianteOnLineView(Number r, Number ratioX)
    {
      if (r < 0)
      {
        throw new ArgumentException(nameof(r));
      }
      if(r == 0 && ratioX != 0)
      {
        throw new ArgumentException("when r is zero, cartesian coordiantes should be too");
      }
      R = r;

      if (ratioX < -1 || ratioX > 1)
      {
        throw new ArgumentException(nameof(ratioX));
      }
      RatioX = ratioX;
      X = r * ratioX;
    }

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalCoordianteOnLineView casted)
      {
        return Equals(casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(HypersphericalCoordianteOnLineView other)
    {
      return R == other.R && RatioX == other.RatioX;
    }

    public override int GetHashCode()
    {
      return R.GetHashCode() ^ RatioX.GetHashCode();
    }

    public override string ToString()
    {
      return $"{{{R}, {RatioX}}}";
    }

    //
    // Properties
    //

    public Number R { get; }

    public Number RatioX { get; }

    public Number X { get; }
  }
}
