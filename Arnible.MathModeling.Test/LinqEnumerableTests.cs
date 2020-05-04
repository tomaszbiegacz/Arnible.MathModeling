using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqEnumerableTests
  {
    [Fact]
    public void RangeUint()
    {
      Assert.Equal(new[] { 1u, 2u, 3u }, LinqEnumerable.RangeUint(1u, 3u));
    }

    [Fact]
    public void Repeat()
    {
      Assert.Equal(new[] { 1u, 1u, 1u }, LinqEnumerable.Repeat(1u, 3u));
    }

    [Fact]
    public void Yield_Prepend_Append()
    {
      Assert.Equal(new[] { 2, 3, 4 }, LinqEnumerable.Yield(3).Prepend(2).Append(4));
    }

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
    public void Duplicate_First()
    {
      Assert.Equal(new[] { 1, 1, 2, 3, 4 }, (new[] { 1, 2, 3, 4 }).DuplicateAt(0));
    }

    [Fact]
    public void Duplicate_1Of3()
    {
      Assert.Equal(new[] { 2d, 1d, 1d, 3d }, (new[] { 2d, 1d, 3d }).DuplicateAt(1));
    }

    [Fact]
    public void SkipExactly()
    {
      Assert.Equal(new[] {1d, 3d }, (new[] { 2d, 1d, 3d }).SkipExactly(1));
    }

    [Fact]
    public void TakeExactly()
    {
      Assert.Equal(new[] { 2d }, (new[] { 2d, 1d, 3d }).TakeExactly(1));
    }
  }
}
