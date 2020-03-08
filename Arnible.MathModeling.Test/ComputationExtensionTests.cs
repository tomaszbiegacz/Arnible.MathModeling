using Xunit;

namespace Arnible.MathModeling.Test
{
  public class ComputationExtensionTests
  {    
    [Fact]
    public void Sum_OfThree()
    {
      Assert.Equal(6d, (new[] { 1d, 2d, 3d }).Sum());
    }

    [Fact]
    public void Sum_OfThreeNumbers()
    {
      Assert.Equal(6, (new Number[] { 1d, 2d, 3d }).Sum());
    }

    [Fact]
    public void Sum_OfThreeNumbers_WithPredicate()
    {
      Assert.Equal(-6, (new Number[] { 1, 2, 3 }).Sum(v => -1 * v));
    }

    [Fact]
    public void Product_OfThree()
    {
      Assert.Equal(24d, (new[] { 4d, 2d, 3d }).Product());
    }

    [Fact]
    public void AggregateCombinations_1ChosenOf3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(1, g => g.Product()));
    }

    [Fact]
    public void AggregateCombinations_2ChosenOf3()
    {
      Assert.Equal(new[] { 8d, 12d, 6d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(2, g => g.Product()));
    }

    [Fact]
    public void AggregateCombinations_3ChosenOf3()
    {
      Assert.Equal(new[] { 24d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(3, g => g.Product()));
    }

    [Fact]
    public void AggregateAllCombinations_3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d, 8d, 12d, 6d, 24d }, (new[] { 4d, 2d, 3d }).AggregateAllCombinations(g => g.Product()));
    }
  }
}
