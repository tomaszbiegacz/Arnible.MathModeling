using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class CountExtensionsTests
  {
    [Fact]
    public void IEnumerable_Count_default()
    {
      IEnumerable<double> src = new double[0];
      Assert.Equal(0u, src.Count());
    }
    
    [Fact]
    public void IEnumerable_Count_values()
    {
      IEnumerable<double> src = new double[] { 1, 2, 5 };
      Assert.Equal(3u, src.Count());
    }
    
    [Fact]
    public void IEnumerable_Count_filtered()
    {
      IEnumerable<double> src = new double[] { 1, 2, 5 };
      Assert.Equal(2u, src.Count(i => i > 1));
    }
    
    [Fact]
    public void IReadOnlyList_Count_values()
    {
      IReadOnlyList<double> src = new double[] { 1, 2, 5 };
      Assert.Equal(3, src.Count());
    }
    
    [Fact]
    public void IList_Count_values()
    {
      IList<double> src = new List<double> { 1, 2, 5 };
      Assert.Equal(3, src.Count());
    }
    
    [Fact]
    public void ReadOnlySpan_Count_values()
    {
      ReadOnlySpan<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(3, src.Count());
    }
    
    [Fact]
    public void ReadOnlySpan_Count_filtered_default()
    {
      ReadOnlySpan<double> src = stackalloc double[0];
      Assert.Equal(0u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void ReadOnlySpan_Count_filtered_values()
    {
      ReadOnlySpan<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(2u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void Span_Count_values()
    {
      Span<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(3, src.Count());
    }
    
    [Fact]
    public void Span_Count_filtered_default()
    {
      Span<double> src = stackalloc double[0];
      Assert.Equal(0u, src.Count((in double v) => v > 0));
    }
    
    [Fact]
    public void Span_Count_filtered_values()
    {
      Span<double> src = stackalloc double[] { -1, 1, 2 };
      Assert.Equal(2u, src.Count((in double v) => v > 0));
    }
  }
}