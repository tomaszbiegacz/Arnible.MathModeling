namespace Arnible.MathModeling.Algebra
{
  public interface INumberRangeDomain
  {
    void Validate(Number value);

    Number Transpose(Number value, Number delta);

    bool IsValidTranspose(Number value, Number delta);

    Number GetValidTransposeRatio(Number value, Number delta);
  }
}
