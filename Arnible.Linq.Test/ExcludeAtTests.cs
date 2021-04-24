using Xunit;

namespace Arnible.Linq.Test
{
  public class ExcludeAtTests
  {
    [Fact]
    public void Exclude_First()
    {
      Assert.True((new[] { 1, 2, 3, 4 }).ExcludeAt(0).SequenceEqual(new[] { 2, 3, 4 }));
    }

    [Fact]
    public void Exclude_1Of3()
    {
      Assert.True((new[] { 2d, 1d, 3d }).ExcludeAt(1).SequenceEqual(new[] { 2d, 3d }));
    }
  }
}