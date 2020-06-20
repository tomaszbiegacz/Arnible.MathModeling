using System;

namespace Arnible.MathModeling.Geometry
{
  interface IHypersphericalCoordinate
  {
    Number R { get; }
    HypersphericalAngleVector Angles { get; }
  }

  public readonly struct HypersphericalCoordinate : IEquatable<HypersphericalCoordinate>, IHypersphericalCoordinate, ICoordinate<HypersphericalCoordinate>
  {
    /// <summary>
    /// Angles:
    /// - from x axis to r(xy) range over [-π, π]
    /// - from xy plane to r(xyz) range over [-π/2, π/2]
    /// - etc..
    /// </summary>
    /// <remarks>
    /// New (hidden) dimension is being created by adding 0 angle
    /// </remarks>
    public HypersphericalAngleVector Angles { get; }

    public Number R { get; }

    public HypersphericalCoordinate(Number r, HypersphericalAngleVector angles)
    {
      if (r < 0)
      {
        throw new ArgumentException($"Negative r: {r}");
      }
      if (r == 0 && angles != 0)
      {
        throw new ArgumentException($"For zero r, angles also has to be empty, got {angles}");
      }

      R = r;
      Angles = angles;
    }

    public static implicit operator HypersphericalCoordinate(PolarCoordinate pc)
    {
      return new HypersphericalCoordinate(pc.R, new HypersphericalAngleVector(pc.Φ));
    }

    public bool Equals(HypersphericalCoordinate other)
    {
      return other.R == R && other.Angles == Angles;
    }

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalCoordinate typed)
      {
        return Equals(typed);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return R.GetHashCode() ^ Angles.GetHashCode();
    }

    public override string ToString()
    {
      return $"{{{R}, {Angles}}}";
    }

    //
    // Properties
    //

    public uint DimensionsCount => Angles.Length + 1;

    //
    // Operations
    //

    public HypersphericalCoordinate AddDimension()
    {
      return new HypersphericalCoordinate(R, Angles.AddDimension());
    }

    public HypersphericalCoordinateOnAxisView ToCartesianView() => new HypersphericalCoordinateOnAxisView(this);

    public HypersphericalCoordinate TranslateByAngle(uint anglePos, Number delta)
    {
      var angleDelta = HypersphericalAngleVector.CreateOrthogonalDirection(anglePos, delta);
      var angles = Angles + angleDelta;
      return new HypersphericalCoordinate(R, angles);
    }
  }
}