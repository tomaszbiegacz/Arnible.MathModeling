namespace Arnible.MathModeling.Optimization
{
  public interface IUnimodalOptimization
  {
    /// <summary>
    /// Best point X
    /// </summary>
    Number X { get; }
    
    /// <summary>
    /// Value for best point
    /// </summary>
    Number Y { get; }

    /// <summary>
    /// Improve solution
    /// </summary>
    /// <returns>Is solution improved?</returns>
    bool MoveNext();
    
    bool IsOptimal { get; }
    
    Number Width { get; }
  }
}