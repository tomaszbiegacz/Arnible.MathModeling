using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public struct NumberVector : IEquatable<NumberVector>, IReadOnlyCollection<Number>
  {
    private readonly Number[] _values;

    public NumberVector(params Number[] parameters)
    {
      _values = parameters?.ToArray();
    }

    public NumberVector(IEnumerable<Number> parameters)
    {
      _values = parameters?.ToArray();
    }

    public static NumberVector Repeat(Number element, uint count)
    {
      return new NumberVector(Enumerable.Repeat(element, (int)count));
    }

    //
    // Properties
    //

    public bool IsZero
    {
      get
      {
        return Values.All(an => an == 0);
      }
    }

    public Number this[uint pos]
    {
      get
      {
        if (pos >= Count)
          throw new InvalidOperationException($"Invalid index: {pos}");
        return _values[pos];
      }
    }

    //
    // IReadOnlyCollection
    //

    private IEnumerable<Number> Values => _values ?? Enumerable.Empty<Number>();

    public IEnumerator<Number> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

    public int Count => _values?.Length ?? 0;

    //
    // IEquatable
    //

    public bool Equals(NumberVector other) => Values.SequenceEqual(other.Values);    

    public override bool Equals(object obj)
    {
      if (obj is NumberVector v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override string ToString()
    {
      return "[" + String.Join(" ", Values) + "]";
    }

    public override int GetHashCode()
    {
      int hc = Count.GetHashCode();
      foreach (var v in Values)
      {
        hc = unchecked(hc * 314159 + v.GetHashCode());
      }
      return hc;
    }

    public static bool operator ==(NumberVector a, NumberVector b) => a.Equals(b);
    public static bool operator !=(NumberVector a, NumberVector b) => !a.Equals(b);

    //
    // query operators
    //

    public NumberVector Transform(Func<Number, Number> transformation)
    {
      return new NumberVector(Values.Select(v => transformation(v)));
    }

    public NumberVector Transform(Func<uint, Number, Number> transformation)
    {
      return new NumberVector(Values.Select((v, i) => transformation((uint)i, v)));
    }

    public NumberVector Reverse()
    {
      return new NumberVector(Values.Reverse());
    }

    //
    // arithmetic operators
    //

    private NumberVector Add(Number[] b)
    {
      if (Count != b?.Length)
        throw new InvalidOperationException("Not comparable");

      return Transform((i, v) => v + b[i]);
    }

    public static NumberVector operator +(NumberVector a, NumberVector b) => a.Add(b._values);
    
    public static NumberVector operator +(NumberVector a, IEnumerable<Number> b) => a.Add(b.ToArray());

    public static NumberVector operator +(IEnumerable<Number> a, NumberVector b) => b.Add(a.ToArray());    
  }
}
