namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowReferenceFieldWriter<TField> : RecordPerRowFieldSerializer, IReferenceRecordWriter<TField> where TField : class?
  {
    private readonly IReferenceRecordSerializer<TField> _serializer;

    public RecordPerRowReferenceFieldWriter(
      RecordPerRowFieldSerializer parent,
      string fieldName,
      IReferenceRecordSerializer<TField> serializer) : base(parent, fieldName)
    {
      _serializer = serializer;
    }
    
    public void Write(TField record)
    {
      BeginRecord();
      _serializer.Serialize(this, record);
    }
  }
}