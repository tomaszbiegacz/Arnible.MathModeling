using Xunit;

namespace Arnible.Linq.Test
{
  public class IndexesTests
  {
    [Fact]
    public void Indexes()
    {
      Assert.True((new[] { 1, 2, 3 }).Indexes().SequenceEqual(new ushort[] { 0, 1, 2 }));
    }
  }
}