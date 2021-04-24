using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling.Geometry;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberVector v = default;
      (v == 0).AssertIsTrue();
      1u.AssertIsEqualTo(v.Length);
      v[0].AssertIsEqualTo(0);
      v.ToString().AssertIsEqualTo("0");

      v.AssertIsEqualTo(default);
      (new NumberVector()).AssertIsEqualTo(default);
      (new NumberVector(new Number[0])).AssertIsEqualTo(default);
      (NumberVector.Repeat(value: 0, length: 5)).AssertIsEqualTo(default);

      v.GetOrDefault(1).AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberVector v = 2;
      (v == 0).AssertIsFalse();
      (v == 2).AssertIsTrue();
      (v != 2).AssertIsFalse();
      v[0].AssertIsEqualTo(2);
      1u.AssertIsEqualTo(v.Length);
      v.ToString().AssertIsEqualTo("2");

      v.GetOrDefault(0).AssertIsEqualTo(2);
      v.GetOrDefault(1).AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      (v == 0).AssertIsFalse();
      v.GetInternalEnumerable().AssertSequenceEqualsTo(new Number[] { 2, 3, 4 });
      3u.AssertIsEqualTo(v.Length);
      v.ToString().AssertIsEqualTo("[2 3 4]");

      NumberVector v1 = new NumberVector(1, 2, 3);
      NumberVector v2 = new NumberVector(3, 5, 7);
      v2.AssertIsEqualTo(v + v1);
    }
    
    [Fact]
    public void FirstNonZeroValueAt()
    {
      NumberVector.NonZeroValueAt(pos: 2, value: 5).AssertIsEqualTo(new NumberVector(0, 0, 5));
    }

    [Fact]
    public void NotEqual_Values()
    {
      (new NumberVector(1, 2) == new NumberVector(1, 3)).AssertIsFalse();
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      (default == new NumberVector(1)).AssertIsFalse();
    }

    [Fact]
    public void Equal_Zero()
    {
      new NumberVector(0).AssertIsEqualTo(default);
    }

    [Fact]
    public void Equal_ZeroSuffix()
    {
      new NumberVector(1, 0, 2, 0, 0).AssertIsEqualTo(new NumberVector(1, 0, 2));
    }

    [Fact]
    public void Equal_Rounding()
    {
      new NumberVector(1, 1, 8.65956056235496E-17).AssertIsEqualTo(new NumberVector(1, 1, 0));
    }
    
    [Fact]
    public void ToArray_Equal()
    {
      new NumberVector(1, 0, 2).ToArray(3).AssertIsEqualTo(new Number[] {1, 0, 2});
    }
    
    [Fact]
    public void ToArray_Greater()
    {
      new NumberVector(1, 0, 2).ToArray(5).AssertIsEqualTo(new Number[] {1, 0, 2, 0, 0});
    }

    [Fact]
    public void Transform()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      v.Transform((i, vv) => vv + i).AssertIsEqualTo(new NumberVector(2, 4, 6));
      v.AssertIsEqualTo(v.Transform((i, vv) => vv));
    }

    [Fact]
    public void TransformSimple()
    {
      new NumberVector(1, -1, 2).Transform(v => v > 0 ? v : 0).AssertIsEqualTo(new NumberVector(1, 0, 2));
    }

    [Fact]
    public void CreateUniform()
    {
      NumberVector.Repeat(2, 3).GetInternalEnumerable().AssertSequenceEqualsTo(new Number[] { 2, 2, 2 });
    }

    [Fact]
    public void CreateFromEnumerable()
    {
      IEnumerable<Number> args = new Number[] { 2, 3, 4 };
      args.ToVector().GetInternalEnumerable().AssertSequenceEqualsTo(new Number[] { 2, 3, 4 });
    }

    [Fact]
    public void Plus()
    {
      (new NumberVector(1, 2, 3) + new NumberVector(1, 3, 5)).AssertIsEqualTo(new NumberVector(2, 5, 8));
    }

    [Fact]
    public void Minus()
    {
      (new NumberVector(1, 2, 3) - new NumberVector(1, 3, 5)).AssertIsEqualTo(new NumberVector(0, -1, -2));
    }

    [Fact]
    public void Minus_Zero()
    {
      (new NumberVector(1, 2, 3) - new NumberVector(1, 3, 3)).AssertIsEqualTo(new NumberVector(0, -1));
    }

    [Fact]
    public void Divide()
    {
      (new NumberVector(2, 4, 6) / 2).AssertIsEqualTo(new NumberVector(1, 2, 3));
    }

    [Fact]
    public void Multiply()
    {
      (new NumberVector(1, 2, 3) * 2).AssertIsEqualTo(new NumberVector(2, 4, 6));
      (2 * new NumberVector(1, 2, 3)).AssertIsEqualTo(new NumberVector(2, 4, 6));
    }    

    [Fact]
    public void Sum_One()
    {
      var arr = new[] { new NumberVector(1, 2, 3) };
      arr.Sum().AssertIsEqualTo(new NumberVector(1, 2, 3));
    }

    [Fact]
    public void Sum_Two()
    {
      var arr = new[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      arr.Sum().AssertIsEqualTo(new NumberVector(2, 5, 8));
    }

    [Fact]
    public void Average_Two()
    {
      var arr = new[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      arr.Average().AssertIsEqualTo(new NumberVector(1, 2.5, 4));
    }

    [Fact]
    public void ToArray_SameSize()
    {
      var v = new NumberVector(1, 2, 3);
      v.ToArray(3).AssertSequenceEqualsTo(new Number[] { 1d, 2, 3 });
    }

    [Fact]
    public void ToArray_GreaterSize()
    {
      var v = new NumberVector(1, 2, 3);
      v.ToArray(4).AssertSequenceEqualsTo(new Number[] { 1d, 2, 3, 0 });
    }
  }
}
