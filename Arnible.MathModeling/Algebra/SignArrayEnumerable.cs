using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public class SignArrayEnumerable : IReadOnlyCollection<UnmanagedArray<Sign>>
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>> _collections;
    private readonly IReadOnlyList<UnmanagedArray<Sign>> _collection;

    static SignArrayEnumerable()
    {
      _collections = new ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>();
    }

    private static IReadOnlyList<UnmanagedArray<Sign>> BuildOSignCollection(uint length)
    {
      return SignArrayCache.GetAllPossibilities(length)
        .Select(s => s.Values)
        .ToReadOnlyList();
    }

    private static IReadOnlyList<UnmanagedArray<Sign>> GetSignCollection(in uint length)
    {
      return _collections.GetOrAdd(length, BuildOSignCollection);
    }
    
    public SignArrayEnumerable(in uint size)
    : this(GetSignCollection(in size))
    {
      // intentionally empty
    }
    
    internal SignArrayEnumerable(IReadOnlyList<UnmanagedArray<Sign>> collection)
    {
      _collection = collection;
    }

    public static SignArrayEnumerable GetOrthogonalSignCollection(in uint length)
    {
      return new SignArrayEnumerable(OrthogonalSignArrayEnumerable.GetOrthogonalSignCollection(length));
    }

    public static SignArrayEnumerable GetOrthogonalSignCollection(uint length, uint singsCount)
    {
      return new SignArrayEnumerable(OrthogonalSignArrayEnumerable.GetOrthogonalSignCollection(length, singsCount));
    }
    
    public static SignArrayEnumerable GetAxisCollection(uint length)
    {
      return new SignArrayEnumerable(SignArrayCache.GetAxisParameters(length));
    }
    
    //
    // IReadOnlyCollection
    //
    
    public int Count => _collection.Count;

    private IEnumerable<UnmanagedArray<Sign>> GetEnumerable() => _collection;

    public IEnumerator<UnmanagedArray<Sign>> GetEnumerator() => GetEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerable().GetEnumerator();
  }
}