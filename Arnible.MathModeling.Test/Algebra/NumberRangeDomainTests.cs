using System;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

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
      AreEqual(expectedValue, _range.Translate(currentValue, evaluatedDelta));
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
      AreEqual(expectedRatio, _range.GetValidTranslationRatio(currentValue, evaluatedDelta));
    }

    [Fact]
    public void Width()
    {
      AreEqual(2, _range.Width);
    }

    [Fact]
    public void Vector_IsValid_True()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      IsTrue(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Vector_IsValid_False()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      IsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Vector_Translate_Subset()
    {
      NumberVector src = new NumberVector(0.1, 0.2, 0.3);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      AreEqual(new NumberVector(0.3, 1, 0.3), _range.Translate(src, delta));
    }

    [Fact]
    public void Vector_Translate_Superset()
    {
      NumberVector src = new NumberVector(0.1);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      AreEqual(new NumberVector(0.3, 0.9), _range.Translate(src, delta));
    }

    [Fact]
    public void Array_IsValid_True()
    {
      var src = new ValueArray<Number>(0.1, 0.2);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      IsTrue(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False()
    {
      var src = new ValueArray<Number>(0.1, 0.2);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3, 0.1);
      IsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_IsValid_False_Overflow()
    {
      var src = new ValueArray<Number>(0.1, 0.9);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.3);
      IsFalse(_range.IsValidTranslation(src, delta));
    }

    [Fact]
    public void Array_Translate_Superset()
    {
      var src = new ValueArray<Number>(0.1, 0.2, 0.1);
      NumberTranslationVector delta = new NumberTranslationVector(0.2, 0.9);
      AreEqual(new ValueArray<Number>(0.3, 1, 0.1), _range.Translate(src, delta));
    }

    [Fact]
    public void GetValidTranslation_Passthrough()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), new NumberTranslationVector(0.2, 0.3));
      AreEqual(new NumberTranslationVector(0.2, 0.3), translation);
    }

    [Fact]
    public void GetValidTranslation_PassthroughDefault()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), default);
      AreEqual(default, translation);
    }

    [Fact]
    public void GetValidTranslation_Default()
    {
      var translation = _range.GetValidTranslation(new NumberVector(1, 0.2), new NumberTranslationVector(0.2, 0.3));
      AreEqual(0, translation);
    }

    [Fact]
    public void GetValidTranslation_Half()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.8), new NumberTranslationVector(0.2, 0.4));
      AreEqual(new NumberTranslationVector(0.1, 0.2), translation);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_Full()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.HalfRightAngle);
      AreExactlyEqual(1, ratio);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutAngle()
    {
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: 1, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      AreEqual(0.5, ratio);
    }

    [Fact]
    public void GetValidTranslationRatioForLastAngle_cutRadius()
    {
      double r = 2 * Math.Sqrt(3) / 3;
      INumberRangeDomain range = new NumberRangeDomain(0, 1);
      Number ratio = range.GetValidTranslationRatioForLastAngle(radius: r, currentAngle: Angle.HalfRightAngle, angleDelta: Angle.RightAngle);
      AreEqual(1d / 6, ratio);
    }
  }
}
