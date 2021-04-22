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
      in ValueArray<Number> value,
      in ValueArray<Number> gradient)
    {
      return value.GetInternalEnumerable().ZipDefensive(
        col2: gradient.GetInternalEnumerable(), 
        merge: (v, t) => domain.GetMaximumValidTranslationRatio(v, t)
        ).MinOrDefault();
    }
  }
}
