using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Logic.Test
{
  public class BitArrayEnumerableTests
  {
    [Fact]
    public void Enumerable_3()
    {
      BitArrayEnumerable number = new BitArrayEnumerable(3);
      AreEqual(3, number.Length);

      // 0
      IsFalse(number[0]);
      IsFalse(number[1]);
      IsFalse(number[2]);

      IsTrue(number.MoveNext());
      IsTrue(number[0]);
      IsFalse(number[1]);
      IsFalse(number[2]);

      IsTrue(number.MoveNext());
      IsFalse(number[0]);
      IsTrue(number[1]);
      IsFalse(number[2]);

      IsTrue(number.MoveNext());
      IsTrue(number[0]);
      IsTrue(number[1]);
      IsFalse(number[2]);

      // 4
      IsTrue(number.MoveNext());
      IsFalse(number[0]);
      IsFalse(number[1]);
      IsTrue(number[2]);

      IsTrue(number.MoveNext());
      IsTrue(number[0]);
      IsFalse(number[1]);
      IsTrue(number[2]);

      IsTrue(number.MoveNext());
      IsFalse(number[0]);
      IsTrue(number[1]);
      IsTrue(number[2]);

      IsTrue(number.MoveNext());
      IsTrue(number[0]);
      IsTrue(number[1]);
      IsTrue(number[2]);

      IsFalse(number.MoveNext());

      IsTrue(number[0]);
      IsTrue(number[1]);
      IsTrue(number[2]);
    }

    [Fact]
    public void FromNumber_11()
    {
      BitArrayEnumerable number = BitArrayEnumerable.FromNumber(11);

      AreEqual(4, number.Length);
      IsTrue(number[0]);
      IsTrue(number[1]);
      IsFalse(number[2]);
      IsTrue(number[3]);
    }
  }
}
