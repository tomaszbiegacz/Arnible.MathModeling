using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class NotNullExtensions
  {
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> src) where T: class
    {
      foreach(T? value in src)
      {
        if(value is not null)
        {
          yield return value;
        }
      }
    }
    
    public static IEnumerable<T> NotNone<T>(this IEnumerable<T?> src) where T: struct
    {
      foreach(T? value in src)
      {
        if(value is not null)
        {
          yield return value.Value;
        }
      }
    }
  }
}