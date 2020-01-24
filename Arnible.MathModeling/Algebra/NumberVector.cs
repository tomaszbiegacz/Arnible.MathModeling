using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public readonly struct NumberVector : IEquatable<NumberVector>, IReadOnlyCollection<Number>
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
        if (Values.Any())
          return Values.All(an => an == 0);
        else
          return true;
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

    public string ToString(CultureInfo cultureInfo)
    {
      return "[" + String.Join(" ", Values.Select(v => v.ToString(cultureInfo))) + "]";
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

    public IEnumerable<uint> Indexes()
    {
      for (uint i = 0; i < Count; ++i)
      {
        yield return i;
      }
    }

    //
    // arithmetic operators
    //

    public static NumberVector operator +(NumberVector a, NumberVector b) => new NumberVector(a.Values.ZipDefensive(b.Values, (va, vb) => va + vb));
    public static NumberVector operator -(NumberVector a, NumberVector b) => new NumberVector(a.Values.ZipDefensive(b.Values, (va, vb) => va - vb));

    public static NumberVector operator /(NumberVector a, double b) => new NumberVector(a.Values.Select(v => v / b));

    public static NumberVector operator *(NumberVector a, double b) => new NumberVector(a.Values.Select(v => v * b));
    public static NumberVector operator *(double a, NumberVector b) => new NumberVector(b.Values.Select(v => v * a));
  }
}
