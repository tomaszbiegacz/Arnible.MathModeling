using System;
using System.Collections;
using System.Collections.Generic;
using Arnible.Export;
using Arnible.Linq;

namespace Arnible.MathModeling
{
  /// <summary>
  /// Immutable value array.  
  /// </summary>
  /// <remarks> 
  /// Features:
  /// * Equals returns true only if both arrays have the same size, and elements in each arrays are the same.
  /// * GetHashCode is calculated from array length and each element's hash.
  /// Usage considerations:
  /// * Structure size is equal to IntPtr.Size, hence there is no need to return/receive structure instance by reference
  /// </remarks>  
  [Serializable]
  public readonly struct ValueArray<T> : 
    IEquatable<ValueArray<T>>, 
    IValueArray<T>, 
    IValueObject 
    where T : struct, IValueObject
  {
    private static IEnumerable<T> _empty = LinqArray<T>.Empty;
    private readonly T[] _values;

    internal ValueArray(T[] items)
    {
      _values = items;
    }

    public static implicit operator ValueArray<T>(T[] v) => new ValueArray<T>(v);

    public static implicit operator ValueArray<T>(in T v) => new ValueArray<T>(new[] { v });

    public override string ToString()
    {
      return "[" + string.Join(" ", GetInternalEnumerable().Select(v => v.ToStringValue())) + "]";
    }
    public string ToStringValue() => ToString();

    public override int GetHashCode()
    {
      int hc = Length.GetHashCode();
      foreach (T v in GetInternalEnumerable())
      {
        hc = unchecked(hc * 314159 + v.GetHashCodeValue());
      }
      return hc;
    }
    public int GetHashCodeValue() => GetHashCode();

    public static implicit operator ReadOnlySpan<T>(ValueArray<T> src) => src._values;
    
    public IReadOnlyList<T> List => _values ?? LinqArray<T>.Empty;
    
    //
    // IEquatable
    //    

    public bool Equals(ValueArray<T> other)
    {
      return System.Linq.Enumerable.SequenceEqual(GetInternalEnumerable(), other.GetInternalEnumerable());
    }

    public override bool Equals(object? obj)
    {
      if (obj is ValueArray<T> v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public static bool operator ==(ValueArray<T> a, ValueArray<T> b) => a.Equals(b);
    public static bool operator !=(ValueArray<T> a, ValueArray<T> b) => !a.Equals(b);

    //
    // IArray
    //
    
    internal IEnumerable<T> GetInternalEnumerable() => _values ?? _empty;

    public ref readonly T this[uint index]
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

    public uint Length => (uint)(_values?.Length ?? 0);

    public ref readonly T First => ref _values[0];
    public ref readonly T Last => ref _values[^1];

    public IEnumerator<T> GetEnumerator() => GetInternalEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerable().GetEnumerator();
    
    //
    // Operations
    //

    public IEnumerable<uint> Indexes()
    {
      return LinqEnumerable.RangeUint(Length);
    }

    public IEnumerable<uint> IndexesWhere(Func<T, bool> predicate)
    {
      for (uint i = 0; i < Length; ++i)
      {
        if (predicate(_values[i]))
        {
          yield return i;
        }
      }
    }

    public ValueArray<T> SubsetFromIndexes(IReadOnlyCollection<uint> indexes)
    {
      T[] result = new T[indexes.Count];

      uint i = 0;
      foreach (uint index in indexes)
      {
        if (index >= Length)
        {
          throw new ArgumentException(nameof(indexes));
        }
        
        result[i] = _values[index];
        i++;
      }

      return new ValueArray<T>(result);
    }
    
    //
    // IEnumerable implementation (to avoid boxing)
    //
    
    public IEnumerable<TOutput> AggregateCombinations<TOutput>(
      uint groupSize,
      Func<IEnumerable<T>, TOutput> aggregator)
    {
      return GetInternalEnumerable().AggregateCombinations(in groupSize, in aggregator);
    }
    
    public bool AllWithDefault(Func<T, bool> predicate)
    {
      return GetInternalEnumerable().AllWithDefault(predicate);
    }

    public bool Any(Func<T, bool> predicate)
    {
      return GetInternalEnumerable().Any(predicate);
    }
    
    public IEnumerable<T> Append(T item, uint count)
    {
      foreach (T srcItem in GetInternalEnumerable())
      {
        yield return srcItem;
      }
      for (uint i = 0; i < count; i++)
      {
        yield return item;
      }
    }
    
    public IEnumerable<T> Append(T item)
    {
      return Append(item, 1);
    }

    public IEnumerable<T> ExcludeAt(uint pos)
    {
      return GetInternalEnumerable().ExcludeAt(pos);
    }

    public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
    {
      return GetInternalEnumerable().Select(selector);
    }

    public IEnumerable<TResult> Select<TResult>(Func<uint, T, TResult> selector)
    {
      return GetInternalEnumerable().Select(selector);
    }

    public T Single()
    {
      return GetInternalEnumerable().Single();
    }
    
    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
      return GetInternalEnumerable().Where(predicate);
    }

    public IEnumerable<TResult> ZipDefensive<TResult>(
      ValueArray<T> col2,
      Func<T, T, TResult> merge)
    {
      return GetInternalEnumerable().ZipDefensive(col2.GetInternalEnumerable(), merge);
    }
  }
}
