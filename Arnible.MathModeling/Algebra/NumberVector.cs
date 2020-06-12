using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberVector : IEquatable<NumberVector>, IEquatable<Number>, IReadOnlyList<Number>
  {
    private readonly static IReadOnlyList<Number> Zero = ImmutableList<Number>.Empty.Add(0);

    class Serializer : ToStringSerializer<NumberVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public static NumberVector Repeat(Number value, uint length)
    {
      if (value == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(LinqEnumerable.Repeat(value, length).ToImmutableArray());
      }
    }

    public static NumberVector FirstNonZeroValueAt(uint pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }
      
      return new NumberVector(LinqEnumerable.Repeat<Number>(0, pos).Append(value).ToImmutableArray());      
    }

    private readonly ImmutableArray<Number> _values;

    private static ImmutableArray<Number> GetNormalizedVector(IEnumerable<Number> parameters)
    {
      List<Number> result = new List<Number>(parameters ?? LinqEnumerable.Empty<Number>());
      while (result.Count > 0 && result[result.Count - 1] == 0)
      {
        result.RemoveAt(result.Count - 1);
      }
      return result.ToImmutableArray();
    }

    internal static NumberVector Create(IEnumerable<Number> parameters)
    {
      return new NumberVector(GetNormalizedVector(parameters));
    }

    public NumberVector(params Number[] parameters)
      : this(GetNormalizedVector(parameters))
    {
      // intentionally empty
    }

    private NumberVector(ImmutableArray<Number> parameters)
    {
      _values = parameters;
    }

    public static implicit operator NumberVector(Number v) => new NumberVector(v);
    public static implicit operator NumberVector(double v) => new NumberVector(v);

    //
    // Properties
    //    

    private IReadOnlyList<Number> Values => _values.IsDefaultOrEmpty ? Zero : _values;

    public Number this[uint pos]
    {
      get
      {
        if (pos >= Length)
          throw new InvalidOperationException($"Invalid index: {pos}");

        return Values[(int)pos];
      }
    }

    public uint Length => (uint)(Values.Count);

    public Number GetOrDefault(uint pos)
    {
      if (pos >= Length)
        return 0;
      else
        return Values[(int)pos];
    }

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

    public bool Equals(NumberVector other) => Values.SequenceEqual(other.Values);

    public bool Equals(Number other)
    {
      if (Length == 1)
      {
        return Values[0] == other;
      }
      else
      {
        return false;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is NumberVector v)
      {
        return Equals(v);
      }
      else if (obj is Number v2)
      {
        return Equals(v2);
      }
      else
      {
        return false;
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      if (Values.Count == 1)
      {
        return Values[0].ToString(cultureInfo);
      }
      else
      {
        return "[" + string.Join(" ", Values.Select(v => v.ToString(cultureInfo))) + "]";
      }
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

    public static bool operator ==(Number a, NumberVector b) => b.Equals(a);
    public static bool operator !=(Number a, NumberVector b) => !b.Equals(a);

    public static bool operator ==(NumberVector a, Number b) => a.Equals(b);
    public static bool operator !=(NumberVector a, Number b) => !a.Equals(b);

    //
    // query operators
    //

    public NumberVector Transform(Func<Number, Number> transformation)
    {
      return Create(Values.Select(transformation));
    }

    public NumberVector Transform(Func<uint, Number, Number> transformation)
    {
      return Create(Values.Select(transformation));
    }    

    public NumberArray ToArray(uint size)
    {
      if(size < Length)
      {
        throw new ArgumentException(nameof(size), $"requested {size} whereas minimum is {Length}");
      }
      NumberTranslationVector translation = new NumberTranslationVector(this);
      return translation.Translate(NumberArray.Repeat(0, size));
    }

    //
    // Arithmetic operators
    //

    public static NumberVector operator +(NumberVector a, NumberVector b) => a.Values.Zip(b.Values, (va, vb) => (va ?? 0) + (vb ?? 0)).ToVector();
    public static NumberVector operator -(NumberVector a, NumberVector b) => a.Values.Zip(b.Values, (va, vb) => (va ?? 0) - (vb ?? 0)).ToVector();

    public static NumberVector operator /(NumberVector a, Number b) => new NumberVector(a.Values.Select(v => v / b).ToImmutableArray());

    public static NumberVector operator *(NumberVector a, Number b)
    {
      if (b == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(a.Values.Select(v => v * b).ToImmutableArray());
      }
    }

    public static NumberVector operator *(Number a, NumberVector b) => b * a;
  }
}
