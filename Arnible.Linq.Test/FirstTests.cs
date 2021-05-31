using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class FirstTests
  {
    [Fact]
    public void IEnumerable_First()
    {
      IEnumerable<int> v = new [] { 1, 2};
      Assert.Equal(1, v.First());
    }
    
    [Fact]
    public void IEnumerable_First_Empty()
    {
      IEnumerable<int> v = new int[0];
      try
      {
        v.First();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }

    [Fact]
    public void IEnumerable_FirstOrNone_Value()
    {
      IEnumerable<int> v = new [] { 1, 2};
      Assert.Equal(1, v.FirstOrNone());
    }
    
    [Fact]
    public void IEnumerable_FirstOrNone_None()
    {
      IEnumerable<int> v = new int[0];
      Assert.True(v.FirstOrNone() == null);
    }
    
    [Fact]
    public void ReadOnlySpan_FirstOrNone_Value()
    {
      ReadOnlySpan<int> v = stackalloc [] { 1, 2};
      Assert.Equal(1, v.FirstOrNone());
    }
    
    [Fact]
    public void ReadOnlySpan_FirstOrNone_None()
    {
      ReadOnlySpan<int> v = stackalloc int[0];
      Assert.True(v.FirstOrNone() == null);
    }
    
    [Fact]
    public void Span_FirstOrNone_Value()
    {
      Span<int> v = stackalloc [] { 1, 2};
      Assert.Equal(1, v.FirstOrNone());
    }
    
    [Fact]
    public void Span_FirstOrNone_None()
    {
      Span<int> v = stackalloc int[0];
      Assert.True(v.FirstOrNone() == null);
    }
    
    [Fact]
    public void IEnumerable_FirstOrNull_Value()
    {
      object o1 = new object();
      object o2 = new object();
      IEnumerable<object> v = new [] { o1, o2};
      Assert.Same(o1, v.FirstOrNull());
    }
    
    [Fact]
    public void IEnumerable_FirstOrNull_Null()
    {
      IEnumerable<object> v = new object[0];
      Assert.Null(v.FirstOrNull());
    }
  }
}