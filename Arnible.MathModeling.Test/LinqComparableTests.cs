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

    readonly struct ForOrdering : IEquatable<ForOrdering>, IValueObject
    {
      public int Root { get; }
      public int Reminder { get; }

      public ForOrdering(int v)
      {
        Root = v / 2;
        Reminder = v % 2;
      }

      public bool Equals(ForOrdering other)
      {
        return Root == other.Root && Reminder == other.Reminder;
      }

      public override string ToString()
      {
        return $"{Root.ToString()} {Reminder.ToString()}";
      }
      public string ToStringValue() => ToString();

      public override int GetHashCode()
      {
        return Root.GetHashCode() ^ Reminder.GetHashCode();
      }
      public int GetHashCodeValue() => GetHashCode();
    }

    [Fact]
    public void OrderByDescending_Default()
    {
      AreEquals(
        new[] { new ForOrdering(3), new ForOrdering(2), new ForOrdering(1) }, 
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root, i => i.Reminder));
    }

    [Fact]
    public void OrderByDescending_WithThen()
    {
      AreEquals(
        new[] { new ForOrdering(2), new ForOrdering(3), new ForOrdering(1) },
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root).ThenOrderBy(i => i.Reminder));
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
