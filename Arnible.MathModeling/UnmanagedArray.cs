using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public readonly struct UnmanagedArray<T> : IUnmanagedArray<T> where T : unmanaged
  {
    private static IEnumerable<T> _empty = LinqEnumerable.Empty<T>().ToReadOnlyList();
    private readonly T[] _items;

    public UnmanagedArray(params T[] items)
    {
      _items = items;
    }

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

    public IEnumerator<T> GetEnumerator() => Enumerable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Enumerable.GetEnumerator();
  }
}
