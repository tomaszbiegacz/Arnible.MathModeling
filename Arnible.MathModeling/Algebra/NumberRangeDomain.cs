using System;

namespace Arnible.MathModeling.Algebra
{
  public interface INumberRangeDomain
  {
    void Validate(Number value);

    Number Transpose(Number value, Number delta);

    bool IsValidTranspose(Number value, Number delta);

    Number GetValidTransposeRatio(Number value, Number delta);
  }

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

    public void Validate(Number value)
    {
      if (value < Minimum || value > Maximum)
      {
        throw new ArgumentException($"Invalid value: {value}");
      }
    }

    private Number GetTransposeDelta(Number value, Number delta)
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

    public bool IsValidTranspose(Number value, Number delta)
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

    public Number Transpose(Number value, Number delta) => value + GetTransposeDelta(value, delta);    

    public Number GetValidTransposeRatio(Number value, Number delta)
    {
      if (delta == 0)
      {
        return 1;
      }
      else
      {
        Number validDelta = GetTransposeDelta(value, delta);
        return validDelta / delta;
      }
    }
  }
}
