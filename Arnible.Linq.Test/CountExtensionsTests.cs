using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class CountExtensionsTests
  {
    [Fact]
    public void ReadOnlySpan_Count_default()
    {
      ReadOnlySpan<double> src = stackalloc double[0];
      Assert.Equal(0u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void ReadOnlySpan_Count_values()
    {
      ReadOnlySpan<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(2u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void Span_Count_default()
    {
      Span<double> src = stackalloc double[0];
      Assert.Equal(0u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void Span_Count_values()
    {
      Span<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(2u, src.Count((in double v) => v > 0));
    }
  }
}