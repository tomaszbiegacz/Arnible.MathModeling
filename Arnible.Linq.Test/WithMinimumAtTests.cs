using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMinimumAtTests
  {
    [Fact]
    public void WithMinimumAt_OfThree_ReadOnlySpan()
    {
      ReadOnlySpan<double> v = new double[] { 4, 6, 3 };
      Assert.Equal(2, v.WithMinimumAt());
    }
    
    [Fact]
    public void WithMinimumAt_OfThree_Span()
    {
      Span<double> v = new double[] { 4, 6, 3 };
      Assert.Equal(2, v.WithMinimumAt());
    }
    
    [Fact]
    public void WithMinimumAt_OfThree_ReadOnlySpan_Error()
    {
      ReadOnlySpan<double> src = new double[0];
      try
      {
        src.WithMinimumAt();
        throw new Exception("Something went wrong");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    }
  }
}