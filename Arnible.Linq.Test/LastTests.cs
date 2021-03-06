using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class LastTests
  {
    [Fact]
    public void IEnumerable_Last()
    {
      IEnumerable<int> v = new [] { 1, 2};
      Assert.Equal(2, v.Last());
    }
    
    [Fact]
    public void IEnumerable_Last_Empty()
    {
      IEnumerable<int> v = new int[0];
      try
      {
        v.Last();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }

    [Fact]
    public void IEnumerable_LastOrNone_Value()
    {
      IEnumerable<int> v = new [] { 1, 2};
      Assert.Equal(2, v.LastOrNone());
    }
    
    [Fact]
    public void IEnumerable_LastOrNone_None()
    {
      IEnumerable<int> v = new int[0];
      Assert.True(v.LastOrNone() == null);
    }
    
    [Fact]
    public void ReadOnlySpan_LastOrNone_Value()
    {
      ReadOnlySpan<int> v = stackalloc [] { 1, 2};
      Assert.Equal(2, v.LastOrNone());
    }
    
    [Fact]
    public void ReadOnlySpan_LastOrNone_None()
    {
      ReadOnlySpan<int> v = stackalloc int[0];
      Assert.True(v.LastOrNone() == null);
    }
    
    [Fact]
    public void Span_LastOrNone_Value()
    {
      Span<int> v = stackalloc [] { 1, 2};
      Assert.Equal(2, v.LastOrNone());
    }
    
    [Fact]
    public void Span_LastOrNone_None()
    {
      Span<int> v = stackalloc int[0];
      Assert.True(v.LastOrNone() == null);
    }
    
    [Fact]
    public void IEnumerable_LastOrNull_Value()
    {
      object o1 = new object();
      object o2 = new object();
      IEnumerable<object> v = new [] { o1, o2};
      Assert.Same(o2, v.LastOrNull());
    }
    
    [Fact]
    public void IEnumerable_LastOrNull_Null()
    {
      IEnumerable<object> v = new object[0];
      Assert.Null(v.LastOrNull());
    }
  }
}