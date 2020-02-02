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
    public static RecordSerializerFileStream<T> ToTempFile() => RecordSerializerFileStream<T>.ToTempFile(new TsvSerializer<T>());

    class ConvertToWritableString
    {
      private ReadOnlyMemory<char> ConvertKnown(byte value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(sbyte value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(short value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(ushort value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(int value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(uint value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(long value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(ulong value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(double value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(Number value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(NumberVector value) => value.ToString(CultureInfo.InvariantCulture).AsMemory();
      private ReadOnlyMemory<char> ConvertKnown(string value)
      {
        if (string.IsNullOrWhiteSpace(value))
          return default;

        string trimValue = value.Trim();
        if (trimValue.Contains(SeparatorChar))
        {
          throw new ArgumentException($"Value [{value}] contains tab!");
        }
        return trimValue.AsMemory();
      }
      private ReadOnlyMemory<char> ConvertKnown(char value) => new ReadOnlyMemory<char>(new[] { value });

      public ReadOnlyMemory<char> Convert(object value)
      {
        if (value == null)
          return default;

        Type valueType = value.GetType();

        if (valueType == typeof(byte))
          return ConvertKnown((byte)value);
        if (valueType == typeof(sbyte))
          return ConvertKnown((sbyte)value);

        if (valueType == typeof(short))
          return ConvertKnown((short)value);
        if (valueType == typeof(ushort))
          return ConvertKnown((ushort)value);

        if (valueType == typeof(int))
          return ConvertKnown((int)value);
        if (valueType == typeof(uint))
          return ConvertKnown((uint)value);

        if (valueType == typeof(long))
          return ConvertKnown((long)value);
        if (valueType == typeof(ulong))
          return ConvertKnown((ulong)value);

        if (valueType == typeof(double))
          return ConvertKnown((double)value);

        if (valueType == typeof(string))
          return ConvertKnown((string)value);
        if (valueType == typeof(char))
          return ConvertKnown((char)value);

        if (valueType == typeof(Number))
          return ConvertKnown((Number)value);
        if (valueType == typeof(NumberVector))
          return ConvertKnown((NumberVector)value);

        throw new ArgumentException("Unknown serializer");
      }
    }

    static readonly char SeparatorChar = '\t';

    static readonly ReadOnlyMemory<char> Separator = new[] { SeparatorChar };
    static readonly ReadOnlyMemory<char> NewLine = Environment.NewLine.AsMemory();

    static readonly IEnumerable<PropertyInfo> Properties = typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
    static readonly ConvertToWritableString Converter = new ConvertToWritableString();

    public SerializationMediaType MediaType => SerializationMediaType.TabSeparatedValues;

    private TextWriter CreateTextWriter(Stream output) => new StreamWriter(output, Encoding.UTF8);

    public async ValueTask SerializeHeader(Stream output, CancellationToken cancellationToken)
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

    public async ValueTask SerializeRecord(T record, Stream output, CancellationToken cancellationToken)
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

    private static async ValueTask WriteValue(TextWriter writer, object value, CancellationToken cancellationToken)
    {
      await writer.WriteAsync(Converter.Convert(value), cancellationToken);
    }
  }
}
