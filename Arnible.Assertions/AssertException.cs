using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public class AssertException : Exception
  {
    public static string ToString<T>(IReadOnlyCollection<T> actual)
    {
      return "[" + string.Join(',', actual.ToArray()) + "]";
    }
    
    public static string ToString<T>(in ReadOnlySpan<T> actual)
    {
      return "[" + string.Join(',', actual.ToArray()) + "]";
    }

    public AssertException(string message)
      : base(message)
    {
      // intentionally empty
    }
    
    public AssertException(string message, string actual)
      : base(message + $".\nActual: {actual}\n")
    {
      // intentionally empty
    }
  }
}