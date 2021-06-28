using System;
using Arnible.Assertions;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

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
    : this(in r, new HypersphericalAngleVector(in angles))
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
    
    public HypersphericalCoordinate Clone(in Span<Number> buffer)
    {
      return new HypersphericalCoordinate(R, Angles.Clone(in buffer));
    }

    public void TranslateSelf(in HypersphericalAngleVector translation)
    {
      Angles.AddSelf(translation);
    }
    
    public void DerivativeByR(in Span<Number> derivativeOnAxis)
    {
      Angles.GetCartesianAxisViewsRatios(in derivativeOnAxis);
    }
    
    public void ToCartesian(in Span<Number> buffer)
    {
      Angles.GetCartesianAxisViewsRatios(in buffer);
      buffer.MultiplySelfBy(R);
    }
    
    public static HypersphericalAngleVector CartesianCoordinatesAngle(
      ushort cartesianDimensionPos,
      in Span<Number> buffer)
    {
      cartesianDimensionPos.AssertIsLessEqualThan(buffer.Length);
      buffer.Clear();
      if(cartesianDimensionPos == 0)
      {
        // on 'X' axis all angles are O
        return new HypersphericalAngleVector(buffer);
      }
      else
      {
        buffer[cartesianDimensionPos - 1] = Angle.RightAngle;
        return new HypersphericalAngleVector(buffer);
      }
    }
  }
}