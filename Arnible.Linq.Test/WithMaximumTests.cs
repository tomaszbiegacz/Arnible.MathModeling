using Xunit;

namespace Arnible.Linq.Test
{
  public class WithMaximumTests
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
    public void WithMaximum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMaximum(v => v.V);
      Assert.Equal(4d, r.V);
    }
  }
}