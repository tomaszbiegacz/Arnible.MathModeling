using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.MathModeling.Algebra;

namespace Arnible.Linq.Algebra
{
  public static class AddExtensions
  {
    public static T[] Add<T>(this T[] src, IReadOnlyCollection<T> value) where T: IAlgebraGroup<T>
    {
      src.Length.AssertIsEqualTo(value.Count);
      
      ushort pos = 0;
      foreach(T item in value)
      {
        src[pos] = src[pos].Add(in item);
        pos++;
      }
      return src;
    }
    
    public static void Add<T>(
      in this ReadOnlySpan<T> src, 
      in ReadOnlySpan<T> value,
      in Span<T> output) where T: IAlgebraGroup<T>
    {
      src.Length.AssertIsEqualTo(value.Length);
      src.Length.AssertIsEqualTo(output.Length);
      
      for(ushort pos = 0; pos < src.Length; pos++)
      {
        output[pos] = src[pos].Add(in value[pos]);
      }
    }
    
    public static void AddSelf<T>(
      in this Span<T> src, 
      in ReadOnlySpan<T> value) where T: IAlgebraGroup<T>
    {
      src.Length.AssertIsEqualTo(value.Length);

      for(ushort pos = 0; pos < src.Length; pos++)
      {
        src[pos] = src[pos].Add(in value[pos]);
      }
    }
    
    public static void AddSelf<T>(
      in this Span<T> src, 
      in Span<T> value) where T: IAlgebraGroup<T>
    {
      AddSelf(in src, (ReadOnlySpan<T>)value);
    }
  }
}