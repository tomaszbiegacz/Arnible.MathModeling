using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMinimumTests
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
    public void WithMinimum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMinimum(v => v.V);
      Assert.Equal(2d, r.V);
    }
    
    [Fact]
    public void WithMinimum_Error()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        (new double[0]).WithMinimum(v => v);
      });
    }
  }
}