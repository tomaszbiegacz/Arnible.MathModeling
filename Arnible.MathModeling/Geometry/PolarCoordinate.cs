namespace Arnible.MathModeling.Geometry
{
  public struct PolarCoordinate
  {
    public Number R { get; }

    public Number Φ { get; }

    public PolarCoordinate(Number r, Number φ)
    {
      R = r;
      Φ = φ;
    }    
  }
}
