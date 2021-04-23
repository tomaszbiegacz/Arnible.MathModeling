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
      IsEqualToExtensions.AssertIsEqualTo(expectedValue, _range.Translate(currentValue, evaluatedDelta));
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
      IsEqualToExtensions.AssertIsEqualTo(expectedRatio, _range.GetValidTranslationRatio(currentValue, evaluatedDelta));
    }

    [Fact]
    public void Width()
    {
      IsEqualToExtensions.AssertIsEqualTo(2, _range.Width);
    }

    [Fact]
    public void Vector_IsValid_True()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberVector delta = new NumberVector(0.2, 0.3);
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Vector_IsValid_False()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberVector delta = new NumberVector(0.2, 0.9);
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_True()
    {
      var src = new Number[] {0.1, 0.2};
      var delta = new Number[] {0.2, 0.3};
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False()
    {
      var src = new Number[] {0.1, 0.2};
      var delta = new Number[] {0.2, 0.3, 0.1};
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False_Overflow()
    {
      var src = new Number[] {0.1, 0.9};
      var delta = new Number[] {0.2, 0.3};
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void GetValidTranslation_Passthrough()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), new NumberVector(0.2, 0.3));
      IsEqualToExtensions.AssertIsEqualTo(new NumberVector(0.2, 0.3), translation);
    }

    [Fact]
    public void GetValidTranslation_PassthroughDefault()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), default);
      IsEqualToExtensions.AssertIsEqualTo(default, translation);
    }

    [Fact]
    public void GetValidTranslation_Default()
    {
      var translation = _range.GetValidTranslation(new NumberVector(1, 0.2), new NumberVector(0.2, 0.3));
      IsEqualToExtensions.AssertIsEqualTo(0, translation);
    }

    [Fact]
    public void GetValidTranslation_Half()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.8), new NumberVector(0.2, 0.4));
      IsEqualToExtensions.AssertIsEqualTo(new NumberVector(0.1, 0.2), translation);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_Full()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.HalfRightAngle);
      IsEqualToExtensions.AssertIsEqualTo<double>(1, (double)ratio);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutAngle()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      IsEqualToExtensions.AssertIsEqualTo(0.5, ratio);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: r, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      IsEqualToExtensions.AssertIsEqualTo(1d / 6, ratio);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Empty()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2}, 
        gradient: new Number[] {0.2, 0.3});
      IsEqualToExtensions.AssertIsEqualTo(translation, 0);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Null()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2}, 
        gradient: new Number[] {0, 0});
      IsNullExtensions.AssertIsNull(translation);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledUp()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {0, 0}, 
        gradient: new Number[] {0.2, 0.1});
      IsEqualToExtensions.AssertIsEqualTo(translation, 5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledDown()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] { 0.2, 0 }, 
        gradient: new Number[] { -2.4, 0.1 });
      IsEqualToExtensions.AssertIsEqualTo(translation, 0.5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Empty_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2, 0.1}, 
        transaction: new NumberVector(0.2, 0.3));
      IsEqualToExtensions.AssertIsEqualTo(translation, 0);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_Null_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {1, 0.2, 0.1}, 
        transaction: new NumberVector(0, 0));
      IsNullExtensions.AssertIsNull(translation);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledUp_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] {0, 0, 0.1}, 
        transaction: new NumberVector(0.2, 0.1));
      IsEqualToExtensions.AssertIsEqualTo(translation, 5);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio_ScaledDown_Vector()
    {
      var translation = _range.GetMaximumValidTranslationRatio(
        value: new Number[] { 0.2, 0, 0.1 }, 
        transaction: new NumberVector( -2.4, 0.1 ));
      IsEqualToExtensions.AssertIsEqualTo(translation, 0.5);
    }

    [Fact]
    public void IsValidTranslation_None_Invalid()
    {
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(-2, Sign.None));
    }
    
    [Fact]
    public void IsValidTranslation_None_Valid()
    {
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(-1, Sign.None));
    }
    
    [Fact]
    public void IsValidTranslation_Negative_Invalid()
    {
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(-1, Sign.Negative));
    }
    
    [Fact]
    public void IsValidTranslation_Negative_Valid()
    {
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(-0.9, Sign.Negative));
    }
    
    [Fact]
    public void IsValidTranslation_Positive_Invalid()
    {
      ConditionExtensions.AssertIsFalse(_range.IsValidTranslation(1, Sign.Positive));
    }
    
    [Fact]
    public void IsValidTranslation_Positive_Valid()
    {
      ConditionExtensions.AssertIsTrue(_range.IsValidTranslation(0.9, Sign.Positive));
    }
  }
}
