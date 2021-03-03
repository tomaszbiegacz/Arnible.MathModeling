using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;

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
    private static IEnumerable<T> _empty = LinqEnumerable.Empty<T>().ToReadOnlyList();
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

    //
    // Serializer
    //

    class Serializer : IRecordWriter<ValueArray<T>>
    {
      private readonly IRecordWriterReadOnlyCollection<T> _serializer;

      public Serializer(
        in IRecordFieldSerializer serializer, 
        in Func<IRecordFieldSerializer, IRecordWriter<T>> writerFactory)
      {
        _serializer = serializer.GetReadOnlyCollectionSerializer(string.Empty, in writerFactory);
      }
      
      public void Write(in ValueArray<T> record)
      {
        _serializer.Write(record._values);
      }

      public void WriteNull()
      {
        // intentionally empty
      }
    }
    
    public static IRecordWriter<ValueArray<T>> CreateSerializer(
      IRecordFieldSerializer serializer,
      Func<IRecordFieldSerializer, IRecordWriter<T>> writerFactory)
    {
      return new Serializer(serializer, writerFactory);
    }
    
    //
    // IEquatable
    //    

    public bool Equals(ValueArray<T> other) => GetInternalEnumerable().SequenceEqual(other.GetInternalEnumerable());

    public override bool Equals(object obj)
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
    
    public bool All(Func<T, bool> predicate)
    {
      return GetInternalEnumerable().All(predicate);
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
