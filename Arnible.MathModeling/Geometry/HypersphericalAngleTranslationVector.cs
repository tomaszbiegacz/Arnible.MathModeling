using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Geometry
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct HypersphericalAngleTranslationVector : IEquatable<HypersphericalAngleTranslationVector>, IEquatable<Number>, IReadOnlyList<Number>
  {
    private readonly HypersphericalAngleVector _change;

    class Serializer : ToStringSerializer<HypersphericalAngleTranslationVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public HypersphericalAngleTranslationVector(params Number[] parameters)
      : this(new HypersphericalAngleVector(parameters))
    {
      // intentionally empty
    }

    public HypersphericalAngleTranslationVector(HypersphericalAngleVector change)
    {
      _change = change;
    }

    public static implicit operator HypersphericalAngleTranslationVector(Number v) => new HypersphericalAngleTranslationVector(v);
    public static implicit operator HypersphericalAngleTranslationVector(double v) => new HypersphericalAngleTranslationVector(v);

    //
    // Properties
    //

    public uint Length => _change.Length;

    public Number this[uint pos] => _change[pos];

    //
    // Equatable
    //

    public override string ToString() => _change.ToString();

    public string ToString(CultureInfo cultureInfo) => _change.ToString(cultureInfo);

    public bool Equals(HypersphericalAngleTranslationVector other) => other._change == _change;

    public bool Equals(Number other) => other == _change;

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalAngleTranslationVector v)
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

    public override int GetHashCode() => _change.GetHashCode();

    public static bool operator ==(HypersphericalAngleTranslationVector a, HypersphericalAngleTranslationVector b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleTranslationVector a, HypersphericalAngleTranslationVector b) => !a.Equals(b);

    public static bool operator ==(HypersphericalAngleTranslationVector a, Number b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleTranslationVector a, Number b) => !a.Equals(b);

    public static bool operator ==(Number a, HypersphericalAngleTranslationVector b) => b.Equals(a);
    public static bool operator !=(Number a, HypersphericalAngleTranslationVector b) => !b.Equals(a);

    /*
     * IReadOnlyList
     */

    Number IReadOnlyList<Number>.this[int pos] => _change[(uint)pos];

    public IEnumerator<Number> GetEnumerator() => _change.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _change.GetEnumerator();

    int IReadOnlyCollection<Number>.Count => (int)_change.Length;

    /*
     * Operations
     */

    public static HypersphericalAngleTranslationVector operator *(HypersphericalAngleTranslationVector a, Number b) => new HypersphericalAngleTranslationVector(b * a._change);
    public static HypersphericalAngleTranslationVector operator *(Number a, HypersphericalAngleTranslationVector b) => new HypersphericalAngleTranslationVector(a * b._change);

    public HypersphericalAngleVector Translate(HypersphericalAngleVector src) => src + _change;

    public HypersphericalCoordinate Translate(HypersphericalCoordinate src) => new HypersphericalCoordinate(src.R,  src.Angles + _change);
  }
}
