using Arnible.MathModeling.Algebra;
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
  public readonly struct HypersphericalAngleVector : 
    IEquatable<HypersphericalAngleVector>, 
    IEquatable<Number>, 
    IValueArray<Number>,
    IValueObject
  {
    private readonly NumberVector _angles;
    
    public static HypersphericalAngleVector CreateOrthogonalDirection(uint anglePos, in Number value)
    {
      return new HypersphericalAngleVector(NumberVector.NonZeroValueAt(pos: anglePos, value: in value));
    }    

    private static IEnumerable<Number> Normalize(IEnumerable<Number> angles)
    {
      bool isFirst = true;
      foreach (Number v in angles)
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

    internal static HypersphericalAngleVector Create(IEnumerable<Number> angles)
    {
      return new HypersphericalAngleVector(Normalize(angles).ToVector());
    }

    public static HypersphericalAngleVector GetIdentityVector(uint dimensionsCount)
    {
      switch (dimensionsCount)
      {
        case 0:
        case 1:
          throw new ArgumentException(nameof(dimensionsCount));
      }

      List<double> angles = new List<double> { Angle.HalfRightAngle };
      for (uint anglePos = 2; anglePos < dimensionsCount; ++anglePos)
      {
        double angle = Math.Atan(Math.Sin(angles[angles.Count - 1]));
        angles.Add(angle);
      }
      return new HypersphericalAngleVector(angles.ToVector());
    }

    public HypersphericalAngleVector(params Number[] angles)
      : this(new NumberVector(angles))
    {
      // intentionally empty
    }

    private HypersphericalAngleVector(NumberVector angles)
    {      
      var first = angles.First;
      if (first > Angle.HalfCycle || first <= -1 * Angle.HalfCycle)
      {
        throw new ArgumentException($"Invalid first angular coordinate: {first}");
      }
      if (angles.GetInternalEnumerable().SkipExactly(1).Any(a => a > Angle.RightAngle || a < -1 * Angle.RightAngle))
      {
        throw new ArgumentException($"Invalid angular coordinates: {angles}");
      }      

      _angles = angles;
    }

    public static implicit operator HypersphericalAngleVector(in Number v) => new HypersphericalAngleVector(v);
    public static implicit operator HypersphericalAngleVector(in double v) => new HypersphericalAngleVector(v);

    public static implicit operator NumberVector(HypersphericalAngleVector v) => v._angles;

    //
    // Properties
    //    

    public ref readonly Number this[uint pos] => ref _angles[pos];

    public uint Length => _angles.Length;

    public Number GetOrDefault(uint pos) => _angles.GetOrDefault(pos);

    //
    // IArray
    //

    internal IEnumerable<Number> GetInternalEnumerable() => _angles.GetInternalEnumerable();

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();    

    //
    // IEquatable
    //    

    public bool Equals(HypersphericalAngleVector other) => other._angles == _angles;

    public bool Equals(in Number other) => other == _angles;

    public bool Equals(Number other) => Equals(in other);

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalAngleVector v)
      {
        return Equals(v);
      }
      else if (obj is Number v2)
      {
        return Equals(in v2);
      }
      else
      {
        return false;
      }
    }
    
    public string ToString(CultureInfo culture) => _angles.ToString(culture);
    public override string ToString() => _angles.ToString();
    public string ToStringValue() => ToString();

    public override int GetHashCode() => _angles.GetHashCode();
    public int GetHashCodeValue() => GetHashCode();

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public static bool operator ==(in Number a, HypersphericalAngleVector b) => b.Equals(in a);
    public static bool operator !=(in Number a, HypersphericalAngleVector b) => !b.Equals(in a);

    public static bool operator ==(HypersphericalAngleVector a, in Number b) => a.Equals(in b);
    public static bool operator !=(HypersphericalAngleVector a, in Number b) => !a.Equals(in b);

    //
    // query operators
    //

    public HypersphericalAngleVector GetOrthogonalDirection(uint anglePos)
    {
      ref readonly Number angle = ref _angles[anglePos];
      return new HypersphericalAngleVector(NumberVector.NonZeroValueAt(pos: anglePos, value: in angle));
    }

    public HypersphericalAngleVector AddDimension()
    {
      return new HypersphericalAngleVector(_angles.GetInternalEnumerable().Append(0).ToVector());
    }

    private IEnumerable<Number> MirrorAngles
    {
      get
      {
        Number firstAngle = _angles.First;
        if (firstAngle > 0)
        {
          yield return -1 * Angle.HalfCycle + firstAngle;
        }
        else
        {
          yield return Angle.HalfCycle + firstAngle;
        }

        foreach (Number angle in _angles.GetInternalEnumerable().SkipExactly(1))
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
      foreach (var angle in GetInternalEnumerable().Reverse())
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

    private static Number RoundAngleFullCycle(in Number v)
    {
      if (v > Angle.HalfCycle)
        return v - Angle.FullCycle;

      if (v < -1 * Angle.HalfCycle)
        return Angle.FullCycle + v;

      return v;
    }

    private static Number RoundAngleHalfCycle(in Number v)
    {
      if (v > Angle.RightAngle)
        return v - Angle.HalfCycle;

      if (v < -1 * Angle.RightAngle)
        return Angle.HalfCycle + v;

      return v;
    }    

    private static IEnumerable<Number> AddAngles(NumberVector a, NumberVector b)
    {
      return Normalize(a.GetInternalEnumerable().Zip(
        col2: b.GetInternalEnumerable(), 
        merge: (v1, v2) => (v1 ?? 0) + (v2 ?? 0)));      
    }

    private static IEnumerable<Number> ScaleAngles(NumberVector a, in Number b)
    {
      return Normalize((a * b).GetInternalEnumerable());
    }

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(AddAngles(a, b).ToVector());
    }

    public static HypersphericalAngleVector operator *(HypersphericalAngleVector a, in Number b)
    {
      return new HypersphericalAngleVector(ScaleAngles(a, b).ToVector());
    }

    public static HypersphericalAngleVector operator *(in Number a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(ScaleAngles(b, a).ToVector());
    }
  }
}
