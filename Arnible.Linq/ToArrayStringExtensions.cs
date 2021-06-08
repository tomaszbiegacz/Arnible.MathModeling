using System;
using System.Text;

namespace Arnible.Linq
{
  public static class ToArrayStringExtensions
  {
    public static string ToArrayString<T>(this in ReadOnlySpan<T> src, string separator = ",")
    {
      StringBuilder builder = new();
      builder.Append("[");
      string currentSeparator = string.Empty;
      foreach(ref readonly T item in src)
      {
        builder.Append(currentSeparator);
        // ReSharper disable once HeapView.PossibleBoxingAllocation
        builder.Append(item);
        currentSeparator = separator;
      }
      builder.Append("]");
      return builder.ToString();
    }
    
    public static string ToArrayString<T>(this in Span<T> src, string separator = ",")
    {
      return ToArrayString((ReadOnlySpan<T>)src, separator);
    }
  }
}