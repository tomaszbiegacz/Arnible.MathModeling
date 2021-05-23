using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling;
using Xunit;

namespace Arnible.Linq.Algebra.Tests
{
  public class SumDefensiveTests
  {
    [Fact]
    public void SumDefensive_IEnumerable()
    {
      IEnumerable<Number> values = new Number[] {1, 2};
      values.SumDefensive().AssertIsEqualTo(3);
    }
    
    [Fact]
    public void SumDefensive_ReadOnlySpan()
    {
      ReadOnlySpan<Number> values = new Number[] {1, 2};
      values.SumDefensive().AssertIsEqualTo(3);
    }
    
    [Fact]
    public void SumDefensive_ReadOnlySpan_Func()
    {
      ReadOnlySpan<Number> values = new Number[] {1, 2};
      values.SumDefensive((in Number v) => 2*v).AssertIsEqualTo(6);
    }
  }
}