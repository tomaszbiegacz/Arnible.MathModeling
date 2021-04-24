namespace Arnible.MathModeling.Analysis
{
  public interface IUnaryOperation<TNumber> where TNumber : struct
  {
    TNumber Value(in TNumber x);
  }
}
