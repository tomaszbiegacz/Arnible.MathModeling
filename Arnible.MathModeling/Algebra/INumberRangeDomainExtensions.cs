using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensions
  {
    public static void Validate(this INumberRangeDomain domain, NumberVector value)
    {
      foreach (Number v in value)
      {
        domain.Validate(v);
      }
    }

    public static NumberVector Translate(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return new NumberVector(value.ZipDefensive(delta, (v, t) => domain.Translate(v, t)));
    }

    public static bool IsValidTranslation(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.ZipDefensive(delta, (v, t) => domain.IsValidTranslation(v, t)).All();
    }

    public static Number GetValidTranslationRatio(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.ZipDefensive(delta, (v, t) => domain.GetValidTranslationRatio(v, t)).Min();
    }
  }
}
