using Xunit;

namespace Arnible.Linq.Test
{
  public class LinqArrayTests
  {
    [Fact]
    public void EmptyTests()
    {
      Assert.Empty(LinqArray<uint>.Empty);
    }
  }
}