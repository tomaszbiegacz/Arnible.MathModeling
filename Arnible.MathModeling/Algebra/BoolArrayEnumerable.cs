using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public class BoolArrayEnumerable : IArrayEnumerable<bool>
  {
    static ConcurrentDictionary<uint, ValueArray<ValueArray<bool>>> _collections = new ConcurrentDictionary<uint, ValueArray<ValueArray<bool>>>();

    private static ValueArray<ValueArray<bool>> BuildCollection(uint length)
    {
      var values = new[] { false, true };
      return values.ToSequncesWithReturning(length).Select(s => new BoolArray(s)).Order().Select(s => s.Values).ToValueArray();
    }

    private static ValueArray<ValueArray<bool>> GetCollection(uint length)
    {
      return _collections.GetOrAdd(length, BuildCollection);
    }

    private readonly ValueArray<ValueArray<bool>> _collection;
    private uint _position;

    public BoolArrayEnumerable(uint size)
    {
      _collection = GetCollection(size);
      _position = 0;
    }

    public override string ToString()
    {
      return $"[{string.Join(',', this)}]";
    }

    /*
     * IArray
     */

    public bool this[uint index] => _collection[_position][index];

    public uint Length => _collection[_position].Length;

    private IEnumerable<bool> GetEnumerable() => _collection[_position];

    public IEnumerator<bool> GetEnumerator() => GetEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerable().GetEnumerator();

    /*
     * Operations
     */

    public bool MoveNext()
    {
      if (_position + 1 < _collection.Length)
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
