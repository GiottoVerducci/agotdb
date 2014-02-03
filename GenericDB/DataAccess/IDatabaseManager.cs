namespace GenericDB.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Windows.Forms;

    using Beyond.ExtendedControls;

    using GenericDB.BusinessObjects;

    public interface IDatabaseManager
    {
        string HDataBaseFilename { get; }

        string DataBaseFilename { get; }

        string DataBasePath { get; }

        string TableNameMain { get; }

        string TableNameVersion { get; }

        /// <summary>
        /// Errata text format used for extracted non-string values (based on the "...errated" column).
        /// For string values, the errata format must be passed along with the other formats,
        /// because the errata bound character(s) are not known.
        /// </summary>
        TextFormat ErrataFormat { get; }

        bool IsConnectedToDatabase { get; }

        OleDbConnection DbConnection { get; }

        List<DatabaseInfo> DatabaseInfos { get; }

        ConnectionResult PrepareConnectionToHumanDatabase();

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
        ConnectionResult Connect(bool createExtendedDb, SoftwareVersion applicationVersion);

        DataTable GetResultFromRequest(string request, CommandParameters parameters);

        DataTable GetResultFromRequest(string request);

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
        bool ConvertDatabase();

        void UpdateCards(Func<IDataRow, int, DatabaseManager.OperationResult> updateAction);

        void ResetAndImportTable(string tableName, Func<IDataRowProvider, DatabaseManager.OperationResult> importAction, string deleteWhere = null);

        void ResetAndImportCards(Func<IDataRowProvider, DatabaseManager.OperationResult> importAction);

        /// <summary>
        /// Adds a filter menu item.
        /// </summary>
        /// <param name="mi">The parent menu item of the created menu item</param>
        /// <param name="tableName">The name of the table where to get the data from</param>
        /// <param name="tb">The textbox which the filter must be applied to</param>
        /// <param name="ecb">The checkbox associated to the textbox, so it can be checked automatically</param>
        void UpdateFilterMenu(ToolStripMenuItem mi, string tableName, TextBox tb, ExtendedCheckBox ecb);

        /// <summary>
        /// Updates the content of a checked list box.
        /// </summary>
        /// <param name="clb">The checked list box which content must be updated</param>
        /// <param name="tableName">The name of the table where to get the data from</param>
        /// <param name="column">the column in the main database associated to this checkbox (used by the filter). Ignored if useKey is true</param>
        /// <param name="tableType">The type indicating the columns to use</param>
        /// <param name="filter">The predicate used to keep some items (if returning true) or discard them so they don't appear in the list (if returning false). Null if no filter is used.</param>
        void UpdateExtendedCheckedListBox(ExtendedCheckedListBox clb, string tableName, string column, TableType tableType, Predicate<DbFilter> filter);

        void UpdateExtendedCheckedListBox(ExtendedCheckedListBox clb, string tableName, string column, TableType tableType);

        /// <summary>
        /// Returns a table (that should have only 1 row) containing the card(s) that have a given universal id.
        /// </summary>
        /// <param name="universalId">The universal id of the card.</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        DataTable GetCardFromUniversalId(int universalId);

        /// <summary>
        /// Returns a table (that should have only 1 row) containing the card(s) that have a given octgn id.
        /// </summary>
        /// <param name="octgnId">The octgn id of the card.</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        DataTable GetCardFromOctgnId(Guid octgnId);

        /// <summary>
        /// Returns a table containing the card(s) that match a name and set.
        /// </summary>
        /// <param name="name">The name of the card.</param>
        /// <param name="set">The set of the card (short format).</param>
        /// <returns>The data table containing the row of the matching card.</returns>
        DataTable GetCardFromNameAndSet(string name, string set);
    }
}