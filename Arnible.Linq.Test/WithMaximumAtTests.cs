using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMaximumAtTests
  {
    readonly struct Value
    {
      public Value(double v)
      {
        V = v;
      }

      public double V { get; }
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_Span()
    {
      Span<double> src = new double[] {4, 6, 3};
      Assert.Equal(1, src.WithMaximumAt());
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_ReadOnlySpan()
    {
      ReadOnlySpan<double> src = new double[] {4, 6, 3};
      Assert.Equal(1, src.WithMaximumAt());
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_ReadOnlySpan_Error()
    {
      ReadOnlySpan<double> src = new double[0];
      try
      {
        src.WithMaximumAt();
        throw new Exception("Something went wrong");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_Span_Func()
    {
      Span<Value> src = new[] {new Value(4), new Value(6), new Value(3)};
      Assert.Equal(1, src.WithMaximumAt((in Value v) => v.V));
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_ReadOnlySpan_Func()
    {
      ReadOnlySpan<Value> src = stackalloc[] {new Value(4), new Value(6), new Value(3)};
      Assert.Equal(1, src.WithMaximumAt((in Value v) => v.V));
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_ReadOnlySpan_Func_Error()
    {
      ReadOnlySpan<Value> src = new Value[0];
      try
      {
        src.WithMaximumAt((in Value v) => v.V);
        throw new Exception("Something went wrong");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    }
  }
}