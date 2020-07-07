using System;

namespace Arnible.MathModeling.Geometry
{
  interface IHypersphericalDirectionOnRectangularView
  {
    Number RatioX { get; }

    Number RatioY { get; }
  }

  public readonly struct HypersphericalDirectionOnRectangularView :
    IEquatable<HypersphericalDirectionOnRectangularView>,
    IHypersphericalDirectionOnRectangularView
  {
    public HypersphericalDirectionOnRectangularView(
      in Number ratioX,
      in Number ratioY)
    {
      if (ratioX < -1 || ratioX > 1)
      {
        throw new ArgumentException(nameof(ratioX));
      }
      RatioX = ratioX;

      if (ratioY < -1 || ratioY > 1)
      {
        throw new ArgumentException(nameof(ratioY));
      }
      RatioY = ratioY;
    }

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalDirectionOnRectangularView casted)
      {
        return Equals(in casted);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(in HypersphericalDirectionOnRectangularView other)
    {
      return RatioX == other.RatioX && RatioY == other.RatioY;
    }

    public bool Equals(HypersphericalDirectionOnRectangularView other) => Equals(in other);

    public override int GetHashCode()
    {
      return RatioX.GetHashCode() ^ RatioY.GetHashCode();
    }

    public override string ToString()
    {
      return $"({RatioX}, {RatioY})";
    }

    //
    // Properties
    //

    public Number RatioX { get; }

    public Number RatioY { get; }
  }
}
