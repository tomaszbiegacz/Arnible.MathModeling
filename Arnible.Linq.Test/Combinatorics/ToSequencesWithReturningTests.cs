using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Combinatorics.Test
{
  public class ToSequencesWithReturningTests
  {
    private static void AssertContainsCollection(IEnumerable<IEnumerable<int>> result, params int[] expected)
    {
      Assert.True(result.Any(d => expected.SequenceEqual(d)));
    }

    private static IEnumerable<IEnumerable<int>> Materialize(IEnumerable<IEnumerable<int>> enumerator)
    {
      return enumerator.Select(e => e.ToArray()).ToArray();
    }
    
    [Fact]
    public void ToSequencesWithReturning()
    {
      Assert.Empty(LinqArray<int>.Empty.ToSequencesWithReturning());

      var result = Materialize(new[] { 1, 2 }.ToSequencesWithReturning()).ToArray();
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      Assert.Equal(4, result.Length);
    }

    [Fact]
    public void ToSequencesWithReturning_Limited()
    {
      Assert.Empty(new[] { 1, 2, 3 }.ToSequencesWithReturning(0));

      var result = Materialize(new[] { 1, 2, 3 }.ToSequencesWithReturning(2)).ToArray();
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);
      AssertContainsCollection(result, 1, 3);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      AssertContainsCollection(result, 2, 3);

      AssertContainsCollection(result, 3, 1);
      AssertContainsCollection(result, 3, 2);
      AssertContainsCollection(result, 3, 3);

      Assert.Equal(9, result.Length);
    }
  }
}