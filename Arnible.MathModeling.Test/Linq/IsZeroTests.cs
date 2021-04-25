using Arnible.Assertions;
using Arnible.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Linq
{
  public class IsZeroTests
  {
    [Fact]
    public void IsZeroSpan()
    {
      stackalloc Number[] { 0, 1}.IsZero().AssertIsFalse();
    }
    
    [Fact]
    public void IsNotZeroEmptySpan()
    {
      stackalloc Number[0].IsZero().AssertIsTrue();
    }
  }
}