using System;
using System.IO;
using System.Reflection;

namespace Arnible.MathModeling.Export
{
  public class RecordSerializerFileStream<T> : RecordSerializerStream<T> where T: struct
  {
    private static MediaTypeSpecificationAttribute GetMediaTypeSpecification(in SerializationMediaType mediaType)
    {
      Type enumType = typeof(SerializationMediaType);
      MemberInfo memInfo = enumType.GetMember(enumType.GetEnumName(mediaType)).Single();
      return (MediaTypeSpecificationAttribute)memInfo.GetCustomAttribute(typeof(MediaTypeSpecificationAttribute));
    }
    
    private static FileInfo NormalizeFileExtension(in FileInfo fileInfo, in IRecordSerializer<T> serializer)
    {
      string expectedExtension = GetMediaTypeSpecification(serializer.MediaType).FileExtension;
      if (expectedExtension != fileInfo.Extension)
        return new FileInfo(fileInfo.FullName + expectedExtension);
      else
        return fileInfo;
    }

    public FileInfo Destination { get; }

    public static RecordSerializerFileStream<T> ToTempFile(in IRecordSerializer<T> serializer)
    {
      return new RecordSerializerFileStream<T>(new FileInfo(Path.GetTempFileName()), serializer);
    }

    public RecordSerializerFileStream(in FileInfo fileInfo, in IRecordSerializer<T> serializer)
      : this(NormalizeFileExtension(in fileInfo, in serializer).FullName, serializer)
    {
      // intentionally empty
    }

    private RecordSerializerFileStream(in string filePath, in IRecordSerializer<T> serializer)
      : base(File.Create(filePath), in serializer)
    {
      Destination = new FileInfo(filePath);
    }
  }
}
