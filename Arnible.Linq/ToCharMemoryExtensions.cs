using System;
using System.Text;

namespace Arnible.Linq
{
  public static class ToCharMemoryExtensions
  {
    public static ReadOnlyMemory<char> ToCharMemory<T>(this in ReadOnlySpan<T> src, string separator = ",")
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
      return builder.ToString().AsMemory();
    }
  }
}