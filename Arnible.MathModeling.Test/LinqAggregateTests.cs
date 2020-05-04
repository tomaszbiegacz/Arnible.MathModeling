using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqAggregateTests
  {
    [Fact]
    public void ZipDefensive()
    {
      Assert.Equal(new[] { 5, 7, 9 }, (new[] { 1, 2, 3 }).ZipDefensive(new[] { 4, 5, 6 }, (a, b) => a + b));
    }

    [Fact]
    public void AggregateCombinations_1ChosenOf3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(1, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateCombinations_2ChosenOf3()
    {
      Assert.Equal(new[] { 8d, 12d, 6d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(2, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateCombinations_3ChosenOf3()
    {
      Assert.Equal(new[] { 24d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(3, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateAllCombinations_3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d, 8d, 12d, 6d, 24d }, (new[] { 4d, 2d, 3d }).AggregateCombinationsAll(g => g.ProductDefensive()));
    }
  }
}
