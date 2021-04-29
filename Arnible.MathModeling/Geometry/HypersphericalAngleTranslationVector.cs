using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Geometry
{
  [Serializable]
  public readonly struct HypersphericalAngleTranslationVector :
    IEquatable<HypersphericalAngleTranslationVector>,
    IEquatable<Number>
  {
    private readonly HypersphericalAngleVector _change;

    public HypersphericalAngleTranslationVector(params Number[] parameters)
      : this(new HypersphericalAngleVector(parameters))
    {
      // intentionally empty
    }

    public HypersphericalAngleTranslationVector(HypersphericalAngleVector change)
    {
      _change = change;
    }

    public static implicit operator HypersphericalAngleTranslationVector(in Number v) =>
      new HypersphericalAngleTranslationVector(v);

    public static implicit operator HypersphericalAngleTranslationVector(in double v) =>
      new HypersphericalAngleTranslationVector(v);

    //
    // Properties
    //

    public ushort Length => _change.Length;

    public ref readonly Number this[ushort pos] => ref _change[pos];

    //
    // Equatable
    //
    
    public override string ToString() => _change.ToString();

    public bool Equals(HypersphericalAngleTranslationVector other) => other._change == _change;

    public bool Equals(in Number other) => other == _change;

    public bool Equals(Number other) => Equals(in other);

    public override bool Equals(object? obj)
    {
      if (obj is HypersphericalAngleTranslationVector v)
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

    public override int GetHashCode() => _change.GetHashCode();
    public int GetHashCodeValue() => GetHashCode();

    public static bool operator ==(HypersphericalAngleTranslationVector a,
      HypersphericalAngleTranslationVector b) => a.Equals(b);

    public static bool operator !=(in HypersphericalAngleTranslationVector a,
      HypersphericalAngleTranslationVector b) => !a.Equals(b);

    public static bool operator ==(HypersphericalAngleTranslationVector a, in Number b) => a.Equals(in b);
    public static bool operator !=(HypersphericalAngleTranslationVector a, in Number b) => !a.Equals(in b);

    public static bool operator ==(in Number a, HypersphericalAngleTranslationVector b) => b.Equals(in a);
    public static bool operator !=(in Number a, HypersphericalAngleTranslationVector b) => !b.Equals(in a);

    /*
     * IArray
     */

    internal IEnumerable<Number> GetInternalEnumerable() => _change.GetInternalEnumerable();

    public IEnumerator<Number> GetEnumerator() => _change.GetEnumerator();

    /*
     * Operations
     */

    public static HypersphericalAngleTranslationVector operator *(HypersphericalAngleTranslationVector a,
      in Number b) => new HypersphericalAngleTranslationVector(b * a._change);

    public static HypersphericalAngleTranslationVector operator *(in Number a,
      HypersphericalAngleTranslationVector b) => new HypersphericalAngleTranslationVector(a * b._change);

    public HypersphericalAngleVector Translate(HypersphericalAngleVector src) => src + _change;

    public HypersphericalCoordinate Translate(HypersphericalCoordinate src) =>
      new HypersphericalCoordinate(src.R, src.Angles + _change);
  }
}