using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class INumberRangeDomainTests
  {
    private readonly INumberRangeDomain _range = new NumberRangeDomain(0, 1);

    [Fact]
    public void GetValidTranslation_Passthrough()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), new NumberTranslationVector(0.2, 0.3));
      Assert.Equal(new NumberTranslationVector(0.2, 0.3), translation);
    }

    [Fact]
    public void GetValidTranslation_PassthroughDefault()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.2), default);
      Assert.Equal(default, translation);
    }

    [Fact]
    public void GetValidTranslation_Default()
    {
      var translation = _range.GetValidTranslation(new NumberVector(1, 0.2), new NumberTranslationVector(0.2, 0.3));
      Assert.Equal(0, translation);
    }

    [Fact]
    public void GetValidTranslation_Half()
    {
      var translation = _range.GetValidTranslation(new NumberVector(0.5, 0.8), new NumberTranslationVector(0.2, 0.4));
      Assert.Equal(new NumberTranslationVector(0.1, 0.2), translation);
    }
  }
}
