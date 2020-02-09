using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class NumberRangeDomainExtension
  {
    public static void Validate(this INumberRangeDomain domain, NumberVector value)
    {
      foreach (Number v in value)
      {
        domain.Validate(v);
      }
    }

    public static NumberVector Transpose(this INumberRangeDomain domain, NumberVector value, NumberVectorTransposition delta)
    {
      return new NumberVector(value.ZipDefensive(delta, (v, t) => domain.Transpose(v, t)));
    }

    public static bool IsValidTranspose(this INumberRangeDomain domain, NumberVector value, NumberVectorTransposition delta)
    {
      return value.ZipDefensive(delta, (v, t) => domain.IsValidTranspose(v, t)).All();
    }

    public static Number GetValidTransposeRatio(this INumberRangeDomain domain, NumberVector value, NumberVectorTransposition delta)
    {
      return value.ZipDefensive(delta, (v, t) => domain.GetValidTransposeRatio(v, t)).Min();
    }
  }
}
