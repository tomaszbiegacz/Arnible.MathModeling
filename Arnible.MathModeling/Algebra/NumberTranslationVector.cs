using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  /// <summary>
  /// Immmutable cartesian translation vector.
  /// Last dimension in the vector is always either non-zero or zero for 1d vector.
  /// </summary>
  /// <remarks> 
  /// Features:
  /// * Equals returns true only if both arrays have the same size, and elements in each arrays are the same.
  /// * GetHashCode is calculated from array length and each element's hash.
  /// Usage considerations:
  /// * Structure size is equal to IntPtr.Size, hence there is no need to return/receive structure instance by reference
  /// </remarks>  
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues)]
  public readonly struct NumberTranslationVector : 
    IEquatable<NumberTranslationVector>, 
    IEquatable<Number>, 
    IValueArray<Number>
  {
    private readonly NumberVector _change;    

    public NumberTranslationVector(params Number[] parameters)
      : this(new NumberVector(parameters))
    {
      // intentionally empty
    }

    public NumberTranslationVector(in NumberVector change)
    {
      _change = change;
    }

    public static implicit operator NumberTranslationVector(in Number v) => new NumberTranslationVector(v);
    public static implicit operator NumberTranslationVector(in double v) => new NumberTranslationVector(v);

    //
    // Properties
    //

    public uint Length => _change.Length;

    public ref readonly Number this[in uint pos]
    {
      get
      {
        return ref _change[pos];
      }
    }

    //
    // Equatable
    //

    public override string ToString() => _change.ToString();

    public string ToString(in CultureInfo cultureInfo) => _change.ToString(cultureInfo);

    public bool Equals(in NumberTranslationVector other) => other._change == _change;

    public bool Equals(NumberTranslationVector other) => Equals(in other);

    public bool Equals(in Number other) => other == _change;

    public bool Equals(Number other) => Equals(in other);

    public override bool Equals(object obj)
    {
      if (obj is NumberTranslationVector v)
      {
        return Equals(in v);
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

    public static bool operator ==(in NumberTranslationVector a, in NumberTranslationVector b) => a.Equals(in b);
    public static bool operator !=(in NumberTranslationVector a, in NumberTranslationVector b) => !a.Equals(in b);

    public static bool operator ==(in NumberTranslationVector a, in Number b) => a.Equals(in b);
    public static bool operator !=(in NumberTranslationVector a, in Number b) => !a.Equals(in b);

    public static bool operator ==(in Number a, in NumberTranslationVector b) => b.Equals(in a);
    public static bool operator !=(in Number a, in NumberTranslationVector b) => !b.Equals(in a);

    /*
     * IArray
     */

    internal IEnumerable<Number> GetInternalEnumerable() => _change.GetInternalEnumerable();

    public IEnumerator<Number> GetEnumerator() => _change.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _change.GetEnumerator();    

    /*
     * Operations
     */

    public static NumberTranslationVector operator *(in NumberTranslationVector a, in Number b) => new NumberTranslationVector(b * a._change);
    public static NumberTranslationVector operator *(in Number a, in NumberTranslationVector b) => new NumberTranslationVector(a * b._change);

    public NumberVector Translate(in NumberVector src) => src + _change;

    public ValueArray<Number> Translate(in ValueArray<Number> src)
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
        return src.GetInternalEnumerable().Zip(
          col2: _change.GetInternalEnumerable(), 
          merge: (s, c) => (s ?? 0) + (c ?? 0)).ToValueArray();
      }
    }
  }
}
