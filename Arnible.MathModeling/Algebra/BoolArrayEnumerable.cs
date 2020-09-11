using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public class BoolArrayEnumerable : IUnmanagedArrayEnumerable<bool>
  {
    static readonly ConcurrentDictionary<uint, IReadOnlyList<IReadOnlyList<bool>>> _collections = new ConcurrentDictionary<uint, IReadOnlyList<IReadOnlyList<bool>>>();

    private static IReadOnlyList<IReadOnlyList<bool>> BuildCollection(uint length)
    {
      var values = new[] { false, true };
      return values
        .ToSequncesWithReturning(length)
        .Select(s => new BoolArray(s))
        .Order()
        .Select(s => s.Values)
        .ToReadOnlyList();
    }

    private static IReadOnlyList<IReadOnlyList<bool>> GetCollection(in uint length)
    {
      return _collections.GetOrAdd(length, BuildCollection);
    }

    private readonly IReadOnlyList<IReadOnlyList<bool>> _collection;
    private int _position;

    public BoolArrayEnumerable(in uint size)
    {
      _collection = GetCollection(in size);
      _position = 0;
    }

    public override string ToString()
    {
      return $"[{string.Join(',', this)}]";
    }

    /*
     * IArray
     */

    public bool this[in uint index]
    {
      get
      {
        return _collection[_position][(int)index];
      }
    }

    public uint Length => (uint)_collection[_position].Count;

    private IEnumerable<bool> GetEnumerable() => _collection[_position];

    public IEnumerator<bool> GetEnumerator() => GetEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerable().GetEnumerator();

    /*
     * Operations
     */

    public bool MoveNext()
    {
      if (_position + 1 < _collection.Count)
      {
        _position++;
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
