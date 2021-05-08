namespace Arnible.MathModeling.Analysis
{
  public readonly struct Derivative1Value : IValueEquatable<Derivative1Value>
  {
    //
    // Properties
    //

    public Number First { get; init; }
    
    //
    // IEquatable
    // 
    //
    
    public bool Equals(Derivative1Value other) => Equals(in other);

    public bool Equals(in Derivative1Value other)
    {
      return First == other.First;
    }

    public override bool Equals(object? obj)
    {
      return obj is Derivative1Value other && Equals(other);
    }

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }

    public override string ToString()
    {
      return First.ToString();
    }
    
    public static bool operator ==(in Derivative1Value a, in Derivative1Value b) => a.Equals(in b);
    public static bool operator !=(in Derivative1Value a, in Derivative1Value b) => !a.Equals(in b);

    //
    // 
    // IEquatable
    //
  }
}
