namespace Arnible.MathModeling
{
  public interface IUnaryOperation
  {
    double Value(double x);    
  }

  public interface IUnaryOperationWithDerivative : IUnaryOperation
  {
    IDerivative Derivative(double x);
  }
}
