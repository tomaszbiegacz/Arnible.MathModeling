using System;

namespace Arnible.MathModeling.Algebra
{
  /// <summary>
  /// Dump number range domain without validation for formal tests
  /// </summary>
  public class NumberRangeDomain : INumberRangeDomain
  {
    public Number Minimum { get; }

    public Number Maximum { get; }

    public NumberRangeDomain(Number minimum, Number maximum)
    {      
      Minimum = minimum;
      Maximum = maximum;
    }

    public void Validate(Number value)
    {
      // intentionally empty
    }

    public Number Transpose(Number value, Number delta) => value + delta;    

    public bool IsValidTranspose(Number value, Number delta)
    {
      return true;
    }

    public Number GetValidTransposeRatio(Number value, Number delta) => 1;    
  }
}
