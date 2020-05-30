using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.xunit
{
  static class AssertNumberCommon
  {
    public static void EqualExact(double expected, double actual)
    {
      Assert.Equal(expected, actual);
    }

    public static void NotEqualExact(double expected, double actual)
    {
      Assert.NotEqual(expected, actual);
    }


    public static void Equal(IEnumerable<Number> expected, IEnumerable<Number> actual)
    {
      var expectedArray = expected.ToArray();
      var actualArray = actual.ToArray();
      Assert.Equal(expectedArray.Length, actualArray.Length);
      for (int i = 0; i < expectedArray.Length; ++i)
      {
        AssertNumber.Equal(expectedArray[i], actualArray[i]);
      }
    }

    public static void Equal(IEnumerable<int> expected, IEnumerable<Number> actual)
    {
      Equal(expected.Select(e => (Number)e), actual);
    }

    public static void Equal(IEnumerable<double> expected, IEnumerable<Number> actual)
    {
      Equal(expected.Select(e => (Number)e), actual);
    }
  }
}
