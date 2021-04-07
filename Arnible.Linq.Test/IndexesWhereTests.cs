using Xunit;

namespace Arnible.Linq.Test
{
  public class IndexesWhereTests
  {
    [Fact]
    public void IndexesWhere()
    {
      Assert.True((new[] { 1, 2, 3 }).IndexesWhere(v => v != 2).SequenceEqual(new ushort[] { 0, 2 }));
    }
  }
}