using Arnible.Assertions;
using Arnible.MathModeling.Analysis.Learning.Error;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test.Learning.Error
{
  public class SquareErrorMeasureTests
  {
    private readonly SquareErrorMeasure _measure = new();
    
    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 2, 1)]
    [InlineData(2, 1, 1)]
    [InlineData(4, 1, 9)]
    public void ErrorValue(double expected, double actual, double error)
    {
      Number errorValue = error;
      errorValue.AssertIsEqualTo(_measure.ErrorValue(expected: expected, actual: actual)); 
    }
    
    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 2, 2)]
    [InlineData(2, 1, -2)]
    [InlineData(3, 1, -4)]
    public void ErrorDerivative(double expected, double actual, double error)
    {
      Number errorValue = error;
      errorValue.AssertIsEqualTo(_measure.ErrorDerivativeByActual(expected: expected, actual: actual).First); 
    }
  }
}