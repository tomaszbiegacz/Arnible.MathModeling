using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public class OrthogonalSignArrayEnumerable : IArray<Sign>
  {
    static ConcurrentDictionary<uint, ValueArray<ValueArray<sbyte>>> _collections = new ConcurrentDictionary<uint, ValueArray<ValueArray<sbyte>>>();

    private static ValueArray<ValueArray<sbyte>> BuildOrthogonalSignCollection(uint length)
    {
      var values = new[] { (sbyte)Sign.Negative, (sbyte)Sign.None, (sbyte)Sign.Positive };
      return values.ToSequncesWithReturning(length).Select(s => new SignArray(s)).Where(s => s.IsOrthogonal).Order().Select(s => s.Signs).ToValueArray();
    }

    private static ValueArray<ValueArray<sbyte>> GetOrthogonalSignCollection(uint length)
    {
      return _collections.GetOrAdd(length, BuildOrthogonalSignCollection);
    }

    private readonly ValueArray<ValueArray<sbyte>> _collection;
    private uint _position;

    public OrthogonalSignArrayEnumerable(uint size)
    {
      _collection = GetOrthogonalSignCollection(size);
      _position = 0;
    }

    public override string ToString()
    {
      return $"[{string.Join(',', this)}]";
    }

    /*
     * IArray
     */

    public Sign this[uint index] => (Sign)_collection[_position][index];

    public uint Length => _collection[_position].Length;

    private IEnumerable<Sign> GetEnumerable() => _collection[_position].Select(s => (Sign)s);

    public IEnumerator<Sign> GetEnumerator() => GetEnumerable().GetEnumerator();

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
