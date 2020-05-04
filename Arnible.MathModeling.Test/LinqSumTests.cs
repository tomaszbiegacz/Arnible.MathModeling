using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqSumTests
  {
    [Fact]
    public void Sum_OfThree_Defensive()
    {
      Assert.Equal(6d, (new[] { 1d, 2d, 3d }).SumDefensive());
    }

    [Fact]
    public void Sum_OfThree_Default()
    {
      Assert.Equal(6d, (new[] { 1d, 2d, 3d }).SumWithDefault());
    }

    [Fact]
    public void Product_Default()
    {
      Assert.Equal(0d, LinqEnumerable.Empty<double>().SumWithDefault());
    }
  }
}
