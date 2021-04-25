using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class WhereTests
  {
    [Fact]
    public void WhereBasic()
    {
      IEnumerable<int> src = new int[] { 1, 2, 3, 4 };
      Assert.True(src.Where(i => i%2 == 0).SequenceEqual(new int[] { 2, 4 }));
    }
    
    [Fact]
    public void WhereReadOnlySpanPositive()
    {
      ReadOnlySpan<int> src = stackalloc int[] { 1, -2, 3, -4 };
      Span<int> output = stackalloc int[4];
      Assert.True(src.Where((in int i) => i > 0, in output));
      Assert.True(output.SequenceEqual(new int[] { 1, 0, 3, 0 }));
    }
  }
}