using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberRangeDomainTests
  {
    private readonly INumberRangeDomain _strategy = new NumberRangeDomain(-1, 1);

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(0, 0, 0)]
    [InlineData(0.5, 0.5, 1)]
    [InlineData(0.5, 0.6, 1)]
    [InlineData(1, 0.6, 1)]
    [InlineData(0.5, -1.5, -1)]
    [InlineData(0.5, -1.6, -1)]
    [InlineData(-1, -0.6, -1)]
    public void Translate(double currentValue, double evaluatedDelta, double expectedValue)
    {
      Assert.Equal<Number>(expectedValue, _strategy.Translate(currentValue, evaluatedDelta));
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(0, 0, 1)]
    [InlineData(0.4, 0.6, 1)]
    [InlineData(0.4, 1.2, 0.5)]
    [InlineData(1, 0.6, 0)]
    [InlineData(-0.4, -0.6, 1)]
    [InlineData(-0.4, -1.2, 0.5)]
    [InlineData(-1, -0.6, 0)]
    [InlineData(0.4, -1.2, 1)]
    [InlineData(0.4, -1.4, 1)]
    [InlineData(0.4, -2.8, 0.5)]
    public void GetValidTranslationRatio(double currentValue, double evaluatedDelta, double expectedValue)
    {
      Assert.Equal<Number>(expectedValue, _strategy.GetValidTranslationRatio(currentValue, evaluatedDelta));
    }

    [Fact]
    public void Width()
    {
      Assert.Equal(2, _strategy.Width);
    }

    [Fact]
    public void Vector_IsValid_True()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      Assert.True(_strategy.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Vector_IsValid_False()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      Assert.False(_strategy.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Vector_Translate_Subset()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      Assert.Equal(new NumberVector(0.3, 1, 0.3), _strategy.Translate(src, delta));
    }

    [Fact]
    public void Vector_Translate_Superset()
    {
      NumberVector src = new NumberVector(0.1);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      Assert.Equal(new NumberVector(0.3, 0.9), _strategy.Translate(src, delta));
    }

    [Fact]
    public void Array_IsValid_True()
    {
      NumberArray src = new NumberArray(0.1, 0.2);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      Assert.True(_strategy.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False()
    {
      NumberArray src = new NumberArray(0.1, 0.2);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3, 0.1);
      Assert.False(_strategy.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False_Overflow()
    {
      NumberArray src = new NumberArray(0.1, 0.9);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      Assert.False(_strategy.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_Translate_Superset()
    {
      NumberArray src = new NumberArray(0.1, 0.2, 0.1);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      Assert.Equal(new NumberArray(0.3, 1, 0.1), _strategy.Translate(src, delta));
    }
  }
}
