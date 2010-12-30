// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using AGoTDB.BusinessObjects;
using AGoTDB.DataAccess;
using Beyond.ExtendedControls;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using System.Linq;

namespace AGoTDB.Forms
{
	/// <summary>
	/// The main form. Performs searches on the card database using filters and criteria. 
	/// New instances may be created afterwards as independant search forms (with no main menu and so on).
	/// </summary>
	public partial class MainForm : Form
	{
		private bool fIsMainForm = false; // false if it's a card list window
		private static MainForm fMainForm;
		private bool fMustClose = false;
		private int fViewIndex = 1;
		private bool fDataTableFirstLoad = true; // used when the data table is loaded for the first time
		private DataRow[] quickFindRows; // quick find results
		private int quickFindIndex; // index of the current quick find result

		private readonly DataTable fDataTable = new DataTable();
		private Query fQuery = new Query();

		/// <summary>
		/// The default constructor.
		/// </summary>
		public MainForm()
		{
			// Cet appel est requis par le Concepteur Windows Form.
			InitializeComponent();
			fDataTable.Locale = System.Threading.Thread.CurrentThread.CurrentCulture; // ZONK to check
			InitializeQueryLocalization();
			InitializeDatabaseConnection();
		}

		private static void InitializeDatabaseConnection()
		{
			var createExtendedDb = UserSettings.Singleton.ReadBool("Startup", "CreateExtendedDB", true);
			ApplicationSettings.DatabaseManager = new AgotDatabaseManager("AGoT.mdb", "AGoTEx.mdb");
			var connectionResult = ApplicationSettings.DatabaseManager.Connect(createExtendedDb, ApplicationSettings.ApplicationVersion);
			switch (connectionResult.ErrorCode)
			{
				case ConnectionErrorCode.Success:
					if (createExtendedDb)
					{
						UserSettings.Singleton.WriteBool("Startup", "CreateExtendedDB", false);
						UserSettings.Singleton.Save();
					}
					break;
				case ConnectionErrorCode.InvalidVersion:
					MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrMinimalSoftwareVersionRequired,
						ApplicationSettings.DatabaseManager.DatabaseInfos[0].MinimalApplicationVersion, ApplicationSettings.ApplicationVersion), Resource1.ErrMinimalSoftwareVersionRequiredTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					break;
				case ConnectionErrorCode.FileNotFound:
					MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrDatabaseNotFound, connectionResult.Data),
						Resource1.ErrDatabaseNotFoundTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case ConnectionErrorCode.InvalidDatabase:
					MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resource1.ErrInvalidDatabase, connectionResult.Data),
						Resource1.ErrInvalidDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
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
			fIsMainForm = true;
			fMainForm = this;
			fMustClose = (UserSettings.Singleton == null) || !ApplicationSettings.DatabaseManager.ConnectedToDatabase;
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
			Text = string.Format(CultureInfo.CurrentCulture, Resource1.ViewFormTitle, fViewIndex) + " | " + fQuery.HumanQuery;
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

		private static void LoadExpansionSets()
		{
			AgotCard.ExpansionSets = new Dictionary<string, bool>();
			var sets = ApplicationSettings.DatabaseManager.GetExpansionSets();
			foreach (DataRow row in sets.Rows)
			{
				if ((int)row["Id"] >= 0)
					AgotCard.ExpansionSets.Add(row["ShortName"].ToString(), (bool)row["LCG"]);
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
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclKeyword, ApplicationSettings.DatabaseManager.TableNameKeyword, "Keywords", TableType.Value);
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclTrigger, ApplicationSettings.DatabaseManager.TableNameTrigger, "Text", TableType.Value);
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclExpansionSet, ApplicationSettings.DatabaseManager.TableNameSet, "Set", TableType.ValueShortName);
			ApplicationSettings.DatabaseManager.UpdateFilterMenu(filterToolStripMenuItem, ApplicationSettings.DatabaseManager.TableNameFilterText, tbCardtext, eclCardtextCheck);
		}

		/// <summary>
		/// Removes the pictures for discrete use.
		/// </summary>
		private void DisableImages()
		{
			picGold.Visible = false;
			picInit.Visible = false;
			picClaim.Visible = false;
			picStrength.Visible = false;
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
			if (i < types.Length - 1)
				return string.Format("IIF(Type={0}, '{1}', {2})", types[i].Key, types[i].Value, BuildTypeExpression(i + 1, types));
			return string.Format("'{0}'", types[i].Value);
		}


		/// <summary>
		/// Adds dynamically computed columns to the main data table.
		/// </summary>
		private void CustomizeDataTable()
		{
			var houseTempColumn = new DataColumn("HouseTemp", typeof(System.String));
			houseTempColumn.Expression = String.Format("IIF(HouseNeutral, '{0}/', '') + IIF(HouseStark, '{1}/', '') + IIF(HouseLannister, '{2}/', '') + IIF(HouseBaratheon, '{3}/', '') + IIF(HouseGreyjoy, '{4}/', '') + IIF(HouseMartell, '{5}/', '') + IIF(HouseTargaryen, '{6}/', '')", AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Neutral), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Stark), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Lannister), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Baratheon), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Greyjoy), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Martell), AgotCard.GetHouseName((Int32)AgotCard.CardHouse.Targaryen));
			fDataTable.Columns.Add(houseTempColumn);

			var houseColumn = new DataColumn("House", typeof(System.String));
			houseColumn.Expression = String.Format("SUBSTRING(HouseTemp, 1, LEN(HouseTemp) - 1)");
			fDataTable.Columns.Add(houseColumn);
			fDataTable.Columns["House"].SetOrdinal(fDataTable.Columns["Name"].Ordinal + 1);

			var typeColumn = new DataColumn("Type ", typeof(System.String));
			typeColumn.Expression = BuildTypeExpression();
			fDataTable.Columns.Add(typeColumn);
			fDataTable.Columns["Type "].SetOrdinal(fDataTable.Columns["Type"].Ordinal + 1);
		}

		/// <summary>
		/// Updates the view exposed by the main data table, according to the filters generated by the controls
		/// </summary>
		private void UpdateDataTableView()
		{
			var query = BuildQueryFromControls();
			if (query.SqlQuery == fQuery.SqlQuery) // query hasn't change, so the result has not change neither
				return; // nothing to do

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				int selectedRowId = -1;
				if (dataGridView.SelectedRows.Count != 0)
					selectedRowId = (int)((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row["UniversalId"];

				fQuery = query;
				using (var dbDataAdapter = new OleDbDataAdapter(query.SqlQuery, ApplicationSettings.DatabaseManager.DbConnection))
				{
					fDataTable.Clear();
					dbDataAdapter.Fill(fDataTable);

					if (fDataTableFirstLoad)
					{
						CustomizeDataTable();
						fDataTableFirstLoad = false;
					}

					foreach (DataColumn column in fDataTable.Columns)
						if (IsColumnHidden(column.ColumnName))
							column.ColumnMapping = MappingType.Hidden;

					fDataTable.Columns["UniversalId"].ColumnMapping = MappingType.Hidden;
				}
				QuickFind(tbFind.Text); // sets the new quick find results
				if (!SelectRow(selectedRowId)) // the previously selected row doesn't exist anymore
					SelectCurrentQuickFindResult(); // we attempt to select the first quick find result
			}
			catch (Exception e)
			{
				MessageBox.Show("Exception while processing query " + query.SqlQuery + "\n" + e.Message + "\n" + e.StackTrace);
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
				// add the lcg unchecked expansions to perform a search on lcg expansions only
				var uncheckedValues = eclExpansionSet.GetItemsByState(CheckState.Unchecked).ConvertAll<DbFilter>(i => (DbFilter)i);
				additionalIncludedSets = new List<DbFilter>(
					uncheckedValues.FindAll(v => AgotCard.ExpansionSets[v.ShortName]));
				if (additionalIncludedSets.Count == 0 // all LCG expansions are removed 
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
				eclIcon, eclVirtue, eclKeyword, eclTrigger, eclExpansionSet);

			ClearTextBoxes(tbGoldLow, tbGoldHigh, tbInitiativeLow, tbInitiativeHigh,
				tbClaimLow, tbClaimHigh, tbStrengthLow, tbStrengthHigh, tbCardtext, tbTraits, tbName);

			ClearCheckBoxes(eclCardtextCheck, eclTraitCheck, eclNameCheck);

			UpdateDataTableView();
		}

		private static void ClearCheckListBoxes(params ExtendedCheckedListBox[] items)
		{
			for (var i = 0; i < items.Length; ++i)
				items[i].ClearCheckBoxes();
		}

		private static void ClearCheckBoxes(params ExtendedCheckBox[] items)
		{
			for (var i = 0; i < items.Length; ++i)
				items[i].Checked = false;
		}

		private static void ClearTextBoxes(params TextBox[] items)
		{
			for (var i = 0; i < items.Length; ++i)
				items[i].Text = "";
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
			string newText = "";
			for (var i = 0; i < text.Length; ++i)
				if ((text[i] >= '0') && (text[i] <= '9'))
					newText += text[i];
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
			if ((quickFindRows == null) || (quickFindRows.Length <= 0))
				return;
			quickFindIndex = (quickFindIndex + 1) % quickFindRows.Length;
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
				quickFindRows = fDataTable.Select(String.Format("Name LIKE '%{0}%'", QueryBuilder.EscapeSqlCharacters(text, false)));
				if (quickFindRows.Length > 0)
					quickFindIndex = 0;
			}
			else
				quickFindRows = null;
		}

		/// <summary>
		/// Selects the row corresponding to the current quick find result.
		/// </summary>
		private void SelectCurrentQuickFindResult()
		{
			if ((quickFindRows != null) && (quickFindIndex >= 0) && (quickFindIndex < quickFindRows.Length))
				SelectRow((int)quickFindRows[quickFindIndex]["UniversalId"]);
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
				for (var i = 0; i < entries.Count; ++i)
				{
					sw.WriteLine(entries[i].ToPlainFullString());
					sw.WriteLine();
				}
			}
		}

		/*public Object Clone(Object obj)
	{
		MemoryStream mem = new MemoryStream();
		BinaryFormatter binFormat = new BinaryFormatter();
		binFormat.Serialize(mem, obj); // serialization of obj in memory
		mem.Seek(0, SeekOrigin.Begin); // go back to the start of the stream
		return binFormat.Deserialize(mem); // create the object
	}
	*/

		private void moveToANewWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateNewWindowWithResults();
		}

		private void CreateNewWindowWithResults()
		{
			var form = new MainForm();
			fViewIndex++;
			form.fQuery = new Query(fQuery.SqlQuery, fQuery.HumanQuery);
			form.InitializeViewForm(fDataTable.Copy());

			// keep a reference to the window in order to display it in the window menu
			form.Show();
			var item = new ToolStripMenuItem(form.Text);
			item.Tag = form;
			item.Click += WindowsViewitem_Click;
			windowToolStripMenuItem.DropDownItems.Add(item);
		}

		/// <summary>
		/// Brings the window associated to the sender to the front.
		/// </summary>
		/// <param name="sender">the item that triggered the event</param>-
		/// <param name="e">event arguments</param>
		void WindowsViewitem_Click(object sender, EventArgs e)
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

		private void Form1_Shown(object sender, EventArgs e)
		{
			if (fMustClose)
				Close();
			else if (fIsMainForm)
				InitializeMainFormForShowing();
		}

		private void InitializeMainFormForShowing()
		{
			if (!UserSettings.Singleton.ReadBool("SearchForm", "DisplayImages", false))
				DisableImages();
			UpdateControlsLabels();
			LoadCardTypeNames();
			LoadCardHouseNames();
			LoadCardTriggerNames();
			LoadExpansionSets();
			UpdateDataTableView();
			dataGridView.DataSource = fDataTable;

			dataGridView.Sort(dataGridView.Columns["Name"], ListSortDirection.Ascending);
			dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);
			// the following code was supposed to change the width of the bool-type columns only. But it doesn't work...
			/*for (int i = 0; i < dataGridView.Columns.Count; ++i)
				if (dataGridView.Columns[i].GetType() != System.Type.GetType("System.Boolean"))
					dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
			dataGridView.AutoResizeColumns();*/
		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			rtbCardDetails.Clear();
			if (dataGridView.SelectedRows.Count != 0)
			{
				DataRow row = ((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row;
				ShowCardDetails(row);
			}
		}

		private void ShowCardDetails(DataRow row)
		{
			var card = new AgotCard(row);
			foreach (FormattedText ft in card.ToFormattedString())
			{
				rtbCardDetails.SelectionFont = new Font(rtbCardDetails.SelectionFont, ft.Format.Style);
				rtbCardDetails.SelectionColor = ft.Format.Color;
				rtbCardDetails.AppendText(ft.Text);
			}
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
			if (fIsMainForm)
			{
				if (DeckBuilderForm.SingletonExists() && DeckBuilderForm.Singleton.Visible) // to be totally bullet-proof, we should create a lock to avoid concurrency
				{
					DeckBuilderForm.Singleton.Close();
					e.Cancel = DeckBuilderForm.Singleton.Visible; // cancel if deck builder form was not closed
				}
			}
			else
			{
				fMainForm.RemoveFormFromWindowMenu(this); // remove reference in the Window menu
			}
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
	}
}