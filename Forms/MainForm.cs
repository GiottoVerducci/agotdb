// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009 Vincent Ripoll
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using AGoT.AGoTDB.BusinessObjects;
using Beyond.ExtendedControls;

namespace AGoT.AGoTDB.Forms
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

    }

    /// <summary>
    /// Initializes this instance as the main form.
    /// </summary>
    public void InitializeMainForm()
    {
      fIsMainForm = true;
      fMainForm = this;
      fMustClose = (UserSettings.Singleton == null) || !DatabaseInterface.Singleton.ConnectedToDatabase;
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
      Text = String.Format(CultureInfo.CurrentCulture, Resource1.ViewFormTitle, fViewIndex) + " | " + fQuery.HumanQuery;
    }

    private static void LoadCardTypeName()
    {
      DataTable types = DatabaseInterface.Singleton.GetResultFromRequest(String.Format("SELECT * FROM {0}", DatabaseInterface.TableName.Type));
      Card.CardTypeNames = new List<TagText>();
      foreach (DataRow row in types.Rows)
      {
        // ZONK cast en int possible?
        if (Int32.Parse(row["Id"].ToString()) >= 0)
          // ZONK cast en int possible?
          Card.CardTypeNames.Add(new TagText(row["Value"].ToString(), Int32.Parse(row["ShortName"].ToString())));
      }
    }

    private static void LoadHouseName()
    {
      DataTable houses = DatabaseInterface.Singleton.GetResultFromRequest(String.Format("SELECT * FROM {0}", DatabaseInterface.TableName.House));
      Card.CardHouseNames = new List<TagText>();
      foreach (DataRow row in houses.Rows)
      {
        // ZONK cast en int possible?
        if (Int32.Parse(row["Id"].ToString()) >= 0)
          // ZONK cast en int possible?
          Card.CardHouseNames.Add(new TagText(row["Value"].ToString(), Int32.Parse(row["Id"].ToString())));
      }
    }

    /// <summary>
    /// Updates the content and caption of the controls with data read in the database.
    /// Therefore, new values (for example a new keyword) are automatically handled by
    /// the controls and the query filter.
    /// </summary>
    private void UpdateControlsLabels()
    {
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclHouse, DatabaseInterface.TableName.House, "House", DatabaseInterface.TableType.ValueKey);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclCardtype, DatabaseInterface.TableName.Type, "Type", DatabaseInterface.TableType.ValueShortName);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclProvides, DatabaseInterface.TableName.Provides, "", DatabaseInterface.TableType.ValueKey);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclMecanism, DatabaseInterface.TableName.Mechanism, "", DatabaseInterface.TableType.ValueKey);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclIcon, DatabaseInterface.TableName.Icon, "", DatabaseInterface.TableType.ValueKey);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclVirtue, DatabaseInterface.TableName.Virtue, "", DatabaseInterface.TableType.ValueKey);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclKeyword, DatabaseInterface.TableName.Keyword, "Keywords", DatabaseInterface.TableType.Value);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclTrigger, DatabaseInterface.TableName.Trigger, "Text", DatabaseInterface.TableType.Value);
      DatabaseInterface.Singleton.UpdateExtendedCheckedListBox(eclExpansionSet, DatabaseInterface.TableName.Set, "Set", DatabaseInterface.TableType.ValueShortName);
      DatabaseInterface.Singleton.UpdateFilterMenu(filterToolStripMenuItem, DatabaseInterface.TableName.FilterText, tbCardtext, eclCardtextCheck);
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
    /// <param name="i">Must be 0 on the first call</param>
    /// <returns>The expression used by the data column</returns>
    private static String BuildTypeExpression(int i)
    {
      if (i < Card.CardTypeNames.Count - 1)
        return String.Format("IIF(Type={0}, '{1}', {2})", Card.CardTypeNames[i].Tag, Card.CardTypeNames[i].Text, BuildTypeExpression(i + 1));
      return String.Format("'{0}'", Card.CardTypeNames[i].Text);
    }

    /// <summary>
    /// Adds dynamically computed columns to the main data table.
    /// </summary>
    private void CustomizeDataTable()
    {
      var houseTempColumn = new DataColumn("HouseTemp", Type.GetType("System.String"));
      houseTempColumn.Expression = String.Format("IIF(HouseNeutral, '{0}/', '') + IIF(HouseStark, '{1}/', '') + IIF(HouseLannister, '{2}/', '') + IIF(HouseBaratheon, '{3}/', '') + IIF(HouseGreyjoy, '{4}/', '') + IIF(HouseMartell, '{5}/', '') + IIF(HouseTargaryen, '{6}/', '')", Card.GetHouseName((Int32)Card.CardHouse.Neutral), Card.GetHouseName((Int32)Card.CardHouse.Stark), Card.GetHouseName((Int32)Card.CardHouse.Lannister), Card.GetHouseName((Int32)Card.CardHouse.Baratheon), Card.GetHouseName((Int32)Card.CardHouse.Greyjoy), Card.GetHouseName((Int32)Card.CardHouse.Martell), Card.GetHouseName((Int32)Card.CardHouse.Targaryen));
      fDataTable.Columns.Add(houseTempColumn);

      var houseColumn = new DataColumn("House", Type.GetType("System.String"));
      houseColumn.Expression = String.Format("SUBSTRING(HouseTemp, 1, LEN(HouseTemp) - 1)");
      fDataTable.Columns.Add(houseColumn);
      fDataTable.Columns["House"].SetOrdinal(fDataTable.Columns["Name"].Ordinal + 1);

      var typeColumn = new DataColumn("Type ", Type.GetType("System.String"));
      typeColumn.Expression = BuildTypeExpression(0);
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
          // ZONK : cast en int possible?
          selectedRowId = Int32.Parse(((DataRowView)dataGridView.SelectedRows[0].DataBoundItem).Row["UniversalId"].ToString());

        fQuery = query;
        using (var dbDataAdapter = new OleDbDataAdapter(query.SqlQuery, DatabaseInterface.Singleton.DbConnection))
        {
          fDataTable.Clear();
          dbDataAdapter.Fill(fDataTable);

          if (fDataTableFirstLoad)
          {
            CustomizeDataTable();
            fDataTableFirstLoad = false;
          }

          for (var i = 0; i < fDataTable.Columns.Count; ++i)
          {
            string colName = fDataTable.Columns[i].ColumnName;
            if ((colName.IndexOf("Style", StringComparison.InvariantCultureIgnoreCase) != -1)
              || (colName.IndexOf("Errated", StringComparison.InvariantCultureIgnoreCase) != -1)
              || ((colName.StartsWith("House", StringComparison.InvariantCultureIgnoreCase) && (colName.Length > "House".Length)))
              || (string.Compare(colName, "Type", StringComparison.InvariantCultureIgnoreCase) == 0))
              fDataTable.Columns[i].ColumnMapping = MappingType.Hidden;
          }
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

    private enum PositiveDataType { LikeValue, ExactValue, Yes, Integer, KeywordValue, TriggerValue };

    /// <summary>
    /// Creates a SQL query/human query pair from the state of the controls.
    /// </summary>
    /// <returns>The SQL query/human query pair reflecting the controls state</returns>
    private Query BuildQueryFromControls()
    {
      var result = new Query(String.Format("SELECT * FROM [{0}]", DatabaseInterface.TableName.Main), "");

      Query filter = GetFilterFromExtendedCheckedListBox(eclCardtype, "OR", PositiveDataType.ExactValue) +
                     GetFilterFromExtendedCheckedListBox(eclHouse, "OR", PositiveDataType.Yes) +
                     GetFilterFromExtendedCheckedListBox(eclProvides, "AND", PositiveDataType.Integer) +
                     GetFilterFromExtendedCheckedListBox(eclMecanism, "AND", PositiveDataType.Yes) +
                     GetFilterFromExtendedCheckedListBox(eclIcon, "AND", PositiveDataType.Yes) +
                     GetFilterFromExtendedCheckedListBox(eclVirtue, "AND", PositiveDataType.Yes) +
                     GetFilterFromExtendedCheckedListBox(eclKeyword, "AND", PositiveDataType.KeywordValue) +
                     GetFilterFromExtendedCheckedListBox(eclTrigger, "OR", PositiveDataType.TriggerValue) +
                     GetFilterFromExtendedCheckedListBox(eclExpansionSet, "OR", PositiveDataType.LikeValue);

      Query costOrIncome = GetFilterFromRangeBoxes(tbGoldLow, tbGoldHigh, "Cost");
      if (!string.IsNullOrEmpty(costOrIncome.SqlQuery))
      {
        costOrIncome.SqlQuery = String.Format("(({0}) OR ({1}))", costOrIncome.SqlQuery, costOrIncome.SqlQuery.Replace("Cost", "Income"));
        costOrIncome.HumanQuery = costOrIncome.HumanQuery.Replace("Cost", Resource1.CostOrIncomeText);
      }
      filter = filter +
              costOrIncome +
              GetFilterFromRangeBoxes(tbInitiativeLow, tbInitiativeHigh, "Initiative") +
              GetFilterFromRangeBoxes(tbClaimLow, tbClaimHigh, "Claim") +
              GetFilterFromRangeBoxes(tbStrengthLow, tbStrengthHigh, "Strength") +
              GetFilterFromTextBox(tbCardtext, eclCardtextCheck, "Text") +
              GetFilterFromTextBox(tbTraits, eclTraitCheck, "Traits") +
              GetFilterFromTextBox(tbName, eclNameCheck, "Name");

      if (!string.IsNullOrEmpty(filter.SqlQuery))
      {
        result.SqlQuery += " WHERE (" + filter.SqlQuery + ")";
        result.HumanQuery += filter.HumanQuery;
      }
      return result;
    }

    /// <summary>
    /// Returns the filtering expression for queries relative to a given column within a range of value
    /// given by the Text member of two TextBox objects.
    /// If the string value of the lower bound of the range is "", then the lower bound is considered to be 0.
    /// If the string value of the upper bound of the range is "", then there is no upper bound.
    /// "X" values are always found, regardless of the bounds
    /// </summary>
    /// <param name="lowTextBox">the TextBox containing the lower bound of the range</param>
    /// <param name="highTextBox">the TextBox containing the upper bound of the range</param>
    /// <param name="column">the name of the field that is filtered</param>
    /// <returns>a string containing the filtering expression</returns>

    private static Query GetFilterFromRangeBoxes(TextBox lowTextBox, TextBox highTextBox, string column)
    {
      var result = new Query();
      string high = highTextBox.Text.Trim();
      string low = lowTextBox.Text.Trim();
      if (string.IsNullOrEmpty(low) && string.IsNullOrEmpty(high))
        return result;

      // sql part of the query
      if (!string.IsNullOrEmpty(low))
        result.SqlQuery = String.Format("({0} >= {1})", column, Int32.Parse(low, CultureInfo.CurrentCulture));
      if (!string.IsNullOrEmpty(high))
        result = result + new Query(String.Format("({0} <= {1})", column, Int32.Parse(high, CultureInfo.CurrentCulture)), "");

      result.SqlQuery = String.Format("(({0} = -1) OR ({1}))", column, result.SqlQuery);

      // human part of the query
      if (!string.IsNullOrEmpty(low))
      {
        if (!string.IsNullOrEmpty(high))
          result.HumanQuery = String.Format(CultureInfo.CurrentCulture, Resource1.RangeBetween, column, low, high);
        else
          result.HumanQuery = String.Format(CultureInfo.CurrentCulture, Resource1.RangeGreaterOrLesser, column, ">=", low);
      }
      else
        result.HumanQuery = String.Format(CultureInfo.CurrentCulture, Resource1.RangeGreaterOrLesser, column, "<=", high);
      return result;
    }

    private static string EscapeSqlCharacters(string text)
    {
      return EscapeSqlCharacters(text, true);
    }

    private static string EscapeSqlCharacters(string text, bool escapePercent)
    {
      string result = text.Replace("[", "[[]").Replace("'", "''").Replace("\"", "\"\"");
      if (escapePercent)
        result = result.Replace("%", "[%]");
      return result;
    }

    /// <summary>
    /// Returns a filter (subquery) reflecting the state of a checked box list. A logical
    /// operator "OR" or "AND" must be provided in order to know if the choices in the
    /// checked box list must be all present ("AND") or if any combination of them is sufficient ("OR").
    /// The value1 field of the object returned is used in SQL queries, the  value2 contains the query
    /// using an user-understandable form.
    /// </summary>
    /// <param name="clb">The checked box list</param>
    /// <param name="logicalOperator">Indicates the combination between the choices</param>
    /// <param name="positiveDataType">The type of result expected to make a selection positive</param>
    /// <returns>A query which can be used in a SQL query</returns>
    private static Query GetFilterFromExtendedCheckedListBox(ExtendedCheckedListBox clb, string logicalOperator, PositiveDataType positiveDataType)
    {
      var result = new Query();

      // included values
      result += GetFilterFromItems(clb.GetItemsByState(CheckState.Checked), true, logicalOperator, positiveDataType);
      // excluded values
      result += GetFilterFromItems(clb.GetItemsByState(CheckState.Indeterminate), false, logicalOperator, positiveDataType);

      if (!string.IsNullOrEmpty(result.SqlQuery))
        result.SqlQuery = "(" + result.SqlQuery.Trim() + ")";
      return result;
    }

    private static Query GetFilterFromItems(IList<object> items, bool include, string logicalOperator, PositiveDataType positiveDataType)
    {
      var result = new Query();
      string prefix = include ? "" : "NOT ";
      string expectedYes = include ? "YES" : "NO";
      if (!include)
        logicalOperator = "AND";

      for (var i = 0; i < items.Count; ++i)
      {
        var filter = (AGoTFilter)items[i];

        string subresult = "";
        switch (positiveDataType)
        {
          case PositiveDataType.LikeValue: subresult = String.Format(" ({0}{1} LIKE '%{2}%') ", prefix, filter.Column, EscapeSqlCharacters(filter.ShortName)); break;
          case PositiveDataType.ExactValue: subresult = String.Format(" ({0}{1} = {2}) ", prefix, filter.Column, EscapeSqlCharacters(filter.ShortName)); break;
          case PositiveDataType.Integer: subresult = String.Format(" (NOT(ISNULL({0}{1}))) ", prefix, filter.Column); break;
          case PositiveDataType.Yes: subresult = String.Format(" ({0} = {1}) ", filter.Column, expectedYes); break;
          case PositiveDataType.KeywordValue:
            string value = filter.ToString();
            int pos = Math.Max(value.IndexOf("..."), value.IndexOf('…'));
            if (pos == -1) // no ellipsis, keep the value as it is
              value = String.Format("%{0}.%", EscapeSqlCharacters(value));
            if (pos != -1) // ellipsis, remove the last word and "..." or "…"
            {
              value = value.Substring(0, value.Substring(0, pos).LastIndexOf(' '));
              value = String.Format("%{0}%.%", EscapeSqlCharacters(value));
            }
            value = value.Replace(" X ", " % "); // ex: Personnages X uniquement
            subresult = String.Format(" ({0}{1} LIKE '{2}') ", prefix, filter.Column, value); break;
          case PositiveDataType.TriggerValue: subresult = String.Format(" ({0}{1} LIKE '%{2}%') ", prefix, filter.Column, EscapeSqlCharacters(filter + ": ")); break;
        }
        if (!include)
          subresult = String.Format(" (({0} IS NULL) OR {1})", filter.Column, subresult);
        result.SqlQuery += subresult + " " + logicalOperator;

        // human part of the query
        result.HumanQuery += ((include) ? "" : "-") + filter + " ";
      }
      if (!string.IsNullOrEmpty(result.SqlQuery))
        result.SqlQuery = result.SqlQuery.Substring(0, result.SqlQuery.LastIndexOf(' ')); // remove the last logical operator
      return result;
    }

    private static Query GetFilterFromTextBox(TextBox textBox, ExtendedCheckBox checkBox, string column)
    {
      if (checkBox.CheckState == CheckState.Unchecked) // no filter to apply
        return new Query();

      string[] filters = textBox.Text.Split(';');
      List<string> included; // items that must appear in the query result
      List<string> excluded; // items that must not appear in the query result

      // add each filter to the right list, depending on the general mode defined by the checkbox
      // and on each individual "+" or "-" modifier
      GetIncludedAndExcluded(filters, out included, out excluded, checkBox.CheckState == CheckState.Indeterminate);
      return BuildIncludeAndExcludeQueryFromLists(included, excluded, column);
    }

    private static void GetIncludedAndExcluded(string[] filters, out List<string> included, out List<string> excluded, bool reversed)
    {
      included = new List<string>();
      excluded = new List<string>();
      // REMARK: trailing space are removed.
      // TODO: leave trailing space between quotes?
      for (var i = 0; i < filters.Length; ++i)
      {
        string current = filters[i].Trim();
        if (string.IsNullOrEmpty(current))
          continue;

        bool currentReversed = reversed;
        if (current.StartsWith("+") || current.StartsWith("-"))
        {
          currentReversed = current.StartsWith("-") ^ reversed;
          current = current.Substring(1).Trim(); // remove "+" or "-";
        }

        current = EscapeSqlCharacters(current, false);
        if (!currentReversed)
          included.Add(current);
        else
          excluded.Add(current);
      }
    }

    /// <summary>
    /// Build a query with items that must appear and items that must not appear.
    /// </summary>
    /// <param name="included">the list of items that must appear</param>
    /// <param name="excluded">the list of items that must not appear</param>
    /// <param name="column">the column on which the filter is applied</param>
    /// <returns></returns>
    private static Query BuildIncludeAndExcludeQueryFromLists(IList<string> included, IList<string> excluded, string column)
    {
      var result = new Query();

      for (var i = 0; i < included.Count; ++i)
      {
        result.SqlQuery += String.Format(" ({0} LIKE '%{1}%') AND", column, included[i]);
        result.HumanQuery += "+" + included[i] + " "; // human form of the query
      }
      for (var i = 0; i < excluded.Count; ++i)
      {
        result.SqlQuery += String.Format(" (NOT {0} LIKE '%{1}%') AND", column, excluded[i]);
        result.HumanQuery += "-" + excluded[i] + " "; // human form of the query
      }

      if (!string.IsNullOrEmpty(result.SqlQuery))
      {
        result.SqlQuery = result.SqlQuery.Substring(0, result.SqlQuery.LastIndexOf(' ')); // remove the last logical operator
        result.SqlQuery = "(" + result.SqlQuery.Trim() + ")";
        result.HumanQuery = column + " " + result.HumanQuery; // human form of the query
      }

      return result;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      ClearCheckListBoxes(eclHouse, eclCardtype, eclProvides, eclMecanism, eclIcon, eclVirtue, eclKeyword,
                          eclTrigger, eclExpansionSet);

      ClearTextBoxes(tbGoldLow, tbGoldHigh, tbInitiativeLow, tbInitiativeHigh, tbClaimLow, tbClaimHigh,
                     tbStrengthLow, tbStrengthHigh, tbCardtext, tbTraits, tbName);

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
        about.ShowDialog();
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
        // ZONK : cast en int possible?
        int curId = Int32.Parse(((DataRowView)dataGridView.Rows[i].DataBoundItem).Row["UniversalId"].ToString());
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
        quickFindRows = fDataTable.Select(String.Format("Name LIKE '%{0}%'", EscapeSqlCharacters(text, false)));
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
        // ZONK : cast en int possible?
        SelectRow(Int32.Parse(quickFindRows[quickFindIndex]["UniversalId"].ToString()));
    }

    private void saveSelectionToTextFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (var fd = new SaveFileDialog())
      {
        fd.Filter = Resource1.ExportSaveDialogFilter;
        fd.FileName = Resource1.ExportDefaultFilename;
        if (fd.ShowDialog() == DialogResult.OK)
        {
          var entries = new List<Card>();
          for (var i = 0; i < dataGridView.Rows.Count; ++i)
            entries.Add(new Card(((DataRowView)dataGridView.Rows[i].DataBoundItem).Row));

          using (var sw = new StreamWriter(fd.FileName, false, System.Text.Encoding.Default))
          {
            for (var i = 0; i < entries.Count; ++i)
            {
              sw.WriteLine(entries[i].ToPlainFullString());
              sw.WriteLine();
            }
          }
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
      var form = new MainForm();
      fViewIndex++;
      form.fQuery = new Query(fQuery.SqlQuery, fQuery.HumanQuery);
      form.InitializeViewForm(fDataTable.Copy());

      // keep a reference to the window in order to display it in the window menu
      form.Show();
      //fDataGridViewForms.Add(form);
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
      form.Show();
      if (form.WindowState == FormWindowState.Minimized)
        form.WindowState = FormWindowState.Normal;
      form.BringToFront();
    }

    private void Form1_Shown(object sender, EventArgs e)
    {
      if (fMustClose)
        Close();
      else
      {
        if (fIsMainForm)
        {
          if (!UserSettings.Singleton.ReadBool("Affichage", "Images", false))
            DisableImages();
          UpdateControlsLabels();
          LoadCardTypeName();
          LoadHouseName();
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
      }
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
      var card = new Card(row);
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
      var card = new Card(row);
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

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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