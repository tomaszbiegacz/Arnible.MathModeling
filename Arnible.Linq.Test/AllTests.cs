using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class AllTests
  {
    [Fact]
    public void ReadOnlySpan_Boolean_OK()
    {
      ReadOnlySpan<bool> v = new bool[] { true, true}; 
      Assert.True(v.All());
    }
    
    [Fact]
    public void ReadOnlySpan_Boolean_Missing()
    {
      ReadOnlySpan<bool> v = new bool[] { false, true};
      Assert.False(v.All());
    }
    
    [Fact]
    public void Span_Boolean_Missing()
    {
      Span<bool> v = new bool[] { false, true};
      Assert.False(v.All());
    }
  }
}