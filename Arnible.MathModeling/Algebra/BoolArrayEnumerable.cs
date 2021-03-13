using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
{
  public class BoolArrayEnumerable : IReadOnlyCollection<UnmanagedArray<bool>>
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<bool>>> _collections;

    static BoolArrayEnumerable()
    {
      _collections = new ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<bool>>>();
    }

    private static IReadOnlyList<UnmanagedArray<bool>> BuildCollection(uint length)
    {
      var values = new[] { false, true };
      return values
        .ToSequencesWithReturning(length)
        .Select(s => new BoolArray(s))
        .Order()
        .Select(s => s.Values)
        .ToReadOnlyList();
    }

    private static IReadOnlyList<UnmanagedArray<bool>> GetCollection(in uint length)
    {
      return _collections.GetOrAdd(length, BuildCollection);
    }

    private readonly IReadOnlyList<UnmanagedArray<bool>> _collection;

    public BoolArrayEnumerable(in uint size)
    {
      _collection = GetCollection(in size);
    }

    /*
     * IReadOnlyCollection
     */

    public int Count => _collection.Count;

    private IEnumerable<UnmanagedArray<bool>> GetEnumerable() => _collection;

    public IEnumerator<UnmanagedArray<bool>> GetEnumerator() => GetEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerable().GetEnumerator();
  }
}
