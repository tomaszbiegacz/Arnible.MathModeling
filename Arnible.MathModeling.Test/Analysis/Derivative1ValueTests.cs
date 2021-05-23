using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test
{
  public class Derivative1ValueTests
  {
    [Fact]
    public void EqualOperator()
    {
      Derivative1Value v1 = new()
      {
        First = 1
      };
      
      Derivative1Value v2 = new()
      {
        First = 1
      };
      
      v1.Equals(default).AssertIsFalse();
      v1.Equals(in v2).AssertIsTrue();
      
      v1.GetHashCode().AssertIsEqualTo(v2.GetHashCode());
      v1.ToString().AssertIsEqualTo(v2.ToString());
      
      (v1 == v2).AssertIsTrue();
      (v1 != v2).AssertIsFalse();
      
      v1.Equals(null).AssertIsFalse();
    }
  }
}