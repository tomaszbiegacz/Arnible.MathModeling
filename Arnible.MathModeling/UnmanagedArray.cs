using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public class UnmanagedArray<T> : IUnmanagedArray<T> where T : unmanaged
  {
    private readonly T[] _items;

    public UnmanagedArray(params T[] items)
    {
      _items = items ?? new T[0];
    }

    public T this[in uint index] => _items[index];

    public uint Length => (uint)_items.Length;

    private IEnumerable<T> Enumerable => _items;

    public IEnumerator<T> GetEnumerator() => Enumerable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Enumerable.GetEnumerator();
  }
}
