using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assert
{
  public class AssertException : Exception
  {
    public static string ToString<T>(IReadOnlyCollection<T> actual)
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