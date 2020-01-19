using Arnible.MathModeling.Algebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public struct HypersphericalAngleVector : IEquatable<HypersphericalAngleVector>, IReadOnlyCollection<Number>
  {
    const double FullCycle = 2 * Math.PI;
    const double HalfCycle = Math.PI;

    public const double RightAngle = Math.PI / 2;

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
        if (first > HalfCycle || first < -1 * HalfCycle)
        {
          throw new ArgumentException($"Invalid first angualr coordinate: {first}");
        }
        if (angles.Skip(1).Any(a => a > RightAngle || a < -1 * RightAngle))
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

    public bool IsEmpty => _angles.Count == 0;

    public int Count => _angles.Count;

    //
    // IReadonlyCollection
    //

    public Number this[uint pos] => _angles[pos];

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();

    //
    // Operators
    //

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public HypersphericalAngleVector AddDimension() => new HypersphericalAngleVector(_angles.Append(0).ToVector());

    //
    // arithmetic
    //

    private static Number RoundAngleFullCycle(Number v)
    {
      if (v > HalfCycle)
        return v - FullCycle;

      if (v < -1 * HalfCycle)
        return FullCycle + v;

      return v;
    }

    private static Number RoundAngleHalfCycle(Number v)
    {
      if (v > RightAngle)
        return v - HalfCycle;

      if (v < -1 * RightAngle)
        return HalfCycle + v;

      return v;
    }

    private static IEnumerable<Number> AddAngles(NumberVector a, NumberVector b)
    {
      if (a.Count != b.Count)
      {
        throw new ArgumentException(nameof(a));
      }

      yield return RoundAngleFullCycle(a[0] + b[0]);
      for (uint i = 1; i < a.Count; ++i)
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
