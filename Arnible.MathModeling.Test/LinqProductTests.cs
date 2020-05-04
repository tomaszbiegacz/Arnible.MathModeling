using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqProductTests
  {
    [Fact]
    public void Product_OfThree_Defensive()
    {
      Assert.Equal(24d, (new[] { 4d, 2d, 3d }).ProductDefensive());
    }

    [Fact]
    public void Product_OfThree_Default()
    {
      Assert.Equal(24d, (new[] { 4d, 2d, 3d }).ProductWithDefault());
    }

    [Fact]
    public void Product_Default()
    {
      Assert.Equal(1d, LinqEnumerable.Empty<double>().ProductWithDefault());
    }
  }
}
