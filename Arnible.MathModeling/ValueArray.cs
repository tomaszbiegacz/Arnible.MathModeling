using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public readonly struct ValueArray<T> : IArray<T> where T : struct
  {
    private readonly T[] _values;

    internal ValueArray(IEnumerable<T> items)
    {
      _values = System.Linq.Enumerable.ToArray(items);
    }

    public static implicit operator ValueArray<T>(T[] v) => new ValueArray<T>(v);

    public static implicit operator ValueArray<T>(T v) => new ValueArray<T>(new[] { v });

    //
    // IArray
    //

    public T this[uint index]
    {
      get
      {
        if (_values == null || index >= _values.Length)
        {
          throw new ArgumentException(nameof(index));
        }
        return _values[index];
      }
    }

    public uint Length
    {
      get
      {
        return (uint)(_values?.Length ?? 0);
      }
    }

    private IEnumerator<T> GetInternalEnumerator()
    {
      if (_values == null)
      {
        return LinqEnumerable.Empty<T>().GetEnumerator();
      }
      else
      {
        return ((IReadOnlyCollection<T>)_values).GetEnumerator();
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      return GetInternalEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetInternalEnumerator();
    }
  }
}
