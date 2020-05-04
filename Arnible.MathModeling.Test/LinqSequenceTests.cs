using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqSequenceTests
  {
    private static void AssertContainsCollection(IEnumerable<IEnumerable<int>> result, params int[] expected)
    {
      Assert.Contains(result, d => expected.SequenceEqual(d));
    }

    private static IEnumerable<IEnumerable<int>> Materialize(IEnumerable<IEnumerable<int>> enumerator)
    {
      return enumerator.Select(e => e.ToArray()).ToArray();
    }

    [Fact]
    public void ToSequncesWithReturning()
    {
      Assert.Empty(LinqEnumerable.Empty<int>().ToSequncesWithReturning());

      var result = Materialize(new[] { 1, 2 }.ToSequncesWithReturning());
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      Assert.Equal(4u, result.Count());
    }

    [Fact]
    public void ToSequncesWithReturning_Limited()
    {
      Assert.Empty(new[] { 1, 2, 3 }.ToSequncesWithReturning(0));

      var result = Materialize(new[] { 1, 2, 3 }.ToSequncesWithReturning(2));
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);
      AssertContainsCollection(result, 1, 3);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      AssertContainsCollection(result, 2, 3);

      AssertContainsCollection(result, 3, 1);
      AssertContainsCollection(result, 3, 2);
      AssertContainsCollection(result, 3, 3);

      Assert.Equal(9u, result.Count());
    }
  }
}
