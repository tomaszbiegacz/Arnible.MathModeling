using System;
using Xunit;

namespace Arnible.Test
{
  public class SpanSingleTests
  {
    [Fact]
    public void Basic()
    {
      SpanSingle<ushort> v = new(new ushort[] { 1 });
      Assert.Equal(1, v);
      Assert.True(v == new SpanSingle<ushort>(new ushort[] { 1 }));
      Assert.False(v == new SpanSingle<ushort>(new ushort[] { 2 }));
      Assert.True(v != new SpanSingle<ushort>(new ushort[] { 2 }));
      
      v.Set(2);
      Assert.False(v == new SpanSingle<ushort>(new ushort[] { 1 }));
      Assert.True(v == new SpanSingle<ushort>(new ushort[] { 2 }));
    }
    
    [Fact]
    public void Invalid()
    {
      try
      {
        SpanSingle<ushort> v = new(new ushort[2]);
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
  }
}