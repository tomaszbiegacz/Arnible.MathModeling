using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  /// <summary>
  /// Hyperspherical angle vector, where first coordinate angle range over [-π, π] and others range over [-π/2, π/2].
  /// </summary>
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct HypersphericalAngleVector : IEquatable<HypersphericalAngleVector>, IEquatable<Number>, IReadOnlyList<Number>
  {
    private readonly NumberVector _angles;

    class Serializer : ToStringSerializer<HypersphericalAngleVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public static HypersphericalAngleVector CreateOrthogonalDirection(uint anglePos, Number value)
    {
      return new HypersphericalAngleVector(NumberVector.FirstNonZeroValueAt(pos: anglePos, value: value));
    }    

    private static IEnumerable<Number> Normalize(IEnumerable<Number> values)
    {
      bool isFirst = true;
      foreach (Number v in values)
      {
        if (isFirst)
        {
          yield return RoundAngleFullCycle(v);
          isFirst = false;
        }
        else
        {
          yield return RoundAngleHalfCycle(v);
        }
      }
    }

    internal static HypersphericalAngleVector Create(IEnumerable<Number> parameters)
    {
      return new HypersphericalAngleVector(Normalize(parameters).ToVector());
    }

    public HypersphericalAngleVector(params Number[] angles)
      : this(new NumberVector(angles))
    {
      // intentionally empty
    }

    private HypersphericalAngleVector(NumberVector angles)
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

      _angles = angles;
    }

    public static implicit operator HypersphericalAngleVector(Number v) => new HypersphericalAngleVector(v);
    public static implicit operator HypersphericalAngleVector(double v) => new HypersphericalAngleVector(v);

    public static implicit operator NumberVector(HypersphericalAngleVector v) => v._angles;

    //
    // Properties
    //    

    public Number this[uint pos] => _angles[pos];

    public uint Length => _angles.Length;

    public Number GetOrDefault(uint pos) => _angles.GetOrDefault(pos);

    //
    // IReadOnlyList
    //    

    Number IReadOnlyList<Number>.this[int pos] => _angles[(uint)pos];

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();

    int IReadOnlyCollection<Number>.Count => (int)Length;

    //
    // IEquatable
    //    

    public bool Equals(HypersphericalAngleVector other) => other._angles == _angles;

    public bool Equals(Number other) => other == _angles;

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalAngleVector v)
      {
        return Equals(v);
      }
      else if (obj is Number v2)
      {
        return Equals(v2);
      }
      else
      {
        return false;
      }
    }

    public override string ToString() => _angles.ToString();

    public string ToString(CultureInfo culture) => _angles.ToString(culture);

    public override int GetHashCode() => _angles.GetHashCode();

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public static bool operator ==(Number a, HypersphericalAngleVector b) => b.Equals(a);
    public static bool operator !=(Number a, HypersphericalAngleVector b) => !b.Equals(a);

    public static bool operator ==(HypersphericalAngleVector a, Number b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, Number b) => !a.Equals(b);

    //
    // query operators
    //

    public HypersphericalAngleVector GetOrthogonalDirection(uint anglePos)
    {
      Number angle = _angles[anglePos];
      return new HypersphericalAngleVector(NumberVector.FirstNonZeroValueAt(pos: anglePos, value: angle));
    }

    public HypersphericalAngleVector AddDimension() => new HypersphericalAngleVector(_angles.Append(0).ToVector());

    private IEnumerable<Number> MirrorAngles
    {
      get
      {
        Number firstAngle = _angles.First();
        if (firstAngle > 0)
        {
          yield return -1 * Angle.HalfCycle + firstAngle;
        }
        else
        {
          yield return Angle.HalfCycle + firstAngle;
        }

        foreach (Number angle in _angles.SkipExactly(1))
        {
          yield return -1 * angle;
        }
      }
    }

    public HypersphericalAngleVector Mirror => new HypersphericalAngleVector(MirrorAngles.ToVector());

    public NumberVector GetCartesianAxisViewsRatios()
    {
      var cartesianDimensions = new List<Number>();
      Number replacement = 1;
      foreach (var angle in this.Reverse())
      {
        cartesianDimensions.Add(replacement * Sin(angle));
        replacement *= Cos(angle);
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return cartesianDimensions.ToVector();
    }

    //
    // Arithmetic operators
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
      return Normalize(a.Zip(b, (v1, v2) => (v1 ?? 0) + (v2 ?? 0)));      
    }

    private static IEnumerable<Number> ScaleAngles(NumberVector a, Number b)
    {
      return Normalize(a * b);
    }

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(AddAngles(a, b).ToVector());
    }

    public static HypersphericalAngleVector operator *(HypersphericalAngleVector a, Number b)
    {
      return new HypersphericalAngleVector(ScaleAngles(a, b).ToVector());
    }

    public static HypersphericalAngleVector operator *(Number a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(ScaleAngles(b, a).ToVector());
    }
  }
}
