using System;

namespace Arnible.Linq
{
  public static class ToArrayExtensions
  {
    public static TResult[] ToArray<TSource, TResult>(
      in this ReadOnlySpan<TSource> src, 
      Func<TSource, TResult> func)
    {
      var result = new TResult[src.Length];
      for(ushort i=0; i<src.Length; ++i)
      {
        result[i] = func(src[i]);
      }
      return result;
    }
    
    public static TResult[] ToArray<TSource, TResult>(
      in this Span<TSource> src, 
      Func<TSource, TResult> func)
    {
      var result = new TResult[src.Length];
      for(ushort i=0; i<src.Length; ++i)
      {
        result[i] = func(src[i]);
      }
      return result;
    }
  }
}