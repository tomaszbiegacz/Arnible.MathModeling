using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test
{
  public class ValueWithDerivative1Tests
  {
    [Fact]
    public void EqualOperator()
    {
      ValueWithDerivative1 v1 = new()
      {
        First = 1,
        Value = 2
      };
      
      ValueWithDerivative1 v2 = new()
      {
        First = 1,
        Value = 2
      };
      
      (v1 == new Derivative1Value {
          First = 1
      }).AssertIsTrue();

      v1.ToString().AssertIsEqualTo(v2.ToString());
    }
  }
}