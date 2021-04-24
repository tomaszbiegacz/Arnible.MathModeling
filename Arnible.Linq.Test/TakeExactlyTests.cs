using Xunit;

namespace Arnible.Linq.Test
{
  public class TakeExactlyTests
  {
    [Fact]
    public void TakeExactly()
    {
      Assert.Equal(2d, (new[] { 2d, 1d, 3d }).TakeExactly(1).Single());
    }
  }
}