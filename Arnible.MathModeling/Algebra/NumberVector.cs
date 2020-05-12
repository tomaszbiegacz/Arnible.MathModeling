using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberVector : IEquatable<NumberVector>, IReadOnlyList<Number>
  {
    class Serializer : ToStringSerializer<NumberVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public static NumberVector Zero(uint length) => Repeat(0, length);

    public static NumberVector Repeat(Number value, uint length) => new NumberVector(LinqEnumerable.Repeat(value, length));

    private readonly IReadOnlyList<Number> _values;

    public NumberVector(params Number[] parameters)
    {
      _values = parameters?.ToArray();
    }

    public NumberVector(IEnumerable<Number> parameters)
    {
      _values = parameters?.ToArray();
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
        if (pos >= Length)
          throw new InvalidOperationException($"Invalid index: {pos}");
        return _values[(int)pos];
      }
    }

    public uint Length => (uint)(_values?.Count ?? 0);

    //
    // IReadOnlyList
    //

    Number IReadOnlyList<Number>.this[int pos] => _values[pos];

    private IEnumerable<Number> Values => _values ?? LinqEnumerable.Empty<Number>();

    public IEnumerator<Number> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

    int IReadOnlyCollection<Number>.Count => (int)Length;

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
      int hc = Length.GetHashCode();
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
      return new NumberVector(Values.Select(transformation));
    }

    public NumberVector Transform(Func<uint, Number, Number> transformation)
    {
      return new NumberVector(Values.Select(transformation));
    }

    public NumberVector Reverse() => new NumberVector(Values.Reverse());

    public IEnumerable<uint> Indexes() => LinqEnumerable.RangeUint(0, Length);

    //
    // Arithmetic operators
    //

    public static NumberVector operator +(NumberVector a, NumberVector b) => new NumberVector(a.Values.ZipDefensive(b.Values, (va, vb) => va + vb));
    public static NumberVector operator -(NumberVector a, NumberVector b) => new NumberVector(a.Values.ZipDefensive(b.Values, (va, vb) => va - vb));

    public static NumberVector operator /(NumberVector a, double b) => new NumberVector(a.Values.Select(v => v / b));

    public static NumberVector operator *(NumberVector a, double b) => new NumberVector(a.Values.Select(v => v * b));
    public static NumberVector operator *(double a, NumberVector b) => new NumberVector(b.Values.Select(v => v * a));
  }
}
