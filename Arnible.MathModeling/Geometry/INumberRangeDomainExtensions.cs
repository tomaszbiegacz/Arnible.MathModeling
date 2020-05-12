using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Geometry
{
  public static class INumberRangeDomainExtensions
  {
    public static Number GetValidTranslationRatio(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.ZipDefensive(delta, (v, t) => domain.GetValidTranslationRatio(v, t)).MinDefensive();
    }

    public static NumberTranslationVector GetValidTranslation(
      this INumberRangeDomain domain,
      NumberVector current,
      HypersphericalAngleQuantified direction,
      Number translationRangeRatio)
    {
      if(!domain.Width.IsValidNumeric())
      {
        throw new InvalidOperationException("Supported only for finite domains.");
      }
      if(translationRangeRatio > 1)
      {
        throw new ArgumentException(nameof(translationRangeRatio));
      }

      Number deltaRange = domain.Width * translationRangeRatio;
      if (deltaRange < 0)
      {
        throw new InvalidOperationException(nameof(translationRangeRatio));
      }

      NumberTranslationVector delta = default;
      if (deltaRange > 0)
      {
        HypersphericalAngleVector anglePositive = direction.ToVector();
        var hcPositive = new HypersphericalCoordinate(deltaRange, anglePositive);
        var deltaPositive = new NumberTranslationVector(hcPositive.ToCartesianView().Coordinates);
        Number deltaPositiveRatio = domain.GetValidTranslationRatio(current, deltaPositive);

        if (deltaPositiveRatio != 0)
        {
          delta = deltaPositiveRatio * deltaPositive;
        }
        else
        {
          HypersphericalAngleVector angleNegative = anglePositive.Mirror;
          var hcNegative = new HypersphericalCoordinate(deltaRange, angleNegative);
          var deltaNegative = new NumberTranslationVector(hcNegative.ToCartesianView().Coordinates);
          Number deltaNegativeRatio = domain.GetValidTranslationRatio(current, deltaNegative);
          if (deltaNegativeRatio != 0)
          {
            delta = deltaNegativeRatio * deltaNegative;
          }
        }
      }

      return delta;
    }
  }
}
