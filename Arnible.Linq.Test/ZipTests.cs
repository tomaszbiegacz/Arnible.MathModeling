using Xunit;

namespace Arnible.Linq.Test
{
  public class ZipTests
  {
    [Fact]
    public void ZipValue()
    {
      Assert.True(
        (new[] { 1, 2 })
        .ZipValue(new[] { 4, 5, 9 }, (a, b) => (a ?? 0) + (b ?? 0))
        .SequenceEqual(new[] { 5, 7, 9 })
        );
    }
  }
}