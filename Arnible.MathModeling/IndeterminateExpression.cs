using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public readonly struct IndeterminateExpression : IEquatable<IndeterminateExpression>, IComparable<IndeterminateExpression>, IPolynomialOperation
  {
    private readonly ElementaryUnaryOperation _modifier;    

    private IndeterminateExpression(char name, ElementaryUnaryOperation modifier, uint power)
    {
      _modifier = modifier;
      Variable = name;
      Power = power;
    }    

    public static implicit operator IndeterminateExpression(char name) => new IndeterminateExpression(name, ElementaryUnaryOperation.Identity, 1);
    public static IndeterminateExpression Sin(char name) => new IndeterminateExpression(name, ElementaryUnaryOperation.Sine, 1);
    public static IndeterminateExpression Cos(char name) => new IndeterminateExpression(name, ElementaryUnaryOperation.Cosine, 1);
    public static double Sin(double value) => SimplifyForConstant(ElementaryUnaryOperation.Sine, value);
    public static double Cos(double value) => SimplifyForConstant(ElementaryUnaryOperation.Cosine, value);

    public override string ToString()
    {
      string powerExpression;
      switch (Power)
      {
        case 0:
          return "1";
        case 1:
          powerExpression = string.Empty;
          break;
        default:
          powerExpression = Power.ToSuperscriptString();
          break;
      }

      switch (_modifier)
      {
        case ElementaryUnaryOperation.Identity:
          return Variable + powerExpression;
        case ElementaryUnaryOperation.Sine:
          return $"Sin{powerExpression}({Variable})";
        case ElementaryUnaryOperation.Cosine:
          return $"Cos{powerExpression}({Variable})";
        default:
          throw new InvalidOperationException("Unknown modifier: " + _modifier);
      }
    }

    public int CompareTo(IndeterminateExpression other)
    {
      int nameCompare = Variable.CompareTo(other.Variable);
      if (nameCompare == 0)
      {
        var modifierCompare = _modifier.CompareTo(other._modifier);
        return modifierCompare == 0 ? Power.CompareTo(other.Power) : modifierCompare;
      }
      else
      {
        return nameCompare;
      }
    }

    public bool Equals(IndeterminateExpression other) => other._modifier == _modifier && other.Variable == Variable && other.Power == Power;

    public override bool Equals(object obj)
    {
      if (obj is IndeterminateExpression v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return Variable.GetHashCode() ^ _modifier.GetHashCode();
    }

    public static bool operator ==(IndeterminateExpression a, IndeterminateExpression b) => a.Equals(b);
    public static bool operator !=(IndeterminateExpression a, IndeterminateExpression b) => !a.Equals(b);

    /*
     * Properties
     */

    public string Signature => $"{_modifier}_{Variable}";

    public bool IsOne => Power == 0;

    public uint Power { get; }

    public char Variable { get; }

    public bool HasUnaryModifier => _modifier != ElementaryUnaryOperation.Identity;

    public double SimplifyForConstant(double value) => SimplifyForConstant(_modifier, value);    

    private static double SimplifyForConstant(ElementaryUnaryOperation modifier, double value)
    {
      switch (modifier)
      {
        case ElementaryUnaryOperation.Identity:
          return value;
        case ElementaryUnaryOperation.Sine:
          if (value.NumericEquals(0)) return 0;
          else if (value.NumericEquals(Angle.RightAngle)) return 1;
          else return Math.Sin(value);
        case ElementaryUnaryOperation.Cosine:
          if (value.NumericEquals(0)) return 1;
          else if (value.NumericEquals(Angle.RightAngle)) return 0;
          else return Math.Cos(value);
        default:
          throw new InvalidOperationException("Unknown modifier: " + modifier);
      }
    }

    public IEnumerable<char> Variables
    {
      get
      {
        if (Variable != default)
          yield return Variable;
      }
    }

    /*
     * Operators
     */

    public static explicit operator char(IndeterminateExpression v)
    {
      if (v.Power == 1 && v._modifier == ElementaryUnaryOperation.Identity)
      {
        return v.Variable;
      }

      throw new InvalidOperationException("Indeterminate expression is not a single variable");
    }

    internal static IEnumerable<IndeterminateExpression> Multiply(IEnumerable<IndeterminateExpression> i1, IEnumerable<IndeterminateExpression> i2)
    {
      using (var ei1 = i1.Order().GetEnumerator())
      using (var ei2 = i2.Order().GetEnumerator())
      {
        bool isEi1Valid = ei1.MoveNext();
        bool isEi2Valid = ei2.MoveNext();
        while (isEi1Valid && isEi2Valid)
        {
          var vi1 = ei1.Current;
          var vi2 = ei2.Current;

          int compareResult = vi1.Variable.CompareTo(vi2.Variable);
          if (compareResult == 0)
            compareResult = vi1._modifier.CompareTo(vi2._modifier);

          if (compareResult < 0)
          {
            yield return vi1;
            isEi1Valid = ei1.MoveNext();
          }
          else if (compareResult > 0)
          {
            yield return vi2;
            isEi2Valid = ei2.MoveNext();
          }
          else
          {
            if (vi1.Power + vi2.Power > 0)
            {
              yield return new IndeterminateExpression(vi1.Variable, vi1._modifier, vi1.Power + vi2.Power);
            }
            isEi1Valid = ei1.MoveNext();
            isEi2Valid = ei2.MoveNext();
          }
        }

        while (isEi1Valid)
        {
          yield return ei1.Current;
          isEi1Valid = ei1.MoveNext();
        }

        while (isEi2Valid)
        {
          yield return ei2.Current;
          isEi2Valid = ei2.MoveNext();
        }
      }      
    }

    public IndeterminateExpression ToPower(uint power)
    {
      if (power == 0)
      {
        throw new ArgumentException(nameof(power));
      }
      return new IndeterminateExpression(Variable, _modifier, Power * power);
    }

    public bool TryDivide(IndeterminateExpression b, out IndeterminateExpression result)
    {
      if (Power < b.Power)
      {
        result = default;
        return false;
      }
      else
      {
        result = new IndeterminateExpression(Variable, _modifier, Power - b.Power);
        return true;
      }
    }

    public PolynomialTerm DerivativeBy(char name)
    {
      if (IsOne)
      {
        throw new InvalidOperationException("Cannot derivative intermediate expresion being IsOne.");
      }
      if (Variable != name)
      {
        throw new InvalidOperationException($"Cannot derivative intermediate expression of {Variable} by {name}.");
      }

      PolynomialTerm result = (double)Power * (PolynomialTerm)(new IndeterminateExpression(Variable, _modifier, Power - 1));
      switch (_modifier)
      {
        case ElementaryUnaryOperation.Identity:
          return result;
        case ElementaryUnaryOperation.Sine:
          return result * (new IndeterminateExpression(Variable, ElementaryUnaryOperation.Cosine, 1));
        case ElementaryUnaryOperation.Cosine:
          return result * -1 * (new IndeterminateExpression(Variable, ElementaryUnaryOperation.Sine, 1));
        default:
          throw new InvalidOperationException("Unknown modifier: " + _modifier);
      }
    }

    /*
     * IPolynomialOperation
     */

    public double Value(IReadOnlyDictionary<char, double> x)
    {
      if (IsOne)
      {
        return 1;
      }
      else
      {
        switch (_modifier)
        {
          case ElementaryUnaryOperation.Identity:
            return x[Variable].ToPower(Power);
          case ElementaryUnaryOperation.Sine:
            return Math.Sin(x[Variable]).ToPower(Power);
          case ElementaryUnaryOperation.Cosine:
            return Math.Cos(x[Variable]).ToPower(Power);
          default:
            throw new InvalidOperationException("Unknown modifier: " + _modifier);
        }
      }
    }
  }
}
