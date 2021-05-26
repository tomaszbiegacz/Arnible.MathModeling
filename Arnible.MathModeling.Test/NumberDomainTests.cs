using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberDomainTests
  {
    private readonly INumberRangeDomain _domain = new NumberDomain();
    
    [Fact]
    public void Width()
    {
      double.PositiveInfinity.AssertIsEqualTo(_domain.Width);
    }
    
    [Fact]
    public void GetValidTranslationRatio()
    {
      _domain.GetValidTranslationRatio(1000, 999).AssertIsEqualTo(1);
    }
    
    [Fact]
    public void GetMaximumValidTranslationRatio()
    {
      _domain.GetMaximumValidTranslationRatio(1000, 2001).AssertIsNull();
    }
    
    [Fact]
    public void IsValid()
    {
      _domain.IsValid(-1000).AssertIsTrue();
    }
    
    [Fact]
    public void Translate()
    {
      _domain.Translate(100, 101).AssertIsEqualTo(201);
    }
    
    [Fact]
    public void GetValidTranslationRatioForLastAngle()
    {
      _domain.GetValidTranslationRatio(1000, 1001).AssertIsEqualTo(1);
    }
    
    [Fact]
    public void IsValidTranslation()
    {
      _domain.IsValidTranslation(-1000, Sign.Negative);
    }
  }
}