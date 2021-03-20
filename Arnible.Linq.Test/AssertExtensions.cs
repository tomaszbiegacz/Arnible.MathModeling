using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public static class AssertExtensions
  {
    public static void AreEquals<T>(this IEnumerable<T> expected, IEnumerable<T> actual)  where T: IEquatable<T>
    {
      Assert.True(expected.SequenceEqual(actual));
    }
  }
}