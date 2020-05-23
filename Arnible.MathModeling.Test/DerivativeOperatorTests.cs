using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class DerivativeOperatorTests
  {
    [Fact]
    public void ForProductByParameter_OneValue()
    {
      var v1 = 2;
      var v1d = new Derivative1Value(3);

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new NumberArray(v1),
        valueDerrivativeByParameter: new IDerivative1[] { v1d });

      Assert.Equal(3, d.First);
    }

    [Fact]
    public void ForProductByParameter_TwoValues()
    {
      var v1 = 2;
      var v1d = new Derivative1Value(3);

      var v2 = 7;
      var v2d = new Derivative1Value(11);

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new NumberArray(v1, v2),
        valueDerrivativeByParameter: new IDerivative1[] { v1d, v2d });

      Assert.Equal(3 * 7 + 2 * 11, d.First);
    }

    [Fact]
    public void ForComposition2_OneValue()
    {
      var d = (new IDerivative2[] { new Derivative2Value(2, 3) }).ForComposition();
      Assert.Equal(2, d.First);
      Assert.Equal(3, d.Second);
    }

    [Fact]
    public void ForComposition2_TwoValues()
    {
      var d = (new IDerivative2[] {
        new Derivative2Value(2, 3),
        new Derivative2Value(5, 7) }).ForComposition();
      Assert.Equal(2 * 5, d.First);
      Assert.Equal(3 * 5 * 5 + 2 * 7, d.Second);
    }

    [Fact]
    public void ForComposition2_ThreeValues()
    {
      var d = (new IDerivative2[] {
        new Derivative2Value(2, 3),
        new Derivative2Value(5, 7),
        new Derivative2Value(11, 13)
      }).ForComposition();
      Assert.Equal(2 * 5 * 11, d.First);
      Assert.Equal(3 * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13, d.Second);
    }

    [Fact]
    public void ForComposition1_OneValue()
    {
      var d = (new IDerivative1[] { new Derivative1Value(2) }).ForComposition();
      Assert.Equal(2, d.First);      
    }

    [Fact]
    public void ForComposition1_TwoValues()
    {
      var d = (new IDerivative1[] {
        new Derivative1Value(2),
        new Derivative1Value(5) }).ForComposition();
      Assert.Equal(2 * 5, d.First);      
    }

    [Fact]
    public void ForEachElementComposition_OneValue()
    {
      Assert.Equal(6, 
        new IDerivative1[] { new Derivative1Value(2) }
        .ForEachElementComposition(new IDerivative1[] { new Derivative1Value(3) })
        .Select(v => v.First).Single());
    }
  }
}
