namespace Arnible.MathModeling.Algebra
{
  /// <summary>
  /// Dump number range domain without validation for formal tests
  /// </summary>
  public class NumberRangeDomain : INumberRangeDomain
  {
    public Number Minimum { get; }

    public Number Maximum { get; }

    public NumberRangeDomain(in Number minimum, in Number maximum)
    {
      Minimum = minimum;
      Maximum = maximum;
    }

    public double Width => (double)(Maximum - Minimum);

    public Number Translate(in Number value, in Number delta) => value + delta;

    public Number GetValidTranslationRatio(in Number value, in Number delta) => 1;

    public bool IsValid(in Number value) => true;

    public Number GetValidTranslationRatioForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta) => 1;
  }
}
