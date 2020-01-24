using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  /// <summary>
  /// TSV file that can be open in Excel
  /// </summary>
  /// <remarks>
  /// https://en.wikipedia.org/wiki/Tab-separated_values
  /// </remarks>
  public class TsvSerializer<T> : IRecordSerializer<T> where T : struct
  {
    static readonly char SeparatorChar = '\t';

    static readonly ReadOnlyMemory<char> Separator = new[] { SeparatorChar };
    static readonly ReadOnlyMemory<char> NewLine = Environment.NewLine.AsMemory();

    static readonly IEnumerable<PropertyInfo> Properties = typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

    public SerializationMediaType MediaType => SerializationMediaType.TabSeparatedValues;

    private TextWriter CreateTextWriter(Stream output) => new StreamWriter(output, Encoding.UTF8);

    public async Task SerializeHeader(Stream output, CancellationToken cancellationToken)
    {
      // stream should be dispose with "output"
      TextWriter writer = CreateTextWriter(output);
      bool useSeparator = false;
      foreach (var header in Properties.Select(p => p.Name.AsMemory()))
      {
        if (useSeparator)
        {
          await writer.WriteAsync(Separator, cancellationToken);
          await writer.WriteAsync(header, cancellationToken);
        }
        else
        {
          await writer.WriteAsync(header, cancellationToken);
          useSeparator = true;
        }
      }
      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }

    public async Task SerializeRecord(T record, Stream output, CancellationToken cancellationToken)
    {
      // stream should be dispose with "output"
      TextWriter writer = CreateTextWriter(output);
      bool useSeparator = false;
      foreach (var property in Properties)
      {
        object value = property.GetValue(record);
        if (useSeparator)
        {
          await writer.WriteAsync(Separator, cancellationToken);
          await WriteValue(writer, value, cancellationToken);
        }
        else
        {
          await WriteValue(writer, value, cancellationToken);
          useSeparator = true;
        }
        
      }
      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }    

    private static async Task WriteValue(TextWriter writer, object value, CancellationToken cancellationToken)
    {
      if(value != null)
      {
        Type valueType = value.GetType();

        if (valueType == typeof(byte))
        {
          await Write(writer, (byte)value, cancellationToken);
          return;
        }
        if (valueType == typeof(sbyte))
        {
          await Write(writer, (sbyte)value, cancellationToken);
          return;
        }

        if (valueType == typeof(short))
        {
          await Write(writer, (short)value, cancellationToken);
          return;
        }
        if (valueType == typeof(ushort))
        {
          await Write(writer, (ushort)value, cancellationToken);
          return;
        }

        if (valueType == typeof(int))
        {
          await Write(writer, (int)value, cancellationToken);
          return;
        }
        if (valueType == typeof(uint))
        {
          await Write(writer, (uint)value, cancellationToken);
          return;
        }

        if (valueType == typeof(long))
        {
          await Write(writer, (long)value, cancellationToken);
          return;
        }
        if (valueType == typeof(ulong))
        {
          await Write(writer, (ulong)value, cancellationToken);
          return;
        }

        if (valueType == typeof(string))
        {
          await Write(writer, (string)value, cancellationToken);
          return;
        }

        if (valueType == typeof(double))
        {
          await Write(writer, (double)value, cancellationToken);
          return;
        }

        if (valueType == typeof(Number))
        {
          await Write(writer, (Number)value, cancellationToken);
          return;
        }
        if (valueType == typeof(NumberVector))
        {
          await Write(writer, (NumberVector)value, cancellationToken);
          return;
        }
      }      
    }

    private static Task Write(TextWriter writer, ReadOnlyMemory<char> value, CancellationToken cancellationToken)
    {
      return writer.WriteAsync(value, cancellationToken);
    }

    private static async Task Write(TextWriter writer, string value, CancellationToken cancellationToken)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        string trimValue = value.Trim();
        if (trimValue.Contains(SeparatorChar))
        {
          throw new ArgumentException($"Value [{value}] contains tab!");
        }
        await Write(writer, trimValue.AsMemory(), cancellationToken);
      }
    }

    private static Task Write(TextWriter writer, byte value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, sbyte value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, short value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, ushort value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, int value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, uint value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, long value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, ulong value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, double value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, Number value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }

    private static Task Write(TextWriter writer, NumberVector value, CancellationToken cancellationToken)
    {
      return Write(writer, value.ToString(CultureInfo.InvariantCulture).AsMemory(), cancellationToken);
    }
  }
}
