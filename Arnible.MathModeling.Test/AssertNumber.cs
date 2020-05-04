using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public static class AssertNumber
  {
    public const int Precision = 10;    

    public static void Equal(Number expected, Number actual)
    {
      Assert.Equal(expected, actual, Precision);
    }

    public static void Equal(IEnumerable<Number> expected, IEnumerable<Number> actual)
    {
      var expectedArray = expected.ToArray();
      var actualArray = actual.ToArray();
      Assert.Equal(expectedArray.Length, actualArray.Length);
      for (int i = 0; i < expectedArray.Length; ++i)
      {
        Equal(expectedArray[i], actualArray[i]);
      }
    }

    public static void Equal(int[] expected, IEnumerable<Number> actual) => Equal(expected.Select(e => (Number)e), actual);
    public static void Equal(double[] expected, IEnumerable<Number> actual) => Equal(expected.Select(e => (Number)e), actual);
  }
}
