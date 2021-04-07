using System;

namespace Arnible.MathModeling
{
  public class NumberRangeDomain : INumberRangeDomain
  {
    public Number Minimum { get; }

    public Number Maximum { get; }

    public NumberRangeDomain(in Number minimum, in Number maximum)
    {
      if (maximum <= minimum)
      {
        throw new ArgumentException();
      }
      Minimum = minimum;
      Maximum = maximum;
    }

    public double Width => (double)(Maximum - Minimum);

    public bool IsValid(in Number value)
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

    private Number GetTranslationDelta(in Number value, in Number delta)
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

    public Number Translate(in Number value, in Number delta) => value + GetTranslationDelta(in value, in delta);

    public Number GetValidTranslationRatio(in Number value, in Number delta)
    {
      if (!IsValid(in value))
      {
        throw new ArgumentException(nameof(value));
      }

      if (delta == 0)
      {
        return 1;
      }
      else
      {
        Number validDelta = GetTranslationDelta(in value, in delta);
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

    public Number? GetMaximumValidTranslationRatio(in Number value, in Number delta)
    {
      if (!IsValid(in value))
      {
        throw new ArgumentException(nameof(value));
      }

      if (delta == 0)
      {
        return null;
      }
      else
      {
        if (delta > 0)
        {
          Number range = Maximum - value;
          return range / delta;
        }
        else
        {
          Number range = value - Minimum;
          return -1 * range / delta;
        }
      }
    }
    
    private static double Asin(Number x)
    {
      return Math.Asin((double)x);
    }

    private Number GetTranslationDeltaForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta)
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

    public Number GetValidTranslationRatioForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta)
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

    public bool IsValidTranslation(in Number value, Sign direction)
    {
      if (IsValid(in value))
      {
        switch (direction)
        {
          case Sign.None:
            return true;
          case Sign.Negative:
            return value > Minimum;
          case Sign.Positive:
            return value < Maximum;
          default:
            throw new ArgumentException(nameof(direction));
        }  
      }
      else
      {
        return false;
      }
    }
  }
}
