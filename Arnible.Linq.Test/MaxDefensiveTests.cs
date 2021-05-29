using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class MaxDefensiveTests
  {
    [Fact]
    public void IEnumerable_Max_OfThree_Defensive()
    {
      IEnumerable<double> toCheck = new[] { 4d, 2d, 3d }; 
      Assert.Equal(4d, toCheck.MaxDefensive());
    }
    
    [Fact]
    public void ReadOnlySpan_Max_OfThree_Defensive()
    {
      ReadOnlySpan<double> toCheck = new[] { 4d, 2d, 3d }; 
      Assert.Equal(4d, toCheck.MaxDefensive());
    }
    
    [Fact]
    public void Span_Max_OfThree_Defensive()
    {
      Span<double> toCheck = new[] { 4d, 2d, 3d }; 
      Assert.Equal(4d, toCheck.MaxDefensive());
    }
  }
}