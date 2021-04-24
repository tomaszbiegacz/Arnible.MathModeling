using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMaximumAtTests
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
    public void WithMaximumAt_OfThree()
    {
      uint r = (new[]
      {
        new Value(4), new Value(6), new Value(3)
      }).WithMaximumAt(v => v.V);
      Assert.Equal(1u, r);
    }
  }
}