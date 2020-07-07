using System.Collections.Generic;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberVector v = default;
      IsTrue(v == 0);
      AreEqual(1u, v.Length);
      AreExactlyEqual(0, v[0]);
      AreEqual("0", v.ToString());

      AreEqual(default, v);
      AreEqual(default, new NumberVector());
      AreEqual(default, new NumberVector(new Number[0]));
      AreEqual(default, NumberVector.Repeat(value: 0, length: 5));

      AreExactlyEqual(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberVector v = 2;
      IsFalse(v == 0);
      IsTrue(v == 2);
      IsFalse(v != 2);
      AreExactlyEqual(2, v[0]);
      AreEqual(1u, v.Length);
      AreEqual("2", v.ToString());

      AreExactlyEqual(2, v.GetOrDefault(0));
      AreExactlyEqual(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      IsFalse(v == 0);
      AreEquals(v, new Number[] { 2, 3, 4 });
      AreEqual(3u, v.Length);
      AreEqual("[2 3 4]", v.ToString());

      NumberVector v1 = new NumberVector(1, 2, 3);
      NumberVector v2 = new NumberVector(3, 5, 7);
      AreEqual(v2, v + v1);
    }
    
    [Fact]
    public void FirstNonZeroValueAt()
    {
      AreEqual(new NumberVector(0, 0, 5), NumberVector.NonZeroValueAt(pos: 2, value: 5));
    }

    [Fact]
    public void NotEqual_Values()
    {
      AreNotEqual(new NumberVector(1, 2), new NumberVector(1, 3));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      AreNotEqual(default, new NumberVector(1));
    }

    [Fact]
    public void Equal_Zero()
    {
      AreEqual(default, new NumberVector(0));
    }

    [Fact]
    public void Equal_ZeroSuffix()
    {
      AreEqual(new NumberVector(1, 0, 2), new NumberVector(1, 0, 2, 0, 0));
    }

    [Fact]
    public void Equal_Rounding()
    {
      AreEqual(new NumberVector(1, 1, 0), new NumberVector(1, 1, 8.65956056235496E-17));
    }

    [Fact]
    public void Transform()
    {
      NumberVector v = new NumberVector(2, 3, 4);
      AreEqual(new NumberVector(2, 4, 6), v.Transform((i, vv) => vv + i));

      AreEqual(v, v.Transform((i, vv) => vv));
    }

    [Fact]
    public void TransformSimple()
    {
      AreEqual(new NumberVector(1, 0, 2), new NumberVector(1, -1, 2).Transform(v => v > 0 ? v : 0));
    }

    [Fact]
    public void CreateUniform()
    {
      AreEquals(new Number[] { 2, 2, 2 }, NumberVector.Repeat(2, 3));
    }

    [Fact]
    public void CreateFromEnumerable()
    {
      IEnumerable<Number> args = new Number[] { 2, 3, 4 };
      AreEquals(new Number[] { 2, 3, 4 }, args.ToVector());
    }

    [Fact]
    public void Plus()
    {
      AreEqual(new NumberVector(2, 5, 8), new NumberVector(1, 2, 3) + new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Minus()
    {
      AreEqual(new NumberVector(0, -1, -2), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 5));
    }

    [Fact]
    public void Minus_Zero()
    {
      AreEqual(new NumberVector(0, -1), new NumberVector(1, 2, 3) - new NumberVector(1, 3, 3));
    }

    [Fact]
    public void Divide()
    {
      AreEqual(new NumberVector(1, 2, 3), new NumberVector(2, 4, 6) / 2);
    }

    [Fact]
    public void Multiply()
    {
      AreEqual(new NumberVector(2, 4, 6), new NumberVector(1, 2, 3) * 2);
      AreEqual(new NumberVector(2, 4, 6), 2 * new NumberVector(1, 2, 3));
    }    

    [Fact]
    public void Sum_One()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3) };
      AreEqual(new NumberVector(1, 2, 3), arr.Sum());
    }

    [Fact]
    public void Sum_Two()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      AreEqual(new NumberVector(2, 5, 8), arr.Sum());
    }

    [Fact]
    public void Average_Two()
    {
      var arr = new NumberVector[] { new NumberVector(1, 2, 3), new NumberVector(1, 3, 5) };
      AreEqual(new NumberVector(1, 2.5, 4), arr.Average());
    }

    [Fact]
    public void ToArray_SameSize()
    {
      var v = new NumberVector(1, 2, 3);
      AreEquals(new Number[] { 1d, 2, 3 }, v.ToValueArray(3));
    }

    [Fact]
    public void ToArray_GreaterSize()
    {
      var v = new NumberVector(1, 2, 3);
      AreEquals(new Number[] { 1d, 2, 3, 0 }, v.ToValueArray(4));
    }
  }
}
