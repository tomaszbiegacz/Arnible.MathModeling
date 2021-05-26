using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test.Assertions
{
  public class IsInFuzzyLogicRangeTests
  {
    [Theory]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(1)]
    public void Ok(double value)
    {
      Number v = value;
      v.AssertIsInFuzzyLogicRange();
    }
    
    [Fact]
    public void NotOk()
    {
      Number v = 1.9;
      try
      {
        v.AssertIsInFuzzyLogicRange();
        throw new Exception("Something is not ok");
      }
      catch (AssertException)
      {
        // all is OK
      }
    }
  }
}