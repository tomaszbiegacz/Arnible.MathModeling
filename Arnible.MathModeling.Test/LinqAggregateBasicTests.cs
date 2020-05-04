using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqAggregateBasicTests
  {
    [Fact]
    public void Min_OfThree_Defensive()
    {
      Assert.Equal(2d, (new[] { 4d, 2d, 3d }).MinDefensive());
    }

    [Fact]
    public void Max_OfThree_Defensive()
    {
      Assert.Equal(4d, (new[] { 4d, 2d, 3d }).MaxDefensive());
    }
  }
}
