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
      ConditionExtensions.AssertIsTrue(v == 0);
      EqualExtensions.AssertEqualTo(1u, v.Length);
      EqualExtensions.AssertEqualTo<double>(0, (double)v[0]);
      EqualExtensions.AssertEqualTo("0", v.ToString());

      EqualExtensions.AssertEqualTo(default, v);
      EqualExtensions.AssertEqualTo(default, new NumberVector());
      EqualExtensions.AssertEqualTo(default, new NumberVector(new Number[0]));
      EqualExtensions.AssertEqualTo(default, NumberVector.Repeat(value: 0, length: 5));

      EqualExtensions.AssertEqualTo<double>(0, (double)v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      EqualExtensions.AssertEqualTo<double>(2, (double)v[0]);
      EqualExtensions.AssertEqualTo(1u, v.Length);
      EqualExtensions.AssertEqualTo("2", v.ToString());

      EqualExtensions.AssertEqualTo<double>(2, (double)v.GetOrDefault(0));
      EqualExtensions.AssertEqualTo<double>(0, (double)v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      ConditionExtensions.AssertIsFalse(v == 0);
      v.GetInternalEnumerable().AssertSequenceEqual(new Number[] { 2, 3, 4 });
      EqualExtensions.AssertEqualTo(3u, v.Length);
      EqualExtensions.AssertEqualTo("[2 3 4]", v.ToString());

      NumberVector v1 = new NumberVector(1, 2, 3);
      NumberVector v2 = new NumberVector(3, 5, 7);
      EqualExtensions.AssertEqualTo(v2, v + v1);
    }
    
    [Fact]
    public void FirstNonZeroValueAt()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(0, 0, 5), NumberVector.NonZeroValueAt(pos: 2, value: 5));
    }

    [Fact]
    public void NotEqual_Values()
    {
      ConditionExtensions.AssertIsFalse(new NumberVector(1, 2) == new NumberVector(1, 3));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      ConditionExtensions.AssertIsFalse(default == new NumberVector(1));
    }

    [Fact]
    public void Equal_Zero()
    {
      EqualExtensions.AssertEqualTo(default, new NumberVector(0));
    }

    [Fact]
    public void Equal_ZeroSuffix()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(1, 0, 2), new NumberVector(1, 0, 2, 0, 0));
    }

    [Fact]
    public void Equal_Rounding()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(1, 1, 0), new NumberVector(1, 1, 8.65956056235496E-17));
    }
    
    [Fact]
    public void ToArray_Equal()
    {
      EqualExtensions.AssertEqualTo(new Number[] {1, 0, 2}, new NumberVector(1, 0, 2).ToArray(3));
    }
    
    [Fact]
    public void ToArray_Greater()
    {
      EqualExtensions.AssertEqualTo(new Number[] {1, 0, 2, 0, 0}, new NumberVector(1, 0, 2).ToArray(5));
    }

    [Fact]
    public void Transform()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      EqualExtensions.AssertEqualTo(new NumberVector(2, 4, 6), v.Transform((i, vv) => vv + i));

      EqualExtensions.AssertEqualTo(v, v.Transform((i, vv) => vv));
    }

    [Fact]
    public void TransformSimple()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(1, 0, 2), new NumberVector(1, -1, 2).Transform(v => v > 0 ? v : 0));
    }

    [Fact]
    public void CreateUniform()
    {
      NumberVector.Repeat(2, 3).GetInternalEnumerable().AssertSequenceEqual(new Number[] { 2, 2, 2 });
    }

    [Fact]
    public void CreateFromEnumerable()
    {
      IEnumerable<Number> args = new Number[] { 2, 3, 4 };
      args.ToVector().GetInternalEnumerable().AssertSequenceEqual(new Number[] { 2, 3, 4 });
    }

    [Fact]
    public void Plus()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(2, 5, 8), new NumberVector(1, 2, 3) + new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Minus()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(0, -1, -2), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Minus_Zero()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(0, -1), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 3));
    }

    [Fact]
    public void Divide()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(1, 2, 3), new NumberVector(2, 4, 6) / 2);
    }

    [Fact]
    public void Multiply()
    {
      EqualExtensions.AssertEqualTo(new NumberVector(2, 4, 6), new NumberVector(1, 2, 3) * 2);
      EqualExtensions.AssertEqualTo(new NumberVector(2, 4, 6), 2 * new NumberVector(1, 2, 3));
    }    

    [Fact]
    public void Sum_One()
    {
      var arr = new[] { new NumberVector(1, 2, 3) };
      EqualExtensions.AssertEqualTo(new NumberVector(1, 2, 3), arr.Sum());
    }

    [Fact]
    public void Sum_Two()
    {
      var arr = new[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      EqualExtensions.AssertEqualTo(new NumberVector(2, 5, 8), arr.Sum());
    }

    [Fact]
    public void Average_Two()
    {
      var arr = new[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      EqualExtensions.AssertEqualTo(new NumberVector(1, 2.5, 4), arr.Average());
    }

    [Fact]
    public void ToArray_SameSize()
    {
      var v = new NumberVector(1, 2, 3);
      v.GetInternalEnumerable().ToValueArray(3).GetInternalEnumerable().AssertSequenceEqual(new Number[] { 1d, 2, 3 });
    }

    [Fact]
    public void ToArray_GreaterSize()
    {
      var v = new NumberVector(1, 2, 3);
      v.GetInternalEnumerable().ToValueArray(4).GetInternalEnumerable().AssertSequenceEqual(new Number[] { 1d, 2, 3, 0 });
    }
  }
}
