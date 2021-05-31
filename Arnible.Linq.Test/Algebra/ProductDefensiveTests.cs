using System;
using Xunit;

namespace Arnible.Linq.Algebra.Test
{
  public class ProductDefensiveTests
  {
    [Fact]
    public void Product_OfThree_Defensive()
    {
      Assert.Equal(24d, (new[] { 4d, 2d, 3d }).ProductDefensive());
    }

    [Fact]
    public void Product_OfThree_Default()
    {
      Assert.Equal(24d, (new[] { 4d, 2d, 3d }).ProductWithDefault());
    }

    [Fact]
    public void Product_Default()
    {
      Assert.Equal(1d, LinqArray<double>.Empty.ProductWithDefault());
    }
    
    [Fact]
    public void Product_Default_Defensive()
    {
      try
      {
        LinqArray<double>.Empty.ProductDefensive();
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    }
  }
}