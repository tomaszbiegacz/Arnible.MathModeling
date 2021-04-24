using Xunit;

namespace Arnible.Linq.Test
{
  public class SkipExactlyTests
  {
    [Fact]
    public void SkipExactly()
    {
      Assert.True((new[] { 2d, 1d, 3d }).SkipExactly(1).SequenceEqual(new[] {1d, 3d }));
    }
  }
}