using Xunit;

namespace Arnible.MathModeling.Test
{
  public class CollectionExtensionTests
  {
    [Fact]
    public void Exclude_First()
    {
      Assert.Equal(new[] { 2, 3, 4 }, (new[] { 1, 2, 3, 4 }).ExcludeAt(0));
    }

    [Fact]
    public void Exclude_1Of3()
    {
      Assert.Equal(new[] { 2d, 3d }, (new[] { 2d, 1d, 3d }).ExcludeAt(1));
    }

    [Fact]
    public void Indexes()
    {
      Assert.Equal(new[] { 0, 1, 2 }, (new[] { 1, 2, 3 }).Indexes());
    }

    [Fact]
    public void SelectMerged()
    {
      Assert.Equal(new[] { 5, 7, 9 }, (new[] { 1, 2, 3 }).SelectMerged(new[] { 4, 5, 6 }, (a, b) => a + b));
    }
  }
}
