using System;
using Xunit;

namespace Arnible.Test
{
  public class Span2DTests
  {
    [Fact]
    public void Basic()
    {
      Span2D<ushort> v = new(new ushort[6], columnsCount: 3);
      Assert.Equal(2, v.RowsCount);
      Assert.Equal(3, v.ColumnsCount);
      
      v.Clear();
      Assert.True(v == new Span2D<ushort>(new ushort[6] { 0, 0, 0, 0, 0, 0 }, columnsCount: 3));
      Assert.False(v != new Span2D<ushort>(new ushort[6] { 0, 0, 0, 0, 0, 0 }, columnsCount: 3));
      
      Assert.False(v == new Span2D<ushort>(new ushort[6] { 0, 0, 0, 0, 0, 0 }, columnsCount: 2));
      Assert.True(v != new Span2D<ushort>(new ushort[6] { 0, 0, 0, 0, 0, 0 }, columnsCount: 2));
      
      Assert.False(v == new Span2D<ushort>(new ushort[6] { 0, 0, 0, 1, 1, 1 }, columnsCount: 3));
      v.Row(1).Fill(1);
      Assert.True(v == new Span2D<ushort>(new ushort[6] { 0, 0, 0, 1, 1, 1 }, columnsCount: 3));
      
      Assert.True(v.Row(0).SequenceEqual(new ushort[] { 0, 0, 0}));
      Assert.True(v.Row(1).SequenceEqual(new ushort[] { 1, 1, 1}));
    }
    
    [Fact]
    public void InvalidBuffer()
    {
      try
      {
        Span2D<ushort> v = new(new ushort[6], columnsCount: 4);
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void InvalidColumnsCount()
    {
      try
      {
        Span2D<ushort> v = new(new ushort[6], columnsCount: 0);
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
  }
}