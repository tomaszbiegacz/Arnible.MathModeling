using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Algebra.Test
{
  public class SumWithDefaultTests
  {
    [Fact]
    public void IEnumerable_ushort_SumWithDefault_Values()
    {
      IEnumerable<ushort> values = new ushort[] { 1, 2, 3 };
      Assert.Equal<uint>(6, values.SumWithDefault());
    }
    
    [Fact]
    public void IEnumerable_ushort_SumWithDefault_Empty()
    {
      IEnumerable<ushort> values = new ushort[0];
      Assert.Equal<uint>(0, values.SumWithDefault());
    }
    
    [Fact]
    public void IEnumerable_uint_SumWithDefault_Values()
    {
      IEnumerable<uint> values = new uint[] { 1, 2, 3 };
      Assert.Equal<ulong>(6, values.SumWithDefault());
    }
    
    [Fact]
    public void IEnumerable_uint_SumWithDefault_Empty()
    {
      IEnumerable<uint> values = new uint[0];
      Assert.Equal<ulong>(0, values.SumWithDefault());
    }
    
    [Fact]
    public void IEnumerable_double_SumWithDefault_Values()
    {
      IEnumerable<double> values = new double[] { 1, 2, 3 };
      Assert.Equal(6.0, values.SumWithDefault());
    }
    
    [Fact]
    public void IEnumerable_double_SumWithDefault_Empty()
    {
      IEnumerable<double> values = new double[0];
      Assert.Equal(0.0, values.SumWithDefault());
    }
    
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