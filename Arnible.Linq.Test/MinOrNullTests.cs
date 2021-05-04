using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class MinOrNullTests
  {
    [Fact]
    public void MinOrNone_OfThree_OrDefault_Nullable()
    {
      Assert.Equal(2d, (new double?[] { 4d, null, 2d }).MinOrNone());
    }
    
    [Fact]
    public void MinOrNone_OfNull_OrDefault_Nullable()
    {
      Assert.False((new double?[] { null, null, null }).MinOrNone().HasValue);
    }
    
    [Fact]
    public void MinOrNone_OfThree_OrDefault()
    {
      Assert.Equal(2d, (new double[] { 4d, 2d }).MinOrNone());
    }
    
    [Fact]
    public void MinOrNone_Empty()
    {
      Assert.False((new double[0]).MinOrNone().HasValue);
    }
    
    [Fact]
    public void MinOrNull_Empty()
    {
      Assert.Null(new string[0].MinOrNull());
    }
    
    [Fact]
    public void MinOrNull_Value()
    {
      Assert.Equal("a", new string[] { "c", "b", "a" }.MinOrNull() ?? throw new Exception("null"));
    }
  }
}