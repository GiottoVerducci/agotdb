// WoMDB - A card searcher and deck builder tool for the CCG "Wizards of Mickey"
// Copyright � 2009 Vincent Ripoll
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
// � Wizards of Mickey CCG ??? ???

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using WoMDB.BusinessObjects;
using Beyond.ExtendedControls;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using System.Linq;
using WoMDB.DataAccess;

namespace WoMDB.Forms
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
			ApplicationSettings.DatabaseManager = new WomDatabaseManager("WoM.mdb", "WoMEx.mdb");
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
			WomCard.CardTypeNames = new Dictionary<int, string>();
			foreach (DataRow row in types.Rows)
			{
				if ((int)row["Id"] >= 0)
					WomCard.CardTypeNames.Add((int)row["ShortName"], row["Value"].ToString());
			}
		}

		private static void LoadCardColorNames()
		{
			var colors = ApplicationSettings.DatabaseManager.GetCardColorNames();
			WomCard.CardColorNames = new Dictionary<int, string>();
			foreach (DataRow row in colors.Rows)
			{
				if ((int)row["Id"] >= 0)
					WomCard.CardColorNames.Add((int)row["Id"], row["Value"].ToString());
			}
		}

		/// <summary>
		/// Updates the content and caption of the controls with data read in the database.
		/// Therefore, new values (for example a new keyword) are automatically handled by
		/// the controls and the query filter.
		/// </summary>
		private void UpdateControlsLabels()
		{
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclColor, ApplicationSettings.DatabaseManager.TableNameColor, "Color", TableType.ValueKey);
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclCardtype, ApplicationSettings.DatabaseManager.TableNameType, "Type", TableType.ValueShortName);
			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclExpansionSet, ApplicationSettings.DatabaseManager.TableNameSet, "Set", TableType.ValueShortName);
			ApplicationSettings.DatabaseManager.UpdateFilterMenu(filterToolStripMenuItem, ApplicationSettings.DatabaseManager.TableNameFilterText, tbCardtext, eclCardtextCheck);

		}

		/// <summary>
		/// Removes the pictures for discrete use.
		/// </summary>
		private void DisableImages()
		{
			picPower.Visible = false;
		}

		/// <summary>
		/// Builds recursively the expression used to display a human "Type" column
		/// </summary>
		/// <returns>The expression used by the data column</returns>
		private static string BuildTypeExpression()
		{
			var types = WomCard.CardTypeNames.ToArray();
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
			var colorTempColumn = new DataColumn("ColorTemp", typeof(System.String));
			colorTempColumn.Expression = String.Format("IIF(ColorNone, '{0}/', '') + IIF(ColorBlue, '{1}/', '') + IIF(ColorGreen, '{2}/', '') + IIF(ColorRed, '{3}/', '') + IIF(ColorYellow, '{4}/', '') + IIF(ColorBlack, '{5}/', '')", WomCard.GetColorName((int)WomCard.CardColor.None), WomCard.GetColorName((int)WomCard.CardColor.Blue), WomCard.GetColorName((int)WomCard.CardColor.Green), WomCard.GetColorName((int)WomCard.CardColor.Red), WomCard.GetColorName((int)WomCard.CardColor.Yellow), WomCard.GetColorName((int)WomCard.CardColor.Black));
			fDataTable.Columns.Add(colorTempColumn);

			var colorColumn = new DataColumn("Color", typeof(System.String));
			colorColumn.Expression = string.Format("SUBSTRING(ColorTemp, 1, LEN(ColorTemp) - 1)");
			fDataTable.Columns.Add(colorColumn);
			fDataTable.Columns["Color"].SetOrdinal(fDataTable.Columns["Type"].Ordinal + 1);

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
				|| ((columnName.StartsWith("Color", StringComparison.InvariantCultureIgnoreCase)
					&& (columnName.Length > "Color".Length)))
				|| (string.Compare(columnName, "Type", StringComparison.InvariantCultureIgnoreCase) == 0));
		}

		/// <summary>
		/// Creates a SQL query/human query pair from the state of the controls.
		/// </summary>
		/// <returns>The SQL query/human query pair reflecting the controls state</returns>
		private Query BuildQueryFromControls()
		{
			var result = new Query(String.Format("SELECT * FROM [{0}]", ApplicationSettings.DatabaseManager.TableNameMain), "");

			Query filter =
				QueryBuilder.GetFilterFromExtendedCheckedListBox(eclCardtype, "OR", PositiveDataType.ExactValue) +
				QueryBuilder.GetFilterFromExtendedCheckedListBox(eclColor, "OR", PositiveDataType.Yes) +
				QueryBuilder.GetFilterFromExtendedCheckedListBox(eclExpansionSet, "OR", PositiveDataType.LikeValue);

			filter +=
				QueryBuilder.GetFilterFromRangeBoxes(tbPowerLow, tbPowerHigh, "Power") +
				QueryBuilder.GetFilterFromTextBox(tbCardtext, eclCardtextCheck, "Text") +
				QueryBuilder.GetFilterFromTextBox(tbTeam, eclTeamCheck, "Team") +
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
			ClearCheckListBoxes(eclColor, eclCardtype, eclExpansionSet);

			ClearTextBoxes(tbPowerLow, tbPowerHigh, tbCardtext, tbTeam, tbName);

			ClearCheckBoxes(eclCardtextCheck, eclTeamCheck, eclNameCheck);

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
			var entries = new List<WomCard>();
			for (var i = 0; i < dataGridView.Rows.Count; ++i)
				entries.Add(new WomCard(((DataRowView)dataGridView.Rows[i].DataBoundItem).Row));

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
			LoadCardColorNames();
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
			var card = new WomCard(row);
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
			var card = new WomCard(row);
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