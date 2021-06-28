using System;
using Arnible.Assertions;
using Arnible.MathModeling;
using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.Linq.Algebra.Tests
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
      ReadOnlySpan<Number> src = new Number[] { 1, 2 };
      ReadOnlySpan<Number> value = new Number[] { 3, 4 };
      Span<Number> output = new Number[2];
      src.Add(in value, in output);
      output.AssertSequenceEqualsTo(new Number[] {4, 6});
    }
    
    [Fact]
    public void Add_Span()
    {
      Span<Number> output = new Number[] { 1, 2 };
      ReadOnlySpan<Number> value = new Number[] { 3, 4 };
      output.AddToSelf(in value);
      output.AssertSequenceEqualsTo(new Number[] {4, 6});
    }
  }
}