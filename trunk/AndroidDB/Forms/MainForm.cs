// AndroidDB - A card searcher and deck builder tool for the LCG "Netrunner Android"
// Copyright © 2013 Vincent Ripoll
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
// © Fantasy Flight Games 2012


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
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclProvides, ApplicationSettings.DatabaseManager.TableNameProvides, "", TableType.ValueKey);
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclIceType, ApplicationSettings.DatabaseManager.TableNameIceType, "IceType", TableType.ValueId);
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
            picCost.Visible = displayImages;
            picMu.Visible = displayImages;
            picDeckSize.Visible = displayImages;
            picStrength.Visible = displayImages;
            picAgendaPoints.Visible = displayImages;
            picDeckSize.Visible = displayImages;
            picLink.Visible = displayImages;
            picTrashCost.Visible = displayImages;
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

            Query filter;

            if (!rbAll.Checked)
            {
                var side = (Int32)(rbCorpOnly.Checked ? NraCard.CardSide.Corp : NraCard.CardSide.Runner);
                filter = new Query { SqlQuery = string.Format("Side = {0}", side) };
            }
            else
                filter = new Query();

            filter +=
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclCardtype, "OR", PositiveDataType.ExactValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclFaction, "OR", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclProvides, "AND", PositiveDataType.Integer) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclIceType, "OR", PositiveDataType.ExactValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclIcon, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclVirtue, "AND", PositiveDataType.Yes) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclKeyword, "AND", PositiveDataType.KeywordValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclTrigger, "OR", PositiveDataType.TriggerValue) +
                QueryBuilder.GetFilterFromExtendedCheckedListBox(eclExpansionSet, "OR", PositiveDataType.LikeValue, additionalIncludedSets, additionalExcludedSets);

            //Query costOrIncome = QueryBuilder.GetFilterFromRangeBoxes(tbCostLow, tbCostHigh, "Cost");
            //if (!string.IsNullOrEmpty(costOrIncome.SqlQuery))
            //{
            //    costOrIncome.SqlQuery = string.Format("(({0}) OR ({1}))", costOrIncome.SqlQuery, costOrIncome.SqlQuery.Replace("Cost", "Income"));
            //    costOrIncome.HumanQuery = costOrIncome.HumanQuery.Replace("Cost", Resource1.CostOrIncomeText);
            //}
            filter +=
                //costOrIncome +
                QueryBuilder.GetFilterFromRangeBoxes(tbCostLow, tbCostHigh, "Cost") +
                QueryBuilder.GetFilterFromRangeBoxes(tbMuLow, tbMuHigh, "MU") +
                QueryBuilder.GetFilterFromRangeBoxes(tbDeckSizeLow, tbDeckSizeHigh, "DeckSize") +
                QueryBuilder.GetFilterFromRangeBoxes(tbStrengthLow, tbStrengthHigh, "Strength") +
                QueryBuilder.GetFilterFromRangeBoxes(tbAgendaPointsLow, tbAgendaPointsHigh, "AgendaPoints") +
                QueryBuilder.GetFilterFromRangeBoxes(tbLinkLow, tbLinkHigh, "Link") +
                QueryBuilder.GetFilterFromRangeBoxes(tbTrashCostLow, tbTrashCostHigh, "TrashCost") +
                QueryBuilder.GetFilterFromRangeBoxes(tbInfluenceLow, tbInfluenceHigh, "Influence") +
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
            ClearCheckListBoxes(eclFaction, eclCardtype, eclProvides, eclIceType,
                eclIcon, eclVirtue, eclKeyword, eclTrigger, eclExpansionSet);

            ClearTextBoxes(tbCostLow, tbCostHigh, 
                tbMuLow, tbMuHigh,
                tbDeckSizeLow, tbDeckSizeHigh, 
                tbStrengthLow, tbStrengthHigh, 
                tbAgendaPointsLow, tbAgendaPointsHigh,
                tbLinkLow, tbLinkHigh,
                tbTrashCostLow, tbTrashCostHigh,
                tbInfluenceLow, tbInfluenceHigh,
                tbCardtext, tbKeywords, tbName);

            ClearCheckBoxes(eclCardtextCheck, eclKeywordCheck, eclNameCheck);

            UpdateDataTableView();
        }

        private void rbSide_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSideChoicesControl();
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
            //UpdateSetsChoicesControl();
            UpdateSideChoicesControl();
        }

        /// <summary>
        /// Updates the extended checklist box control according to the 
        /// user settings.
        /// </summary>
        private void UpdateSideChoicesControl()
        {
            var side = (Int32)(rbAll.Checked ? NraCard.CardSide.None : (rbCorpOnly.Checked ? NraCard.CardSide.Corp : NraCard.CardSide.Runner));
            var neutralSide = (Int32)NraCard.CardSide.None;

            ExtendedCheckListBoxHelper.UpdateEclAccordingToDatabase(eclCardtype, ApplicationSettings.DatabaseManager,
                ApplicationSettings.DatabaseManager.TableNameType, "Type", TableType.ValueShortName,
                side != 0
                    ? (item => 
                    { 
                        var currentSide = NraCard.CardTypeSide[Convert.ToInt32(item.ShortName)];
                        return currentSide == neutralSide || currentSide == side;
                    })
                    : (Predicate<DbFilter>)null);

            ExtendedCheckListBoxHelper.UpdateEclAccordingToDatabase(eclFaction, ApplicationSettings.DatabaseManager, 
                ApplicationSettings.DatabaseManager.TableNameFaction, "Faction", TableType.ValueKey,
                side != 0
                    ? (item =>
                    {
                        var faction = NraCard.CardFactionNames.First(kvp => kvp.Value == item.ShortName);
                        var currentSide = NraCard.CardFactionSide[faction.Key];
                        return currentSide == neutralSide || currentSide == side;
                    })
                    : (Predicate<DbFilter>)null);
        }
    }
}
