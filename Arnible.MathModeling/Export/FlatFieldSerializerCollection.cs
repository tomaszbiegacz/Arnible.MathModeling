using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  interface IFlatFieldSerializerCollection : IEnumerable<IFlatFieldSerializer>
  {
    Type FieldsType { get; }

    Task SerializeHeader(TextWriter writer, ReadOnlyMemory<char> separator, CancellationToken cancellationToken);
    Task SerializeRecord(object record, TextWriter writer, ReadOnlyMemory<char> separator, CancellationToken cancellationToken);
  }

  abstract class FlatFieldSerializerCollection : IFlatFieldSerializerCollection
  {
    protected static IEnumerable<PropertyInfo> GetProperties(in Type t) => t
      .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
      .Where(p => p.GetCustomAttributes<RecordSerializerIgnoreAttribute>().Count() == 0)
      .ToReadOnlyList();

    protected readonly IEnumerable<FlatFieldSerializer> _fields;

    protected FlatFieldSerializerCollection(in Type type, in IEnumerable<FlatFieldSerializer> fields)
    {
      FieldsType = type ?? throw new ArgumentNullException(nameof(type));
      _fields = fields?.ToReadOnlyList() ?? throw new ArgumentNullException(nameof(fields));
    }

    public Type FieldsType { get; }

    public IEnumerator<IFlatFieldSerializer> GetEnumerator() => _fields.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _fields.GetEnumerator();

    public async Task SerializeHeader(TextWriter writer, ReadOnlyMemory<char> separator, CancellationToken cancellationToken)
    {
      bool useSeparator = false;
      foreach (IFlatFieldSerializer field in _fields)
      {
        if (useSeparator)
        {
          await writer.WriteAsync(separator, cancellationToken);
        }

        await writer.WriteAsync(field.Header, cancellationToken);
        useSeparator = true;
      }
    }

    public async Task SerializeRecord(object record, TextWriter writer, ReadOnlyMemory<char> separator, CancellationToken cancellationToken)
    {
      if(record != null)
      {
        bool useSeparator = false;
        foreach (IFlatFieldSerializer field in _fields)
        {
          if (useSeparator)
          {
            await writer.WriteAsync(separator, cancellationToken);
          }
          await field.Serializer(record, writer, cancellationToken);
          useSeparator = true;
        }
      }      
    }
  }
}
