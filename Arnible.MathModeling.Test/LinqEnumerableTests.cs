using Arnible.Linq;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqEnumerableTests
  {
    [Fact]
    public void RangeUint()
    {
      AreEquals(new[] { 1u, 2u, 3u }, LinqEnumerable.RangeUint(1u, 3u));
    }

    [Fact]
    public void Repeat()
    {
      AreEquals(new[] { 1u, 1u, 1u }, LinqEnumerable.Repeat(1u, 3u));
    }

    [Fact]
    public void Yield_Prepend_Append()
    {
      AreEquals(new[] { 2, 3, 4 }, LinqEnumerable.Yield(3).Prepend(2).Append(4));
    }

    [Fact]
    public void Exclude_First()
    {
      AreEquals(new[] { 2, 3, 4 }, (new[] { 1, 2, 3, 4 }).ExcludeAt(0));
    }

    [Fact]
    public void Exclude_Two()
    {
      AreEquals(new[] { 2, 3 }, (new[] { 1, 2, 3, 4 }).ExcludeAt(new[] { 0u, 3u }));
    }

    [Fact]
    public void Exclude_1Of3()
    {
      AreEquals(new[] { 2d, 3d }, (new[] { 2d, 1d, 3d }).ExcludeAt(1));
    }

    [Fact]
    public void Duplicate_First()
    {
      AreEquals(new[] { 1, 1, 2, 3, 4 }, (new[] { 1, 2, 3, 4 }).DuplicateAt(0));
    }

    [Fact]
    public void Duplicate_Two()
    {
      AreEquals(new[] { 1, 1, 2, 3, 3, 4 }, (new[] { 1, 2, 3, 4 }).DuplicateAt(new[] { 0u, 2u }));
    }

    [Fact]
    public void Duplicate_1Of3()
    {
      AreEquals(new[] { 2d, 1d, 1d, 3d }, (new[] { 2d, 1d, 3d }).DuplicateAt(1));
    }

    [Fact]
    public void SkipExactly()
    {
      AreEquals(new[] {1d, 3d }, (new[] { 2d, 1d, 3d }).SkipExactly(1));
    }

    [Fact]
    public void TakeExactly()
    {
      AreEquals(new[] { 2d }, (new[] { 2d, 1d, 3d }).TakeExactly(1));
    }

    [Fact]
    public void TakeAtMost_Subset()
    {
      AreEquals(new[] { 2d, 1d }, (new[] { 2d, 1d, 3d }).TakeAtMost(2));
    }

    [Fact]
    public void TakeAtMost_All()
    {
      AreEquals(new[] { 2d, 1d }, (new[] { 2d, 1d }).TakeAtMost(3));
    }
    
    [Fact]
    public void Append_Two()
    {
      AreEquals(new[] { 2d, 1d, 3, 3 }, (new[] { 2d, 1d }).Append(3, 2));
    }
  }
}
