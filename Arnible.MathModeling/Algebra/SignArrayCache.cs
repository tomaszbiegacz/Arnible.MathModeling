using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  static class SignArrayCache
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<SignArray>> _collections;
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>> _collectionsAxis;

    static SignArrayCache()
    {
      _collections = new ConcurrentDictionary<uint, IReadOnlyList<SignArray>>();
      _collectionsAxis = new ConcurrentDictionary<uint, IReadOnlyList<UnmanagedArray<Sign>>>();
    }
    
    private static IReadOnlyList<SignArray> BuildSignCollection(uint length)
    {
      var values = new[] { Sign.Negative, Sign.None, Sign.Positive };
      return values
        .ToSequencesWithReturning(length)
        .Select(s => new SignArray(s))
        .Order()
        .ToReadOnlyList();
    }
    
    public static IReadOnlyList<SignArray> GetAllPossibilities(in uint length)
    {
      return _collections.GetOrAdd(length, BuildSignCollection);
    }
    
    private static IEnumerable<UnmanagedArray<Sign>> BuildAxisEnumerable(uint length)
    {
      Sign[] result = new Sign[length];
      Array.Fill(result, Sign.None);
      
      for (uint i = 0; i < length; ++i)
      {
        result[i] = Sign.Positive;
        yield return result.ToUnmanagedArray();
        result[i] = Sign.None;
      }
    }

    private static IReadOnlyList<UnmanagedArray<Sign>> BuildAxisCollection(uint length)
    {
      return BuildAxisEnumerable(length).ToReadOnlyList();
    }
    
    public static IReadOnlyList<UnmanagedArray<Sign>> GetAxisParameters(in uint length)
    {
      return _collectionsAxis.GetOrAdd(length, BuildAxisCollection);
    }
  }
}