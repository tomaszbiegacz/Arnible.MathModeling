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
    public void IEnumerable_Single_Empty()
    {
      IEnumerable<int> v = new int[0];
      try
      {
        v.Single();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void IEnumerable_Single_Two()
    {
      IEnumerable<int> v = new int[] { 1, 2 };
      try
      {
        v.Single();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }

    [Fact]
    public void IReadOnlyList_Single()
    {
      IReadOnlyList<int> v = new [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void IReadOnlyList_Single_Empty()
    {
      IReadOnlyList<int> v = new int[0];
      try
      {
        v.Single();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void ReadOnlySpan_Single()
    {
      ReadOnlySpan<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void ReadOnlySpan_Single_Empty()
    {
      ReadOnlySpan<int> v = stackalloc int[0];
      try
      {
        v.Single();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
    }
    
    [Fact]
    public void Span_Single()
    {
      Span<int> v = stackalloc [] { 1 };
      Assert.Equal(1, v.Single());
    }
    
    [Fact]
    public void Span_Single_Empty()
    {
      Span<int> v = stackalloc int[0];
      try
      {
        v.Single();
        throw new Exception("I should not get");
      }
      catch(ArgumentException)
      {
        // all is fine
      }
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