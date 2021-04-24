namespace Arnible.MathModeling.Analysis
{
  public interface IBinaryOperation<TNumber> where TNumber : struct
  {
    TNumber Value(in TNumber x, in TNumber y);
  }
}
