using System;
using System.Runtime.CompilerServices;

namespace Arnible.Assertions
{
  public static class IsEqualToExtensions
  {
    public static T AssertIsEqualTo<T>(this T actual, in T expected) where T: IEquatable<T>
    {
      if(!expected.Equals(actual))
      {
        throw new AssertException($"Expected \n{expected}\n got \n{actual}");
      }
      return actual;
    }
    
    public static T AssertIsEqualToEnum<T>(this T actual, T expected) where T: Enum
    {
      int actualValue = Unsafe.As<T, int>(ref actual);
      int expectedValue = Unsafe.As<T, int>(ref expected);
      if(actualValue != expectedValue)
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
      return actual;
    }
    
    public static T? AssertIsEqualTo<T>(in this T? actual, in T expected) where T: struct, IEquatable<T>
    {
       if(!actual.HasValue)
       {
         throw new AssertException($"Expected \n{expected}\n got \n{actual}");
       }
       else if(!expected.Equals(actual.Value))
       {
         throw new AssertException($"Expected \n{expected}\n got \n{actual}");
       }
       return actual;
    }
    
    public static void AssertIsEqualTo(this ushort actual, int expected)
    {
      if(actual != expected)
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
    }
    
    public static void AssertIsEqualTo(this byte actual, int expected)
    {
      if(actual != expected)
      {
        throw new AssertException($"Expected {expected} got {actual}");
      }
    }
  }
}