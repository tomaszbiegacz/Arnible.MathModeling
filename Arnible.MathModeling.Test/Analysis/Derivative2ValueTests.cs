using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test
{
  public class Derivative2ValueTests
  {
    [Fact]
    public void EqualOperator()
    {
      Derivative2Value v1 = new()
      {
        First = 1,
        Second = 2
      };
      
      Derivative2Value v2 = new()
      {
        First = 1,
        Second = 2
      };
      
      (v1 == new Derivative1Value {
        First = 1
      }).AssertIsTrue();

      v1.ToString().AssertIsEqualTo(v2.ToString());
    }
  }
}