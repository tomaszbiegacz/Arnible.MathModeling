using Xunit;

namespace Arnible.Linq.Test
{
  public class MaxDefensiveTests
  {
    [Fact]
    public void Max_OfThree_Defensive()
    {
      Assert.Equal(4d, (new[] { 4d, 2d, 3d }).MaxDefensive());
    }
  }
}