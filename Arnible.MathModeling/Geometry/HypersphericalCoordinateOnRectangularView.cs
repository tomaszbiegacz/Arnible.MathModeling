﻿using System;

namespace Arnible.MathModeling.Geometry
{
  public interface IHypersphericalCoordinateOnRectangularView : IRectangularCoordianate
  {
    Number R { get; }

    Number RatioX { get; }

    Number RatioY { get; }
  }

  public readonly struct HypersphericalCoordinateOnRectangularView :
    IEquatable<HypersphericalCoordinateOnRectangularView>,
    IHypersphericalCoordinateOnRectangularView
  {
    public HypersphericalCoordinateOnRectangularView(
      Number r,
      Number ratioX,
      Number ratioY)
    {
      if (r < 0)
      {
        throw new ArgumentException(nameof(r));
      }
      R = r;

      if (ratioX < 0 || ratioX > 1)
      {
        throw new ArgumentException(nameof(ratioX));
      }
      RatioX = ratioX;

      if (ratioY < 0 || ratioY > 1 || ratioX == ratioY)
      {
        throw new ArgumentException(nameof(ratioY));
      }
      RatioY = ratioY;
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
      return R == other.R && RatioX == other.RatioX && RatioY == other.RatioY;
    }

    public override int GetHashCode()
    {
      int hash = 17;
      hash = hash * 23 + R.GetHashCode();
      hash = hash * 23 + RatioX.GetHashCode();
      hash = hash * 23 + RatioY.GetHashCode();
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

    public Number X => R * RatioX;

    public Number Y => R * RatioY;
  }
}
