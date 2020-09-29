using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.xunit;
using Xunit;

namespace Arnible.MathModeling.Test.xunit
{
  public class AssertNumericTests
  {
    [Fact]
    public void AssertMinimum_Single_Success()
    {
      AssertNumeric.AssertMinimum(
        func: point => (point[0] - 1).ToPower(2), 
        point: new Number[] { 1, 1 },
        distance: 0.5, 
        valueDomain: new NumberDomain());
    }
    
    [Fact]
    public void AssertMinimum_Failure_Saddle()
    {
      AssertNumber.Throws<InvalidOperationException>(() => AssertNumeric.AssertMinimum(
        func: point => (point[0] - 1) * (point[1] + 3), 
        point: new Number[] { 1, -3 },  
        distance: 0.5, 
        valueDomain: new NumberDomain()));
    }
    
    [Fact]
    public void AssertMinimum_Multiple_Success()
    {
      AssertNumeric.AssertMinimum(
        func: point => (point[0] - 1).ToPower(2), 
        point: new Number[] { 1, 1 },
        valueDomain: new NumberDomain(),
        distance: 0.5, 
        iterationsCount: 3);
    }
  }
}