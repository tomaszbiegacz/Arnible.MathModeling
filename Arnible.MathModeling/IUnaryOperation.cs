namespace Arnible.MathModeling
{
  public interface IUnaryOperation<TNumber> where TNumber : struct
  {
    TNumber Value(TNumber x);
  }
}
