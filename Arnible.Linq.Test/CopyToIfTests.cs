using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class CopyToIfTests
  {
    [Fact]
    public void CopyToWithCondition()
    {
      Span<int> src = stackalloc int[] { 1, -2, 3, -4 };
      Span<int> output = stackalloc int[4];
      Assert.True(src.CopyToIf((in int i) => i > 0, 0, in output));
      Assert.True(output.SequenceEqual(new int[] { 1, 0, 3, 0 }));
    }
    
    [Fact]
    public void NotEqualLength()
    {
      Span<int> src = stackalloc int[] { 1, -2, 3, -4 };
      Span<int> output = stackalloc int[3];
      try
      {
        src.CopyToIf((in int i) => i > 0, 0, in output);
        throw new Exception("Not expected");
      }
      catch(ArgumentException)
      {
        // all is OK
      }
    }
  }
}