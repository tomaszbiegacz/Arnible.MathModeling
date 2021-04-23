using System;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensionsNumeric
  {
    //
    // GetValidTranslation
    //

    public static Number? GetMaximumValidTranslationRatio(
      this INumberRangeDomain domain,
      ReadOnlyArray<Number> value,
      ReadOnlyArray<Number> gradient)
    {
      return value.AsList().ZipDefensive(
        col2: gradient.AsList(), 
        merge: (v, t) => domain.GetMaximumValidTranslationRatio(v, t)
        ).MinOrNone();
    }
  }
}
