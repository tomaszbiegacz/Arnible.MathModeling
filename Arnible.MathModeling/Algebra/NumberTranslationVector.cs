using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public readonly struct NumberTranslationVector : IEquatable<NumberTranslationVector>, IEnumerable<Number>
  {
    private readonly NumberVector _change;

    public NumberTranslationVector(NumberVector change)
    {
      _change = change;
    }

    public bool Equals(NumberTranslationVector other) => other._change == _change;

    public override string ToString() => _change.ToString();

    public override bool Equals(object obj)
    {
      if (obj is NumberTranslationVector v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode() => _change.GetHashCode();

    public static bool operator ==(NumberTranslationVector a, NumberTranslationVector b) => a.Equals(b);
    public static bool operator !=(NumberTranslationVector a, NumberTranslationVector b) => !a.Equals(b);

    public static NumberTranslationVector operator *(NumberTranslationVector a, double b) => new NumberTranslationVector(b * a._change);
    public static NumberTranslationVector operator *(double a, NumberTranslationVector b) => new NumberTranslationVector(a * b._change);

    /*
     * IEnumerable
     */

    public IEnumerator<Number> GetEnumerator() => _change.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _change.GetEnumerator();

    /*
     * Properties
     */

    public bool IsZero => _change.IsZero;

    public Number this[uint pos] => _change[pos];

    public Number GetLengthSquare() => _change.Sum(v => v * v);

    /*
     * Operations
     */

    public NumberVector Translate(NumberVector src) => src + _change;    
  }
}
