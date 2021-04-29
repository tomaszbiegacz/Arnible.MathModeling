using System;

namespace Arnible.Assertions
{
  public static class IsNotEqualToExtensions
  {
    public static T AssertIsNotEqualTo<T>(this T actual, T expected) where T: IEquatable<T>
    {
      if(expected.Equals(actual))
      {
        throw new AssertException($"Not expected {expected}");
      }
      return actual;
    }
  }
}