using System;
using Arnible.Assertions;
using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.MathModeling.Test.Algebra
{
  public class AddTests
  {
    [Fact]
    public void Add_IReadOnlyCollection()
    {
      new Number[] {1, 2}.Add(new Number[] { 3, 4 }).AssertSequenceEqualsTo(new Number[] {4, 6});
    }
    
    [Fact]
    public void Add_ReadOnlySpan()
    {
      ReadOnlySpan<Number> src = stackalloc Number[] { 1, 2 };
      ReadOnlySpan<Number> value = stackalloc Number[] { 3, 4 };
      Span<Number> output = stackalloc Number[2];
      src.Add(in value, in output);
      output.AssertSequenceEqualsTo(new Number[] {4, 6});
    }
    
    [Fact]
    public void Add_Span()
    {
      Span<Number> output = stackalloc Number[] { 1, 2 };
      ReadOnlySpan<Number> value = stackalloc Number[] { 3, 4 };
      output.Add(in value);
      output.AssertSequenceEqualsTo(new Number[] {4, 6});
    }
  }
}