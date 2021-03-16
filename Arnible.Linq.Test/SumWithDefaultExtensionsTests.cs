using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class SumWithDefaultExtensionsTests
  {
    [Fact]
    public void ReadOnlySpan_double_SumWithDefault_Empty()
    {
      ReadOnlySpan<double> src = stackalloc double[0];
      Assert.Equal(0d, src.SumWithDefault((in double v) => v + 1));
    }
    
    [Fact]
    public void Span_double_SumWithDefault_Empty()
    {
      Span<double> src = stackalloc double[0];
      Assert.Equal(0d, src.SumWithDefault((in double v) => v + 1));
    }
    
    [Fact]
    public void ReadOnlySpan_double_SumWithDefault_2Values()
    {
      ReadOnlySpan<double> src = stackalloc[] {1d, 3d,};
      Assert.Equal(-4d, src.SumWithDefault((in double v) => -1 * v));
    }
    
    [Fact]
    public void Span_double_SumWithDefault_2Values()
    {
      Span<double> src = stackalloc[] {1d, 3d,};
      Assert.Equal(-4d, src.SumWithDefault((in double v) => -1 * v));
    }
  }
}