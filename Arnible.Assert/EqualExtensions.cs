using System;

namespace Arnible.Assert
{
  public static class EqualExtensions
  {
    public static void AssertEqualTo<T>(this T actual, T expected) where T: IEquatable<T>
    {
      if(!actual.Equals(expected))
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
    }
    
    public static void AssertEqualTo(this ushort actual, int expected)
    {
      if(actual != expected)
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
    }
  }
}