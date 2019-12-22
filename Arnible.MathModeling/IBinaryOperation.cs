namespace Arnible.MathModeling
{
  public interface IBinaryOperation<TNumber> where TNumber : struct
  {
    TNumber Value(TNumber x, TNumber y);
  }
}
