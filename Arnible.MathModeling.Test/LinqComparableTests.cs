using System;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqComparableTests
  {
    [Fact]
    public void SequenceCompare_Equal()
    {
      AreEqual(0, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void SequenceCompare_SecondGreater()
    {
      AreEqual(1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 0, 3 }));
    }

    [Fact]
    public void SequenceCompare_ThirdLower()
    {
      AreEqual(-1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 4 }));
    }

    [Fact]
    public void Distinct_Number()
    {
      AreEquals(new Number[] { 1, 2, 3 }, (new Number[] { 1, 2, 2, 3, 2.00000000001 }).Distinct());
    }
    
    [Fact]
    public void Distinct_NumberArray()
    {
      var expected = new ValueArray<Number>[]
      {
        new Number[] {1, 2, 3},
        new Number[] {1, 2},
      };
      var withDuplicates = new ValueArray<Number>[]
      {
        new Number[] {1, 2, 3},
        new Number[] {1, 2},
        new Number[] {1.00000000001, 2},
        new Number[] {1, 2, 3.00000000001},
      };
      AreEquals(expected, withDuplicates.Distinct());
    }
  }
}
