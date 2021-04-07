namespace Arnible.MathModeling.Algebra
{
  /// <summary>
  /// Dump number range domain without validation for formal tests
  /// </summary>
  public record NumberRangeDomain(in Number Minimum, in Number Maximum) : INumberRangeDomain
  {
    public Number Minimum { get; } = Minimum;
    public Number Maximum { get; } = Maximum;

    public double Width => (double)(Maximum - Minimum);

    public Number Translate(in Number value, in Number delta) => value + delta;

    public Number GetValidTranslationRatio(in Number value, in Number delta) => 1;
    
    public Number? GetMaximumValidTranslationRatio(in Number value, in Number delta) => null;
    
    public bool IsValid(in Number value) => true;

    public Number GetValidTranslationRatioForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta) => 1;
    
    public bool IsValidTranslation(in Number value, Sign direction) => true;
  }
}
