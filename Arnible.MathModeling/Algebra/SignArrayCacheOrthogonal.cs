using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  static class OrthogonalSignArrayEnumerable
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>> _collectionsAll;
    private static readonly ConcurrentDictionary<uint, ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>> _collectionsCount;

    static OrthogonalSignArrayEnumerable()
    {
      _collectionsAll = new ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>();
      _collectionsCount = new ConcurrentDictionary<uint, ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>>();
    }

    private static IReadOnlyList<UnmanagedArray<Sign>> BuildOrthogonalSignCollection(uint length)
    {
      return SignArrayCache.GetAllPossibilities(length)
          .Where(s => s.GetIsOrthogonal())
          .Select(s => s.Values)
          .ToReadOnlyList();
    }
    
    public static IReadOnlyList<UnmanagedArray<Sign>> GetOrthogonalSignCollection(in uint length)
    {
      return _collectionsAll.GetOrAdd(length, BuildOrthogonalSignCollection);
    }
    
    private static uint NonZeroCount(UnmanagedArray<Sign> values)
    {
      return values.Where(s => s != 0).Count();
    }
    
    private static IReadOnlyList<UnmanagedArray<Sign>> BuildOrthogonalSignCollection(uint length, uint singsCount)
    {
      return GetOrthogonalSignCollection(length)
        .Where(s => NonZeroCount(s) == singsCount)
        .ToReadOnlyList();
    }

    public static IReadOnlyList<UnmanagedArray<Sign>> GetOrthogonalSignCollection(uint length, uint singsCount)
    {
      if (singsCount == 0 || singsCount > length)
      {
        throw new ArgumentException(nameof(singsCount));
      }
      
      var collection = _collectionsCount.GetOrAdd(length, i => new ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>());
      return collection.GetOrAdd(singsCount, i => BuildOrthogonalSignCollection(length, i));
    }
  }
}
