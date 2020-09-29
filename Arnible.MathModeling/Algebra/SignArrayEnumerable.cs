using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public class SignArrayEnumerable : IUnmanagedArrayEnumerable<Sign>
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<IReadOnlyList<sbyte>>> _collections;

    static SignArrayEnumerable()
    {
      _collections = new ConcurrentDictionary<uint, IReadOnlyList<IReadOnlyList<sbyte>>>();
    }

    private static IReadOnlyList<IReadOnlyList<sbyte>> BuildOSignCollection(uint length)
    {
      var values = new[] { (sbyte)Sign.Negative, (sbyte)Sign.None, (sbyte)Sign.Positive };
      return values
        .ToSequencesWithReturning(length)
        .Select(s => new SignArray(s))
        .Order()
        .Select(s => s.Values)
        .ToReadOnlyList();
    }

    private static IReadOnlyList<IReadOnlyList<sbyte>> GetSignCollection(in uint length)
    {
      return _collections.GetOrAdd(length, BuildOSignCollection);
    }

    private readonly IReadOnlyList<IReadOnlyList<sbyte>> _collection;
    private int _position;

    public SignArrayEnumerable(in uint size)
    {
      _collection = GetSignCollection(in size);
      _position = 0;
    }

    public override string ToString()
    {
      return $"[{string.Join(',', this)}]";
    }

    /*
     * IArray
     */

    public Sign this[in uint index] => (Sign)_collection[_position][(int)index];

    public uint Length => (uint)_collection[_position].Count;

    private IEnumerable<Sign> GetEnumerable() => _collection[_position].Select(s => (Sign)s);

    public IEnumerator<Sign> GetEnumerator() => GetEnumerable().GetEnumerator();

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