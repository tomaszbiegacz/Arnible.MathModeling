using System;
using Arnible.MathModeling;

namespace Arnible.Linq
{
  public static class IsZeroExtensions
  {
    public static bool IsZero(in this Span<Number> src)
    {
      foreach (ref readonly Number item in src)
      {
        if (item != 0)
        {
          return false;
        }
      }
      return true;
    }
  }
}