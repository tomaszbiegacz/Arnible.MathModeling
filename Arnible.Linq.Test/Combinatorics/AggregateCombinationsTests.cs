using Arnible.Linq.Algebra;
using Xunit;

namespace Arnible.Linq.Combinatorics.Test
{
  public class AggregateCombinationsTests
  {
    [Fact]
    public void AggregateCombinations_1ChosenOf3()
    {
      Assert.True((new[] { 4d, 2d, 3d })
        .AggregateCombinations(1, g => g.ProductDefensive())
        .SequenceEqual(new[] { 4d, 2d, 3d })
        );
    }

    [Fact]
    public void AggregateCombinations_2ChosenOf3()
    {
      Assert.True((new[] { 4d, 2d, 3d })
        .AggregateCombinations(2, g => g.ProductDefensive())
        .SequenceEqual(new[] { 8d, 12d, 6d })
        );
    }

    [Fact]
    public void AggregateCombinations_3ChosenOf3()
    {
      Assert.True((new[] { 4d, 2d, 3d })
        .AggregateCombinations(3, g => g.ProductDefensive())
        .SequenceEqual(new[] { 24d })
        );
    }

    [Fact]
    public void AggregateAllCombinations_3()
    {
      Assert.True((new[] { 4d, 2d, 3d })
        .AggregateCombinationsAll(g => g.ProductDefensive())
        .SequenceEqual(new[] { 4d, 2d, 3d, 8d, 12d, 6d, 24d })
        );
    }
  }
}