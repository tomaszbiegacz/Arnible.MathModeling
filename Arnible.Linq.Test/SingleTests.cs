using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class SingleTests
  {
    [Fact]
    public void IEnumerable_Single()
    {
      IEnumerable<int> v = new [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void IList_Single()
    {
      IList<int> v = new [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void IReadOnlyList_Single()
    {
      IReadOnlyList<int> v = new [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void ReadOnlySpan_Single()
    {
      ReadOnlySpan<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void Span_Single()
    {
      Span<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void IEnumerable_SingleOrNone_Value()
    {
      IEnumerable<int> v = new [] { 1 };
      Assert.Equal(1, v.SingleOrNone());
    }
    
    [Fact]
    public void IEnumerable_SingleOrNone_None()
    {
      IEnumerable<int> v = new int[0];
      Assert.True(v.SingleOrNone() == null);
    }
    
    [Fact]
    public void ReadOnlySpan_SingleOrNone_Value()
    {
      ReadOnlySpan<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.SingleOrNone());
    }
    
    [Fact]
    public void ReadOnlySpan_SingleOrNone_None()
    {
      ReadOnlySpan<int> v = stackalloc int[0];
      Assert.True(v.SingleOrNone() == null);
    }
    
    [Fact]
    public void Span_SingleOrNone_Value()
    {
      Span<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.SingleOrNone());
    }
    
    [Fact]
    public void Span_SingleOrNone_None()
    {
      Span<int> v = stackalloc int[0];
      Assert.True(v.SingleOrNone() == null);
    }
    
    [Fact]
    public void IEnumerable_SingleOrNull_Value()
    {
      object o1 = new object();
      IEnumerable<object> v = new [] { o1 };
      Assert.Same(o1, v.SingleOrNull());
    }
    
    [Fact]
    public void IEnumerable_SingleOrNull_Null()
    {
      IEnumerable<object> v = new object[0];
      Assert.Null(v.SingleOrNull());
    }
  }
}