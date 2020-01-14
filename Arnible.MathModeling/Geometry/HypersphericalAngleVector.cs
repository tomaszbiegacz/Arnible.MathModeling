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

    public static Number AxisEraseAngle => Math.PI / 2;

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
        if (angles.Any(a => a < 0))
        {
          throw new ArgumentException($"Found negative angular cooridnate: {angles}");
        }
        if (angles.First() >= FullCycle)
        {
          throw new ArgumentException($"Invalid last angualr coordinate: {angles.Last()}");
        }
        if (angles.Skip(1).Any(a => a > HalfCycle))
        {
          throw new ArgumentException($"Invalid first angular coordinates: {angles}");
        }
      }

      _angles = angles;
    }

    public bool IsEmpty => _angles.Count == 0;

    public int Count => _angles.Count;

    public Number this[uint pos] => _angles[pos];

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();

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

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public static implicit operator NumberVector(HypersphericalAngleVector v) => v._angles;

    public HypersphericalAngleVector AddDimension() => new HypersphericalAngleVector(_angles.Append(AxisEraseAngle).ToVector());

    //
    // arithmetic
    //

    private static Number RoundAngle(Number v, Number maxValue)
    {
      if (v > maxValue)
        return 2 * maxValue - v;
      else
        return v;
    }

    private static IEnumerable<Number> AddAngles(NumberVector a, NumberVector b)
    {
      if (a.Count != b.Count)
      {
        throw new ArgumentException(nameof(a));
      }

      yield return RoundAngle(a[0] + b[0], FullCycle);
      for (uint i = 1; i < a.Count; ++i)
      {
        yield return RoundAngle(a[i] + b[i], HalfCycle);
      }
    }    

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(AddAngles(a, b).ToVector());
    }
  }
}
