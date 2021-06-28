using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class SequenceEqualTests
  {
    [Fact]
    public void ReadOnlySpan_InvalidLength()
    {
      ReadOnlySpan<int> actual = stackalloc int[] { 1, 2 };
      ReadOnlySpan<int> expected = stackalloc int[] { 1, 2, 3 };
      Assert.False(actual.SequenceEqual(expected));
    }
    
    [Fact]
    public void ReadOnlySpan_NotEqualElement()
    {
      ReadOnlySpan<int> actual = stackalloc int[] { 1, 2 };
      ReadOnlySpan<int> expected = stackalloc int[] { 1, 3 };
      Assert.False(actual.SequenceEqual(expected));
    }
    
    [Fact]
    public void ReadOnlySpan_Ok()
    {
      ReadOnlySpan<int> actual = stackalloc int[] { 1, 3 };
      ReadOnlySpan<int> expected = stackalloc int[] { 1, 3 };
      Assert.True(actual.SequenceEqual(expected));
    }
  }
}