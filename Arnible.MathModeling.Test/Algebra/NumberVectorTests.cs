using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberVector v = default;
      Assert.True(v == 0);
      Assert.Equal(1u, v.Length);      
      Assert.Equal(0, v[0]);
      Assert.Equal("0", v.ToString());

      Assert.Equal(default, v);
      Assert.Equal(default, new NumberVector());
      Assert.Equal(default, new NumberVector(new Number[0]));
      Assert.Equal(default, NumberVector.Repeat(value: 0, length: 5));
      Assert.Equal(default, NumberVector.FirstNonZeroValueAt(pos: 5, value: 0));
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberVector v = 2;
      Assert.False(v == 0);
      Assert.True(v == 2);
      Assert.False(v != 2);
      Assert.Equal(2, v[0]);
      Assert.Equal(1u, v.Length);
      Assert.Equal("2", v.ToString());      
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      Assert.False(v == 0);
      Assert.Equal(v, new Number[] { 2, 3, 4 });
      Assert.Equal(3u, v.Length);
      Assert.Equal("[2 3 4]", v.ToString());

      NumberVector v1 = new NumberVector(1, 2, 3);
      NumberVector v2 = new NumberVector(3, 5, 7);
      Assert.Equal(v2, v + v1);
    }
    
    [Fact]
    public void FirstNonZeroValueAt()
    {
      Assert.Equal(new NumberVector(0, 0, 5), NumberVector.FirstNonZeroValueAt(pos: 2, value: 5));
    }

    [Fact]
    public void NotEqual_Values()
    {
      Assert.NotEqual(new NumberVector(1, 2), new NumberVector(1, 3));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      Assert.NotEqual(default, new NumberVector(1));
    }

    [Fact]
    public void Equal_Zero()
    {
      Assert.Equal(default, new NumberVector(0));
    }

    [Fact]
    public void Equal_ZeroSuffix()
    {
      Assert.Equal(new NumberVector(1, 0, 2), new NumberVector(1, 0, 2, 0, 0));
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
    public void Minus_Zero()
    {
      Assert.Equal(new NumberVector(0, -1), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 3));
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
