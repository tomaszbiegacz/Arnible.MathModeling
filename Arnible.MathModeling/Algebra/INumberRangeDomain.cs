namespace Arnible.MathModeling.Algebra
{
  public interface INumberRangeDomain
  {
    /// <summary>
    /// Range width, can be Infinity
    /// </summary>
    double Width { get; }

    /// <summary>
    /// Checks if value is withing domain constraints
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool IsValid(in Number value);    

    /// <summary>
    /// Translate preserving number range constraints
    /// </summary>    
    Number Translate(in Number value, in Number delta);

    /// <summary>
    /// Get valid translation ratio, taking under account number range.
    /// </summary>
    Number GetValidTranslationRatio(in Number value, in Number delta);

    /// <summary>
    /// Get maximum valid translation ratio, taking under account number range.
    /// </summary>
    Number? GetMaximumValidTranslationRatio(in Number value, in Number delta);

    /// <summary>
    /// Get valid angle translation ratio for last angle
    /// </summary>    
    Number GetValidTranslationRatioForLastAngle(in Number radius, in Number currentAngle, in Number angleDelta);

    /// <summary>
    /// Checks whether given translation is valid
    /// </summary>
    bool IsValidTranslation(in Number value, in Sign direction);
  }
}
