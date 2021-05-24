using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling;
using Xunit;

namespace Arnible.Linq.Algebra.Tests
{
  public class SumWithDefaultTests
  {
    [Fact]
    public void SumWithDefault_IEnumerable()
    {
      IEnumerable<Number> values = new Number[] {1, 2};
      values.SumWithDefault().AssertIsEqualTo(3);
    }
  }
}