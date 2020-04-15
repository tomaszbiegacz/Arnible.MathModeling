using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberVector v = default;
      Assert.True(v.IsZero);
      Assert.Empty(v);
#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
      Assert.Equal(0, v.Count);
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.      
      Assert.Equal("[]", v.ToString());

      Assert.Equal(default, v);
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      Assert.False(v.IsZero);
      Assert.Equal(v, new Number[] { 2, 3, 4 });
      Assert.Equal(3, v.Count);
      Assert.Equal("[2 3 4]", v.ToString());

      NumberVector v1 = new NumberVector(1, 2, 3);
      NumberVector v2 = new NumberVector(3, 5, 7);
      Assert.Equal(v2, v + v1);
    }

    [Fact]
    public void NotEqual_Values()
    {
      Assert.NotEqual(new NumberVector(1, 2), new NumberVector(1, 3));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      Assert.NotEqual(default, new NumberVector(0));
    }

    [Fact]
    public void Equal_Rounding()
    {
      Assert.Equal(new NumberVector(1, 1, 0), new NumberVector(1, 1, 8.65956056235496E-17));
    }

    [Fact]
    public void Transform()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      Assert.Equal(new NumberVector(2, 4, 6), v.Transform((i, vv) => vv + i));

      Assert.Equal(v, v.Transform((i, vv) => vv));
    }

    [Fact]
    public void TransformSimple()
    {
      Assert.Equal(new NumberVector(1, 0, 2), new NumberVector(1, -1, 2).Transform(v => v > 0 ? v : 0));
    }

    [Fact]
    public void Reverse()
    {
      Assert.Equal(new NumberVector(3, 2, 1), new NumberVector(1, 2, 3).Reverse());
    }

    [Fact]
    public void CreateUniform()
    {
      Assert.Equal(new Number[] { 2, 2, 2 }, NumberVector.Repeat(2, 3));
    }

    [Fact]
    public void CreateFromEnumerable()
    {
      IEnumerable<Number> args = new Number[] { 2, 3, 4 };
      Assert.Equal(new Number[] { 2, 3, 4 }, args.ToVector());
    }

    [Fact]
    public void Plus()
    {
      Assert.Equal(new NumberVector(2, 5, 8), new NumberVector(1, 2, 3) + new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Minus()
    {
      Assert.Equal(new NumberVector(0, -1, -2), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Divide()
    {
      Assert.Equal(new NumberVector(1, 2, 3), new NumberVector(2, 4, 6) / 2);
    }

    [Fact]
    public void Multiply()
    {
      Assert.Equal(new NumberVector(2, 4, 6), new NumberVector(1, 2, 3) * 2);
      Assert.Equal(new NumberVector(2, 4, 6), 2 * new NumberVector(1, 2, 3));
    }

    [Fact]
    public void Indexes()
    {
      Assert.Equal(Enumerable.Range(0, 3).Select(i => (uint)i), new NumberVector(1, 2, 3).Indexes());
    }

    [Fact]
    public void Sum_One()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3) };
      Assert.Equal(new NumberVector(1, 2, 3), arr.Sum());
    }

    [Fact]
    public void Sum_Two()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      Assert.Equal(new NumberVector(2, 5, 8), arr.Sum());
    }

    [Fact]
    public void Average_Two()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      Assert.Equal(new NumberVector(1, 2.5, 4), arr.Average());
    }
  }
}
