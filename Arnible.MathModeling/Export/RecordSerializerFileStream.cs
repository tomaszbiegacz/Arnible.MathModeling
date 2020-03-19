using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Arnible.MathModeling.Export
{
  public class RecordSerializerFileStream<T> : RecordSerializerStream<T>
  {
    private static MediaTypeSpecificationAttribute GetMediaTypeSpecification(SerializationMediaType mediaType)
    {
      Type enumType = typeof(SerializationMediaType);
      MemberInfo memInfo = enumType.GetMember(enumType.GetEnumName(mediaType)).Single();
      return (MediaTypeSpecificationAttribute)memInfo.GetCustomAttribute(typeof(MediaTypeSpecificationAttribute));
    }
    
    private static FileInfo NormalizeFileExtension(FileInfo fileInfo, IRecordSerializer<T> serializer)
    {
      string expectedExtension = GetMediaTypeSpecification(serializer.MediaType).FileExtension;
      if (expectedExtension != fileInfo.Extension)
        return new FileInfo(fileInfo.FullName + expectedExtension);
      else
        return fileInfo;
    }

    public FileInfo Destination { get; }

    public static RecordSerializerFileStream<T> ToTempFile(IRecordSerializer<T> serializer)
    {
      return new RecordSerializerFileStream<T>(new FileInfo(Path.GetTempFileName()), serializer);
    }

    public RecordSerializerFileStream(FileInfo fileInfo, IRecordSerializer<T> serializer)
      : this(NormalizeFileExtension(fileInfo, serializer).FullName, serializer)
    {
      // intentionally empty
    }

    private RecordSerializerFileStream(string filePath, IRecordSerializer<T> serializer)
      : base(File.Create(filePath), serializer)
    {
      Destination = new FileInfo(filePath);
    }
  }
}
