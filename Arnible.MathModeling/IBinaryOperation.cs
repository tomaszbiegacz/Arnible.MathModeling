namespace Arnible.MathModeling
{
  public interface IBinaryOperation
  {
    double Value(double x, double y);    
  }

  public interface IBinaryOperationWithDerivative : IBinaryOperation
  {
    IDerivative DerivativeByX(double x, double y);

    IDerivative DerivativeByY(double x, double y);
  }
}
