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

    public double Width => (double)(Maximum - Minimum);

    public Number Translate(Number value, Number delta) => value + delta;

    public Number GetValidTranslationRatio(Number value, Number delta) => 1;

    public bool IsValid(Number value) => true;

    public Number GetValidTranslationRatioForLastAngle(Number radius, Number currentAngle, Number angleDelta) => 1;
  }
}
