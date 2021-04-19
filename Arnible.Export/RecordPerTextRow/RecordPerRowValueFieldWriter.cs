namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowValueFieldWriter<TField> : RecordPerRowFieldSerializer, IValueRecordWriter<TField> where TField : struct
  {
    private readonly IValueRecordSerializer<TField> _serializer;

    public RecordPerRowValueFieldWriter(
      RecordPerRowFieldSerializer parent,
      string fieldName,
      IValueRecordSerializer<TField> serializer) : base(parent, fieldName)
    {
      _serializer = serializer;
    }
    
    public void Write(in TField? record)
    {
      BeginRecord();
      _serializer.Serialize(this, in record);
    }
    
    public void Write(in TField record)
    {
      BeginRecord();
      _serializer.Serialize(this, in record);
    }
  }
}