using Arnible.Assertions;
using Xunit;
using static Arnible.MathModeling.Algebra.Polynomials.MetaMath;
using static Arnible.MathModeling.Algebra.Polynomials.Term;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{  
  public class PolarCoordinatesTest
  {
    [Fact]
    public void ErrorExpression()
    {
      // error expression in cartesian coordinates (x, y)
      var error = (c - x * y).ToPower(2);
      
      EqualExtensions.AssertEqualTo(
        -2 * y * (c - x * y), 
        error.DerivativeBy(x));

      EqualExtensions.AssertEqualTo(
        -2 * x * (c - x * y), 
        error.DerivativeBy(y));

      // error expression in polar coordinates (r, θ)
      var errorPolar = error.Composition(x, r * Cos(θ)).Composition(y, r * Sin(θ));

      EqualExtensions.AssertEqualTo(
        (c - r.ToPower(2) * Sin(θ) * Cos(θ)).ToPower(2),
        errorPolar);

      EqualExtensions.AssertEqualTo(
        -4 * r * Sin(θ) * Cos(θ) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)), 
        errorPolar.DerivativeBy(r));

      EqualExtensions.AssertEqualTo(
        -2 * r.ToPower(2) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)) * (Cos(θ).ToPower(2) - Sin(θ).ToPower(2)),
        errorPolar.DerivativeBy(θ));
    }
  }
}
