using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  public readonly struct IndeterminateExpression : 
    IValueEquatable<IndeterminateExpression>, 
    IComparable<IndeterminateExpression>, 
    IPolynomialOperation
  {
    private readonly ElementaryUnaryOperation _modifier;

    private IndeterminateExpression(in char name, in ElementaryUnaryOperation modifier, in uint power)
    {
      _modifier = modifier;
      Variable = name;
      Power = power;
    }

    public static implicit operator IndeterminateExpression(in char name)
    {
      return new IndeterminateExpression(in name, ElementaryUnaryOperation.Identity, 1);
    }

    public static IndeterminateExpression Sin(in char name) => new IndeterminateExpression(in name, ElementaryUnaryOperation.Sine, 1);
    public static IndeterminateExpression Cos(in char name) => new IndeterminateExpression(in name, ElementaryUnaryOperation.Cosine, 1);
    
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
          return $"Sin{powerExpression}({Variable.ToString()})";
        case ElementaryUnaryOperation.Cosine:
          return $"Cos{powerExpression}({Variable.ToString()})";
        default:
          throw new InvalidOperationException("Unknown modifier: " + _modifier);
      }
    }

    public int CompareTo(IndeterminateExpression other)
    {
      int nameCompare = Variable.CompareTo(other.Variable);
      if (nameCompare == 0)
      {
        var modifierCompare = ((int)_modifier).CompareTo((int)other._modifier);
        return modifierCompare == 0 ? Power.CompareTo(other.Power) : modifierCompare;
      }
      else
      {
        return nameCompare;
      }
    }

    public bool Equals(in IndeterminateExpression other) => other._modifier == _modifier && other.Variable == Variable && other.Power == Power;

    public bool Equals(IndeterminateExpression other) => Equals(in other);

    public override bool Equals(object? obj)
    {
      if (obj is IndeterminateExpression v)
      {
        return Equals(in v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return Variable.GetHashCode() ^ (int)_modifier;
    }
    public int GetHashCodeValue() => GetHashCode();

    public static bool operator ==(in IndeterminateExpression a, in IndeterminateExpression b) => a.Equals(in b);
    public static bool operator !=(in IndeterminateExpression a, in IndeterminateExpression b) => !a.Equals(in b);

    /*
     * Properties
     */

    public string Signature => $"{((int)_modifier).ToString()}_{Variable.ToString()}";

    public bool IsOne => Power == 0;

    public uint Power { get; }

    public char Variable { get; }

    public bool HasUnaryModifier => _modifier != ElementaryUnaryOperation.Identity;

    public double SimplifyForConstant(in double value)
    {    
      switch (_modifier)
      {
        case ElementaryUnaryOperation.Identity:
          return value;
        case ElementaryUnaryOperation.Sine:
          return DoubleExtension.RoundedSin(value);
        case ElementaryUnaryOperation.Cosine:
          return DoubleExtension.RoundedCos(value);
        default:
          throw new InvalidOperationException("Unknown modifier: " + _modifier);
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

    public static explicit operator char(in IndeterminateExpression v)
    {
      if (v.Power == 1 && v._modifier == ElementaryUnaryOperation.Identity)
      {
        return v.Variable;
      }

      throw new InvalidOperationException("Indeterminate expression is not a single variable");
    }

    internal static IEnumerable<IndeterminateExpression> Multiply(
      IEnumerable<IndeterminateExpression> i1, 
      IEnumerable<IndeterminateExpression> i2)
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
            compareResult = ((int)vi1._modifier).CompareTo((int)vi2._modifier);

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

    public IndeterminateExpression ToPower(in uint power)
    {
      if (power == 0)
      {
        throw new ArgumentException(nameof(power));
      }
      return new IndeterminateExpression(Variable, _modifier, Power * power);
    }

    public IndeterminateExpression ReducePowerBy(in uint power)
    {
      if (power == 0 || power > Power)
      {
        throw new ArgumentException(nameof(power));
      }
      return new IndeterminateExpression(Variable, _modifier, Power - power);
    }

    public bool TryDivide(in IndeterminateExpression b, out IndeterminateExpression result)
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

    public PolynomialTerm DerivativeBy(in char name)
    {
      if (IsOne)
      {
        throw new InvalidOperationException("Cannot derivative intermediate expresion being IsOne.");
      }
      if (Variable != name)
      {
        throw new InvalidOperationException($"Cannot derivative intermediate expression of {Variable} by {name}.");
      }

      PolynomialTerm result = Power * (PolynomialTerm)(new IndeterminateExpression(Variable, _modifier, Power - 1));
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
        double value = x[Variable];
        switch (_modifier)
        {
          case ElementaryUnaryOperation.Identity:
            return value.ToPower(Power);
          case ElementaryUnaryOperation.Sine:
            return DoubleExtension.RoundedSin(value).ToPower(Power);
          case ElementaryUnaryOperation.Cosine:
            return DoubleExtension.RoundedCos(value).ToPower(Power);
          default:
            throw new InvalidOperationException("Unknown modifier: " + _modifier);
        }
      }
    }
  }
}
