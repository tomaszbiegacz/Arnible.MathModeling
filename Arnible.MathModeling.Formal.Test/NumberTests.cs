using Arnible.Assertions;
using Arnible.MathModeling.Algebra.Polynomials;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberTests
  {
    private static readonly Number A = Term.a;

    [Fact]
    public void Equality()
    {
      ConditionExtensions.AssertIsTrue(0 == A - A);
      ConditionExtensions.AssertIsFalse(0 != A - A);

      ConditionExtensions.AssertIsTrue(1 == A + 1 - A);
      ConditionExtensions.AssertIsFalse(1 != A + 1 - A);

      ConditionExtensions.AssertIsFalse(1 == A - A);
      ConditionExtensions.AssertIsTrue(1 != A - A);      
    }

    [Fact]
    public void Greater()
    {
      ConditionExtensions.AssertIsFalse(0 > A + 1 - A);
      ConditionExtensions.AssertIsTrue(0 < A + 1 - A);

      ConditionExtensions.AssertIsTrue(2 > A + 1 - A);
      ConditionExtensions.AssertIsFalse(2 < A + 1 - A);

      ConditionExtensions.AssertIsFalse(2 * A > A);
      ConditionExtensions.AssertIsFalse(2 * A < A);
      ConditionExtensions.AssertIsFalse(2 * A >= A);
      ConditionExtensions.AssertIsFalse(2 * A <= A);

      ConditionExtensions.AssertIsFalse(A > Term.a);
      ConditionExtensions.AssertIsFalse(A < Term.a);
      ConditionExtensions.AssertIsTrue(A <= Term.a);
      ConditionExtensions.AssertIsTrue(A >= Term.a);
    }
  }
}
