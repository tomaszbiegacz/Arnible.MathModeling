using System;
using System.IO;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  public class TsvFileSerializer : IAsyncDisposable
  {
    private readonly LoggerTextSerializer _writer;

    public TsvFileSerializer()
    {
      Destination = StreamExtensions.GetTempFile(TsvConst.FileExtension);
      _writer = new LoggerTextSerializer(StreamExtensions.CreateTextWriter(Destination));
      
      FieldSerializer = new RecordPerRowSerializerField(
        _writer, 
        headerPartsSeparator: TsvConst.HeaderFieldSeparator,
        fieldSeparator: TsvConst.RowFieldSeparator);
    }

    public FileInfo Destination { get; }
    
    public IRecordFieldSerializer FieldSerializer { get; }
    
    public ValueTask DisposeAsync()
    {
      return _writer.DisposeAsync();
    }
  }
}