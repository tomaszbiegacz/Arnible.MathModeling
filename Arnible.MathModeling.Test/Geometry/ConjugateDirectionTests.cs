using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class ConjugateDirectionTests
  {
    [Fact]
    public void Default()
    {
      ConjugateDirection v = default;
      Assert.Equal(0, v.DirectionsDimensionsCount);
      Assert.Equal(0, v.DirectionsMemorySize);
    }
    
    [Fact]
    public void OneDirection()
    {
      ConjugateDirection v = new(
        new Span2D<Number>(new Number[4], columnsCount: 2),
        new SpanSingle<ushort>(new ushort[1]),
        new SpanSingle<bool>(new bool[1]));
      
      v.AddDirection(new Number[] { 1, 2});
      v.GetConjugateDirection(new Number[2]).AssertSequenceEqualsTo(new Number[] { 1, 2 });
    }
    
    [Fact]
    public void LastTwoDirections()
    {
      ConjugateDirection v = new(
        new Span2D<Number>(new Number[4], columnsCount: 2),
        new SpanSingle<ushort>(new ushort[1]),
        new SpanSingle<bool>(new bool[1]));
      
      v.AddDirection(new Number[] { 1, 2 });
      v.AddDirection(new Number[] { 2, 4 });
      v.AddDirection(new Number[] { 4, 8 });
      v.GetConjugateDirection(new Number[2]).AssertSequenceEqualsTo(new Number[] { 6, 12 });
    }
  }
}