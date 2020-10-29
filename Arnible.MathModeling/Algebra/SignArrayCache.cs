using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  static class SignArrayCache
  {
    private static readonly ConcurrentDictionary<uint, IReadOnlyList<SignArray>> _collections;

    static SignArrayCache()
    {
      _collections = new ConcurrentDictionary<uint, IReadOnlyList<SignArray>>();
    }
    
    private static IReadOnlyList<SignArray> BuildOSignCollection(uint length)
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
      return _collections.GetOrAdd(length, BuildOSignCollection);
    }
  }
}