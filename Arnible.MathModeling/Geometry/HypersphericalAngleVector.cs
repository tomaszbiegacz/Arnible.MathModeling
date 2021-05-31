using System;
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

    public ushort Length => (ushort)_angles.Length;
    
    public ReadOnlySpan<Number> Span => _angles;
    
    public bool IsOrthogonal()
    {
      return _angles.Count((in Number v) => v != 0) == 1;
    }

    //
    // Query operators
    //
    
    public bool Equals(in HypersphericalAngleVector b)
    {
      return _angles.SequenceEqual(in b._angles);
    }
    
    public bool IsZero() => _angles.IsZero();
    
    public void GetCartesianAxisViewsRatios(in Span<Number> cartesianCoordinate)
    {
      cartesianCoordinate.Length.AssertIsEqualTo(_angles.Length + 1);

      Number replacement = 1;
      for(ushort anglePos = 0; anglePos < _angles.Length; ++anglePos)
      {
        Number angle = _angles[_angles.Length - 1 - anglePos];
        cartesianCoordinate[_angles.Length - anglePos] = replacement * NumberMath.Sin(in angle);
        replacement *= NumberMath.Cos(in angle);
      }
      cartesianCoordinate[0] = replacement;
    }
    
    //
    // Factories
    //
    
    public static HypersphericalAngleVector CreateOrthogonalDirection(
      ushort anglePos, 
      in Number value,
      in Span<Number> buffer)
    {
      buffer.Length.AssertIsGreaterThan(anglePos);
      
      buffer.Clear();
      buffer[anglePos] = value;
      return new HypersphericalAngleVector(in buffer);
    }

    public static HypersphericalAngleVector GetIdentityVector(in Span<Number> buffer)
    {
      buffer.Length.AssertIsGreaterThan(0);
      
      buffer[0] = Angle.HalfRightAngle;
      for (ushort anglePos = 1; anglePos < buffer.Length; ++anglePos)
      {
        buffer[anglePos] = Math.Atan(Math.Sin((double)buffer[anglePos - 1]));
      }

      return new HypersphericalAngleVector(in buffer);
    }
    
    public HypersphericalAngleVector Clone(in Span<Number> buffer)
    {
      _angles.CopyTo(buffer);
      if(buffer.Length > _angles.Length)
      {
        buffer[_angles.Length..].Clear();
      }
      return new HypersphericalAngleVector(in buffer);
    }
    
    public HypersphericalAngleVector GetMirrorAngles(in Span<Number> buffer)
    {
      buffer.Length.AssertIsEqualTo(_angles.Length);

      ref readonly Number firstAngle = ref _angles[0];
      if (firstAngle > 0)
      {
        buffer[0] = -1 * Angle.HalfCycle + firstAngle;
      }
      else
      {
        buffer[0] = Angle.HalfCycle + firstAngle;
      }

      for(ushort i=1; i<_angles.Length; ++i)
      {
        buffer[i] = -1 * _angles[i];
      }
      
      return new HypersphericalAngleVector(in buffer);
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
  }
}
