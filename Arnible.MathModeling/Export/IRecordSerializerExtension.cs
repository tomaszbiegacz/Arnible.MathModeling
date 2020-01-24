using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public static class IRecordSerializerExtension
  {
    public static async Task Serialize<T>(
      this IRecordSerializer<T> serializer,
      IEnumerable<T> records,
      Stream output,
      CancellationToken cancellationToken) where T : struct
    {
      await serializer.SerializeHeader(output, cancellationToken);
      foreach (T record in records)
      {
        await serializer.SerializeRecord(record, output, cancellationToken);
      }
    }

    public static async Task<FileInfo> SerializeToTempFile<T>(
      this IRecordSerializer<T> serializer,
      IEnumerable<T> records,
      CancellationToken cancellationToken) where T: struct
    {
      FileInfo tempFile = CreateTempFilePath(serializer.MediaType);
      await using (FileStream stream = File.Create(tempFile.FullName))
      {
        await serializer.Serialize(records, stream, cancellationToken);
      }
      return tempFile;
    }

    private static MediaTypeSpecificationAttribute GetMediaTypeSpecification(SerializationMediaType mediaType)
    {
      Type enumType = typeof(SerializationMediaType);
      MemberInfo memInfo = enumType.GetMember(enumType.GetEnumName(mediaType)).Single();
      return (MediaTypeSpecificationAttribute)memInfo.GetCustomAttribute(typeof(MediaTypeSpecificationAttribute));
    }

    private static FileInfo CreateTempFilePath(SerializationMediaType mediaType)
    {
      return new FileInfo(Path.GetTempFileName() + GetMediaTypeSpecification(mediaType).FileExtension);
    }
  }
}
