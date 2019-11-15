namespace Arnible.MathModeling
{
  public interface IUnaryOperation
  {
    double Value(double x);

    IDerivative Derivative(double x);
  }
}
