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

    public double Width => Maximum - Minimum;

    public Number Validate(Number value)
    {
      if (value == Minimum)
      {
        return Minimum;
      }

      if (value == Maximum)
      {
        return Maximum;
      }

      if (value < Minimum || value > Maximum)
      {
        throw new ArgumentException($"Invalid value: {value}");
      }
      else
      {
        return value;
      }
    }

    private Number GetTranslationDelta(Number value, Number delta)
    {
      Validate(value);

      if (delta != 0)
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
      }

      return delta;
    }

    public bool IsValidTranslation(Number value, Number delta)
    {
      Validate(value);

      if (delta != 0)
      {
        Number minimum = Minimum - value;
        if (delta < minimum)
        {
          return false;
        }

        Number maximum = Maximum - value;
        if (delta > maximum)
        {
          return false;
        }
      }

      return true;
    }

    public Number Translate(Number value, Number delta) => value + GetTranslationDelta(value, delta);

    public Number GetValidTranslationRatio(Number value, Number delta)
    {
      if (delta == 0)
      {
        return 1;
      }
      else
      {
        Number validDelta = GetTranslationDelta(value, delta);
        return validDelta / delta;
      }
    }
  }
}
