using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class CollectionExtensionTests
  {
    [Fact]
    public void Yield_Prepend_Append()
    {
      Assert.Equal(new[] { 2, 3, 4 }, 3.Yield().Prepend(2).Append(4));
    }

    [Fact]
    public void Exclude_First()
    {
      Assert.Equal(new[] { 2, 3, 4 }, (new[] { 1, 2, 3, 4 }).ExcludeAt(0));
    }

    [Fact]
    public void Exclude_1Of3()
    {
      Assert.Equal(new[] { 2d, 3d }, (new[] { 2d, 1d, 3d }).ExcludeAt(1));
    }

    [Fact]
    public void Indexes()
    {
      Assert.Equal(new[] { 0, 1, 2 }, (new[] { 1, 2, 3 }).Indexes());
    }

    [Fact]
    public void ZipDefensive()
    {
      Assert.Equal(new[] { 5, 7, 9 }, (new[] { 1, 2, 3 }).ZipDefensive(new[] { 4, 5, 6 }, (a, b) => a + b));
    }

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
      Assert.Empty(Enumerable.Empty<int>().ToSequncesWithReturning());

      var result = Materialize(new[] { 1, 2 }.ToSequncesWithReturning());
      AssertContainsCollection(result, 1, 1);
      AssertContainsCollection(result, 1, 2);

      AssertContainsCollection(result, 2, 1);
      AssertContainsCollection(result, 2, 2);
      Assert.Equal(4, result.Count());
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

      Assert.Equal(9, result.Count());
    }
  }
}
