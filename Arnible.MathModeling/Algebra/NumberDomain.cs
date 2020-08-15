namespace Arnible.MathModeling.Algebra
{
  public class NumberDomain : INumberRangeDomain
  {
    public double Width => double.PositiveInfinity;

    public Number GetValidTranslationRatio(in Number value, in Number delta) => 1;

    public bool IsValid(in Number value) => true;    
    
    public Number Translate(in Number value, in Number delta) => value + delta;    

    public Number GetValidTranslationRatioForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta) => angleDelta;
  }
}
