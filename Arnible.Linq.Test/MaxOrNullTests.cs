using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class MaxOrNullTests
  {
    [Fact]
    public void MaxOrNone_Empty()
    {
      Assert.False(new int[0].MaxOrNone().HasValue);
    }
    
    [Fact]
    public void MaxOrNone_Value()
    {
      Assert.Equal(3, new int[] { 1, 3 }.MaxOrNone() ?? throw new Exception("null"));
    }
    
    [Fact]
    public void MaxOrNull_Empty()
    {
      Assert.Null(new string[0].MaxOrNull());
    }
    
    [Fact]
    public void MaxOrNull_Value()
    {
      Assert.Equal("b", new string[] { "a", "b" }.MaxOrNull() ?? throw new Exception("null"));
    }
  }
}