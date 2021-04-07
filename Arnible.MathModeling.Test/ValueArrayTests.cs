using Arnible.Linq;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

namespace Arnible.MathModeling.Test
{
  public class ValueArrayTests
  {
    [Fact]
    public void Constructor_Default()
    {
      ValueArray<Number> v = default;
      AreEqual(0u, v.Length);
      AreEqual("[]", v.ToString());      

      AreEqual(default, v);
      AreEqual(default, new ValueArray<Number>());
      AreEqual(default, new ValueArray<Number>(new Number[0]));      
    }

    [Fact]
    public void Constructor_Explicit()
    {
      ValueArray<Number> v = new Number[] { 2, 3, 4 };
      AreEquals(new Number[] { 2, 3, 4 }, v);
      AreEqual(3u, v.Length);
      AreExactlyEqual(3, v[1]);
      AreEqual("[2 3 4]", v.ToString());
    }

    [Fact]
    public void ToArray_All()
    {
      AreEqual(new ValueArray<Number>(new Number[] { 1, 2, 3 }), new Number[] { 1d, 2d, 3d }.ToArray());
    }

    [Fact]
    public void ToArray_WithDefaults()
    {
      AreEqual(new Number[] { 1, 2, 3, 0, 0 }.ToArray(), new Number[] { 1d, 2d, 3d }.ToValueArray(5));
    }

    [Fact]
    public void DistanceSquareTo()
    {
      ValueArray<Number> a1 = new Number[] { 1, 2, 3 }.ToArray();
      ValueArray<Number> a2 = new Number[] { 2, 1, 1 }.ToArray();
      AreEqual(6d, a1.DistanceSquareTo(a2));
    }

    [Fact]
    public void SumDefensive()
    {
      ValueArray<Number> v1 = new Number[] {1, 2};
      ValueArray<Number> v2 = new Number[] {3, 5};
      ValueArray<Number> expected = new Number[] {4, 7};

      AreEqual(expected, new[] {v1, v2}.SumDefensive());
    }
    
    [Fact]
    public void SumDefensiveToScalar()
    {
      ValueArray<Number> v1 = new Number[] {1, 2};
      Number expected = 3;

      AreEqual(expected, v1.SumDefensive());
    }
    
    [Fact]
    public void Multiply()
    {
      ValueArray<Number> v1 = new Number[] {1, 2};
      Number v2 = 3;
      ValueArray<Number> expected = new Number[] {3, 6};

      AreEqual(expected, v1.Multiply(in v2));
    }
  }
}
