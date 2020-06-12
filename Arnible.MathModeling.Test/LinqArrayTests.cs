﻿using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqArrayTests
  {
    [Fact]
    public void Indexes()
    {
      AreEquals(new[] { 0u, 1u, 2u }, (new[] { 1, 2, 3 }).Indexes());
    }

    [Fact]
    public void ToJaggedArray()
    {
      AreEquals(new int[][] { new[] { 0, 1, 2 }, new[] { 1, 2, 3 } }, new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJagged());
    }

    [Fact]
    public void ToInversedJaggedArray()
    {
      AreEquals(new int[][] { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 2, 3 } }, new int[,] { { 0, 1, 2 }, { 1, 2, 3 } }.ToArrayJaggedInversed());
    }
  }
}
