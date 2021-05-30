using System;
using System.Collections.Generic;
using System.Globalization;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
{
  /// <summary>
  /// Hyperspherical angle vector, where first coordinate angle range over [-π, π] and others range over [-π/2, π/2].
  /// </summary>
  public readonly ref struct HypersphericalAngleVector
  {
    private readonly Span<Number> _angles;
    
    public static HypersphericalAngleVector CreateOrthogonalDirection(
      ushort anglesCount,
      ushort anglePos, 
      in Number value)
    {
      anglesCount.AssertIsGreaterThan(anglePos);
      Span<Number> vector = new Number[anglesCount];
      vector[anglePos] = value;
      return new HypersphericalAngleVector(in vector);
    }

    public static HypersphericalAngleVector GetIdentityVector(ushort dimensionsCount)
    {
      dimensionsCount.AssertIsGreaterThan(1);

      Span<Number> angles = new Number[dimensionsCount - 1];
      angles[0] = Angle.HalfRightAngle;
      for (ushort anglePos = 1; anglePos < dimensionsCount - 1; ++anglePos)
      {
        angles[anglePos] = Math.Atan(Math.Sin((double)angles[anglePos - 1]));
      }

      return new HypersphericalAngleVector(in angles);
    }

    public HypersphericalAngleVector(in Span<Number> angles)
    {      
      angles.Length.AssertIsGreaterThan(0);
      angles[0].AssertIsBetween(-1 * Angle.HalfCycle, Angle.HalfCycle);
      angles[1..].AssertIsBetween(-1 * Angle.RightAngle, Angle.RightAngle);

      _angles = angles;
    }
    
    public static implicit operator HypersphericalAngleVector(in Span<Number> angles) => new(in angles); 
    public static implicit operator HypersphericalAngleVector(Number[] angles) => new(angles);

    //
    // Properties
    //    

    public ref readonly Number this[ushort pos] => ref _angles[pos];

    public ushort Length => (ushort)_angles.Length;

    //
    // query operators
    //
    
    public bool Equals(in HypersphericalAngleVector b)
    {
      return _angles.SequenceEqual(in b._angles);
    }
    
    public bool IsZero() => _angles.IsZero();
    
    public Number[] ToArray()
    {
      return _angles.ToArray();
    }

    public HypersphericalAngleVector AddDimension()
    {
      Span<Number> angles = new Number[_angles.Length + 1];
      _angles.CopyTo(angles);
      angles[^1] = 0;
      return new HypersphericalAngleVector(in angles);
    }
    
    public HypersphericalAngleVector GetMirrorAngles()
    {
      Span<Number> angles = new Number[_angles.Length];

      ref readonly Number firstAngle = ref _angles[0];
      if (firstAngle > 0)
      {
        angles[0] = -1 * Angle.HalfCycle + firstAngle;
      }
      else
      {
        angles[0] = Angle.HalfCycle + firstAngle;
      }

      for(ushort i=1; i<_angles.Length; ++i)
      {
        angles[i] = -1 * _angles[i];
      }
      
      return new HypersphericalAngleVector(in angles);
    }

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

    private static void RoundAngleFullCycle(ref Number v)
    {
      if (v > Angle.HalfCycle)
        v = v - Angle.FullCycle;
      else if (v < -1 * Angle.HalfCycle)
        v = Angle.FullCycle + v;
    }

    private static void RoundAngleHalfCycle(ref Number v)
    {
      if (v > Angle.RightAngle)
        v = v - Angle.HalfCycle;
      else if (v < -1 * Angle.RightAngle)
        v = Angle.HalfCycle + v;
    }    
    
    private static void Normalize(in Span<Number> angles)
    {
      RoundAngleFullCycle(ref angles[0]);
      for(ushort i=1; i<angles.Length; ++i)
      {
        RoundAngleHalfCycle(ref angles[i]);
      }
    }

    public void AddSelf(in HypersphericalAngleVector b)
    {
      _angles.AddSelf(in b._angles);
      Normalize(in _angles);
    }

    public void ScaleSelf(in Number b)
    {
      _angles.MultiplySelf(in b);
      Normalize(in _angles);
    }
    
    private HypersphericalAngleVector Clone()
    {
      // TODO: remove this
      Span<Number> result = new Number[_angles.Length];
      _angles.CopyTo(result);
      return new HypersphericalAngleVector(in _angles);
    }

    public static HypersphericalAngleVector operator +(HypersphericalAngleVector a, HypersphericalAngleVector b)
    {
      var result = a.Clone();
      result.AddSelf(in b);
      return result;
    }

    public static HypersphericalAngleVector operator *(HypersphericalAngleVector a, in Number b)
    {
      var result = a.Clone();
      result.ScaleSelf(in b);
      return result;
    }

    public static HypersphericalAngleVector operator *(in Number a, HypersphericalAngleVector b)
    {
      var result = b.Clone();
      result.ScaleSelf(in a);
      return result;
    }
  }
}
