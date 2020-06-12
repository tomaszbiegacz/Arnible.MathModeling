﻿using System;
using System.Diagnostics.CodeAnalysis;
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
  }
}
