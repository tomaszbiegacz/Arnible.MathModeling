namespace Arnible.MathModeling.Analysis.Learning.Error
{
  public class SquareErrorMeasure : IErrorMeasureSupervisedLearning<Number>
  {
    public Number ErrorValue(in Number expected, in Number actual)
    {
      var diff = actual - expected;
      return diff * diff;
    }
    
    public Derivative1Value ErrorDerivativeByActual(in Number expected, in Number actual)
    {
      return new Derivative1Value
      {
        First = 2 * (actual - expected)
      };
    }
  }
}
