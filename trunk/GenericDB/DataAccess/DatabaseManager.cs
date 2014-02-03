// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Beyond.ExtendedControls;
using GenericDB.BusinessObjects;
using GenericDB.Extensions;

namespace GenericDB.DataAccess
{
    /// <summary>
    /// Provides an interface with the card database.
    /// </summary>
    public abstract class DatabaseManager : IDatabaseManager
    {
        protected OleDbConnection _hdbConnection; // human database
        protected OleDbConnection _dbConnection; // extended database
        protected List<DatabaseInfo> _databaseInfos;
        public string HDataBaseFilename { get; protected set; }
        public string DataBaseFilename { get; protected set; }
        public virtual string DataBasePath { get { return "Databases" + Path.DirectorySeparatorChar; } }

        public abstract string TableNameMain { get; }
        public abstract string TableNameVersion { get; }
        /// <summary>
        /// Errata text format used for extracted non-string values (based on the "...errated" column).
        /// For string values, the errata format must be passed along with the other formats,
        /// because the errata bound character(s) are not known.
        /// </summary>
        public abstract TextFormat ErrataFormat { get; }

        public bool IsConnectedToDatabase { get; protected set; }
        public OleDbConnection DbConnection { get { return _dbConnection; } }
        public List<DatabaseInfo> DatabaseInfos { get { return _databaseInfos; } }

        protected DatabaseManager(string dbFileName, string exDbFileName)
        {
            HDataBaseFilename = dbFileName;
            DataBaseFilename = exDbFileName;
        }

        public virtual ConnectionResult PrepareConnectionToHumanDatabase()
        {
            return ConnectToHumanDatabase();
        }

        /// <summary>
        /// Establishes the connection with the database, initializing the extended database
        /// if asked for. A check is performed on the minimal software version required by the database, 
        /// which must be lesser or equal to the application version.
        /// </summary>
        /// <param name="createExtendedDb">Indicates whether the manager should connect 
        /// the human database first and initialize the extended database from it (true),
        /// or just connect to the extended database (false).</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <returns>Indicates whether the connection/conversion was successful, or if not, the reason why.</returns>
        public virtual ConnectionResult Connect(bool createExtendedDb, SoftwareVersion applicationVersion)
        {
            if (createExtendedDb)
                return ConnectWithConversion();

            var result = ConnectToExtendedDatabase();
            IsConnectedToDatabase = result.ErrorCode == ConnectionErrorCode.Success;
            if (!IsConnectedToDatabase)
                return result;

            ReadDatabaseInformations(_dbConnection, out _databaseInfos);

            if ((_databaseInfos.Count > 0) && (applicationVersion.CompareTo(_databaseInfos[0].MinimalApplicationVersion) < 0))
            {
                IsConnectedToDatabase = false;
                return new ConnectionResult { ErrorCode = ConnectionErrorCode.InvalidVersion };
            }
            return new ConnectionResult { ErrorCode = ConnectionErrorCode.Success };
        }

        /// <summary>
        /// Establishes the connection with the human database, initializes the extended database
        /// from it then establishes a connection with the extended database.
        /// </summary>
        /// <returns>Indicates whether the conversion + connection was successful, or if not, the reason why.</returns>
        protected virtual ConnectionResult ConnectWithConversion()
        {
            var result = ConnectToHumanDatabase();
            if (result.ErrorCode != ConnectionErrorCode.Success)
                return result;
            result = ConnectToExtendedDatabase();
            if (result.ErrorCode != ConnectionErrorCode.Success)
                return result;

            IsConnectedToDatabase = true; // required by ConvertDatabase
            ReadDatabaseInformations(_hdbConnection, out _databaseInfos);

            if (ConvertDatabase())
            {
                result = ConnectToExtendedDatabase();
                IsConnectedToDatabase = result.ErrorCode == ConnectionErrorCode.Success;
                return result;
            }
            IsConnectedToDatabase = false;
            return new ConnectionResult { ErrorCode = ConnectionErrorCode.ConversionFailed };
        }

        /// <summary>
        /// Establishes a connection to the database.
        /// </summary>
        /// <param name="dbConnection">The handle to the connection that is initialized.</param>
        /// <param name="dbFilename">The name of the database to establish a connection to.</param>
        /// <returns>True if the method succeeds, false otherwise.</returns>
        protected virtual ConnectionResult ConnectToDatabase(ref OleDbConnection dbConnection, string dbFilename)
        {
            // There is not a 64 bit version of jet that is why there's an error under winxp64.
            // To force your app to use the 32 bit change the target cpu to x86 in the advanced compiler options.
            // Project Properties...Compile tab...Advanced Compile Options button...Target CPU dropdown
            // (this option is not supported in the Express versions)
            // Additionnal info :  http://support.microsoft.com/kb/278604
            // "I used regsvr32 with  the five dll's under the Jet 4.0 OLD DB provider and it worked for me."
            var connectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=""|DataDirectory|{0}{1}""", Path.DirectorySeparatorChar, dbFilename);
            try
            {
                dbConnection = new OleDbConnection();
                dbConnection.ConnectionString = connectionString;
                dbConnection.Open();
                return new ConnectionResult { ErrorCode = ConnectionErrorCode.Success };
            }
            catch (FileNotFoundException)
            {
                return new ConnectionResult { ErrorCode = ConnectionErrorCode.FileNotFound, Data = dbFilename };
            }
            catch (InvalidOperationException ioe)
            {
                return new ConnectionResult { ErrorCode = ConnectionErrorCode.ConnectionError, Data = connectionString + Environment.NewLine + ioe };
            }
            catch
            {
                return new ConnectionResult { ErrorCode = ConnectionErrorCode.InvalidDatabase, Data = dbFilename };
            }
        }

        /// <summary>
        /// Reads the database informations. These informations are sorted by VersionId, descending, which means
        /// that the more recent version of the informations is at index 0.
        /// </summary>
        /// <param name="dbConnection">The connection used to access the database.</param>
        /// <param name="dbInformations">The list of informations about the database.</param>
        protected virtual void ReadDatabaseInformations(OleDbConnection dbConnection, out List<DatabaseInfo> dbInformations)
        {
            dbInformations = new List<DatabaseInfo>();
            var query = String.Format("SELECT * FROM [{0}] ORDER BY [VersionId] DESC", TableNameVersion);
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
                    SoftwareVersion.TryParse(GetRowValue(row, "MinimalApplicationVersion"), out dbSoftwareVersion) ? dbSoftwareVersion : null,
                    GetRowValue(row, "Comments")));
            }
        }

        /// <summary>
        /// Establishes a connection to the human database.
        /// </summary>
        /// <returns>True if the method succeeds, false otherwise</returns>
        protected virtual ConnectionResult ConnectToHumanDatabase()
        {
            return ConnectToDatabase(ref _hdbConnection, DataBasePath + HDataBaseFilename);
        }

        /// <summary>
        /// Establishes a connection to the extended database.
        /// </summary>
        /// <returns>True if the method succeeds, false otherwise</returns>
        protected virtual ConnectionResult ConnectToExtendedDatabase()
        {
            return ConnectToDatabase(ref _dbConnection, DataBasePath + DataBaseFilename);
        }

        protected virtual DataTable GetResultFromRequest(string request, OleDbConnection aDbConnection, CommandParameters parameters)
        {
            var table = new DataTable();
            table.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
            if (IsConnectedToDatabase)
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

        public virtual DataTable GetResultFromRequest(string request, CommandParameters parameters)
        {
            return GetResultFromRequest(request, DbConnection, parameters);
        }

        public virtual DataTable GetResultFromRequest(string request)
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
        public virtual bool ConvertDatabase()
        {
            string query = String.Format("DELETE FROM [{0}]", TableNameMain);
            GetResultFromRequest(query, _dbConnection, null);

            query = String.Format("SELECT * FROM [{0}]", TableNameMain);
            DataTable humanData = GetResultFromRequest(query, _hdbConnection, null);

            using (var dbDataAdapter = new OleDbDataAdapter(query, _dbConnection))
            {
                new OleDbCommandBuilder(dbDataAdapter) { QuotePrefix = "[", QuoteSuffix = "]" }; // required (as a side-effect)

                var dataSet = new DataSet();
                dataSet.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
                dbDataAdapter.Fill(dataSet);

                foreach (DataRow row in humanData.Rows)
                {
                    try
                    {
                        ConvertCard(row, dataSet.Tables[0].Rows);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(string.Format("Une erreur de conversion est survenue sur la carte {0}\n{1}", row["UniversalId"], e));
                        return false;
                    }
                }
                dbDataAdapter.Update(dataSet);
            }
            return true;
        }

        public enum OperationResult
        {
            Ok,
            Done,
            Abort
        }

        public void UpdateCards(Func<IDataRow, int, OperationResult> updateAction)
        {
            var query = String.Format("SELECT * FROM [{0}]", TableNameMain);
            using (var dbDataAdapter = new OleDbDataAdapter(query, _dbConnection))
            {
                var commandBuilder = new OleDbCommandBuilder(dbDataAdapter) { QuotePrefix = "[", QuoteSuffix = "]" }; // required (as a side-effect)

                dbDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand(true);

                var dataSet = new DataSet();
                dataSet.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
                dbDataAdapter.Fill(dataSet);

                var progress = 0;
                var rows = dataSet.Tables[0].Rows;
                foreach (DataRow row in rows)
                {
                    ++progress;
                    var result = updateAction(new OldeDbDataRowWrapper(row), progress * 100 / rows.Count);
                    if (result == OperationResult.Abort)
                        return; // abort
                    if (result == OperationResult.Done)// not used
                        break;
                }
                dbDataAdapter.Update(dataSet); // ZONK: ne met pas à jour la base :(
            }
        }

        public void ResetAndImportTable(string tableName, Func<IDataRowProvider, OperationResult> importAction, string deleteWhere = null)
        {
            string query = String.Format("DELETE FROM [{0}] {1}", tableName, deleteWhere);
            GetResultFromRequest(query, _hdbConnection, null);

            query = String.Format("SELECT * FROM [{0}]", tableName);

            using (var dbDataAdapter = new OleDbDataAdapter(query, _hdbConnection))
            {
                var commandBuilder = new OleDbCommandBuilder(dbDataAdapter) { QuotePrefix = "[", QuoteSuffix = "]" }; // required (as a side-effect)

                var dataSet = new DataSet();
                dataSet.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
                dbDataAdapter.Fill(dataSet);

                OperationResult result;
                do
                {
                    var rows = new OldeDbDataRowProvider(dataSet.Tables[0].Rows);

                    result = importAction(rows);
                }
                while (result == OperationResult.Ok);
                dbDataAdapter.Update(dataSet);
            }
        }

        public void ResetAndImportCards(Func<IDataRowProvider, OperationResult> importAction)
        {
            ResetAndImportTable(TableNameMain, importAction);
        }

        protected abstract void ConvertCard(DataRow sourceRow, DataRowCollection destinationRows);

        private class FormatSectionTracker
        {
            public char Special { get; private set; }
            public int StartIndex { get; private set; }
            public TextFormat Format { get; private set; }

            public FormatSectionTracker(char special, int startIndex, TextFormat format)
            {
                Special = special;
                StartIndex = startIndex;
                Format = format;
            }
        }

        /// <summary>
        /// Reads a formatted string value from a row and returns a FormattedValue object containing the value read
        /// and its format.
        /// The read string can use any of given the bound formats.
        /// </summary>
        /// <param name="row">The row from which to read the string value.</param>
        /// <param name="columnName">The colum in the row containing the string value to read.</param>
        /// <param name="boundFormats">The formats with their bound characters.</param>
        /// <returns>A formatted value.</returns>
        protected static FormattedValue<string> ExtractFormattedStringValueFromRow(DataRow row, string columnName, params BoundFormat[] boundFormats)
        {
            return ExtractFormattedStringValueFromRow(row, columnName, null, boundFormats);
        }

        /// <summary>
        /// Reads a formatted string value from a row, trims it and returns a FormattedValue object containing the value read 
        /// and transformed using a transformation function, and its format.
        /// The read string can use any of given the bound formats.
        /// </summary>
        /// <param name="row">The row from which to read the string value.</param>
        /// <param name="columnName">The colum in the row containing the string value to read.</param>
        /// <param name="valueTransformer">The function used to transform the value read in the row before processing it.</param>
        /// <param name="boundFormats">The formats with their bound characters.</param>
        /// <returns>A formatted value.</returns>
        protected static FormattedValue<string> ExtractFormattedStringValueFromRow(DataRow row, string columnName, Func<string, string> valueTransformer, params BoundFormat[] boundFormats)
        {
            string value = row[columnName].ToString().Trim();
            if (valueTransformer != null)
                value = valueTransformer(value);

            if (boundFormats == null || boundFormats.Length <= 0)
                return new FormattedValue<string>(value, new List<FormatSection>());

            var formatSections = new List<FormatSection>();
            var tracker = new Stack<FormatSectionTracker>();
            var newValue = new StringBuilder();

            int offset = 0; // offset accumulator
            for (var i = 0; i < value.Length; ++i)
            {
                bool boundChar = false;

                // TODO : replace ForEach by dictionary
                foreach (var boundFormat in boundFormats)
                {
                    var lastSpecialDoesntMatch = (tracker.Count == 0) || (tracker.Peek().Special != boundFormat.Opening);
                    if ((value[i] == boundFormat.Opening)
                        && (boundFormat.DistinctOpenClose || lastSpecialDoesntMatch))
                    {
                        tracker.Push(new FormatSectionTracker(boundFormat.Opening, i - offset, boundFormat.Format));
                        // we discard the bound character except for empty sections (eg. "{}" or "~~")
                        boundChar = !((i + 1 < value.Length) && (value[i + 1] == boundFormat.Closing));
                        break;
                    }
                    if (value[i] == boundFormat.Closing)
                    {
                        if (boundFormat.DistinctOpenClose && lastSpecialDoesntMatch)
                            throw new ApplicationException(string.Format(CultureInfo.InvariantCulture,
                                "Mismatch in database: '{0}' unexpected, column = {1}, value = {2}",
                                boundFormat.Closing, columnName, value));
                        var section = tracker.Pop();
                        boundChar = value[i - 1] != boundFormat.Opening;
                        formatSections.Add(new FormatSection(section.StartIndex, i - offset + (boundChar ? 0 : 1), section.Format));
                        break;
                    }
                }

                if (!boundChar)
                    newValue.Append(value[i]);
                else
                    offset++;
            }
            if (tracker.Count != 0)
                throw new ApplicationException(string.Format(CultureInfo.InvariantCulture,
                    "Mismatch in database: {0} formatting characters left, column = {1}, value = {2}",
                    tracker.Count, columnName, value));

            formatSections.Sort(); // order them by starting index
            return new FormattedValue<string>(newValue.ToString(), formatSections);
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
        protected virtual FormattedValue<int> ExtractFormattedIntValueFromRow(DataRow row, string columnName, string refTableName, string[] refTableColumnNames = null)
        {
            FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName);
            string[] values = stringValue.Value.Split('/').Where(v => !string.IsNullOrWhiteSpace(v)).ToArray();

            int intValue = 0;
            var formatSections = new List<FormatSection>();
            if (stringValue.Formats.Count != 0)
                formatSections.Add(new FormatSection(0, 0, ErrataFormat));

            if (refTableColumnNames == null)
                refTableColumnNames = new[] { "Value" };
            
            var wherePredicate = string.Join(" OR ", refTableColumnNames.Select(c => string.Format("{0} like :value", c)));

            for (var i = 0; i < values.Length; ++i)
            {
                var table = GetResultFromRequest(
                    string.Format("SELECT Id FROM {0} WHERE {1}", refTableName, wherePredicate),
                    _hdbConnection, new CommandParameters().Add("value", values[i]));
                intValue += Int32.Parse(table.Rows[0]["Id"].ToString(), CultureInfo.InvariantCulture);
            }

            return new FormattedValue<int>(intValue, formatSections);
        }

        /// <summary>
        /// Reads a string value from a row and returns a FormattedValue containing the boolean represented
        /// by the string and its format.
        /// </summary>
        /// <param name="row">The row from which to read the string value.</param>
        /// <param name="columnName">The colum in the row containing the string value to read.</param>
        /// <param name="boundFormats">The formats with their bound characters.</param>
        /// <returns>A formatted value.</returns>
        protected virtual FormattedValue<bool?> ExtractFormattedBoolValueFromRow(DataRow row, string columnName, params BoundFormat[] boundFormats)
        {
            FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName, boundFormats);

            string value = stringValue.Value.ToUpper();

            bool? bvalue = null;
            if (!string.IsNullOrEmpty(value))
                bvalue = (value == "YES");
            var formatSections = new List<FormatSection>();
            if (stringValue.Formats.Count != 0)
                formatSections.Add(new FormatSection(0, 0, ErrataFormat));
            return new FormattedValue<bool?>(bvalue, formatSections);
        }

        /// <summary>
        /// Reads a string value from a row and returns a FormattedValue containing the XInt value represented
        /// by the string and its format.
        /// </summary>
        /// <param name="row">The row from which to read the string value.</param>
        /// <param name="columnName">The colum in the row containing the string value to read.</param>
        /// <param name="boundFormats">The formats with their bound characters.</param>
        /// <returns>A formatted value.</returns>
        protected virtual FormattedValue<XInt> ExtractFormattedXIntValueFromRow(DataRow row, string columnName, params BoundFormat[] boundFormats)
        {
            FormattedValue<string> stringValue = ExtractFormattedStringValueFromRow(row, columnName, boundFormats);

            string value = stringValue.Value.ToUpper();

            XInt xvalue = string.IsNullOrEmpty(value) ? null : ((value == "X") ? new XInt() : new XInt(Int32.Parse(value, CultureInfo.InvariantCulture)));

            var formatSections = new List<FormatSection>();
            if (stringValue.Formats.Count != 0)
                formatSections.Add(new FormatSection(0, 0, ErrataFormat));
            return new FormattedValue<XInt>(xvalue, formatSections);
        }

        protected virtual string GetRowValue(DataRow row, string columnName)
        {
            return row[columnName].ToString();
        }

        /// <summary>
        /// Gets the string representing a given XInt value in the database.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representing the value.</returns>
        protected virtual string XIntToString(XInt value)
        {
            if (value == null)
                return null;
            return value.IsX ? "-1" : value.Value.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Adds a filter menu item.
        /// </summary>
        /// <param name="mi">The parent menu item of the created menu item</param>
        /// <param name="tableName">The name of the table where to get the data from</param>
        /// <param name="tb">The textbox which the filter must be applied to</param>
        /// <param name="ecb">The checkbox associated to the textbox, so it can be checked automatically</param>
        public virtual void UpdateFilterMenu(ToolStripMenuItem mi, string tableName, TextBox tb, ExtendedCheckBox ecb)
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
        /// <param name="filter">The predicate used to keep some items (if returning true) or discard them so they don't appear in the list (if returning false). Null if no filter is used.</param>
        public virtual void UpdateExtendedCheckedListBox(ExtendedCheckedListBox clb, string tableName, string column, TableType tableType, Predicate<DbFilter> filter)
        {
            DataTable table = GetResultFromRequest(String.Format("SELECT * FROM [{0}] ORDER BY Id", tableName));

            clb.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                ecl.Items.Clear();

                // define the way the data row is converted to a DbFilter item
                Converter<DataRow, DbFilter> converter;
                switch (tableType)
                {
                    case TableType.Value: converter = (r => new DbFilter(r["Value"].ToString(), column)); break;
                    case TableType.ValueKey: converter = (r => new DbFilter(r["Value"].ToString(), r["Key"].ToString())); break;
                    case TableType.ValueShortName: converter = (r => new DbFilter(r["Value"].ToString(), column, r["ShortName"].ToString())); break;
                    case TableType.ValueId: converter = (r => new DbFilter(r["Value"].ToString(), column, r["Id"].ToString())); break;
                    default: throw new ArgumentException(string.Format("TableType {0} not supported.", tableType));
                }

                // add the items that are not discarded by the filter predicate as DbFilter items
                for (var i = 1; i < table.Rows.Count; ++i)
                {
                    var dbFilter = converter(table.Rows[i]);
                    if (filter == null || filter(dbFilter))
                        ecl.Items.Add(dbFilter);
                }

                ecl.Summary = table.Rows[0]["Value"].ToString();
                ecl.UpdateSize();
            });
        }

        public virtual void UpdateExtendedCheckedListBox(ExtendedCheckedListBox clb, string tableName, string column, TableType tableType)
        {
            UpdateExtendedCheckedListBox(clb, tableName, column, tableType, null);
        }

        /// <summary>
        /// Returns a table (that should have only 1 row) containing the card(s) that have a given universal id.
        /// </summary>
        /// <param name="universalId">The universal id of the card.</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        public virtual DataTable GetCardFromUniversalId(int universalId)
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}] WHERE UniversalId = :universalId", TableNameMain),
                new CommandParameters().Add("universalId", universalId));
        }

        /// <summary>
        /// Returns a table (that should have only 1 row) containing the card(s) that have a given octgn id.
        /// </summary>
        /// <param name="octgnId">The octgn id of the card.</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        public virtual DataTable GetCardFromOctgnId(Guid octgnId)
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}] WHERE OctgnId = :octgnId", TableNameMain),
                new CommandParameters().Add("octgnId", octgnId.ToString()));
        }

        /// <summary>
        /// Returns a table containing the card(s) that match a name and set.
        /// </summary>
        /// <param name="name">The name of the card.</param>
        /// <param name="set">The set of the card (short format).</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        public virtual DataTable GetCardFromNameAndSet(string name, string set)
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}] WHERE Name = :name AND Set LIKE '%' + :set + '%'", TableNameMain),
                new CommandParameters().Add("name", name)
                    .Add("set", set));
        }
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
        /// </summary>
        ValueId
    };

    public struct ConnectionResult
    {
        public ConnectionErrorCode ErrorCode { get; set; }
        public object Data { get; set; }
    }

    public enum ConnectionErrorCode
    {
        Undefined = 0,
        Success,
        ConversionFailed,
        InvalidVersion,
        FileNotFound,
        InvalidDatabase,
        ConnectionError
    }
}
