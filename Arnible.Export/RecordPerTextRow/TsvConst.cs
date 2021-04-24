namespace Arnible.Export.RecordPerTextRow
{
  static class TsvConst
  {
    /// <summary>
    /// Separator used to create multi word column names
    /// </summary>
    public const string HeaderFieldSubNameSeparator = "_";
    
    /// <summary>
    /// Separator between values in the file
    /// </summary>
    public const string RowFieldSeparator = "\t";
    
    public const string FileExtension = ".tsv";
  }
}