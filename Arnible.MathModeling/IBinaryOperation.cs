namespace Arnible.MathModeling
{
  public interface IBinaryOperation
  {
    double Value(double x, double y);

    IDerivative DerivativeByX(double x, double y);

    IDerivative DerivativeByY(double x, double y);
  }
}
