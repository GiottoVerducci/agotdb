namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Represents a pair of string and TextFormat.
  /// Eg. a "Hello world" string in bold and red.
  /// </summary>
  public struct FormattedText
  {
    public string Text { get; private set; }
    public TextFormat Format { get; private set; }

    public FormattedText(string text, TextFormat format) : this()
    {
      Text = text;
      Format = format;
    }

    public FormattedText(string text) : this()
    {
      Text = text;
      Format = TextFormat.Regular;
    }
  }
}