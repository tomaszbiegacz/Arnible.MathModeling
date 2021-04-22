using Xunit;

namespace Arnible.Linq.Test
{
  public class MinDefensiveTests
  {
    [Fact]
    public void Min_OfThree_Defensive()
    {
      Assert.Equal(2d, (new[] { 4d, 2d, 3d }).MinDefensive());
    }
  }
}