namespace Arnible.MathModeling.Algebra
{
  public class NumberDomain : INumberRangeDomain
  {
    public Number GetValidTransposeRatio(Number value, Number delta) => 1;

    public bool IsValidTranspose(Number value, Number delta) => true;

    public Number Transpose(Number value, Number delta) => value + delta;

    public void Validate(Number value)
    {
      // intentionally empty
    }
  }
}
