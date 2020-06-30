using System;

namespace Arnible.MathModeling.Algebra
{
  public static class ISignArrayExtensions
  {
    public static bool HasSign(this IArray<Sign> array, uint index)
    {
      return array[index] != Sign.None;
    }

    public static int SignFactory(this IArray<Sign> array, uint index)
    {
      switch (array[index])
      {
        case Sign.None:
          return 0;
        case Sign.Negative:
          return -1;
        case Sign.Positive:
          return 1;
      }

      throw new InvalidOperationException("Something went wrong.");
    }
  }
}
