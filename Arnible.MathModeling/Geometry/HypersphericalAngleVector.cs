using System;
using System.Collections.Generic;
using System.Globalization;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
{
  /// <summary>
  /// Hyperspherical angle vector, where first coordinate angle range over [-π, π] and others range over [-π/2, π/2].
  /// </summary>
  public readonly struct HypersphericalAngleVector
  {
    static readonly HypersphericalAngleVector _zero = 0;
    
    private readonly ReadOnlyArray<Number> _angles;
    
    public static HypersphericalAngleVector CreateOrthogonalDirection(uint anglePos, in Number value)
    {
      Number[] vector = new Number[anglePos + 1];
      vector[anglePos] = value;
      return new HypersphericalAngleVector(vector);
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
      return new HypersphericalAngleVector(Normalize(angles).ToArray());
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
      return new HypersphericalAngleVector(angles.Select(v => (Number)v).ToArray());
    }
    
    public HypersphericalAngleVector(params Number[] angles)
    : this((ReadOnlyArray<Number>)angles)
    {
    }
    
    public HypersphericalAngleVector(ReadOnlyArray<Number> angles)
    {      
      Number first = angles.First;
      if (first > Angle.HalfCycle || first < -1 * Angle.HalfCycle)
      {
        throw new ArgumentException($"Invalid first angular coordinate: {first}");
      }
      if (angles.AsList().SkipExactly(1).Any(a => a > Angle.RightAngle || a < -1 * Angle.RightAngle))
      {
        throw new ArgumentException($"Invalid angular coordinates: {angles}");
      }      

      _angles = angles;
    }

    public static implicit operator HypersphericalAngleVector(in Number v) => new HypersphericalAngleVector(v);
    public static implicit operator HypersphericalAngleVector(in double v) => new HypersphericalAngleVector(v);

    public static implicit operator ReadOnlyArray<Number>(HypersphericalAngleVector v) => v._angles;

    //
    // Properties
    //    

    public ref readonly Number this[ushort pos] => ref _angles[pos];

    public ushort Length => _angles.Length;

    public Number GetOrDefault(ushort pos) => _angles.AsList().GetOrDefault(pos);

    //
    // IArray
    //
    
    public ReadOnlyArray<Number> AsArray() => _angles;

    internal IEnumerable<Number> GetInternalEnumerable() => _angles.AsList();

    public IEnumerator<Number> GetEnumerator() => _angles.GetEnumerator();
    
    public ReadOnlySpan<Number> Span
    {
      get
      {
        return _angles;
      }
    }
    

    //
    // IEquatable
    //    

    public bool Equals(HypersphericalAngleVector other) => other._angles == _angles;

    public bool Equals(in Number other) => _angles.Length == 1 && other == _angles[0];

    public bool Equals(Number other) => Equals(in other);

    public override bool Equals(object? obj)
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
    
    public override string ToString() => _angles.ToString();
    public override int GetHashCode() => _angles.GetHashCode();

    public static bool operator ==(HypersphericalAngleVector a, HypersphericalAngleVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleVector a, HypersphericalAngleVector b) => !a.Equals(b);

    public static bool operator ==(in Number a, HypersphericalAngleVector b) => b.Equals(in a);
    public static bool operator !=(in Number a, HypersphericalAngleVector b) => !b.Equals(in a);

    public static bool operator ==(HypersphericalAngleVector a, in Number b) => a.Equals(in b);
    public static bool operator !=(HypersphericalAngleVector a, in Number b) => !a.Equals(in b);

    //
    // query operators
    //

    public HypersphericalAngleVector GetOrthogonalDirection(ushort anglePos)
    {
      ref readonly Number angle = ref _angles[anglePos];
      Number[] values = new Number[anglePos + 1];
      values[anglePos] = angle;
      return new HypersphericalAngleVector(values);
    }

    public HypersphericalAngleVector AddDimension()
    {
      return new HypersphericalAngleVector(_angles.AsList().Append(0).ToArray());
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

        foreach (Number angle in _angles.AsList().SkipExactly(1))
        {
          yield return -1 * angle;
        }
      }
    }

    public HypersphericalAngleVector Mirror => new HypersphericalAngleVector(MirrorAngles.ToArray());

    public void GetCartesianAxisViewsRatios(in Span<Number> cartesianDimensions)
    {
      cartesianDimensions.Length.AssertIsEqualTo(_angles.Length + 1);

      Number replacement = 1;
      for(ushort anglePos = 0; anglePos < _angles.Length; ++anglePos)
      {
        Number angle = _angles[_angles.Length - 1 - anglePos];
        cartesianDimensions[_angles.Length - anglePos] = replacement * NumberMath.Sin(angle);
        replacement *= NumberMath.Cos(angle);
      }
      cartesianDimensions[0] = replacement;
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

    private static IEnumerable<Number> AddAngles(ReadOnlyArray<Number> a, ReadOnlyArray<Number> b)
    {
      return Normalize(a.AsList().ZipValue(
        col2: b.AsList(), 
        merge: (v1, v2) => (v1 ?? 0) + (v2 ?? 0)));      
    }

    private static IEnumerable<Number> ScaleAngles(ReadOnlyArray<Number> a, Number b)
    {
      return Normalize(a.AsList().Select(v => v*b));
    }

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(AddAngles(a, b).ToArray());
    }

    public static HypersphericalAngleVector operator *(HypersphericalAngleVector a, in Number b)
    {
      return new HypersphericalAngleVector(ScaleAngles(a, b).ToArray());
    }

    public static HypersphericalAngleVector operator *(in Number a, HypersphericalAngleVector b)
    {
      return new HypersphericalAngleVector(ScaleAngles(b, a).ToArray());
    }
  }
}
