using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMinimumAtTests
  {
    readonly struct Value
    {
      public Value(double v)
      {
        V = v;
      }

      public double V { get; }
    }
    
    [Fact]
    public void WithMinimumAt_OfThree()
    {
      uint r = (new[]
      {
        new Value(4), new Value(6), new Value(3)
      }).WithMinimumAt(v => v.V);
      Assert.Equal(2u, r);
    }
  }
}