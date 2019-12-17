using Xunit;

namespace Arnible.MathModeling.Test
{
  using static Arnible.MathModeling.MetaMath;
  using static Arnible.MathModeling.Term;

  public class PolarCoordinatesTest
  {
    [Fact]
    public void ErrorExpression()
    {
      // error expression in cartesian coordinates (x, y)
      var error = (c - x * y).ToPower(2);
      
      Assert.Equal(
        -2 * y * (c - x * y), 
        error.DerivativeBy(x));

      Assert.Equal(
        -2 * x * (c - x * y), 
        error.DerivativeBy(y));

      // error expression in polar coordinates (r, θ)
      var errorPolar = error.Composition(x, r * Cos(θ)).Composition(y, r * Sin(θ));

      Assert.Equal(
        (c - r.ToPower(2) * Sin(θ) * Cos(θ)).ToPower(2),
        errorPolar);

      Assert.Equal(
        -4 * r * Sin(θ) * Cos(θ) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)), 
        errorPolar.DerivativeBy(r));

      Assert.Equal(
        -2 * r.ToPower(2) * (c - r.ToPower(2) * Sin(θ) * Cos(θ)) * (Cos(θ).ToPower(2) - Sin(θ).ToPower(2)),
        errorPolar.DerivativeBy(θ));
    }
  }
}
