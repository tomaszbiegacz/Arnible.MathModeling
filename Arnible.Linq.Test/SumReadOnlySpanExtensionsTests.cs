using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class SumReadOnlySpanExtensionsTests
  {
    [Fact]
    public void double_SumWithDefault_Empty()
    {
      ReadOnlySpan<double> src = new double[0];
      Assert.Equal(0d, src.SumWithDefault(v => v + 1));
    }
    
    [Fact]
    public void double_SumWithDefault_2Values()
    {
      ReadOnlySpan<double> src = new[] {1d, 3d,};
      Assert.Equal(-4d, src.SumWithDefault(v => -1 * v));
    }
  }
}