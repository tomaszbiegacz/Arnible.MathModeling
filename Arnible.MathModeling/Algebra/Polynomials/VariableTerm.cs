using System;

namespace Arnible.MathModeling.Algebra.Polynomials
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

    public override bool Equals(object? obj)
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

    public bool Equals(VariableTerm other)
    {
      return Variable == other.Variable && Power == other.Power;
    }

    public override int GetHashCode()
    {
      HashCode hashCode = new HashCode();
      hashCode.Add(Variable);
      hashCode.Add(Power);
      return hashCode.ToHashCode();
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
