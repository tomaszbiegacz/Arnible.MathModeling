using System;

namespace Arnible.Assertions
{
  public static class EqualExtensions
  {
    public static void AssertEqualTo<T>(this T actual, T expected) where T: IEquatable<T>
    {
      if(!expected.Equals(actual))
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
    }
    
    public static void AssertEqualTo<T>(this T? actual, T expected) where T: struct, IEquatable<T>
    {
       if(!actual.HasValue)
       {
         throw new AssertException($"Expected {expected} got {actual}");
       }
       else if(!expected.Equals(actual.Value))
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