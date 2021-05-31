using System;

namespace Arnible.Linq
{
  public static class CopyToExtensions
  {
    public static bool CopyTo<T>(
      in this ReadOnlySpan<T> source, 
      FuncIn<T, bool> predicate,
      in T defaultValue,
      in Span<T> output)
    {
      if(source.Length != output.Length)
      {
        throw new ArgumentException(nameof(output));
      }
      
      bool result = false;
      for(ushort i =0; i<source.Length; ++i)
      {
        if(predicate(in source[i]))
        {
          output[i] = source[i];
          result = true;
        }
        else
        {
          output[i] = defaultValue;
        }
      }
      return result;
    }
    
    public static bool CopyTo<T>(
      in this Span<T> source, 
      FuncIn<T, bool> predicate,
      in T defaultValue,
      in Span<T> output)
    {
      return CopyTo((ReadOnlySpan<T>)source, predicate, in defaultValue, in output);
    }
  }
}