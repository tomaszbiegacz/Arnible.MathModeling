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
    bool IsValid(Number value);    

    /// <summary>
    /// Translate preserving number range constraints
    /// </summary>    
    Number Translate(Number value, Number delta);

    /// <summary>
    /// Get valid transaltion ration, taking under account number range.
    /// </summary>
    Number GetValidTranslationRatio(Number value, Number delta);

    /// <summary>
    /// Get valid angle translation ratio for last angle
    /// </summary>    
    Number GetValidTranslationRatioForLastAngle(Number radius, Number currentAngle, Number angleDelta);
  }
}
