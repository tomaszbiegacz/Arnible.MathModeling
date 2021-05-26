using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test.Assertions
{
  public class IsInFuzzyLogicRangeNotSharpTests
  {
    [Theory]
    [InlineData(0.5)]
    public void Ok(double value)
    {
      Number v = value;
      v.AssertIsInFuzzyLogicRangeNotSharp();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1.5)]
    [InlineData(1)]
    public void NotOk(double value)
    {
      Number v = value;
      try
      {
        v.AssertIsInFuzzyLogicRangeNotSharp();
        throw new Exception("Something is not ok");
      }
      catch (AssertException)
      {
        // all is OK
      }
    }
  }
}