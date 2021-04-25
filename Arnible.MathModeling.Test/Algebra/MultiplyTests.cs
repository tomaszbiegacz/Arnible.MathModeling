using System;
using Arnible.Assertions;
using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.MathModeling.Test.Algebra
{
  public class MultiplyTests
  {
    [Fact]
    public void Multiply_IReadOnlyCollection()
    {
      new Number[] {1, 2}.Multiply(2).AssertSequenceEqualsTo(new Number[] {2, 4});
    }
    
    [Fact]
    public void Multiply_ReadOnlySpan()
    {
      ReadOnlySpan<Number> src = stackalloc Number[] { 1, 2 };
      Span<Number> output = stackalloc Number[2];
      src.Multiply(2, in output);
      output.AssertSequenceEqualsTo(new Number[] {2, 4});
    }
    
    [Fact]
    public void Multiply_Span()
    {
      Span<Number> output = stackalloc Number[] { 1, 2 };
      output.Multiply(2);
      output.AssertSequenceEqualsTo(new Number[] {2, 4});
    }
  }
}