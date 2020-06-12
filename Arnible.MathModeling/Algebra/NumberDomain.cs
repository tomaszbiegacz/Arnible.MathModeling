namespace Arnible.MathModeling.Algebra
{
  public class NumberDomain : INumberRangeDomain
  {
    public double Width => double.PositiveInfinity;

    public Number GetValidTranslationRatio(Number value, Number delta) => 1;

    public bool IsValid(Number value) => true;    

    public bool IsValidTranslation(Number value, Number delta) => true;

    public Number Translate(Number value, Number delta) => value + delta;    

    public Number GetValidTranslationRatioForLastAngle(Number radius, Number currentAngle, Number angleDelta) => angleDelta;
  }
}
