using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Xunit;

namespace Arnible.Linq.Test
{
  public class MinDefensiveTests
  {
    [Fact]
    public void IEnumerable_Min_Defensive_Error()
    {
      IEnumerable<int> v = new int[0];
      try
      {
        v.MinDefensive();
        throw new Exception("We should not get here");
      }
      catch(AssertException)
      {
        // all is OK
      }
    }
    
    [Fact]
    public void IEnumerable_Min_OfThree_Defensive()
    {
      IEnumerable<double> v = new[] { 4d, 2d, 3d }; 
      Assert.Equal(2d, v.MinDefensive());
    }
    
    [Fact]
    public void ReadOnlySpan_Min_Defensive_Error()
    {
      ReadOnlySpan<int> v = new int[0];
      try
      {
        v.MinDefensive();
        throw new Exception("We should not get here");
      }
      catch(AssertException)
      {
        // all is OK
      }
    }
    
    [Fact]
    public void ReadOnlySpan_Min_OfThree_Defensive()
    {
      ReadOnlySpan<double> v = new[] { 4d, 2d, 3d }; 
      Assert.Equal(2d, v.MinDefensive());
    }
    
    [Fact]
    public void Span_Min_OfThree_Defensive()
    {
      Span<double> v = new[] { 4d, 2d, 3d }; 
      Assert.Equal(2d, v.MinDefensive());
    }
  }
}