using System;

namespace Arnible.MathModeling.Algebra
{
  public class NumberRangeDomain : INumberRangeDomain
  {
    public Number Minimum { get; }

    public Number Maximum { get; }

    public NumberRangeDomain(Number minimum, Number maximum)
    {
      if (maximum <= minimum)
      {
        throw new ArgumentException();
      }
      Minimum = minimum;
      Maximum = maximum;
    }

    public double Width => (double)(Maximum - Minimum);

    public bool IsValid(Number value)
    {
      if (value == Minimum || value == Maximum)
      {
        return true;
      }
      else
      {
        return Minimum < value && value < Maximum;
      }
    }

    private Number GetTranslationDelta(Number value, Number delta)
    {
      Number minimum = Minimum - value;
      if (delta < minimum)
      {
        return minimum;
      }

      Number maximum = Maximum - value;
      if (delta > maximum)
      {
        return maximum;
      }

      return delta;
    }

    public Number Translate(Number value, Number delta) => value + GetTranslationDelta(value, delta);

    public Number GetValidTranslationRatio(Number value, Number delta)
    {
      if (!IsValid(value))
      {
        throw new ArgumentException(nameof(value));
      }

      if (delta == 0)
      {
        return 1;
      }
      else
      {
        Number validDelta = GetTranslationDelta(value, delta);
        if (validDelta == delta)
        {
          return 1;
        }
        else
        {
          return validDelta / delta;
        }
      }
    }

    private static double Asin(Number x)
    {
      return Math.Asin((double)x);
    }

    private Number GetTranslationDeltaForLastAngle(Number radius, Number currentAngle, Number angleDelta)
    {
      if (radius == 0)
      {
        throw new ArgumentException(nameof(radius));
      }

      Number angleMin = Asin(Minimum / radius);
      if (currentAngle < angleMin)
      {
        throw new ArgumentException(nameof(currentAngle));
      }

      Number angleMax = Asin(Maximum / radius);
      if (currentAngle > angleMax)
      {
        throw new ArgumentException(nameof(currentAngle));
      }

      Number minimum = angleMin - currentAngle;
      if (angleDelta < minimum)
      {
        return minimum;
      }

      Number maximum = angleMax - currentAngle;
      if (angleDelta > maximum)
      {
        return maximum;
      }

      return angleDelta;
    }

    public Number GetValidTranslationRatioForLastAngle(Number radius, Number currentAngle, Number angleDelta)
    {
      if (angleDelta == 0)
      {
        return 1;
      }
      else
      {
        Number validDelta = GetTranslationDeltaForLastAngle(radius: radius, currentAngle: currentAngle, angleDelta: angleDelta);
        if (validDelta == angleDelta)
        {
          return 1;
        }
        else
        {
          return validDelta / angleDelta;
        }
      }
    }
  }
}
