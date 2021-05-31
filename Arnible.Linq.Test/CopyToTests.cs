using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class CopyToTests
  {
    [Fact]
    public void CopyToWithCondition()
    {
      Span<int> src = stackalloc int[] { 1, -2, 3, -4 };
      Span<int> output = stackalloc int[4];
      Assert.True(src.CopyTo((in int i) => i > 0, 0, in output));
      Assert.True(output.SequenceEqual(new int[] { 1, 0, 3, 0 }));
    }
  }
}