using Arnible.MathModeling.Polynomials;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    private static readonly Number a = Term.a;

    [Fact]
    public void Equality()
    {
      IsTrue(0 == a - a);
      IsFalse(0 != a - a);

      IsTrue(1 == a + 1 - a);
      IsFalse(1 != a + 1 - a);

      IsFalse(1 == a - a);
      IsTrue(1 != a - a);      
    }

    [Fact]
    public void Greater()
    {
      IsFalse(0 > a + 1 - a);
      IsTrue(0 < a + 1 - a);

      IsTrue(2 > a + 1 - a);
      IsFalse(2 < a + 1 - a);

      IsFalse(2 * a > a);
      IsFalse(2 * a < a);
      IsFalse(2 * a >= a);
      IsFalse(2 * a <= a);

      IsFalse(a > Term.a);
      IsFalse(a < Term.a);
      IsTrue(a <= Term.a);
      IsTrue(a >= Term.a);
    }
  }
}
