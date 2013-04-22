// NRADB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Beyond.ExtendedControls;

using GenericDB.DataAccess;
using GenericDB.Extensions;
using GenericDB.Helper;

using AndroidDB;
using NRADB.BusinessObjects;
using NRADB.DataAccess;

namespace NRADB.Forms
{
    /// <summary>
    /// The main form. Performs searches on the card database using filters and criteria. 
    /// New instances may be created afterwards as independant search forms (with no main menu and so on).
    /// </summary>
    public partial class MainForm : Form
    {
        const string ReportAddress = "agotdeckbuilder@gmail.com";

        private void InitializeDatabaseManager()
        {
            ApplicationSettings.DatabaseManager = new NraDatabaseManager("NRA.mdb", "NRAEx.mdb");
        }

        private static void LoadCardTypeNames()
        {
            var types = ApplicationSettings.DatabaseManager.GetCardTypeNames();
            NraCard.CardTypeNames = new Dictionary<int, string>();
            NraCard.CardTypeSide = new Dictionary<int, int>();
            foreach (DataRow row in types.Rows)
            {
                if ((int)row["Id"] >= 0)
                {
                    NraCard.CardTypeNames.Add((int)row["ShortName"], row["Value"].ToString());
                    NraCard.CardTypeSide.Add((int)row["ShortName"], (int)row["Side"]);
                }
            }
        }

        private static void LoadCardFactionNames()
        {
            var factions = ApplicationSettings.DatabaseManager.GetCardFactionNames();
            NraCard.CardFactionNames = new Dictionary<int, string>();
            NraCard.CardFactionSide = new Dictionary<int, int>();
            foreach (DataRow row in factions.Rows)
            {
                if ((int)row["Id"] >= 0)
                {
                    NraCard.CardFactionNames.Add((int)row["Id"], row["Value"].ToString());
                    NraCard.CardFactionSide.Add((int)row["Id"], (int)row["Side"]);
                }
            }
        }

        private static void LoadCardSideNames()
        {
            var sides = ApplicationSettings.DatabaseManager.GetCardSideNames();
            NraCard.CardSideNames = new Dictionary<int, string>();
            foreach (DataRow row in sides.Rows)
            {
                if ((int)row["Id"] >= 0)
                {
                    NraCard.CardSideNames.Add((int)row["Id"], row["Value"].ToString());
                }
            }
        }

        //private static void LoadCardTriggerNames()
        //{
        //    var triggers = ApplicationSettings.DatabaseManager.GetCardTriggerNames();
        //    NraCard.CardTriggerNames = new List<string>();
        //    foreach (DataRow row in triggers.Rows)
        //    {
        //        if ((int)row["Id"] >= 0)
        //            NraCard.CardTriggerNames.Add(row["Value"].ToString());
        //    }
        //}

        //private static void LoadCardPatterns()
        //{
        //    var patterns = ApplicationSettings.DatabaseManager.GetCardPatterns();
        //    NraCard.CardPatterns = new Dictionary<NraCard.Pattern, string>();
        //    foreach (DataRow row in patterns.Rows)
        //    {
        //        if ((int)row["Id"] >= 0)
        //            NraCard.CardPatterns.Add((NraCard.Pattern)row["Id"], row["Value"].ToString());
        //    }
        //}

        private static void LoadExpansionSets()
        {
            NraCard.ExpansionSets = new Dictionary<string, bool>();
            var sets = ApplicationSettings.DatabaseManager.GetExpansionSets();
            foreach (DataRow row in sets.Rows)
            {
                if ((int)row["Id"] >= 0)
                    NraCard.ExpansionSets.Add(row["ShortName"].ToString(), (bool)row["LCG"]);
            }

            NraCard.ChaptersNames = new Dictionary<string, string>();

            foreach (DataRow row in sets.Rows)
            {
                if ((int)row["Id"] >= 0 && (bool)row["ByChapter"])
                    NraCard.ChaptersNames.Add(row["ShortName"].ToString(), row["ChaptersNames"].ToString());
            }
        }

        /// <summary>
        /// Updates the content and caption of the controls with data read in the database.
        /// Therefore, new values (for example a new keyword) are automatically handled by
        /// the controls and the query filter.
        /// </summary>
        private void UpdateControlsLabels()
        {
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclFaction, ApplicationSettings.DatabaseManager.TableNameFaction, "Faction", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclCardtype, ApplicationSettings.DatabaseManager.TableNameType, "Type", TableType.ValueShortName);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclProvides, ApplicationSettings.DatabaseManager.TableNameProvides, "", TableType.ValueKey);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclMecanism, ApplicationSettings.DatabaseManager.TableNameMechanism, "", TableType.ValueKey);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclIcon, ApplicationSettings.DatabaseManager.TableNameIcon, "", TableType.ValueKey);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclVirtue, ApplicationSettings.DatabaseManager.TableNameVirtue, "", TableType.ValueKey);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclKeyword, ApplicationSettings.DatabaseManager.TableNameKeyword, "Keywords", TableType.Value);
            //ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclTrigger, ApplicationSettings.DatabaseManager.TableNameTrigger, "Text", TableType.Value);
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
            var types = NraCard.CardTypeNames.ToArray();
            return BuildAggregateExpression("Type", 0, types);
        }

        /// <summary>
        /// Builds recursively the expression used to display a human "Side" column
        /// </summary>
        /// <returns>The expression used by the data column</returns>
        private static string BuildSideExpression()
        {
            var sides = NraCard.CardSideNames.ToArray();
            return BuildAggregateExpression("Side", 0, sides);
        }

        /// <summary>
        /// Adds dynamically computed columns to the main data table.
        /// </summary>
        private void CustomizeDataTable()
        {
            var factionTempColumn = CreateAggregateBoolTempColumn("Faction",
                Enum.GetValues(typeof(NraCard.CardFaction))
                    .Cast<NraCard.CardFaction>()
                    .Where(e => e != NraCard.CardFaction.Unknown)
                    .Select(e => new KeyValuePair<Int32, string>((Int32)e, e.ToString())),
                NraCard.GetFactionName);
            _dataTable.Columns.Add(factionTempColumn);

            var factionColumn = new DataColumn("Faction", typeof(String))
            {
                Expression = String.Format("SUBSTRING(FactionTemp, 1, LEN(FactionTemp) - 1)")
            };
            _dataTable.Columns.Add(factionColumn);
            _dataTable.Columns["Faction"].SetOrdinal(_dataTable.Columns["Name"].Ordinal + 1);

            var typeColumn = new DataColumn("Type ", typeof(String))
            {
                Expression = BuildTypeExpression()
            };
            _dataTable.Columns.Add(typeColumn);
            _dataTable.Columns["Type "].SetOrdinal(_dataTable.Columns["Type"].Ordinal + 1);

            var sideColumn = new DataColumn("Side ", typeof(String))
            {
                Expression = BuildSideExpression()
            };
            _dataTable.Columns.Add(sideColumn);
            _dataTable.Columns["Side "].SetOrdinal(_dataTable.Columns["Side"].Ordinal + 1);

            GridViewHelper.SetDataTableColumnsSettings(_dataTable, UserSettings.ColumnsSettings);
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
                || ((columnName.StartsWith("Faction", StringComparison.InvariantCultureIgnoreCase)
                    && (columnName.Length > "Faction".Length)))
                || (string.Compare(columnName, "Type", StringComparison.InvariantCultureIgnoreCase) == 0)
                || (string.Compare(columnName, "Side", StringComparison.InvariantCultureIgnoreCase) == 0));
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
                additionalIncludedSets = checkedValues.Any(v => NraCard.ExpansionSets[v.ShortName])
                    ? null
                    : new List<DbFilter>(uncheckedValues.FindAll(v => NraCard.ExpansionSets[v.ShortName]));

                if (additionalIncludedSets != null && additionalIncludedSets.Count == 0 // all LCG expansions are removed 
                    && eclExpansionSet.GetItemsByState(CheckState.Checked).Count == 0) //and no other non-LCG set is explicit added
                    additionalExcludedSets = new List<DbFilter>(uncheckedValues);
            }

            Query filter =
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclCardtype, "OR", PositiveDataType.ExactValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclFaction, "OR", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclProvides, "AND", PositiveDataType.Integer) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclMecanism, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclIcon, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclVirtue, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclKeyword, "AND", PositiveDataType.KeywordValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclTrigger, "OR", PositiveDataType.TriggerValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclExpansionSet, "OR", PositiveDataType.LikeValue, additionalIncludedSets, additionalExcludedSets);

            Query costOrIncome = QueryBuilder.GetFilterFromRangeBoxes(tbCostLow, tbCostHigh, "Cost");
            if (!string.IsNullOrEmpty(costOrIncome.SqlQuery))
            {
                costOrIncome.SqlQuery = string.Format("(({0}) OR ({1}))", costOrIncome.SqlQuery, costOrIncome.SqlQuery.Replace("Cost", "Income"));
                costOrIncome.HumanQuery = costOrIncome.HumanQuery.Replace("Cost", Resource1.CostOrIncomeText);
            }
            filter +=
                costOrIncome +
                QueryBuilder.GetFilterFromRangeBoxes(tbMuLow, tbMuHigh, "Stat") +
                QueryBuilder.GetFilterFromRangeBoxes(tbClaimLow, tbClaimHigh, "Stat") +
                QueryBuilder.GetFilterFromRangeBoxes(tbStrengthLow, tbStrengthHigh, "Stat") +
                QueryBuilder.GetFilterFromTextBox(tbCardtext, eclCardtextCheck, "Text") +
                QueryBuilder.GetFilterFromTextBox(tbKeywords, eclKeywordCheck, "Keywords") +
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
            ClearCheckListBoxes(eclFaction, eclCardtype, eclProvides, eclMecanism,
                eclIcon, eclVirtue, eclKeyword, eclTrigger, eclExpansionSet);

            ClearTextBoxes(tbCostLow, tbCostHigh, tbMuLow, tbMuHigh,
                tbClaimLow, tbClaimHigh, tbStrengthLow, tbStrengthHigh, tbCardtext, tbKeywords, tbName);

            ClearCheckBoxes(eclCardtextCheck, eclKeywordCheck, eclNameCheck);

            UpdateDataTableView();
        }

        private void SetupDisplay()
        {
            DisplayIcons(UserSettings.DisplayImages);
        }

        private void LoadGameData()
        {
            LoadCardSideNames();
            LoadCardTypeNames();
            LoadCardFactionNames();
            //LoadCardTriggerNames();
            //LoadCardPatterns();
            LoadExpansionSets();
        }

        private void SetGameOptions()
        {
            miLcgSetsOnly.Checked = UserSettings.LcgSetsOnly;
            UpdateSetsChoicesControl();
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
                    ? (item => NraCard.ExpansionSets[item.ShortName])
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
    }
}
