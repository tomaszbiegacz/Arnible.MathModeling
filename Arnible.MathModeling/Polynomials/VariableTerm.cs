using System;

namespace Arnible.MathModeling.Polynomials
{
  public readonly struct VariableTerm : IEquatable<VariableTerm>
  {
    public char Variable { get; }

    public uint Power { get; }

    public VariableTerm(char variable, uint power)
    {
      if (power < 1)
      {
        throw new ArgumentException(nameof(power));
      }
      Variable = variable;
      Power = power;
    }

    public override bool Equals(object obj)
    {
      if (obj is VariableTerm v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public bool Equals(VariableTerm other) => Variable == other.Variable && Power == other.Power;

    public override int GetHashCode()
    {
      return Variable.GetHashCode() ^ Power.GetHashCode();
    }

    public override string ToString()
    {
      if(Power == 1)
      {
        return Variable.ToString();
      }
      else
      {
        return $"{Variable.ToString()}{Power.ToSuperscriptString()}";
      }
    }
  }
}
