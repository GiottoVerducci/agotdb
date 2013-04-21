// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using AGoTDB.BusinessObjects;
using AGoTDB.DataAccess;
using AGoTDB.OCTGN;

using Beyond.ExtendedControls;

using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Extensions;
using GenericDB.Helper;

namespace AGoTDB.Forms
{
    /// <summary>
    /// The main form. Performs searches on the card database using filters and criteria. 
    /// New instances may be created afterwards as independant search forms (with no main menu and so on).
    /// </summary>
    public partial class MainForm : Form
    {
        private bool _isMainForm; // false if it's a card list window
        private static MainForm _mainForm;
        private static SplashScreen _splashScreen;
        private int _viewIndex = 1;
        private bool _dataTableFirstLoad = true; // used when the data table is loaded for the first time
        private bool _isDataBaseLoaded;
        private DataRow[] _quickFindRows; // quick find results
        private int _quickFindIndex; // index of the current quick find result
        private AgotCard _displayedCard;
        private readonly CardPreviewForm _cardPreviewForm = new CardPreviewForm();

        private readonly DataTable _dataTable = new DataTable();
        private Query _query = new Query();

        /// <summary>
        /// The default constructor.
        /// </summary>
        public MainForm(bool isMainForm = false)
        {
            // Cet appel est requis par le Concepteur Windows Form.
            InitializeComponent();
            _dataTable.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
            InitializeQueryLocalization();

            if (isMainForm)
            {
                ApplicationSettings.ImagesFolder = String.Format("{0}{1}Images", Application.StartupPath, Path.DirectorySeparatorChar);
                ApplicationSettings.ImagesFolderExists = Directory.Exists(ApplicationSettings.ImagesFolder);

                ApplicationSettings.DatabaseManager = new AgotDatabaseManager("AGoT.mdb", "AGoTEx.mdb");
                var url = UserSettings.CheckForUpdatesOnStartup ? CheckNewerVersion() : null;

                InitializeMainForm();
                checkNewVersionBackgroundWorker.DoWork += checkNewVersionBw_DoWork;
                checkNewVersionBackgroundWorker.RunWorkerCompleted += CheckNewVersionBw_RunWorkerCompleted;
                checkNewVersionBackgroundWorker.RunWorkerAsync(url);
            }
        }

        private string CheckNewerVersion()
        {
            var databaseFilePath = ApplicationSettings.DatabaseManager.DataBasePath + ApplicationSettings.DatabaseManager.HDataBaseFilename;
            string url = null;
            try
            {
                // get the current database last modification date
                var databaseDate = File.Exists(databaseFilePath) ? File.GetLastWriteTimeUtc(databaseFilePath) : DateTime.MinValue;
                // download the latest database informations to compare the dates and get the download url
                url = UserSettings.UpdateInformationsUrl;
                string data;
                using (var webClient = new WebClient())
                {
                    //TODO: set proxy (if any)
                    data = Encoding.ASCII.GetString(webClient.DownloadData(url));
                }

                // find the line matching our database language
                var lines = data.Split('\n');

                string dbUrl = null;
                foreach (var line in lines)
                {
                    // data will follow the format: language yyyy-mm-dd url
                    var split = line.Split(' ');
                    if (!string.Equals(UserSettings.DatabaseLanguage, split[0], StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    var lastDateItems = split[1].Split('-').Select(int.Parse).ToArray();
                    var lastDate = new DateTime(lastDateItems[0], lastDateItems[1], lastDateItems[2]);

                    if (lastDate < databaseDate)
                        return null;
                    dbUrl = split[2];
                    break;
                }
                if (dbUrl == null)
                    return null;

                // a new version has been detected
                if (MessageBox.Show(Resource1.DatabaseUpdateFound,
                    Resource1.DatabaseUpdateFoundTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return null;
                }
                return dbUrl;
            }
            catch (Exception e)
            {
                var errorMessage = string.Format(Resource1.ErrDatabaseUpdateInformationsCouldntBeRetrieved, ApplicationSettings.ApplicationVersion, url, databaseFilePath, e);
                MessageBox.Show(errorMessage, Resource1.ErrDatabaseUpdateInformationsTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void checkNewVersionBw_DoWork(object sender, DoWorkEventArgs e)
        {
            var url = (string)e.Argument;
            if (url != null)
            {
                var executionTrace = new ExecutionTrace();
                // we must download the new database file
                try
                {
                    using (var webClient = new WebClient())
                    {
                        //TODO: set proxy (if any)

                        var zipPath = Path.GetTempFileName();
                        var step = executionTrace.AddStep(string.Format(Resource1.UpdatingDB_DownloadingFileTo, url, zipPath));
                        webClient.DownloadFile(url, zipPath);
                        step.IsSuccessful = true;

                        // we must unzip it
                        step = executionTrace.AddStep(string.Format(Resource1.UpdatingDB_UnzipingFile, zipPath));
                        var filePaths = ZipHelper.UnZipFile(zipPath);
                        step.IsSuccessful = true;

                        if (filePaths != null)
                        {
                            // overwrite our database files with it
                            step = executionTrace.AddStep(string.Format(Resource1.UpdatingDB_CopyingFiles, ApplicationSettings.DatabaseManager.DataBasePath));
                            foreach (var filePath in filePaths)
                                File.Copy(filePath, ApplicationSettings.DatabaseManager.DataBasePath + Path.GetFileName(filePath), true);
                            step.IsSuccessful = true;
                        }
                        else
                        {
                            throw new Exception("Empty or corrupted zip file");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format(Resource1.ErrDatabaseUpdateFailed, ApplicationSettings.ApplicationVersion, url, executionTrace, ex);
                    MessageBox.Show(errorMessage, Resource1.ErrDatabaseUpdateTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            e.Result = InitializeDatabaseConnection();
        }

        private void CheckNewVersionBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            checkNewVersionBackgroundWorker.RunWorkerCompleted -= CheckNewVersionBw_RunWorkerCompleted;
            _splashScreen.CancelAndClose();
            var connectionResult = (ConnectionResult)e.Result;
            switch (connectionResult.ErrorCode)
            {
                case ConnectionErrorCode.Success:
                    var createExtendedDb = UserSettings.CreateExtendedDB;
                    if (createExtendedDb)
                    {
                        UserSettings.CreateExtendedDB = false;
                        UserSettings.Save();
                    }
                    _isDataBaseLoaded = true;
                    break;
                case ConnectionErrorCode.InvalidVersion:
                    if (MessageBox.Show(string.Format(CultureInfo.CurrentCulture,
                        Resource1.ErrMinimalSoftwareVersionRequired,
                        ApplicationSettings.DatabaseManager.DatabaseInfos[0].MinimalApplicationVersion,
                        ApplicationSettings.ApplicationVersion),
                        Resource1.ErrMinimalSoftwareVersionRequiredTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        _isDataBaseLoaded = true;
                    }
                    break;
                case ConnectionErrorCode.FileNotFound:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrDatabaseNotFound, connectionResult.Data),
                        Resource1.ErrDatabaseNotFoundTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    break;
                case ConnectionErrorCode.InvalidDatabase:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrInvalidDatabase, connectionResult.Data),
                        Resource1.ErrInvalidDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    break;
                case ConnectionErrorCode.ConnectionError:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrConnectingToDatabase, connectionResult.Data),
                        Resource1.ErrConnectingToDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    break;
            }
            if (!UserSettings.IsAvailable() || !ApplicationSettings.DatabaseManager.ConnectedToDatabase)
                Application.Exit();
            this.Visible = true;
            InitializeMainFormForShowing();
        }

        private static ConnectionResult InitializeDatabaseConnection()
        {
            var createExtendedDb = UserSettings.CreateExtendedDB;
            var connectionResult = ApplicationSettings.DatabaseManager.Connect(createExtendedDb, ApplicationSettings.ApplicationVersion);
            return connectionResult;
        }

        private static void InitializeQueryLocalization()
        {
            var queryLocalization = new QueryLocalization
            {
                And = Resource1.And,
                RangeBetween = Resource1.RangeBetween,
                RangeGreaterOrLesser = Resource1.RangeGreaterOrLesser
            };
            Query.Localization = queryLocalization;
        }

        /// <summary>
        /// Initializes this instance as the main form.
        /// </summary>
        public void InitializeMainForm()
        {
            _isMainForm = true;
            _mainForm = this;
            _splashScreen = new SplashScreen();
            _splashScreen.Show(100);
            _splashScreen.Update();
        }

        /// <summary>
        /// Initializes this instance as an independant search form.
        /// </summary>
        public void InitializeViewForm(Object dataTable)
        {
            var i = 0;
            while (i < splitContainer1.Panel1.Controls.Count)
                if (splitContainer1.Panel1.Controls[i] != dataGridView)
                    splitContainer1.Panel1.Controls[i].Parent = null;
                else
                    ++i;

            dataGridView.Dock = DockStyle.Fill;
            dataGridView.DataSource = dataTable;
            menuStrip1.Visible = false;
            moveToANewWindowToolStripMenuItem.Visible = false;
            Text = string.Format("{0} | {1}", string.Format(CultureInfo.CurrentCulture, Resource1.ViewFormTitle, _viewIndex), _query.HumanQuery);
        }

        private static void LoadCardTypeNames()
        {
            var types = ApplicationSettings.DatabaseManager.GetCardTypeNames();
            AgotCard.CardTypeNames = new Dictionary<int, string>();
            foreach (DataRow row in types.Rows)
            {
                if ((int)row["Id"] >= 0)
                    AgotCard.CardTypeNames.Add((int)row["ShortName"], row["Value"].ToString());
            }
        }

        private static void LoadCardHouseNames()
        {
            var houses = ApplicationSettings.DatabaseManager.GetCardHouseNames();
            AgotCard.CardHouseNames = new Dictionary<int, string>();
            foreach (DataRow row in houses.Rows)
            {
                if ((int)row["Id"] >= 0)
                    AgotCard.CardHouseNames.Add((int)row["Id"], row["Value"].ToString());
            }
        }

        private static void LoadCardTriggerNames()
        {
            var triggers = ApplicationSettings.DatabaseManager.GetCardTriggerNames();
            AgotCard.CardTriggerNames = new List<string>();
            foreach (DataRow row in triggers.Rows)
            {
                if ((int)row["Id"] >= 0)
                    AgotCard.CardTriggerNames.Add(row["Value"].ToString());
            }
        }

        private static void LoadCardPatterns()
        {
            var patterns = ApplicationSettings.DatabaseManager.GetCardPatterns();
            AgotCard.CardPatterns = new Dictionary<AgotCard.Pattern, string>();
            foreach (DataRow row in patterns.Rows)
            {
                if ((int)row["Id"] >= 0)
                    AgotCard.CardPatterns.Add((AgotCard.Pattern)row["Id"], row["Value"].ToString());
            }
        }

        private static void LoadExpansionSets()
        {
            AgotCard.ExpansionSets = new Dictionary<string, bool>();
            var sets = ApplicationSettings.DatabaseManager.GetExpansionSets();
            foreach (DataRow row in sets.Rows)
            {
                if ((int)row["Id"] >= 0)
                    AgotCard.ExpansionSets.Add(row["ShortName"].ToString(), (bool)row["LCG"]);
            }

            AgotCard.ChaptersNames = new Dictionary<string, string>();

            foreach (DataRow row in sets.Rows)
            {
                if ((int)row["Id"] >= 0 && (bool)row["ByChapter"])
                    AgotCard.ChaptersNames.Add(row["ShortName"].ToString(), row["ChaptersNames"].ToString());
            }
        }

        /// <summary>
        /// Updates the content and caption of the controls with data read in the database.
        /// Therefore, new values (for example a new keyword) are automatically handled by
        /// the controls and the query filter.
        /// </summary>
        private void UpdateControlsLabels()
        {
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclHouse, ApplicationSettings.DatabaseManager.TableNameHouse, "House", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclCardtype, ApplicationSettings.DatabaseManager.TableNameType, "Type", TableType.ValueShortName);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclProvides, ApplicationSettings.DatabaseManager.TableNameProvides, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclMecanism, ApplicationSettings.DatabaseManager.TableNameMechanism, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclIcon, ApplicationSettings.DatabaseManager.TableNameIcon, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclVirtue, ApplicationSettings.DatabaseManager.TableNameVirtue, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclCE, ApplicationSettings.DatabaseManager.TableNameChallengeEnhancement, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclKeyword, ApplicationSettings.DatabaseManager.TableNameKeyword, "Keywords", TableType.Value);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclTrigger, ApplicationSettings.DatabaseManager.TableNameTrigger, "Text", TableType.Value);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclExpansionSet, ApplicationSettings.DatabaseManager.TableNameSet, "Set", TableType.ValueShortName);
            ApplicationSettings.DatabaseManager.UpdateFilterMenu(filterToolStripMenuItem, ApplicationSettings.DatabaseManager.TableNameFilterText, tbCardtext, eclCardtextCheck);
        }

        /// <summary>
        /// Enables or removes the icons for discrete use.
        /// </summary>
        private void DisplayIcons(bool displayImages)
        {
            picGold.Visible = displayImages;
            picInit.Visible = displayImages;
            picClaim.Visible = displayImages;
            picStrength.Visible = displayImages;
        }

        /// <summary>
        /// Builds recursively the expression used to display a human "Type" column
        /// </summary>
        /// <returns>The expression used by the data column</returns>
        private static string BuildTypeExpression()
        {
            var types = AgotCard.CardTypeNames.ToArray();
            return BuildTypeExpression(0, types);
        }

        private static string BuildTypeExpression(int i, KeyValuePair<int, string>[] types)
        {
            return i < types.Length - 1
                ? string.Format("IIF(Type={0}, '{1}', {2})", types[i].Key, types[i].Value, BuildTypeExpression(i + 1, types))
                : string.Format("'{0}'", types[i].Value);
        }

        /// <summary>
        /// Adds dynamically computed columns to the main data table.
        /// </summary>
        private void CustomizeDataTable()
        {
            var houseTempColumn = new DataColumn("HouseTemp", typeof(String))
            {
                Expression = String.Format(
                    "IIF(HouseNeutral, '{0}/', '') + IIF(HouseStark, '{1}/', '') + IIF(HouseLannister, '{2}/', '') + IIF(HouseBaratheon, '{3}/', '') + IIF(HouseGreyjoy, '{4}/', '') + IIF(HouseMartell, '{5}/', '') + IIF(HouseTargaryen, '{6}/', '')",
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Neutral),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Stark),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Lannister),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Baratheon),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Greyjoy),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Martell),
                    AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Targaryen))
            };
            _dataTable.Columns.Add(houseTempColumn);

            var houseColumn = new DataColumn("House", typeof(String))
            {
                Expression = String.Format("SUBSTRING(HouseTemp, 1, LEN(HouseTemp) - 1)")
            };
            _dataTable.Columns.Add(houseColumn);
            _dataTable.Columns["House"].SetOrdinal(_dataTable.Columns["Name"].Ordinal + 1);

            var typeColumn = new DataColumn("Type ", typeof(String))
            {
                Expression = BuildTypeExpression()
            };
            _dataTable.Columns.Add(typeColumn);
            _dataTable.Columns["Type "].SetOrdinal(_dataTable.Columns["Type"].Ordinal + 1);

            if (UserSettings.ColumnsSettings != null)
            {
                var columnsSettings = UserSettings.ColumnsSettings.Split('|').Select(s => new ColumnSetting(s)).OrderByDescending(cs => cs.Index);
                foreach (var columnSetting in columnsSettings)
                {
                    var dataTableColumn = _dataTable.Columns[columnSetting.Name];
                    dataTableColumn.SetOrdinal(Convert.ToInt32(columnSetting.Index));
                }
            }
        }

        /// <summary>
        /// Updates the view exposed by the main data table, according to the filters generated by the controls
        /// </summary>
        private void UpdateDataTableView()
        {
            if (!_isDataBaseLoaded)
                return;
            var query = BuildQueryFromControls();
            if (query.SqlQuery == _query.SqlQuery) // query hasn't change, so the result has not change neither
                return; // nothing to do

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int selectedRowId = -1;
                if (dataGridView.SelectedRows.Count != 0)
                    selectedRowId = (int)((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row["UniversalId"];

                _query = query;
                using (var dbDataAdapter = new OleDbDataAdapter(query.SqlQuery, ApplicationSettings.DatabaseManager.DbConnection))
                {
                    _dataTable.Clear();
                    dbDataAdapter.Fill(_dataTable);

                    if (_dataTableFirstLoad)
                    {
                        CustomizeDataTable();
                        _dataTableFirstLoad = false;
                    }

                    foreach (DataColumn column in _dataTable.Columns)
                        if (IsColumnHidden(column.ColumnName))
                            column.ColumnMapping = MappingType.Hidden;

                    _dataTable.Columns["UniversalId"].ColumnMapping = MappingType.Hidden;
                }
                QuickFind(tbFind.Text); // sets the new quick find results
                if (!SelectRow(selectedRowId)) // the previously selected row doesn't exist anymore
                    SelectCurrentQuickFindResult(); // we attempt to select the first quick find result
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Exception while processing query {0}\n{1}\n{2}", query.SqlQuery, e.Message, e.StackTrace));
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Indicates whether the column in the displayed grid must be hidden or not, based on its name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>True if the column must be hidden, false otherwise.</returns>
        private static bool IsColumnHidden(string columnName)
        {
            return ((columnName.IndexOf("Style", StringComparison.InvariantCultureIgnoreCase) != -1)
                || (columnName.IndexOf("Errated", StringComparison.InvariantCultureIgnoreCase) != -1)
                || ((columnName.StartsWith("House", StringComparison.InvariantCultureIgnoreCase)
                    && (columnName.Length > "House".Length)))
                || (string.Compare(columnName, "Type", StringComparison.InvariantCultureIgnoreCase) == 0));
        }

        /// <summary>
        /// Creates a SQL query/human query pair from the state of the controls.
        /// </summary>
        /// <returns>The SQL query/human query pair reflecting the controls state</returns>
        private Query BuildQueryFromControls()
        {
            var result = new Query(String.Format("SELECT * FROM [{0}]", ApplicationSettings.DatabaseManager.TableNameMain), "");

            IList<DbFilter> additionalIncludedSets = null;
            IList<DbFilter> additionalExcludedSets = null;

            if (miLcgSetsOnly.Checked)
            {
                var uncheckedValues = eclExpansionSet.GetItemsByState(CheckState.Unchecked).ConvertAll(i => (DbFilter)i);
                var checkedValues = eclExpansionSet.GetItemsByState(CheckState.Checked).ConvertAll(i => (DbFilter)i);

                // add the lcg unchecked expansions to perform a search on all lcg expansions only
                // only if no lcg expansion is checked
                additionalIncludedSets = checkedValues.Any(v => AgotCard.ExpansionSets[v.ShortName])
                    ? null
                    : new List<DbFilter>(uncheckedValues.FindAll(v => AgotCard.ExpansionSets[v.ShortName]));

                if (additionalIncludedSets != null && additionalIncludedSets.Count == 0 // all LCG expansions are removed 
                    && eclExpansionSet.GetItemsByState(CheckState.Checked).Count == 0) //and no other non-LCG set is explicit added
                    additionalExcludedSets = new List<DbFilter>(uncheckedValues);
            }

            Query filter =
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclCardtype, "OR", PositiveDataType.ExactValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclHouse, "OR", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclProvides, "AND", PositiveDataType.Integer) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclMecanism, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclIcon, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclVirtue, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclCE, "AND", PositiveDataType.ChallengeEnhancement) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclKeyword, "AND", PositiveDataType.KeywordValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclTrigger, "OR", PositiveDataType.TriggerValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclExpansionSet, "OR", PositiveDataType.LikeValue, additionalIncludedSets, additionalExcludedSets);

            Query costOrIncome = QueryBuilder.GetFilterFromRangeBoxes(tbGoldLow, tbGoldHigh, "Cost");
            if (!string.IsNullOrEmpty(costOrIncome.SqlQuery))
            {
                costOrIncome.SqlQuery = string.Format("(({0}) OR ({1}))", costOrIncome.SqlQuery, costOrIncome.SqlQuery.Replace("Cost", "Income"));
                costOrIncome.HumanQuery = costOrIncome.HumanQuery.Replace("Cost", Resource1.CostOrIncomeText);
            }
            filter +=
                costOrIncome +
                QueryBuilder.GetFilterFromRangeBoxes(tbInitiativeLow, tbInitiativeHigh, "Initiative") +
                QueryBuilder.GetFilterFromRangeBoxes(tbClaimLow, tbClaimHigh, "Claim") +
                QueryBuilder.GetFilterFromRangeBoxes(tbStrengthLow, tbStrengthHigh, "Strength") +
                QueryBuilder.GetFilterFromTextBox(tbCardtext, eclCardtextCheck, "Text") +
                QueryBuilder.GetFilterFromTextBox(tbTraits, eclTraitCheck, "Traits") +
                QueryBuilder.GetFilterFromTextBox(tbName, eclNameCheck, "Name");

            if (!string.IsNullOrEmpty(filter.SqlQuery))
            {
                result.SqlQuery += " WHERE (" + filter.SqlQuery + ")";
                result.HumanQuery += filter.HumanQuery;
            }
            return result;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearCheckListBoxes(eclHouse, eclCardtype, eclProvides, eclMecanism,
                eclIcon, eclVirtue, eclCE, eclKeyword, eclTrigger, eclExpansionSet);

            ClearTextBoxes(tbGoldLow, tbGoldHigh, tbInitiativeLow, tbInitiativeHigh,
                tbClaimLow, tbClaimHigh, tbStrengthLow, tbStrengthHigh, tbCardtext, tbTraits, tbName);

            ClearCheckBoxes(eclCardtextCheck, eclTraitCheck, eclNameCheck);

            UpdateDataTableView();
        }

        private static void ClearCheckListBoxes(params ExtendedCheckedListBox[] items)
        {
            foreach (ExtendedCheckedListBox listBox in items)
                listBox.ClearCheckBoxes();
        }

        private static void ClearCheckBoxes(params ExtendedCheckBox[] items)
        {
            foreach (ExtendedCheckBox checkBox in items)
                checkBox.Checked = false;
        }

        private static void ClearTextBoxes(params TextBox[] items)
        {
            foreach (TextBox textBox in items)
                textBox.Text = "";
        }

        private void eclMouseLeave(object sender, EventArgs e)
        {
            UpdateDataTableView();
        }

        private void tbLowHigh_Validated(object sender, EventArgs e)
        {
            UpdateDataTableView();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutForm())
            {
                about.ShowDialog();
            }
        }

        private void tbLowHigh_TextChanged(object sender, EventArgs e)
        {
            var senderTextBox = (TextBox)sender;
            if (senderTextBox.Tag != null) // to avoid reentrance
                return;
            string text = senderTextBox.Text;
            string newText = text
                .Where(c => (c >= '0') && (c <= '9'))
                .Aggregate("", (current, t) => current + t);
            senderTextBox.Tag = 0;
            senderTextBox.Text = newText;
            senderTextBox.Tag = null;
        }

        private void eclCheck_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateDataTableView();
        }

        /// <summary>
        /// Selects the row in the view which Id field is equal to a given id. If the row is not found,
        /// no selection is made and the method returns False.
        /// </summary>
        /// <remarks>Id is not the index of the row in the view.</remarks>
        /// <param name="rowId">the Id of the row to select</param>
        /// <returns>True if a row was selected, False otherwise.</returns>
        private bool SelectRow(int rowId)
        {
            for (var i = 0; i < dataGridView.Rows.Count; ++i)
            {
                var curId = (int)((DataRowView)dataGridView.Rows[i].DataBoundItem).Row["UniversalId"];
                if (curId != rowId)
                    continue;

                dataGridView.ClearSelection();
                dataGridView.Rows[i].Selected = true;
                // scroll in the view in order to have the selected row centered
                int displayedRowsCount = dataGridView.Height / dataGridView.RowTemplate.Height;
                dataGridView.FirstDisplayedScrollingRowIndex = Math.Max(0, i - (displayedRowsCount / 2) + 1);
                return true;
            }
            return false;
        }

        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            QuickFind(tbFind.Text);
            SelectCurrentQuickFindResult(); // select the first row found that matches
        }

        private void btnQuickFindNext_Click(object sender, EventArgs e)
        {
            if ((_quickFindRows == null) || (_quickFindRows.Length <= 0))
                return;
            _quickFindIndex = (_quickFindIndex + 1) % _quickFindRows.Length;
            SelectCurrentQuickFindResult();
        }

        /// <summary>
        /// Sets the quick find results by searching all rows whose Name column matches a given string.
        /// </summary>
        /// <param name="text">The part of the name to search in the data rows.</param>
        private void QuickFind(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _quickFindRows = _dataTable.Select(String.Format("Name LIKE '%{0}%'", QueryBuilder.EscapeSqlCharacters(text, false)));
                if (_quickFindRows.Length > 0)
                    _quickFindIndex = 0;
            }
            else
                _quickFindRows = null;
        }

        /// <summary>
        /// Selects the row corresponding to the current quick find result.
        /// </summary>
        private void SelectCurrentQuickFindResult()
        {
            if ((_quickFindRows != null) && (_quickFindIndex >= 0) && (_quickFindIndex < _quickFindRows.Length))
                SelectRow((int)_quickFindRows[_quickFindIndex]["UniversalId"]);
        }

        private void saveSelectionToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fd = new SaveFileDialog())
            {
                fd.Filter = Resource1.ExportSaveDialogFilter;
                fd.FileName = Resource1.ExportDefaultFilename;
                if (fd.ShowDialog() == DialogResult.OK)
                    SaveSelectionToTextFile(fd.FileName);
            }
        }

        private void SaveSelectionToTextFile(string fileName)
        {
            var entries = new List<AgotCard>();
            for (var i = 0; i < dataGridView.Rows.Count; ++i)
                entries.Add(new AgotCard(((DataRowView)dataGridView.Rows[i].DataBoundItem).Row));

            using (var sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                foreach (AgotCard card in entries)
                {
                    sw.WriteLine(card.ToPlainFullString());
                    sw.WriteLine();
                }
            }
        }

        //public Object Clone(Object obj)
        //{
        //	MemoryStream mem = new MemoryStream();
        //	BinaryFormatter binFormat = new BinaryFormatter();
        //	binFormat.Serialize(mem, obj); // serialization of obj in memory
        //	mem.Seek(0, SeekOrigin.Begin); // go back to the start of the stream
        //	return binFormat.Deserialize(mem); // create the object
        //}


        private void moveToANewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewWindowWithResults();
        }

        private void CreateNewWindowWithResults()
        {
            var form = new MainForm();
            _viewIndex++;
            form._query = new Query(_query.SqlQuery, _query.HumanQuery);
            form.InitializeViewForm(_dataTable.Copy());

            // keep a reference to the window in order to display it in the window menu
            form.Show();
            var item = new ToolStripMenuItem(form.Text) { Tag = form };
            item.Click += WindowsViewitem_Click;
            windowToolStripMenuItem.DropDownItems.Add(item);
        }

        /// <summary>
        /// Brings the window associated to the sender to the front.
        /// </summary>
        /// <param name="sender">the item that triggered the event</param>-
        /// <param name="e">event arguments</param>
        private static void WindowsViewitem_Click(object sender, EventArgs e)
        {
            var form = (Form)((ToolStripMenuItem)sender).Tag;
            ShowFormAndBringItToFront(form);
        }

        private static void ShowFormAndBringItToFront(Form form)
        {
            form.Show();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
            form.BringToFront();
        }

        private void SetupDisplay()
        {
            DisplayIcons(UserSettings.DisplayImages);
        }

        private void InitializeMainFormForShowing()
        {
            //DownloadService.DownloadFile("http://agotdb.googlecode.com/files/AGoTDB%20-%20EN%20-%20Beta%200.722.zip", "c:\\temp\\toto.zip");

            SetupDisplay();
            UpdateControlsLabels();
            LoadCardTypeNames();
            LoadCardHouseNames();
            LoadCardTriggerNames();
            LoadCardPatterns();
            LoadExpansionSets();
            UpdateDataTableView();
            dataGridView.DataSource = _dataTable;

            ApplicationSettings.IsOctgnReady = ApplicationSettings.DatabaseManager.HasOctgnData();

            dataGridView.Sort(dataGridView.Columns["Name"], ListSortDirection.Ascending);
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);
            // the following code was supposed to change the width of the bool-type columns only. But it doesn't work...
            /*for (int i = 0; i < dataGridView.Columns.Count; ++i)
                if (dataGridView.Columns[i].GetType() != System.Type.GetType("System.Boolean"))
                    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridView.AutoResizeColumns();*/

            // set options
            miLcgSetsOnly.Checked = UserSettings.LcgSetsOnly;
            UpdateSetsChoicesControl();

            if (UserSettings.DisplayImages)
            {
                _cardPreviewForm.Visible = true;
            }
            else
            {
                splitCardText.Panel2Collapsed = true;
                splitCardText.IsSplitterFixed = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            rtbCardDetails.Clear();
            cardPreviewControl.Visible = false;
            btnReportError.Enabled = false;
            if (dataGridView.SelectedRows.Count != 0)
            {
                DataRow row = ((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row;
                ShowCardDetails(row);
                btnReportError.Enabled = true;
            }
        }

        private void ShowCardDetails(DataRow row)
        {
            _displayedCard = new AgotCard(row);
            lblUniversalId.Text = _displayedCard.UniversalId.ToString();
            foreach (FormattedText ft in _displayedCard.ToFormattedString())
            {
                rtbCardDetails.SelectionFont = new Font(rtbCardDetails.SelectionFont, ft.Format.Style);
                rtbCardDetails.SelectionColor = ft.Format.Color;
                rtbCardDetails.AppendText(ft.Text);
            }
            if (UserSettings.DisplayImages)
                UpdateCardImage(_displayedCard);
        }

        private void deckBuilderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeckBuilderForm.Singleton.Show();
            DeckBuilderForm.Singleton.Activate();
        }

        /// <summary>
        /// Adds a card to the deck or sideboard using the given delegate.
        /// </summary>
        /// <param name="addCard"></param>
        private void AddCardToDeckOrSide(CardOperation addCard)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;
            DataRow row = ((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row;
            var card = new AgotCard(row);
            if (!addCard(card))
                MessageBox.Show(Resource1.WarnImpossibleToAddCard, Resource1.WarnImpossibleToAddCardTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((e is MouseEventArgs) && (((MouseEventArgs)e).Y > dataGridView.ColumnHeadersHeight)) // below the column header?
            {
                CardOperation addCard = DeckBuilderForm.Singleton.AddCardToCurrent;
                AddCardToDeckOrSide(addCard);
            }
        }

        private void addCardToDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CardOperation addCard = DeckBuilderForm.Singleton.AddCardToDeck;
            AddCardToDeckOrSide(addCard);
        }

        private void addCardToSideboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CardOperation addCard = DeckBuilderForm.Singleton.AddCardToSide;
            AddCardToDeckOrSide(addCard);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isMainForm)
            {
                if (DeckBuilderForm.SingletonExists() && DeckBuilderForm.Singleton.Visible) // to be totally bullet-proof, we should create a lock to avoid concurrency
                {
                    DeckBuilderForm.Singleton.Close();
                    e.Cancel = DeckBuilderForm.Singleton.Visible; // cancel if deck builder form was not closed
                }
                if (!e.Cancel)
                    SaveGridSettings();
            }
            else
            {
                _mainForm.RemoveFormFromWindowMenu(this); // remove reference in the Window menu
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_isMainForm)
                Application.Exit();
        }

        private void RemoveFormFromWindowMenu(MainForm form)
        {
            for (var i = 0; i < windowToolStripMenuItem.DropDownItems.Count; ++i)
                if (windowToolStripMenuItem.DropDownItems[i].Tag == form)
                {
                    windowToolStripMenuItem.DropDownItems.RemoveAt(i);
                    return;
                }
        }

        private void SaveGridSettings()
        {
            var columnsSettings = new List<string>();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                var setting = new ColumnSetting(column.Name, column.DisplayIndex, column.Width);
                columnsSettings.Add(setting.ToString());
            }
            UserSettings.ColumnsSettings = string.Join("|", columnsSettings);
            UserSettings.Save();
        }

        private class ColumnSetting
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public int Width { get; set; }

            public override string ToString()
            {
                return string.Format("{0};{1};{2}", Name, Index, Width);
            }

            public ColumnSetting(string setting)
            {
                var values = setting.Split(';');
                Name = values[0];
                Index = Convert.ToInt32(values[1]);
                Width = Convert.ToInt32(values[2]);
            }

            public ColumnSetting(string name, int index, int width)
            {
                Name = name;
                Index = index;
                Width = width;
            }
        }

        private void miLcgSetsOnly_Click(object sender, EventArgs e)
        {
            var lcgSetsOnly = miLcgSetsOnly.Checked;
            UserSettings.LcgSetsOnly = lcgSetsOnly;
            UserSettings.Save();

            UpdateSetsChoicesControl();
        }

        /// <summary>
        /// Updates the extended checklist box control according to the 
        /// user settings.
        /// </summary>
        private void UpdateSetsChoicesControl()
        {
            bool lcgSetsOnly = UserSettings.LcgSetsOnly;
            // keep the state of the checked items
            var checkedItems = eclExpansionSet.GetItemsByState(CheckState.Checked);
            var indeterminateItems = eclExpansionSet.GetItemsByState(CheckState.Indeterminate);

            // reload the items by filtering them if the "LCG only" checkbox is checked
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclExpansionSet, ApplicationSettings.DatabaseManager.TableNameSet, "Set", TableType.ValueShortName,
                lcgSetsOnly
                    ? (item => AgotCard.ExpansionSets[item.ShortName])
                    : (Predicate<DbFilter>)null);

            // recheck the items the way they were before
            eclExpansionSet.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                for (int i = 0; i < ecl.Items.Count; ++i)
                {
                    var item = (DbFilter)ecl.Items[i];
                    if (checkedItems.Any(o => ((DbFilter)o).ShortName == item.ShortName))
                        ecl.SetItemCheckState(i, CheckState.Checked);
                    else if (indeterminateItems.Any(o => ((DbFilter)o).ShortName == item.ShortName))
                        ecl.SetItemCheckState(i, CheckState.Indeterminate);
                }
            });
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var optionsForm = new OptionsForm())
            {
                if (optionsForm.ShowDialog() == DialogResult.OK)
                    SetupDisplay();
            }
        }

        private void btnReportError_Click(object sender, EventArgs e)
        {
            const string address = "agotdeckbuilder@gmail.com";
            var subject = string.Format(Resource1.ReportCardErrorMessageTitle, _displayedCard.UniversalId);
            var body = new StringBuilder();
            body.AppendLine(Resource1.ReportCardErrorMessageBody);
            body.AppendLine();
            body.AppendLine("--------------------");
            body.AppendLine();

            foreach (FormattedText ft in _displayedCard.ToFormattedString())
                body.Append(ft.Text);

            body.AppendLine();
            body.AppendLine();
            body.AppendLine(ApplicationSettings.ApplicationVersion.ToString());
            var databaseInfo = ApplicationSettings.DatabaseManager.DatabaseInfos.Count > 0
                ? ApplicationSettings.DatabaseManager.DatabaseInfos[0]
                : null;
            if (databaseInfo != null)
                body.AppendLine(string.Format(CultureInfo.InvariantCulture, "DB version: {0} ({1})",
                databaseInfo.VersionId, databaseInfo.DateCreation.HasValue ? databaseInfo.DateCreation.Value.ToShortDateString() : string.Empty));

            var encodedBody = Uri.EscapeDataString(body.ToString());

            var process = string.Format(@"mailto:{0}?subject={1}&body={2}",
                address,
                Uri.EscapeDataString(subject),
                encodedBody);
            // open the client messaging
            System.Diagnostics.Process.Start(process);
        }

        private void cardPreviewControl_MouseEnter(object sender, EventArgs e)
        {
            ShowCardPreviewForm(cardPreviewControl.CardUniversalId);
        }

        private void cardPreviewControl_MouseLeave(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        private void cardPreviewControl_MouseCaptureChanged(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        /// <summary>
        /// Updates the card image preview with a given card.
        /// </summary>
        private void UpdateCardImage(AgotCard card)
        {
            cardPreviewControl.Visible = true;
            cardPreviewControl.CardUniversalId = card.UniversalId;
        }

        /// <summary>
        /// Shows the card preview form for given card id.
        /// </summary>
        /// <param name="universalId">The card id.</param>
        private void ShowCardPreviewForm(int universalId)
        {
            if (!ApplicationSettings.ImagesFolderExists || !UserSettings.DisplayImages)
                return;
            _cardPreviewForm.CardUniversalId = universalId;
            var x = this.Location.X + this.Width;
            var y = this.Location.Y + 10;

            _cardPreviewForm.Location = new Point(x, y);
        }

        /// <summary>
        /// Hides the card preview form.
        /// </summary>
        private void HideCardPreviewForm()
        {
            _cardPreviewForm.Hide();
        }

        private void lblUniversalId_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblUniversalId.Text);
        }

        private void loadOCTGNDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OctgnManager.PromptForInitialization();
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (UserSettings.ColumnsSettings != null)
            {
                var columnsSettings = UserSettings.ColumnsSettings.Split('|').Select(s => new ColumnSetting(s)).OrderByDescending(cs => cs.Index);
                foreach (var columnSetting in columnsSettings)
                {
                    var column = dataGridView.Columns[columnSetting.Name];
                    column.Width = Convert.ToInt32(columnSetting.Width);
                }
            }
        }
    }
}
