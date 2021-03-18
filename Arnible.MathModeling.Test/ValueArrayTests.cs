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
    public void Indexes()
    {
      AreEquals(LinqEnumerable.RangeUint(0, 3), new Number[] { 1, 2, 3 }.ToValueArray().Indexes());
    }

    [Fact]
    public void ToArray_All()
    {
      AreEqual(new ValueArray<Number>(new Number[] { 1, 2, 3 }), new Number[] { 1d, 2d, 3d }.ToValueArray());
    }

    [Fact]
    public void ToArray_WithDefaults()
    {
      AreEqual(new Number[] { 1, 2, 3, 0, 0 }.ToValueArray(), new Number[] { 1d, 2d, 3d }.ToValueArray(5));
    }

    [Fact]
    public void IndexesWhere()
    {
      AreEquals(new[] { 0u, 2u }, new Number[] { 1, 2, 3 }.ToValueArray().IndexesWhere(v => v != 2));
    }

    [Fact]
    public void DistanceSquareTo()
    {
      var a1 = new Number[] { 1, 2, 3 }.ToValueArray();
      var a2 = new Number[] { 2, 1, 1 }.ToValueArray();
      AreEqual(6d, a1.DistanceSquareTo(a2));
    }

    [Fact]
    public void SubsetFromIndexes()
    {
      AreEquals(new Number[] { 1, 3 }, new Number[] { 1, 2, 3 }.ToValueArray().SubsetFromIndexes(new[] { 0u, 2u }));
    }
    
    [Fact]
    public void Add()
    {
      AreEquals(new Number[] { 2, 5 }, new Number[] { 1, 2 }.ToValueArray().Add(new Number[] { 1, 3 }));
    }
    
    [Fact]
    public void Substract()
    {
      AreEquals(new Number[] { 0, -1 }, new Number[] { 1, 2 }.ToValueArray().Substract(new Number[] { 1, 3 }));
    }
    
    [Fact]
    public void AddAtPos()
    {
      AreEquals(new Number[] { 1, 5 }, new Number[] { 1, 2 }.ToValueArray().AddAtPos(1, 3));
    }
    
    [Fact]
    public void SetAtPos()
    {
      AreEquals(new Number[] { 1, 3 }, new Number[] { 1, 2 }.ToValueArray().SetAtPos(1, 3));
    }
    
    [Fact]
    public void TranslateCoordinate()
    {
      AreEquals(new Number[] { 3, 13 }, new Number[] { 1, 3 }.ToValueArray().TranslateCoordinate(
        new Number[] { 1, 5 }, 
        2));
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
