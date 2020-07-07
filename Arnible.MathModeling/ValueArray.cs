using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  /// <summary>
  /// Immmutable value array.  
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
  public readonly struct ValueArray<T> : IEquatable<ValueArray<T>>, IValueArray<T> where T : struct
  {
    static IEnumerable<T> Empty = LinqEnumerable.Empty<T>().ToReadOnlyList();
    private readonly T[] _values;

    internal ValueArray(in IEnumerable<T> items)
    {
      _values = System.Linq.Enumerable.ToArray(items);
    }

    public ValueArray(params T[] items)
    {
      _values = new T[items.Length];
      Array.Copy(items, _values, items.Length);
    }

    private IEnumerable<T> Values => _values ?? Empty;

    public static implicit operator ValueArray<T>(in T[] v) => new ValueArray<T>(v);

    public static implicit operator ValueArray<T>(in T v) => new ValueArray<T>(new[] { v });

    public override string ToString()
    {
      return "[" + string.Join(" ", Values.Select(v => v.ToString())) + "]";
    }

    public override int GetHashCode()
    {
      int hc = Length.GetHashCode();
      foreach (var v in Values)
      {
        hc = unchecked(hc * 314159 + v.GetHashCode());
      }
      return hc;
    }

    //
    // IEquatable
    //    

    public bool Equals(in ValueArray<T> other) => Values.SequenceEqual(other.Values);

    public bool Equals(ValueArray<T> other) => Equals(in other);

    public override bool Equals(object obj)
    {
      if (obj is ValueArray<T> v)
      {
        return Equals(in v);
      }
      else
      {
        return false;
      }
    }

    public static bool operator ==(in ValueArray<T> a, in ValueArray<T> b) => a.Equals(in b);
    public static bool operator !=(in ValueArray<T> a, in ValueArray<T> b) => !a.Equals(in b);

    //
    // IArray
    //

    public ref readonly T this[in uint index]
    {
      get
      {
        if (_values == null || index >= _values.Length)
        {
          throw new ArgumentException(nameof(index));
        }
        return ref _values[index];
      }
    }

    public uint Length
    {
      get
      {
        return (uint)(_values?.Length ?? 0);
      }
    }

    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
  }
}
