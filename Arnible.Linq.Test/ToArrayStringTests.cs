using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class ToArrayStringTests
  {
    [Fact]
    public void ReadOnlySpan_ToArrayString()
    {
      ReadOnlySpan<int> src = stackalloc int[] { 1, 2, 3 };
      string actual = src.ToArrayString(); 
      Assert.Equal("[1,2,3]", actual);
    }
  }
}