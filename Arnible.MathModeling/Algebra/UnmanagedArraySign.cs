using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  class UnmanagedArraySign : IUnmanagedArray<Sign>
  {
    private readonly IReadOnlyList<sbyte> _items;

    public UnmanagedArraySign(IReadOnlyList<sbyte> items)
    {
      _items = items;
    }
    
    private IEnumerable<Sign> GetEnumerable() => _items.Select(s => (Sign)s);
    public IEnumerator<Sign> GetEnumerator() => GetEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerable().GetEnumerator();

    public uint Length => (uint)_items.Count;

    public Sign this[in uint index] => (Sign) _items[(int)index];
  }
}