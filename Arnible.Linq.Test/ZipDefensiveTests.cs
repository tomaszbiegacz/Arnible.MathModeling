using Xunit;

namespace Arnible.Linq.Test
{
  public class ZipDefensiveTests
  {
    [Fact]
    public void ZipDefensive()
    {
      Assert.True((new[] { 1, 2, 3 })
        .ZipDefensive(new[] { 4, 5, 6 }, (a, b) => a + b)
        .SequenceEqual(new[] { 5, 7, 9 })
        );
    }
  }
}