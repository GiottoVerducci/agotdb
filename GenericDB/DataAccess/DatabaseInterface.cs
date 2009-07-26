// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright � 2007, 2008, 2009 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// � A Game of Thrones 2005 George R. R. Martin
// � A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// � Le Tr�ne de Fer JCC 2005-2007 Stratag�mes �ditions / X�nomorphe S�rl

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using AGoT.AGoTDB.Forms;
using Beyond.ExtendedControls;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Provides an interface with the card database.
  /// </summary>
  public sealed class DatabaseInterface
  {
    private static readonly DatabaseInterface fSingleton = new DatabaseInterface();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit (for singleton template implementation)
    static DatabaseInterface() { }

    /// <summary>
    /// Gets the unique shared singleton instance of this class.
    /// </summary>
    public static DatabaseInterface Singleton
    {
      get { return fSingleton; }
    }

    private OleDbConnection fHDbConnection; // human database
    private OleDbConnection fDbConnection; // extended database
    private readonly List<DatabaseInfo> fDatabaseInfos;
    private const string HDataBaseFilename = "AGoT.mdb";
    private const string DataBaseFilename = "AGoTEx.mdb";
    private const string DataBasePath = @"Databases\";

    public static class TableName
    {
      public const string Main = "AGoT-db";
      public const string FilterText = "TableFilterText";
      public const string House = "TableHouse";
      public const string Icon = "TableIcon";
      public const string Keyword = "TableKeyword";
      public const string Mechanism = "TableMecanism";
      public const string Provides = "TableProvides";
      public const string Set = "TableSet";
      public const string Trigger = "TableTrigger";
      public const string Type = "TableType";
      public const string Virtue = "TableVirtue";
      public const string Version = "TableVersion";
    }

    public bool ConnectedToDatabase
    {
      get;
      private set;
    }

    public OleDbConnection DbConnection
    {
      get { return fDbConnection; }
    }

    public List<DatabaseInfo> DatabaseInfos { get { return fDatabaseInfos; } }

    private DatabaseInterface()
    {
      if (UserSettings.Singleton.ReadBool("Startup", "CreateExtendedDB", true))
      {
        if (!ConnectToHumanDatabase())
          return;
        if (!ConnectToExtendedDatabase())
          return;
        ConnectedToDatabase = true; // required by ConvertDatabase
        ReadDatabaseInformations(fHDbConnection, out fDatabaseInfos);
        if (ConvertDatabase())
        {
          UserSettings.Singleton.WriteBool("Startup", "CreateExtendedDB", false);
          UserSettings.Singleton.Save();
          ConnectedToDatabase = ConnectToExtendedDatabase();
        }
        else
          ConnectedToDatabase = false;
      }
      else
      {
        ConnectedToDatabase = ConnectToExtendedDatabase();
        ReadDatabaseInformations(fDbConnection, out fDatabaseInfos);
      }
      if ((fDatabaseInfos.Count > 0) && (ApplicationSettings.ApplicationVersion.CompareTo(fDatabaseInfos[0].MinimalAgotDbVersion) < 0))
        if (DialogResult.No == MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrMinimalSoftwareVersionRequired, fDatabaseInfos[0].MinimalAgotDbVersion, ApplicationSettings.ApplicationVersion),
          Resource1.ErrMinimalSoftwareVersionRequiredTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
          ConnectedToDatabase = false;
    }

    /// <summary>
    /// Establishes a connection to the database.
    /// </summary>
    /// <returns>True if the method succeeds, false otherwise</returns>
    private static bool ConnectToDatabase(ref OleDbConnection dbConnection, string dbFilename)
    {
      // There is not a 64 bit version of jet that is why there's an error under winxp64.
      // To force your app to use the 32 bit change the target cpu to x86 in the advanced compiler options.
      // Project Properties...Compile tab...Advanced Compile Options button...Target CPU dropdown
      // (this option is not supported in the Express versions)
      // Additionnal info :  http://support.microsoft.com/kb/278604
      // "I used regsvr32 with  the five dll's under the Jet 4.0 OLD DB provider and it worked for me."
      try
      {
        dbConnection = new OleDbConnection();
        dbConnection.ConnectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""|DataDirectory|\{0}""", dbFilename);
        dbConnection.Open();
        return true;
      }
      catch (FileNotFoundException)
      {
        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrDatabaseNotFound, dbFilename),
          Resource1.ErrDatabaseNotFoundTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      catch
      {
        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrInvalidDatabase, dbFilename),
          Resource1.ErrInvalidDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    /// <summary>
    /// Reads the database informations. Those informations are sorted by VersionId, descending, which means
    /// that the more recent version of the informations is at index 0.
    /// </summary>
    /// <param name="dbConnection">The connection used to access the database.</param>
    /// <param name="dbInformations">The list of informations about the database.</param>
    private void ReadDatabaseInformations(OleDbConnection dbConnection, out List<DatabaseInfo> dbInformations)
    {
      dbInformations = new List<DatabaseInfo>();
      var query = String.Format("SELECT * FROM [{0}] ORDER BY [VersionId] DESC", TableName.Version);
      DataTable data = GetResultFromRequest(query, dbConnection, null);
      foreach (DataRow row in data.Rows)
      {
        int dbVersion;
        if (!Int32.TryParse(GetRowValue(row, "VersionId"), out dbVersion))
          return;
        DateTime dbDate;
        SoftwareVersion dbSoftwareVersion;
        dbInformations.Add(new DatabaseInfo(dbVersion,
          DateTime.TryParse(GetRowValue(row, "Date"), out dbDate) ? dbDate : (DateTime?)null,
          SoftwareVersion.TryParse(GetRowValue(row, "MinimalAGoTDBVersion"), out dbSoftwareVersion) ? dbSoftwareVersion : null,
          GetRowValue(row, "Comments")));
      }
    }

    /// <summary>
    /// Establishes a connection to the human database.
    /// </summary>
    /// <returns>True if the method succeeds, false otherwise</returns>
    private bool ConnectToHumanDatabase()
    {
      return ConnectToDatabase(ref fHDbConnection, DataBasePath + HDataBaseFilename);
    }

    /// <summary>
    /// Establishes a connection to the extended database.
    /// </summary>
    /// <returns>True if the method succeeds, false otherwise</returns>
    private bool ConnectToExtendedDatabase()
    {
      return ConnectToDatabase(ref fDbConnection, DataBasePath + DataBaseFilename);
    }

    /// <summary>
    /// Indicates for a reference table what columns are used, and what they contain.
    /// </summary>
    public enum TableType
    {
      /// <summary>
      /// Only the "Value" column must be used. The name of the column in the main db must be given as an argument.
      /// </summary>
      Value,
      /// <summary>
      /// Use the "Value" column for the display, and use the "Key" column to know in which column of the main db this filter applies.
      /// </summary>
      ValueKey,
      /// <summary>
      /// Same as Value but the "Value" column contains the long name (display name) and the "ShortName" column contains the name that appears in the main db.
      /// </summary>
      ValueShortName,
      /// <summary>
      /// The id value is used to identify the value selected (we don't want to use the index in the list of items).
      ///    </summary>
      ValueId
    };

    private DataTable GetResultFromRequest(string request, OleDbConnection aDbConnection, CommandParameters parameters)
    {
      var table = new DataTable();
      table.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
      if (ConnectedToDatabase)
      {
        var command = new OleDbCommand(request, aDbConnection);
        if (parameters != null)
          parameters.AppendToCommand(command);

        using (var dbDataAdapter = new OleDbDataAdapter(command))
        {
          dbDataAdapter.Fill(table);
        }
      }
      return table;
    }

    // TODO : remplacer toutes les requ�tes avec String.Format("...{n}...") par des appels avec CommandParameters
    public DataTable GetResultFromRequest(string request, CommandParameters parameters)
    {
      return GetResultFromRequest(request, DbConnection, parameters);
    }

    public DataTable GetResultFromRequest(string request)
    {
      return GetResultFromRequest(request, DbConnection, null);
    }

    /// <summary>
    /// Converts the "human" version of the database to the advanced format of the database.
    /// 1) cardtypes from string to integer (eg. Attachment becomes 2)
    /// 2) house(s) from string to integer (bitwise combinaison) (eg. Stark/Lannister becomes 1|2 = 3)
    /// 3) traits, keywords and cardtext are left as text.
    /// 4) Doomed, Endless, Military, Intrigue, Power, War, Holy, Noble, Learned, Shadow, Multiplayer from string to Yes/No
    /// 5) Cost, Strength, Income, Initiative, Claim, Influence from string to integer (with 'X' converted to -1)
    /// A column "Style" is added for each colum except UniversalId. It allows the base to have the plain values and the style separated. Otherwise,
    /// the style may interfer with the search functions.
    /// Styles are: errata (in human mode: {errata}), trait (~trait~)
    /// Style is encoded in the Style column as follows: style1, start1-stop1; ... ;styleN, startN-stopN;
    /// For non-text column, start and stop are ignored. The style is applied to the whole field.
    /// </summary>
    /// <returns>True if the conversion was successful, False otherwise.</returns>
    public bool ConvertDatabase()
    {
      string query = String.Format("DELETE FROM [{0}]", TableName.Main);
      GetResultFromRequest(query, fDbConnection, null);

      query = String.Format("SELECT * FROM [{0}]", TableName.Main);
      DataTable humanData = GetResultFromRequest(query, fHDbConnection, null);

      using (var dbDataAdapter = new OleDbDataAdapter(query, fDbConnection))
      {
        var builder = new OleDbCommandBuilder(dbDataAdapter);
        builder.QuotePrefix = "[";
        builder.QuoteSuffix = "]";

        var dataSet = new DataSet();
        dataSet.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
        dbDataAdapter.Fill(dataSet);

        foreach (DataRow row in humanData.Rows)
        {
          try
          {
            ConvertCard(row, dataSet.Tables[0].Rows);
          }
          catch
          {
            MessageBox.Show("Une erreur de conversion est survenue sur la carte " + row["UniversalId"]);
            return false;
          }
        }
        dbDataAdapter.Update(dataSet);
      }
      return true;
    }

    private static string GetRowValue(DataRow row, string columnName)
    {
      return row[columnName].ToString();
    }

    /// <summary>
    /// Reads a formatted string value from a row and returns a FormattedValue object containing the value read
    /// and its format.
    /// The read string must use the { } (errata) and ~ ~ (trait) formats.
    /// </summary>
    /// <param name="row">The row from which to read the string value.</param>
    /// <param name="columnName">The colum in the row containing the string value to read.</param>
    /// <returns>A formatted value.</returns>
    public static FormattedValue<string> ExtractFormattedStringValueFromRow(DataRow row, string columnName)
    {
      string value = row[columnName].ToString();

      var bounds = new List<int>();
      var formats = new List<TextFormat>();

      var special = new List<char>();
      string newValue = "";

      int dec = 0;
      for (var i = 0; i < value.Length; ++i)
      {
        bool boundChar = true;
        switch (value[i])
        {
          case '{': special.Add('{'); formats.Add(Card.ErrataFormat); break;
          case '}':
            if (special[special.Count - 1] != '{')
              throw new ApplicationException(String.Format(CultureInfo.InvariantCulture, "Mismatch in database: '}' unexpected, column = {0}, value = {1}", columnName, value));
            special.RemoveAt(special.Count - 1);
            break;
          case '~':
            if (special[special.Count - 1] != '~')
            {
              special.Add('~');
              formats.Add(Card.TraitsFormat);
            }
            else
              special.RemoveAt(special.Count - 1);
            break;
          default:
            newValue += value[i];
            boundChar = false;
            break;
        }
        if (boundChar)
          bounds.Add(i - dec++);
      }
      if (special.Count != 0)
        throw new ApplicationException(String.Format(CultureInfo.InvariantCulture, "Mismatch in database: {0} formatting characters left, column = {1}, value = {2}", special.Count, columnName, value));

      var formatSections = new List<FormatSection>();
      for (var i = 0; i < formats.Count; ++i)
        formatSections.Add(new FormatSection(bounds[i * 2], bounds[i * 2 + 1], formats[i]));
      return new FormattedValue<string>(newValue, formatSections);
    }

    /// <summary>
    /// Reads a string value from a row and returns a FormattedValue containing the bitwise combinaison
    /// representing that value and its format.
    /// The value(s) associated to the string(s) is/are found in the refTableName. If there is more than
    /// one string value, the string values must be separated by a '/' character.
    /// If one of the value is errated, we consider that the whole combination of values is errated.
    /// Eg. The value in the row is 'Attachment'. We look in the refTableName table and find 4. We return that value.
    /// Eg2. The value in the row is 'Stark/{Lannister}'. We return 1+2=3 and the value is marked as errated.
    /// </summary>
    /// <param name="row">The row from which to read the string value.</param>
    /// <param name="columnName">The colum in the row containing the string value to read.</param>
    /// <param name="refTableName">The table in which we can find the association between a string value and its bit value.</param>
    /// <returns>A formatted value.</returns>
    public FormattedValue<int> ExtractFormattedIntValueFromRow(DataRow row, string columnName, string refTableName)
    {
      FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName);
      string[] values = stringValue.Value.Split('/');

      int intValue = 0;
      var formatSections = new List<FormatSection>();
      if (stringValue.Formats.Count != 0)
        formatSections.Add(new FormatSection(0, 0, Card.ErrataFormat));

      for (var i = 0; i < values.Length; ++i)
      {
        DataTable dt =
          GetResultFromRequest(String.Format("SELECT Id FROM {0} WHERE Value LIKE '{1}'", refTableName, values[i]),
                               fHDbConnection, null);
        intValue += Int32.Parse(dt.Rows[0]["Id"].ToString(), CultureInfo.InvariantCulture);
      }

      return new FormattedValue<int>(intValue, formatSections);
    }

    /// <summary>
    /// Reads a string value from a row and returns a FormattedValue containing the boolean represented
    /// by the string and its format.
    /// </summary>
    /// <param name="row">The row from which to read the string value.</param>
    /// <param name="columnName">The colum in the row containing the string value to read.</param>
    /// <returns>A formatted value.</returns>
    public static FormattedValue<bool?> ExtractFormattedBoolValueFromRow(DataRow row, string columnName)
    {
      FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName);

      string value = stringValue.Value.ToUpper();

      bool? bvalue = null;
      if (!string.IsNullOrEmpty(value))
        bvalue = (value == "YES");
      var formatSections = new List<FormatSection>();
      if (stringValue.Formats.Count != 0)
        formatSections.Add(new FormatSection(0, 0, Card.ErrataFormat));
      return new FormattedValue<bool?>(bvalue, formatSections);
    }

    /// <summary>
    /// Reads a string value from a row and returns a FormattedValue containing the XInt value represented
    /// by the string and its format.
    /// </summary>
    /// <param name="row">The row from which to read the string value.</param>
    /// <param name="columnName">The colum in the row containing the string value to read.</param>
    /// <returns>A formatted value.</returns>
    public static FormattedValue<XInt> ExtractFormattedXIntValueFromRow(DataRow row, string columnName)
    {
      FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName);

      string value = stringValue.Value.ToUpper();

      XInt xvalue = string.IsNullOrEmpty(value) ? null : ((value == "X") ? new XInt() : new XInt(Int32.Parse(value, CultureInfo.InvariantCulture)));

      var formatSections = new List<FormatSection>();
      if (stringValue.Formats.Count != 0)
        formatSections.Add(new FormatSection(0, 0, Card.ErrataFormat));
      return new FormattedValue<XInt>(xvalue, formatSections);
    }

    /// <summary>
    /// Gets the string representing a given bool? value. If the value is null, the null object is returned.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representing the value, or null if the value is null.</returns>
    private static string NullableBoolToString(bool? value)
    {
      return value.HasValue ? value.ToString() : null;
    }

    /// <summary>
    /// Gets the string representing a given bool value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representing the value.</returns>
    private static string BoolToString(bool value)
    {
      return value.ToString();
    }

    /// <summary>
    /// Gets the string representing a given XInt value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representing the value.</returns>
    private static string XIntToString(XInt value)
    {
      if (value == null)
        return null;
      return value.IsX ? "-1" : value.Value.ToString(CultureInfo.CurrentCulture);
    }

    public void ConvertCard(DataRow sourceRow, DataRowCollection destinationRows)
    {
      Int32 UniversalId;
      FormattedValue<string> Name, Traits, Keywords, Text, Set, OriginalName;
      FormattedValue<int> Type, House;
      FormattedValue<bool?> Unique, Doomed, Endless, Military, Intrigue, Power, War, Holy, Noble, Learned, Shadow, Multiplayer;
      FormattedValue<XInt> Cost, Strength, Income, Initiative, Claim, Influence;

      // ZONK pas possible de caster directement?
      UniversalId = Int32.Parse(GetRowValue(sourceRow, "UniversalId"), CultureInfo.InvariantCulture);

      Name = ExtractFormattedStringValueFromRow(sourceRow, "Name");
      Traits = ExtractFormattedStringValueFromRow(sourceRow, "Traits");
      Keywords = ExtractFormattedStringValueFromRow(sourceRow, "Keywords");
      Text = ExtractFormattedStringValueFromRow(sourceRow, "Text");
      Set = ExtractFormattedStringValueFromRow(sourceRow, "Set");
      OriginalName = ExtractFormattedStringValueFromRow(sourceRow, "OriginalName");

      Type = ExtractFormattedIntValueFromRow(sourceRow, "Type", "TableType");
      House = ExtractFormattedIntValueFromRow(sourceRow, "House", "TableHouse");

      Unique = ExtractFormattedBoolValueFromRow(sourceRow, "Unique");
      Doomed = ExtractFormattedBoolValueFromRow(sourceRow, "Doomed");
      Endless = ExtractFormattedBoolValueFromRow(sourceRow, "Endless");
      Military = ExtractFormattedBoolValueFromRow(sourceRow, "Military");
      Intrigue = ExtractFormattedBoolValueFromRow(sourceRow, "Intrigue");
      Power = ExtractFormattedBoolValueFromRow(sourceRow, "Power");
      War = ExtractFormattedBoolValueFromRow(sourceRow, "War");
      Holy = ExtractFormattedBoolValueFromRow(sourceRow, "Holy");
      Noble = ExtractFormattedBoolValueFromRow(sourceRow, "Noble");
      Learned = ExtractFormattedBoolValueFromRow(sourceRow, "Learned");
      Shadow = ExtractFormattedBoolValueFromRow(sourceRow, "Shadow");
      Multiplayer = ExtractFormattedBoolValueFromRow(sourceRow, "Multiplayer");

      Cost = ExtractFormattedXIntValueFromRow(sourceRow, "Cost");
      Strength = ExtractFormattedXIntValueFromRow(sourceRow, "Strength");
      Income = ExtractFormattedXIntValueFromRow(sourceRow, "Income");
      Initiative = ExtractFormattedXIntValueFromRow(sourceRow, "Initiative");
      Claim = ExtractFormattedXIntValueFromRow(sourceRow, "Claim");
      Influence = ExtractFormattedXIntValueFromRow(sourceRow, "Influence");

#if DEBUG
      // we compare the case of the name of the card in its title and its cardtext.
      int index;
      string text = Text.Value;
      do
      {
        index = text.IndexOf(Name.Value, StringComparison.CurrentCultureIgnoreCase);
        if (index != -1)
        {
          if (string.Compare(text.Substring(index, Name.Value.Length), Name.Value, StringComparison.CurrentCulture) != 0)
            System.Diagnostics.Debug.WriteLine(CultureInfo.CurrentCulture, String.Format("{0} {1}", Name.Value, UniversalId));
          text = text.Substring(index + Name.Value.Length);
        }
      } while (index != -1);
#endif

      destinationRows.Add(UniversalId.ToString(CultureInfo.InvariantCulture),
                   Name.Value, Name.FormatsToString(),
                   Type.Value, (Type.Formats.Count > 0).ToString(),
                   (House.Value == 0).ToString(),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Stark) != 0),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Lannister) != 0),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Baratheon) != 0),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Greyjoy) != 0),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Martell) != 0),
                   BoolToString((House.Value & (Int32)Card.CardHouse.Targaryen) != 0),
                   BoolToString(House.Formats.Count > 0),
                   NullableBoolToString(Unique.Value), BoolToString(Unique.Formats.Count > 0),
                   Traits.Value, Traits.FormatsToString(),
                   Keywords.Value, Keywords.FormatsToString(),
                   Text.Value, Text.FormatsToString(),
                   NullableBoolToString(Doomed.Value), BoolToString(Doomed.Formats.Count > 0),
                   NullableBoolToString(Endless.Value), BoolToString(Endless.Formats.Count > 0),
                   XIntToString(Cost.Value), BoolToString(Cost.Formats.Count > 0),
                   XIntToString(Strength.Value), BoolToString(Strength.Formats.Count > 0),
                   NullableBoolToString(Military.Value), BoolToString(Military.Formats.Count > 0),
                   NullableBoolToString(Intrigue.Value), BoolToString(Intrigue.Formats.Count > 0),
                   NullableBoolToString(Power.Value), BoolToString(Power.Formats.Count > 0),
                   NullableBoolToString(War.Value), BoolToString(War.Formats.Count > 0),
                   NullableBoolToString(Holy.Value), BoolToString(Holy.Formats.Count > 0),
                   NullableBoolToString(Noble.Value), BoolToString(Noble.Formats.Count > 0),
                   NullableBoolToString(Learned.Value), BoolToString(Learned.Formats.Count > 0),
                   NullableBoolToString(Shadow.Value), BoolToString(Shadow.Formats.Count > 0),
                   XIntToString(Income.Value), BoolToString(Income.Formats.Count > 0),
                   XIntToString(Initiative.Value), BoolToString(Initiative.Formats.Count > 0),
                   XIntToString(Claim.Value), BoolToString(Claim.Formats.Count > 0),
                   XIntToString(Influence.Value), BoolToString(Influence.Formats.Count > 0),
                   BoolToString(Multiplayer.Value.Value), BoolToString(Multiplayer.Formats.Count > 0), // Multiplayer shouldn't be null
                   Set.Value, Set.FormatsToString(),
                   OriginalName.Value, OriginalName.FormatsToString());
    }


    /// <summary>
    /// Adds a filter menu item.
    /// </summary>
    /// <param name="mi">The parent menu item of the created menu item</param>
    /// <param name="tableName">The name of the table where to get the data from</param>
    /// <param name="tb">The textbox which the filter must be applied to</param>
    /// <param name="ecb">The checkbox associated to the textbox, so it can be checked automatically</param>
    public void UpdateFilterMenu(ToolStripMenuItem mi, string tableName, TextBox tb, ExtendedCheckBox ecb)
    {
      DataTable table = GetResultFromRequest(String.Format("SELECT * FROM [{0}] ORDER BY Id", tableName));
      for (var i = 1; i < table.Rows.Count; ++i)
      {
        var item = new ToolStripMenuItem(table.Rows[i]["Value"].ToString(), null,
          delegate(object sender, EventArgs e)
          {
            var tag = ((ToolStripMenuItem)sender).Tag;
            if (String.IsNullOrEmpty(tb.Text))
              tb.Text = tag.ToString();
            else
              tb.Text += ";" + tag;
            if (ecb.CheckState == CheckState.Unchecked)
              ecb.CheckState = CheckState.Checked;
          });
        item.Tag = table.Rows[i]["Filter"].ToString();
        mi.DropDownItems.Add(item);
      }
    }

    /// <summary>
    /// Updates the content of a checked list box.
    /// </summary>
    /// <param name="clb">The checked list box which content must be updated</param>
    /// <param name="tableName">The name of the table where to get the data from</param>
    /// <param name="column">the column in the main database associated to this checkbox (used by the filter). Ignored if useKey is true</param>
    /// <param name="tableType">The type indicating the columns to use</param>
    public void UpdateExtendedCheckedListBox(ExtendedCheckedListBox clb, string tableName, string column, TableType tableType)
    {
      DataTable table = GetResultFromRequest(String.Format("SELECT * FROM [{0}] ORDER BY Id", tableName));
      clb.Items.Clear();
      switch (tableType)
      {
        case TableType.Value:
          for (var i = 1; i < table.Rows.Count; ++i)
            clb.Items.Add(new AGoTFilter(table.Rows[i]["Value"].ToString(), column)); break;
        case TableType.ValueKey:
          for (var i = 1; i < table.Rows.Count; ++i)
            clb.Items.Add(new AGoTFilter(table.Rows[i]["Value"].ToString(), table.Rows[i]["Key"].ToString())); break;
        case TableType.ValueShortName:
          for (var i = 1; i < table.Rows.Count; ++i)
            clb.Items.Add(new AGoTFilter(table.Rows[i]["Value"].ToString(), column, table.Rows[i]["ShortName"].ToString())); break;
        case TableType.ValueId:
          for (var i = 1; i < table.Rows.Count; ++i)
            clb.Items.Add(new AGoTFilter(table.Rows[i]["Value"].ToString(), table.Rows[i]["Id"].ToString())); break;
      }
      clb.Summary = table.Rows[0]["Value"].ToString();
      clb.UpdateSize();
    }
  }
}