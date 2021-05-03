using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class ToCharMemoryExtensionsTests
  {
    [Fact]
    public void ToCharMemory()
    {
      ReadOnlySpan<int> src = stackalloc int[] { 1, 2, 3 };
      ReadOnlyMemory<char> actual = src.ToCharMemory(); 
      Assert.Equal("[1,2,3]", actual.ToString());
    }
  }
}