using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class FirstIndexOfOrNullTests
  {
    [Fact]
    public void Value_Null()
    {
      IReadOnlyList<int> src = new int[] { 1, 2};
      Assert.False(src.FirstIndexOfOrNull(3).HasValue);
    }
    
    [Fact]
    public void Value_Position()
    {
      IReadOnlyList<int> src = new int[] { 1, 2, 2};
      Assert.Equal(1, src.FirstIndexOfOrNull(2) ?? throw new Exception("null"));
    }
  }
}