using System;
using System.Collections;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public readonly struct NumberVectorTransposition : IEquatable<NumberVectorTransposition>, IEnumerable<Number>
  {
    private readonly NumberVector _change;

    public NumberVectorTransposition(NumberVector change)
    {
      _change = change;
    }

    public bool Equals(NumberVectorTransposition other) => other._change == _change;

    public override string ToString() => _change.ToString();

    public override bool Equals(object obj)
    {
      if (obj is NumberVectorTransposition v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode() => _change.GetHashCode();

    public static bool operator ==(NumberVectorTransposition a, NumberVectorTransposition b) => a.Equals(b);
    public static bool operator !=(NumberVectorTransposition a, NumberVectorTransposition b) => !a.Equals(b);

    public static NumberVectorTransposition operator *(NumberVectorTransposition a, double b) => new NumberVectorTransposition(b * a._change);
    public static NumberVectorTransposition operator *(double a, NumberVectorTransposition b) => new NumberVectorTransposition(a * b._change);

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

    public NumberVector Transpose(NumberVector src) => src + _change;    
  }
}
