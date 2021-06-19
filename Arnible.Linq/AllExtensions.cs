using System;

namespace Arnible.Linq
{
  public static class AllExtensions
  {
    public static bool All(in this ReadOnlySpan<bool> source)
    {
      foreach (bool item in source)
      {
        if (!item)
        {
          return false;
        }
      }
      return true;
    }

    public static bool All(in this Span<bool> source)
    {
      return All((ReadOnlySpan<bool>)source);
    }
  }
}