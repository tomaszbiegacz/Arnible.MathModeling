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
      var v1d = new Derivative1Value
      {
        First = 3
      };

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new[] { v1 },
        valueDerivativeByParameter: new[] { v1d });

      d.First.AssertIsEqualTo(3d);
    }

    [Fact]
    public void ForProductByParameter_TwoValues()
    {
      Number v1 = 2;
      var v1d = new Derivative1Value
      {
        First = 3
      };

      Number v2 = 7;
      var v2d = new Derivative1Value
      {
        First = 11
      };

      var d = DerivativeOperator.ForProductByParameter(
        productValues: new[] { v1, v2 },
        valueDerivativeByParameter: new[] { v1d, v2d });

      d.First.AssertIsEqualTo(3d * 7 + 2 * 11);
    }

    [Fact]
    public void ForComposition2_OneValue()
    {
      var d = (new[]
      {
        new Derivative2Value
        {
          First = 2,
          Second = 3
        }
      }).ForComposition();
      d.First.AssertIsEqualTo(2);
      d.Second.AssertIsEqualTo(3);
    }

    [Fact]
    public void ForComposition2_TwoValues()
    {
      var d = (new[] {
        new Derivative2Value
        {
          First = 2,
          Second = 3
        },
        new Derivative2Value
        {
          First = 5,
          Second = 7
        }
      }).ForComposition();
      d.First.AssertIsEqualTo(2d * 5);
      d.Second.AssertIsEqualTo(3d * 5 * 5 + 2 * 7);
    }

    [Fact]
    public void ForComposition2_ThreeValues()
    {
      var d = (new[] {
        new Derivative2Value
        {
          First = 2,
          Second = 3
        },
        new Derivative2Value
        {
          First = 5,
          Second = 7
        },
        new Derivative2Value
        {
          First = 11,
          Second = 13
        }
      }).ForComposition();
      d.First.AssertIsEqualTo(2d * 5 * 11);
      d.Second.AssertIsEqualTo(3d * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13);
    }

    [Fact]
    public void ForComposition1_OneValue()
    {
      var d = (new[]
      {
        new Derivative1Value
        {
          First = 2
        }
      }).ForComposition();
      d.First.AssertIsEqualTo(2);
    }

    [Fact]
    public void ForComposition1_TwoValues()
    {
      var d = (new[] {
        new Derivative1Value
        {
          First = 2
        },
        new Derivative1Value
        {
          First = 5
        }
      }).ForComposition();
      d.First.AssertIsEqualTo(2d * 5);
    }

    [Fact]
    public void ForEachElementComposition_OneValue()
    {
      new[]
        {
          new Derivative1Value
          {
            First = 2
          }
        }
        .ForEachElementComposition(new[]
        {
          new Derivative1Value
          {
            First = 3
          }
        })
        .Select(v => v.First)
        .Single()
        .AssertIsEqualTo(6);
    }
  }
}
