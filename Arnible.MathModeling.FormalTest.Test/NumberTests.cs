using Xunit;

namespace Arnible.MathModeling.FormalTest.Test
{
  public class NumberTests
  {
    private static readonly Number a = Term.a;

    [Fact]
    public void Equality()
    {
      Assert.True(0 == a - a);
      Assert.False(0 != a - a);

      Assert.True(1 == a + 1 - a);
      Assert.False(1 != a + 1 - a);

      Assert.False(1 == a - a);
      Assert.True(1 != a - a);      
    }

    [Fact]
    public void Greater()
    {
      Assert.False(0 > a + 1 - a);
      Assert.True(0 < a + 1 - a);

      Assert.True(2 > a + 1 - a);
      Assert.False(2 < a + 1 - a);

      Assert.False(2 * a > a);
      Assert.False(2 * a < a);
      Assert.False(2 * a >= a);
      Assert.False(2 * a <= a);

      Assert.False(a > Term.a);
      Assert.False(a < Term.a);
      Assert.True(a <= Term.a);
      Assert.True(a >= Term.a);
    }
  }
}
