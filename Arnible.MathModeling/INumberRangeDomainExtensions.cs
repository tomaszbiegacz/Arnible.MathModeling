using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.Linq;

namespace Arnible.MathModeling
{
  public static class INumberRangeDomainExtensions
  {
    public static bool IsValid(
      this INumberRangeDomain domain,
      in ReadOnlySpan<Number> value)
    {
      foreach(ref readonly Number v in value)
      {
        if(!domain.IsValid(v))
        {
          return false;
        }
      }
      return true;
    }
    
    public static bool IsValid(
      this INumberRangeDomain domain,
      in Span<Number> value)
    {
      return IsValid(domain, (ReadOnlySpan<Number>)value);
    }

    public static bool IsValidTranslation(
      this INumberRangeDomain domain, 
      in Number value, 
      in Number delta)
    {
      domain.IsValid(value).AssertIsTrue();
      return domain.IsValid(value + delta);
    }
    
    public static bool IsValidTranslation(
      this INumberRangeDomain domain, 
      in ReadOnlySpan<Number> value, 
      in ReadOnlySpan<Number> delta)
    {
      value.Length.AssertIsEqualTo(delta.Length);
      for(ushort i=0; i<value.Length; ++i)
      {
        if(!domain.IsValidTranslation(in value[i], in delta[i]))
        {
          return false;
        }
      }
      return true;
    }
  }
}
