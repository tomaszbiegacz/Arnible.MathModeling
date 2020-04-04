using Arnible.MathModeling;
using Arnible.MathModeling.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberMatrix : IEquatable<NumberMatrix>
  {
    class Serializer : ToStringSerializer<NumberMatrix>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    private readonly IReadOnlyList<NumberVector> _rows;

    public NumberMatrix(Number[,] parameters)
    {
      _rows = parameters?.ToJaggedArray().Select(v => v.ToVector()).ToArray();
    }

    private NumberMatrix(IEnumerable<NumberVector> rows)
    {
      _rows = rows.ToArray();
    }

    public static NumberMatrix Repeat(Number element, uint width, uint height)
    {
      return new NumberMatrix(Enumerable.Repeat(NumberVector.Repeat(element, width), (int)height));
    }

    //
    // Properties
    //

    public uint Width => _rows?.FirstOrDefault().Length ?? 0u;

    public uint Height => (uint)(_rows?.Count ?? 0);

    public bool IsZero => Height == 0;

    public Number this[uint column, uint row] => Row(row).ElementAt((int)column);

    public IEnumerable<Number> Row(uint row)
    {
      if (row >= Height)
        throw new InvalidOperationException($"Invalid row: {row}");
      return _rows[(int)row];
    }

    public IEnumerable<Number> Column(uint column)
    {
      if (column >= Width)
        throw new InvalidOperationException($"Invalid column: {column}");

      foreach(NumberVector row in _rows)
      {
        yield return row[column];
      }      
    }

    private IEnumerable<NumberVector> Rows => _rows ?? Enumerable.Empty<NumberVector>();

    //
    // IEquatable
    //

    public bool Equals(NumberMatrix other) => Rows.SequenceEqual(other.Rows);

    public override bool Equals(object obj)
    {
      if (obj is NumberMatrix v)
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
      return "{" + string.Join(" ", Rows) + "}";
    }

    public string ToString(CultureInfo cultureInfo)
    {
      return "{" + string.Join(" ", Rows.Select(v => v.ToString(cultureInfo))) + "}";
    }

    public override int GetHashCode()
    {
      int hc = Height.GetHashCode();
      foreach (var v in Rows)
      {
        hc = unchecked(hc * 314159 + v.GetHashCode());
      }
      return hc;
    }

    public static bool operator ==(NumberMatrix a, NumberMatrix b) => a.Equals(b);
    public static bool operator !=(NumberMatrix a, NumberMatrix b) => !a.Equals(b);

    //
    // Arithmetic operators
    //

    public static NumberMatrix operator +(NumberMatrix a, NumberMatrix b) => new NumberMatrix(a.Rows.ZipDefensive(b.Rows, (va, vb) => va + vb));
    public static NumberMatrix operator -(NumberMatrix a, NumberMatrix b) => new NumberMatrix(a.Rows.ZipDefensive(b.Rows, (va, vb) => va - vb));

    //
    // Design operations
    //

    public NumberMatrix AppendColumn(Number value)
    {
      return new NumberMatrix(Rows.Select(row => row.Append(value).ToVector()));
    }

    public NumberMatrix RemoveColumn(uint pos)
    {
      return new NumberMatrix(Rows.Select(row => row.ExcludeAt(pos).ToVector()));
    }

    public NumberMatrix DuplicateRow(uint pos)
    {
      return new NumberMatrix(Rows.DuplicateAt(pos));
    }

    public NumberMatrix RemoveRow(uint pos)
    {
      return new NumberMatrix(Rows.ExcludeAt(pos));
    }
  }
}
