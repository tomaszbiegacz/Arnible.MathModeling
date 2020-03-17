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
  public class TsvSerializerShared
  {
    protected static readonly char SeparatorChar = '\t';
    protected static readonly ReadOnlyMemory<char> Separator = new[] { SeparatorChar };

    protected static readonly ReadOnlyMemory<char> NewLine = Environment.NewLine.AsMemory();
    protected static TextWriter CreateTextWriter(Stream output) => new StreamWriter(output, Encoding.UTF8);

    static readonly Dictionary<Type, Func<object, ReadOnlyMemory<char>>> _valueSerializators;

    public SerializationMediaType MediaType => SerializationMediaType.TabSeparatedValues;


    private static ReadOnlyMemory<char> ConvertKnown(string value)
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

    static TsvSerializerShared()
    {
      _valueSerializators = new Dictionary<Type, Func<object, ReadOnlyMemory<char>>>
        {
          { typeof(byte),     new ToStringSerializer<byte>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(sbyte),    new ToStringSerializer<sbyte>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(short),    new ToStringSerializer<short>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(ushort),   new ToStringSerializer<ushort>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(int),      new ToStringSerializer<int>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(uint),     new ToStringSerializer<uint>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(long),     new ToStringSerializer<long>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(ulong),    new ToStringSerializer<ulong>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(double),   new ToStringSerializer<double>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(char),     new ToStringSerializer<char>(v => new ReadOnlyMemory<char>(new[] { v })).Serializator },
          { typeof(string),   new ToStringSerializer<string>(v => ConvertKnown(v)).Serializator }
        };
    }

    protected static Func<object, TextWriter, CancellationToken, Task> GetValueSerializer(Type type)
    {
      Func<object, ReadOnlyMemory<char>> valueSerializer;
      if (_valueSerializators.TryGetValue(type, out var serializator))
      {
        valueSerializer = serializator;
      }
      else
      {
        var serializatorAttr = type
          .GetCustomAttributes()
          .OfType<RecordSerializerAttribute>()
          .Where(a => a.MediaType == SerializationMediaType.TabSeparatedValues)
          .SingleOrDefault();

        if (serializatorAttr == null)
        {
          throw new InvalidOperationException($"Unknown serializator for {type}");
        }
        if (serializatorAttr.HasStructure)
          valueSerializer = null;
        else
          valueSerializer = serializatorAttr.Serialize;

        _valueSerializators.Add(type, valueSerializer);
      }

      if (valueSerializer == null)
        return null;
      else
        return (value, stream, token) => stream.WriteAsync(valueSerializer(value), token);
    }
  }

  /// <summary>
  /// TSV file that can be open in Excel
  /// </summary>
  /// <remarks>
  /// https://en.wikipedia.org/wiki/Tab-separated_values
  /// </remarks>
  public class TsvSerializer<T> : TsvSerializerShared, IRecordSerializer<T> where T : struct
  {
    public static RecordSerializerFileStream<T> ToTempFile() => RecordSerializerFileStream<T>.ToTempFile(new TsvSerializer<T>());

    static IEnumerable<PropertyInfo> GetProperties(Type t) => t
      .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
      .Where(p => !p.GetCustomAttributes<RecordPropertyIgnoreAttribute>().Any());

    
    private static string GetHeaderNameWithPrefix(string name, string prefix)
    {
      return prefix == null ? name : $"{prefix}_{name}";
    }

    private static IEnumerable<string> GetHeaders(Type t, string prefix)
    {
      foreach (var property in GetProperties(t))
      {
        Type propertyType = property.PropertyType;
        if (GetValueSerializer(propertyType) != null)
        {
          
          yield return GetHeaderNameWithPrefix(property.Name, prefix);
        }
        else
        {
          foreach (string name in GetHeaders(propertyType, property.Name))
          {
            yield return GetHeaderNameWithPrefix(name, prefix);
          }
        }
      }
    }

    static readonly IEnumerable<ReadOnlyMemory<char>> Headers = GetHeaders(typeof(T), null).Select(n => n.AsMemory()).ToArray();

    public async ValueTask SerializeHeader(Stream output, CancellationToken cancellationToken)
    {
      // stream should be dispose with "output"
      TextWriter writer = CreateTextWriter(output);
      bool useSeparator = false;
      foreach (var header in Headers)
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

    static async Task SerializeRecord(
      object record, 
      TextWriter writer, 
      CancellationToken cancellationToken, 
      bool useSeparator,
      bool isRoot)
    {            
      if (record != null)
      {
        Type recordType = record.GetType();
        Func<object, TextWriter, CancellationToken, Task> valueSerializer = null;
        if (!isRoot)
        {
          valueSerializer = GetValueSerializer(recordType);
        }

        if (valueSerializer == null)
        {
          foreach (var property in GetProperties(recordType))
          {
            object value = property.GetValue(record);
            await SerializeRecord(value, writer, cancellationToken, useSeparator, isRoot: false);            
            useSeparator = true;            
          }
        }
        else
        {
          if (useSeparator)
          {
            await writer.WriteAsync(Separator, cancellationToken);
          }

          await valueSerializer(record, writer, cancellationToken);
        }
      }
      else
      {
        if (useSeparator)
        {
          await writer.WriteAsync(Separator, cancellationToken);
        }
      }      
    }

    public async ValueTask SerializeRecord(T record, Stream output, CancellationToken cancellationToken)
    {
      // stream should be disposed with "output"
      TextWriter writer = CreateTextWriter(output);      
      await SerializeRecord(record, writer, cancellationToken, useSeparator: false, isRoot: true);

      await writer.WriteAsync(NewLine);
      await writer.FlushAsync();
    }
  }
}
