namespace Arnible.MathModeling
{
  public interface IBinaryOperation<TNumber> where TNumber : struct
  {
    TNumber Value(in TNumber x, in TNumber y);
  }
}
