namespace AGoT.AGoTDB.Forms
{
  public class AGoTFilter
  {
    /// <summary>
    /// Value used for the display
    /// </summary>
    public string LongName { get; private set; } 

    /// <summary>
    /// Value that is truly used in the database
    /// </summary>
    public string ShortName { get; private set; }

    public string Column { get; private set; }

    public AGoTFilter(string longName, string column)
    {
      LongName = longName;
      ShortName = longName;
      Column = column;
    }

    public AGoTFilter(string longName, string column, string shortName)
    {
      LongName = longName;
      Column = column;
      ShortName = shortName;
    }

    public override string ToString()
    {
      return LongName;
    }
  }
}