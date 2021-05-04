using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class ToArrayTests
  {
    [Fact]
    public void FromReadOnlySpan()
    {
      ReadOnlySpan<int> v = stackalloc int[] { 1, 2 };
      AssertExtensions.AreEquals(v.ToArray(i => i+1), new int[] { 2, 3});
    }
    
    [Fact]
    public void FromSpan()
    {
      Span<int> v = stackalloc int[] { 1, 2 };
      AssertExtensions.AreEquals(v.ToArray(i => i+1), new int[] { 2, 3});
    }
  }
}