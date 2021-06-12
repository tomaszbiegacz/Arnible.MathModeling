using System;

namespace Arnible.Export.Test
{
  public readonly struct ReadOnlyArray<T>
  {
    private readonly T[]? _src;
    
    public ReadOnlyArray(params T[] items)
    {
      _src = items;
    }
    
    public static implicit operator ReadOnlyArray<T>(T[] v) => new(v);
    
    public ReadOnlySpan<T> Span => _src ?? ReadOnlySpan<T>.Empty;
  }
}