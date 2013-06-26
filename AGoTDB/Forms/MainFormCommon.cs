using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using Beyond.ExtendedControls;

using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Helper;

using AGoTDB.BusinessObjects;
using AGoTDB.OCTGN;

using TCard = AGoTDB.BusinessObjects.AgotCard;

namespace AGoTDB.Forms
{
    public partial class MainForm
    {
        private TCard _displayedCard;
        private bool _isMainForm; // false if it's a card list window
        private static MainForm _mainForm;
        private static SplashScreen _splashScreen;
        private int _viewIndex = 1;
        private bool _dataTableFirstLoad = true; // used when the data table is loaded for the first time
        private bool _isDataBaseLoaded;
        private DataRow[] _quickFindRows; // quick find results
        private int _quickFindIndex; // index of the current quick find result
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
                InitializeDatabaseManager();

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

        public bool ShowConnectionErrorMessage(ConnectionResult connectionResult)
        {
            switch (connectionResult.ErrorCode)
            {
                case ConnectionErrorCode.Success:
                    return true;
                case ConnectionErrorCode.InvalidVersion:
                    return MessageBox.Show(string.Format(
                            CultureInfo.CurrentCulture,
                            Resource1.ErrMinimalSoftwareVersionRequired,
                            ApplicationSettings.DatabaseManager.DatabaseInfos[0].MinimalApplicationVersion,
                            ApplicationSettings.ApplicationVersion),
                        Resource1.ErrMinimalSoftwareVersionRequiredTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No;
                case ConnectionErrorCode.FileNotFound:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrDatabaseNotFound, connectionResult.Data),
                        Resource1.ErrDatabaseNotFoundTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                case ConnectionErrorCode.InvalidDatabase:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrInvalidDatabase, connectionResult.Data),
                        Resource1.ErrInvalidDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                case ConnectionErrorCode.ConnectionError:
                    MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrConnectingToDatabase, connectionResult.Data),
                        Resource1.ErrConnectingToDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
            }
            return true;
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
                        MessageBox.Show("Database update successful! Application will now restart.");
                        Application.Exit();
                        Process.Start(Application.ExecutablePath, string.Empty);
                        return;
                    }
                    _isDataBaseLoaded = true;
                    break;
                case ConnectionErrorCode.InvalidVersion:
                    if (ShowConnectionErrorMessage(connectionResult))
                    {
                        Application.Exit();
                        return;
                    }
                    _isDataBaseLoaded = true;
                    break;
                case ConnectionErrorCode.FileNotFound:
                    ShowConnectionErrorMessage(connectionResult);
                    Application.Exit();
                    break;
                case ConnectionErrorCode.InvalidDatabase:
                    ShowConnectionErrorMessage(connectionResult);
                    Application.Exit();
                    break;
                case ConnectionErrorCode.ConnectionError:
                    ShowConnectionErrorMessage(connectionResult);
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

        private static string BuildAggregateExpression(string fieldName, int i, KeyValuePair<int, string>[] types)
        {
            return i < types.Length - 1
                ? string.Format("IIF({0}={1}, '{2}', {3})", fieldName, types[i].Key, types[i].Value, BuildAggregateExpression(fieldName, i + 1, types))
                : string.Format("'{0}'", types[i].Value);
        }

        /// <summary>
        /// Creates a single column (name: {columnPrefix}Temp) to replace multiple bool value columns.
        /// </summary>
        /// <param name="columnPrefix">The prefix of the column, used to name the aggregate column and to retrieve the aggregated columns.</param>
        /// <param name="values">The pairs of column value / column suffix of the aggregated columns.</param>
        /// <param name="nameGetter">The functor used to retrieved the displayed value matching the aggregated columns.</param>
        private DataColumn CreateAggregateBoolTempColumn(string columnPrefix, IEnumerable<KeyValuePair<int, string>> values, Func<Int32, string> nameGetter)
        {
            var expressions = values.Select(v => string.Format("IIF({0}{1}, '{2}/', '')", columnPrefix, v.Value, nameGetter(v.Key)));
            var expression = string.Join(" + ", expressions);
            return new DataColumn(columnPrefix + "Temp", typeof(string)) { Expression = expression };
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
                if (!_dataTableFirstLoad)
                    SaveGridSettings();

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

        private void InitializeMainFormForShowing()
        {
            //DownloadService.DownloadFile("http://agotdb.googlecode.com/files/AGoTDB%20-%20EN%20-%20Beta%200.722.zip", "c:\\temp\\toto.zip");

            SetupDisplay();
            UpdateControlsLabels();
            LoadGameData();
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
            SetGameOptions();

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
            var entries = new List<Card>();
            for (var i = 0; i < dataGridView.Rows.Count; ++i)
                entries.Add(new TCard(((DataRowView)dataGridView.Rows[i].DataBoundItem).Row));

            using (var sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                foreach (Card card in entries)
                {
                    sw.WriteLine(card.ToPlainFullString());
                    sw.WriteLine();
                }
            }
        }

        private void moveToANewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewWindowWithResults();
        }

        private void resizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);
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
            _displayedCard = new TCard(row);
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
            var card = new TCard(row);
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

        private void SaveGridSettings()
        {
            UserSettings.ColumnsSettings = GridViewHelper.GetDataGridViewColumnsSettings(dataGridView);
            UserSettings.Save();
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
            var subject = string.Format(ApplicationSettings.ApplicationName + "-" + Resource1.ReportCardErrorMessageTitle, _displayedCard.UniversalId);
            var body = new StringBuilder();
            body.AppendLine(Resource1.ReportCardErrorMessageBody);
            body.AppendLine();
            body.AppendLine("--------------------");
            body.AppendLine();

            foreach (FormattedText ft in _displayedCard.ToFormattedString())
                body.Append(ft.Text);

            body.AppendLine();
            body.AppendLine();
            body.AppendLine(ApplicationSettings.ApplicationName + " " + ApplicationSettings.ApplicationVersion);
            var databaseInfo = ApplicationSettings.DatabaseManager.DatabaseInfos.Count > 0
                ? ApplicationSettings.DatabaseManager.DatabaseInfos[0]
                : null;
            if (databaseInfo != null)
                body.AppendLine(string.Format(CultureInfo.InvariantCulture, "DB version: {0} ({1})",
                databaseInfo.VersionId, databaseInfo.DateCreation.HasValue ? databaseInfo.DateCreation.Value.ToShortDateString() : string.Empty));

            var encodedBody = Uri.EscapeDataString(body.ToString());

            var process = string.Format(@"mailto:{0}?subject={1}&body={2}",
                ReportAddress,
                Uri.EscapeDataString(subject),
                encodedBody);
            // open the client messaging
            System.Diagnostics.Process.Start(process);
        }

        private void cardPreviewControl_MouseEnter(object sender, EventArgs e)
        {
            ShowCardPreviewForm(cardPreviewControl.CardUniversalId, cardPreviewControl.CardOctgnId);
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
        private void UpdateCardImage(TCard card)
        {
            cardPreviewControl.Visible = true;
            cardPreviewControl.SetId(card.UniversalId, card.OctgnId);
        }

        /// <summary>
        /// Shows the card preview form for given card id.
        /// </summary>
        /// <param name="universalId">The card id.</param>
        private void ShowCardPreviewForm(int universalId, Guid octgnId)
        {
            if (!ApplicationSettings.ImagesFolderExists || !UserSettings.DisplayImages)
                return;
            _cardPreviewForm.SetId(universalId, octgnId);
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
            var connectionResult = ApplicationSettings.DatabaseManager.PrepareConnectionToHumanDatabase();
            if (connectionResult.ErrorCode != ConnectionErrorCode.Success)
            {
                ShowConnectionErrorMessage(connectionResult);
                return;
            }

            OctgnManager.PromptForInitialization(
                () =>
                {
                    UserSettings.CreateExtendedDB = true;
                    connectionResult = InitializeDatabaseConnection();
                    if (connectionResult.ErrorCode != ConnectionErrorCode.Success)
                    {
                        ShowConnectionErrorMessage(connectionResult);
                        return;
                    }
                    UserSettings.CreateExtendedDB = false;
                    UserSettings.Save();
                    MessageBox.Show("OCTGN Import successful! Application will now restart.");
                    Application.Exit();
                    Process.Start(Application.ExecutablePath, string.Empty);
                });
        }

        private void importOCTGNImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OctgnManager.ImportImages();
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);
            GridViewHelper.SetDataGridViewColumnsSettings(dataGridView, UserSettings.ColumnsSettings);
        }
    }
}
