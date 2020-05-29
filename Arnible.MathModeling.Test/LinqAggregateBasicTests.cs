using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqAggregateBasicTests
  {
    struct Value
    {
      public Value(double v)
      {
        V = v;
      }

      public double V { get; }
    }

    [Fact]
    public void Min_OfThree_Defensive()
    {
      Assert.Equal(2d, (new[] { 4d, 2d, 3d }).MinDefensive());
    }

    [Fact]
    public void WithMinimum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMinimum(v => v.V);
      Assert.Equal(2d, r.V);
    }

    [Fact]
    public void Max_OfThree_Defensive()
    {
      Assert.Equal(4d, (new[] { 4d, 2d, 3d }).MaxDefensive());
    }

    [Fact]
    public void WithMaximum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMaximum(v => v.V);
      Assert.Equal(4d, r.V);
    }

    [Fact]
    public void Median_OfThree_Defensive()
    {
      Assert.Equal(3d, (new[] { 4d, 2d, 3d }).MedianDefensive());
    }

    [Fact]
    public void Median_OfFive_Defensive()
    {
      Assert.Equal(3d, (new[] { 1d, 3d, 4d, 2d, 3d }).MedianDefensive());
    }

    [Fact]
    public void Median_OfOne_Defensive()
    {
      Assert.Equal(2d, (new[] { 2d }).MedianDefensive());
    }
  }
}
