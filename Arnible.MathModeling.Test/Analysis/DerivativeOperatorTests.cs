using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Analysis.Test
{
  public class DerivativeOperatorTests
  {
    [Fact]
    public void ForProductByParameter_OneValue()
    {
      Number v1 = 2d;
      var v1d = new Derivative1Value(3);

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new[] { v1 },
        valueDerivativeByParameter: new[] { v1d });

      EqualExtensions.AssertEqualTo(3d, d.First);
    }

    [Fact]
    public void ForProductByParameter_TwoValues()
    {
      Number v1 = 2;
      var v1d = new Derivative1Value(3);

      Number v2 = 7;
      var v2d = new Derivative1Value(11);

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new[] { v1, v2 },
        valueDerivativeByParameter: new[] { v1d, v2d });

      EqualExtensions.AssertEqualTo(3d * 7 + 2 * 11, d.First);
    }

    [Fact]
    public void ForComposition2_OneValue()
    {
      var d = (new[] { new Derivative2Value(2, 3) }).ForComposition();
      EqualExtensions.AssertEqualTo<double>(2, (double)d.First);
      EqualExtensions.AssertEqualTo<double>(3, (double)d.Second);
    }

    [Fact]
    public void ForComposition2_TwoValues()
    {
      var d = (new[] {
        new Derivative2Value(2, 3),
        new Derivative2Value(5, 7) }).ForComposition();
      EqualExtensions.AssertEqualTo(2d * 5, d.First);
      EqualExtensions.AssertEqualTo(3d * 5 * 5 + 2 * 7, d.Second);
    }

    [Fact]
    public void ForComposition2_ThreeValues()
    {
      var d = (new[] {
        new Derivative2Value(2, 3),
        new Derivative2Value(5, 7),
        new Derivative2Value(11, 13)
      }).ForComposition();
      EqualExtensions.AssertEqualTo(2d * 5 * 11, d.First);
      EqualExtensions.AssertEqualTo(3d * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13, d.Second);
    }

    [Fact]
    public void ForComposition1_OneValue()
    {
      var d = (new[] { new Derivative1Value(2) }).ForComposition();
      EqualExtensions.AssertEqualTo<double>(2, (double)d.First);
    }

    [Fact]
    public void ForComposition1_TwoValues()
    {
      var d = (new[] {
        new Derivative1Value(2),
        new Derivative1Value(5) }).ForComposition();
      EqualExtensions.AssertEqualTo(2d * 5, d.First);
    }

    [Fact]
    public void ForEachElementComposition_OneValue()
    {
      EqualExtensions.AssertEqualTo(6d,
        new[] { new Derivative1Value(2) }
        .ForEachElementComposition(new[] { new Derivative1Value(3) })
        .Select(v => v.First).Single());
    }
  }
}
