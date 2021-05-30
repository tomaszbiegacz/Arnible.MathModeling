using System;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public readonly ref struct HypersphericalCoordinate
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

    public HypersphericalCoordinate(in Number r, in Span<Number> angles)
    : this(in r, new HypersphericalAngleVector(angles))
    {
      // intentionally empty
    }
    
    public HypersphericalCoordinate(in Number r, in HypersphericalAngleVector angles)
    {
      r.AssertIsGreaterEqualThan(0);
      if (r == 0 && !angles.IsZero())
      {
        throw new ArgumentException("For zero r, angles also has to be empty");
      }

      R = r;
      Angles = angles;
    }
    
    public static implicit operator HypersphericalCoordinate(in PolarCoordinate pc)
    {
      return new HypersphericalCoordinate(pc.R, new [] { pc.Φ });
    }

    //
    // Properties
    //

    public ushort DimensionsCount => (ushort)(Angles.Length + 1);

    //
    // Operations
    //

    public HypersphericalCoordinate AddDimension()
    {
      return new HypersphericalCoordinate(R, Angles.AddDimension());
    }

    public HypersphericalCoordinateOnAxisView ToCartesianView() => new HypersphericalCoordinateOnAxisView(this);

    public HypersphericalCoordinate Translate(ushort anglePos, in Number delta)
    {
      var angleDelta = HypersphericalAngleVector.CreateOrthogonalDirection(Angles.Length, anglePos, in delta);
      var angles = Angles + angleDelta;
      return new HypersphericalCoordinate(R, in angles);
    }
    
    public HypersphericalCoordinate Translate(in HypersphericalAngleVector translation)
    {
      // TODO: remove array creation
      return new HypersphericalCoordinate(R, translation + Angles);
    }
  }
}