using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberTranslationVector : IEquatable<NumberTranslationVector>, IEquatable<Number>, IArray<Number>
  {
    private readonly NumberVector _change;

    class Serializer : ToStringSerializer<NumberTranslationVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public NumberTranslationVector(params Number[] parameters)
      : this(new NumberVector(parameters))
    {
      // intentionally empty
    }

    public NumberTranslationVector(NumberVector change)
    {
      _change = change;
    }

    public static implicit operator NumberTranslationVector(Number v) => new NumberTranslationVector(v);
    public static implicit operator NumberTranslationVector(double v) => new NumberTranslationVector(v);

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

    public bool Equals(NumberTranslationVector other) => other._change == _change;

    public bool Equals(Number other) => other == _change;

    public override bool Equals(object obj)
    {
      if (obj is NumberTranslationVector v)
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

    public static bool operator ==(NumberTranslationVector a, NumberTranslationVector b) => a.Equals(b);
    public static bool operator !=(NumberTranslationVector a, NumberTranslationVector b) => !a.Equals(b);

    public static bool operator ==(NumberTranslationVector a, Number b) => a.Equals(b);
    public static bool operator !=(NumberTranslationVector a, Number b) => !a.Equals(b);

    public static bool operator ==(Number a, NumberTranslationVector b) => b.Equals(a);
    public static bool operator !=(Number a, NumberTranslationVector b) => !b.Equals(a);

    /*
     * IArray
     */    

    public IEnumerator<Number> GetEnumerator() => _change.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _change.GetEnumerator();    

    /*
     * Operations
     */

    public static NumberTranslationVector operator *(NumberTranslationVector a, Number b) => new NumberTranslationVector(b * a._change);
    public static NumberTranslationVector operator *(Number a, NumberTranslationVector b) => new NumberTranslationVector(a * b._change);

    public NumberVector Translate(NumberVector src) => src + _change;

    public NumberArray Translate(NumberArray src)
    {
      if(src.Length < _change.Length)
      {
        throw new ArgumentException(nameof(src));
      }

      if(_change == default)
      {
        return src;
      }
      else
      {
        return src.Zip(_change, (s, c) => (s ?? 0) + (c ?? 0)).ToNumberArray();
      }
    }
  }
}
