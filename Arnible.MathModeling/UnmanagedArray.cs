using System;
using System.Collections;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling
{
  public readonly struct UnmanagedArray<T> : IUnmanagedArray<T> where T : unmanaged
  {
    private static IEnumerable<T> _empty = LinqEnumerable.Empty<T>().ToReadOnlyList();
    private readonly T[] _items;

    internal UnmanagedArray(T[] items)
    {
      _items = items;
    }
    
    public static implicit operator UnmanagedArray<T>(in T[] v) => new UnmanagedArray<T>(v);

    public static implicit operator UnmanagedArray<T>(in T v) => new UnmanagedArray<T>(new[] { v });
    
    public override string ToString()
    {
      return $"[{string.Join(',', GetEnumerator())}]";
    }
    
    internal IEnumerable<T> GetInternalEnumerable() => _items ?? _empty;

    public T this[in uint index]
    {
      get
      {
        if (_items == null || index >= _items.Length)
        {
          throw new ArgumentException(nameof(index));
        }
        return _items[index];
      }
    }

    public uint Length => (uint)(_items?.Length ?? 0);

    private IEnumerable<T> Enumerable => _items;

    public IEnumerator<T> GetEnumerator() => GetInternalEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerable().GetEnumerator();
    
    //
    // Operations
    //

    public UnmanagedArray<T> SubsetFromIndexes(in IReadOnlyCollection<uint> indexes)
    {
      T[] result = new T[indexes.Count];

      uint i = 0;
      foreach (uint index in indexes)
      {
        result[i] = this[index];
        i++;
      }

      return new UnmanagedArray<T>(result);
    }
    
    //
    // IEnumerable implementation (to avoid boxing)
    //
    
    public IEnumerable<TResult> Select<TResult>(in Func<T, TResult> selector)
    {
      return GetInternalEnumerable().Select(selector);
    }
    
    public IEnumerable<TResult> Select<TResult>(in Func<uint, T, TResult> selector)
    {
      return GetInternalEnumerable().Select(selector);
    }
    
    public IEnumerable<T> Where(in Func<T, bool> predicate)
    {
      return GetInternalEnumerable().Where(predicate);
    }

    public bool SequenceEqual(in UnmanagedArray<T> other)
    {
      return GetInternalEnumerable().SequenceEqual(other.GetInternalEnumerable());
    }
  }
}
