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
    public void WithMaximumAt_OfThree_IReadOnlyList()
    {
      IReadOnlyList<Value> src = new[] {new Value(4), new Value(6), new Value(3)};
      Assert.Equal(1, src.WithMaximumAt(v => v.V));
    }
    
    [Fact]
    public void WithMaximumAt_OfThree_ReadOnlySpan()
    {
      ReadOnlySpan<Value> src = stackalloc[] {new Value(4), new Value(6), new Value(3)};
      Assert.Equal(1, src.WithMaximumAt((in Value v) => v.V));
    }
  }
}