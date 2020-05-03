namespace Arnible.MathModeling.Algebra
{
  public interface INumberRangeDomain
  {
    double Width { get; }

    void Validate(Number value);

    Number Translate(Number value, Number delta);

    bool IsValidTranslation(Number value, Number delta);

    Number GetValidTranslationRatio(Number value, Number delta);
  }
}
