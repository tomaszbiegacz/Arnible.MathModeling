using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Arnible.MathModeling
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberArray : IEquatable<NumberArray>, IReadOnlyList<Number>
  {
    class Serializer : ToStringSerializer<NumberArray>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    private static IReadOnlyList<Number> Empty = ImmutableList<Number>.Empty;

    public static NumberArray Repeat(Number value, uint length)
    {
      return new NumberArray(LinqEnumerable.Repeat(value, length).ToImmutableArray());
    }

    private readonly ImmutableArray<Number> _values;

    internal static NumberArray Create(IEnumerable<Number> src)
    {
      return new NumberArray(src?.ToImmutableArray() ?? ImmutableArray<Number>.Empty);
    }

    public NumberArray(params Number[] parameters)
      : this(parameters.ToImmutableArray())
    {
      // intentionally empty
    }

    private NumberArray(ImmutableArray<Number> parameters)
    {
      _values = parameters;
    }

    //
    // Properties
    //        

    private IReadOnlyList<Number> Values => _values.IsDefaultOrEmpty ? Empty : _values;

    public Number this[uint pos]
    {
      get
      {
        if (pos >= Length)
          throw new InvalidOperationException($"Invalid index: {pos}");

        return _values[(int)pos];
      }
    }

    public uint Length => (uint)(Values.Count);

    public bool IsZero => _values.IsDefaultOrEmpty || _values.All(v => v == 0);

    //
    // IReadOnlyList
    //

    Number IReadOnlyList<Number>.this[int pos] => Values[pos];

    public IEnumerator<Number> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();

    int IReadOnlyCollection<Number>.Count => Values.Count;

    //
    // IEquatable
    //

    public bool Equals(NumberArray other) => Values.SequenceEqual(other.Values);


    public override bool Equals(object obj)
    {
      if (obj is NumberArray v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      return "[" + string.Join(" ", Values.Select(v => v.ToString(cultureInfo))) + "]";
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

    public static bool operator ==(NumberArray a, NumberArray b) => a.Equals(b);
    public static bool operator !=(NumberArray a, NumberArray b) => !a.Equals(b);

    //
    // query operators
    //

    public NumberArray Transform(Func<Number, Number> transformation)
    {
      return new NumberArray(Values.Select(transformation).ToImmutableArray());
    }

    public NumberArray Transform(Func<uint, Number, Number> transformation)
    {
      return new NumberArray(Values.Select(transformation).ToImmutableArray());
    }    
  }
}
