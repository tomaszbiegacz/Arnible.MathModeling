using System;

namespace Arnible.Assertions
{
  public static class LengthExtensions
  {
    public static void AssertLength<T>(in this ReadOnlySpan<T> src, ushort length)
    {
      if(src.Length != length)
      {
        throw new AssertException(
          $"Expected length {length} got {src.Length}",
          AssertException.ToString(src.ToArray()));
      }
    }
  }
}