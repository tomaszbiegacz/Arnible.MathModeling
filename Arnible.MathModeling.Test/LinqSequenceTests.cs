using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqSequenceTests
  {
    private static void AssertContainsCollection(IEnumerable<IEnumerable<int>> result, params int[] expected)
    {
      IsTrue(result.Any(d => expected.SequenceEqual(d)));
    }

    private static IEnumerable<IEnumerable<int>> Materialize(IEnumerable<IEnumerable<int>> enumerator)
    {
      return enumerator.Select(e => e.ToReadOnlyList()).ToReadOnlyList();
    }

    [Fact]
    public void ToSequncesWithReturning()
    {
      IsEmpty(LinqEnumerable.Empty<int>().ToSequncesWithReturning());

      var result = Materialize(new[] { 1, 2 }.ToSequncesWithReturning());
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      AreEqual(4u, result.Count());
    }

    [Fact]
    public void ToSequncesWithReturning_Limited()
    {
      IsEmpty(new[] { 1, 2, 3 }.ToSequncesWithReturning(0));

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

      AreEqual(9u, result.Count());
    }
  }
}
