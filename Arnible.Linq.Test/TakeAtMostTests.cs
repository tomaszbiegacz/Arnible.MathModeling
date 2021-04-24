using Xunit;

namespace Arnible.Linq.Test
{
  public class TakeAtMostTests
  {
    [Fact]
    public void TakeAtMost_Subset()
    {
      Assert.True((new[] { 2d, 1d, 3d }).TakeAtMost(2).SequenceEqual(new[] { 2d, 1d }));
    }

    [Fact]
    public void TakeAtMost_All()
    {
      Assert.True((new[] { 2d, 1d }).TakeAtMost(3).SequenceEqual(new[] { 2d, 1d }));
    }
  }
}