using System;
using Arnible.Assertions;
using Arnible.MathModeling.Geometry;
using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberRangeDomainTests
  {
    private readonly INumberRangeDomain _range = new NumberRangeDomain(-1, 1);

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
      _range.Translate(currentValue, evaluatedDelta).AssertIsEqualTo(expectedValue);
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
    public void GetValidTranslationRatio(double currentValue, double evaluatedDelta, double expectedRatio)
    {
      _range.GetValidTranslationRatio(currentValue, evaluatedDelta).AssertIsEqualTo(expectedRatio);
    }

    [Fact]
    public void Width()
    {
      _range.Width.AssertIsEqualTo(2);
    }

    [Fact]
    public void Vector_IsValid_True()
    {
      var src = new Number[] { 0.1, 0.2, 0.3 };
      var delta = new Number[] {0.2, 0.3, 0};
      _range.IsValidTranslation(src, delta).AssertIsTrue();
    }

    [Fact]
    public void Vector_IsValid_False()
    {
      var src = new Number[] {0.1, 0.2, 0.3};
      var delta = new Number[] {0.2, 0.9, 0};
      _range.IsValidTranslation(src, delta).AssertIsFalse();
    }

    [Fact]
    public void Array_IsValid_True()
    {
      var src = new Number[] {0.1, 0.2};
      var delta = new Number[] {0.2, 0.3};
      _range.IsValidTranslation(src, delta).AssertIsTrue();
    }

    [Fact]
    public void Array_IsValid_False_Overflow()
    {
      var src = new Number[] {0.1, 0.9};
      var delta = new Number[] {0.2, 0.3};
      _range.IsValidTranslation(src, delta).AssertIsFalse();
    }

    [Fact]
    public void GetValidTranslation_Passthrough()
    {
      Span<Number> translation = new Number[] {0.2, 0.3};
      _range.GetValidTranslation(new Number[] { 0.5, 0.2 }, in translation);
      translation.AssertSequenceEqualsTo(new Number[] {0.2, 0.3});
    }

    [Fact]
    public void GetValidTranslation_PassthroughDefault()
    {
      Span<Number> translation = new Number[] {0, 0};
      _range.GetValidTranslation(new Number[] {0.5, 0.2}, in translation);
      translation.AssertSequenceEqualsTo(new Number[] { 0, 0 });
    }

    [Fact]
    public void GetValidTranslation_Default()
    {
      Span<Number> translation = new Number[] {0.2, 0.3};
      _range.GetValidTranslation(new Number[] {1, 0.2}, in translation);
      translation.AssertSequenceEqualsTo(new Number[] { 0, 0 });
    }

    [Fact]
    public void GetValidTranslation_Half()
    {
      Span<Number> translation = new Number[] {0.2, 0.4};
      _range.GetValidTranslation(new Number[] {0.5, 0.8}, in translation);
      translation.AssertSequenceEqualsTo(new Number[] {0.1, 0.2});
    }
    
    [Fact]
    public void GetValidTranslationRatioForLastAngle_None()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: 0);
      ratio.AssertIsEqualTo(1);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_Full()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.HalfRightAngle);
      ratio.AssertIsEqualTo(1);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutAngle()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      ratio.AssertIsEqualTo(0.5);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: r, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      ratio.AssertIsEqualTo(1d / 6);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Empty()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2}, 
        delta: new Number[] {0.2, 0.3});
      translation.AssertIsEqualTo(0);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Null()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2}, 
        delta: new Number[] {0, 0});
      translation.AssertIsNull();
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledUp()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {0, 0}, 
        delta: new Number[] {0.2, 0.1});
      translation.AssertIsEqualTo(5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledDown()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] { 0.2, 0 }, 
        delta: new Number[] { -2.4, 0.1 });
      translation.AssertIsEqualTo(0.5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Empty_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2, 0.1}, 
        delta: new Number[] {0.2, 0.3, 0});
      translation.AssertIsEqualTo(0);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Null_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2, 0.1}, 
        delta: new Number[] {0, 0, 0});
      translation.AssertIsNull();
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledUp_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {0, 0, 0.1}, 
        delta: new Number[] {0.2, 0.1, 0});
      translation.AssertIsEqualTo(5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledDown_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] { 0.2, 0, 0.1 }, 
        delta: new Number[] { -2.4, 0.1, 0 });
      translation.AssertIsEqualTo(0.5);
    }

    [Fact]
    public void IsValidTranslation_None_Invalid()
    {
      _range.IsValidTranslation(-2, Sign.None).AssertIsFalse();
    }
    
    [Fact]
    public void IsValidTranslation_None_Valid()
    {
      _range.IsValidTranslation(-1, Sign.None).AssertIsTrue();
    }
    
    [Fact]
    public void IsValidTranslation_Negative_Invalid()
    {
      _range.IsValidTranslation(-1, Sign.Negative).AssertIsFalse();
    }
    
    [Fact]
    public void IsValidTranslation_Negative_Valid()
    {
      _range.IsValidTranslation(-0.9, Sign.Negative).AssertIsTrue();
    }
    
    [Fact]
    public void IsValidTranslation_Positive_Invalid()
    {
      _range.IsValidTranslation(1, Sign.Positive).AssertIsFalse();
    }
    
    [Fact]
    public void IsValidTranslation_Positive_Valid()
    {
      _range.IsValidTranslation(0.9, Sign.Positive).AssertIsTrue();
    }
    
    [Fact]
    public void IncorrectRange()
    {
      Assert.Throws<ArgumentException>(() => new NumberRangeDomain(minimum: 2, maximum: 1));
    }
    
    [Fact]
    public void IsValidTranslation_UnknownSign()
    {
      try
      {
        _range.IsValidTranslation(0, (Sign)1000);
        throw new Exception("I should not get here");
      }
      catch(ArgumentException)
      {
        // all is ok
      }
    }
  }
}
