using Arnible.MathModeling.Algebra;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling.Geometry
{
  /// <summary>
  /// Hyperspherical angle vector, where first coordinate angle range over [-π, π] and others range over [-π/2, π/2].
  /// </summary>
  public readonly struct HypersphericalAngleVector : IEquatable<HypersphericalAngleVector>, IReadOnlyCollection<Number>
  {
    private readonly NumberVector _angles;

    public HypersphericalAngleVector(params Number[] angles)
      : this(new NumberVector(angles))
    {
      // intentionally empty
    }

    public HypersphericalAngleVector(NumberVector angles)
    {
      if (angles.Any())
      {
        var first = angles.First();
        if (first > Angle.HalfCycle || first <= -1 * Angle.HalfCycle)
        {
          throw new ArgumentException($"Invalid first angualr coordinate: {first}");
        }
        if (angles.SkipExactly(1).Any(a => a > Angle.RightAngle || a < -1 * Angle.RightAngle))
        {
          throw new ArgumentException($"Invalid angular coordinates: {angles}");
        }
      }

      _angles = angles;
    }

    public override string ToString() => _angles.ToString();

    public override int GetHashCode() => _angles.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalAngleVector v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(HypersphericalAngleVector other) => other._angles == _angles;

    public static implicit operator NumberVector(HypersphericalAngleVector v) => v._angles;

    //
    // Properties
    //    

    public uint Length => _angles.Length;

    public bool IsEmpty => Length == 0;

    //
    // IReadonlyCollection
    //

    public Number this[uint pos] => _angles[pos];

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();

    int IReadOnlyCollection<Number>.Count => (int)Length;

    //
    // Operators
    //

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public HypersphericalAngleVector AddDimension() => new HypersphericalAngleVector(_angles.Append(0).ToVector());

    private IEnumerable<Number> MirrorAngles
    {
      get
      {
        if(_angles.Length > 0)
        {
          Number firstAngle = _angles.First();
          if(firstAngle > 0)
          {
            yield return -1 * Angle.HalfCycle + firstAngle;
          }
          else
          {
            yield return Angle.HalfCycle + firstAngle;
          }

          foreach(Number angle in _angles.SkipExactly(1))
          {
            yield return -1 * angle;
          }
        }
      }
    }

    public HypersphericalAngleVector Mirror => new HypersphericalAngleVector(MirrorAngles.ToVector());

    //
    // arithmetic
    //

    private static Number RoundAngleFullCycle(Number v)
    {
      if (v > Angle.HalfCycle)
        return v - Angle.FullCycle;

      if (v < -1 * Angle.HalfCycle)
        return Angle.FullCycle + v;

      return v;
    }

    private static Number RoundAngleHalfCycle(Number v)
    {
      if (v > Angle.RightAngle)
        return v - Angle.HalfCycle;

      if (v < -1 * Angle.RightAngle)
        return Angle.HalfCycle + v;

      return v;
    }

    private static IEnumerable<Number> AddAngles(NumberVector a, NumberVector b)
    {
      if (a.Length != b.Length)
      {
        throw new ArgumentException(nameof(a));
      }

      yield return RoundAngleFullCycle(a[0] + b[0]);
      for (uint i = 1; i < a.Length; ++i)
      {
        yield return RoundAngleHalfCycle(a[i] + b[i]);
      }
    }

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(AddAngles(a, b).ToVector());
    }
  }
}
