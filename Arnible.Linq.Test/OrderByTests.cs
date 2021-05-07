using System;
using Xunit;

namespace Arnible.Linq.Test
{
  public class OrderByTests
  {
    readonly struct ForOrdering : IEquatable<ForOrdering>
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
    public void IEnumerable_Order_Default()
    {
      AssertExtensions.AreEquals(
        new[] { 1, 2, 3 }, 
        (new[] { 3, 1, 2 }).Order());
    }
    
    [Fact]
    public void IEnumerable_OrderDescending_Default()
    {
      AssertExtensions.AreEquals(
        new[] { 3, 2, 1 }, 
        (new[] { 3, 1, 2 }).OrderDescending());
    }
    
    [Fact]
    public void IEnumerable_OrderByDescending_Default()
    {
      AssertExtensions.AreEquals(
        new[] { new ForOrdering(3), new ForOrdering(2), new ForOrdering(1) }, 
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root, i => i.Reminder));
    }

    [Fact]
    public void IEnumerable_OrderByDescending_WithThen()
    {
      AssertExtensions.AreEquals(
        new[] { new ForOrdering(2), new ForOrdering(3), new ForOrdering(1) },
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root).ThenOrderBy(i => i.Reminder));
    }
    
    [Fact]
    public void IEnumerable_OrderByDescending_WithThenDescending()
    {
      AssertExtensions.AreEquals(
        new[] { new ForOrdering(3), new ForOrdering(2), new ForOrdering(1) },
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root).ThenOrderByDescending(i => i.Reminder));
    }
    
    [Fact]
    public void IEnumerable_OrderBy_Default()
    {
      AssertExtensions.AreEquals(
        new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }, 
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderBy(i => i.Root, i => i.Reminder));
    }
  }
}