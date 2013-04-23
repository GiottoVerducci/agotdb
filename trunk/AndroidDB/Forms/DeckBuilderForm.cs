// NRADB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010 Vincent Ripoll
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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Beyond.ExtendedControls;

using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Extensions;
using GenericDB.Helper;
using NRADB.BusinessObjects;
using NRADB.Components;

namespace NRADB.Forms
{
    /// <summary>
    /// Form displaying versioned decks.
    /// </summary>
    public partial class DeckBuilderForm : Form
    {
        #region Form events (FormShown/Closed, NodeClick or FactionValue changed, tvHistory select)
        private void DeckBuilderForm_Shown(object sender, EventArgs e)
        {
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclFaction, ApplicationSettings.DatabaseManager.TableNameFaction, "Faction", TableType.ValueId);
            eclFaction.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                ecl.Summary += " - " + ecl.Items[0];
                ecl.Items.RemoveAt(0); // remove neutral faction
                ecl.UpdateSize(); // update size because neutral faction has been removed
            });
            //UpdateAgendaComboBox();
            UpdateHistoryFromVersionedDeck();
            //treeViewDeck.TreeViewNodeSorter = deckTreeNodeSorter; // we don't use it because it's slower than inserting directly at the right place (visible when loading decks)
            miGenerateProxyPdf.Visible = ApplicationSettings.ImagesFolderExists; // show the proxy generator only if the images folder exists

            if (UserSettings.DisplayImages)
            {
                _cardPreviewForm.Visible = true;
            }
            else
            {
                splitCardText.Panel2Collapsed = true;
                splitCardText.IsSplitterFixed = true;
            }
            UpdateSideChoicesControl();
            _currentDeck.Side = rbCorp.Checked ? NraCard.CardSide.Corp : NraCard.CardSide.Runner;
        }

        private void EclFaction_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateFactionFromControls();
        }

        #endregion

        #region Form update methods
        private static void UpdateTypeNodeText(TreeNode typeNode)
        {
            var count = 0;
            int maxWidth = 0, maxLength = 0;

            foreach (TreeNode node in typeNode.Nodes)
            {
                count += ((NraCard)node.Tag).Quantity;
                Font font = GetNodeFont(node);
                var cardNodeInfo = GetCardNodeInfo(node);
                maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(cardNodeInfo.Value1, font).Width);
                maxLength = Math.Max(maxLength, cardNodeInfo.Value1.Length);
            }
            var currentInfo = (TypeNodeInfo)typeNode.Tag;
            currentInfo.Width = maxWidth;
            currentInfo.Length = maxLength;

            var typeNodeValue = Convert.ToInt32(typeNode.Name, CultureInfo.InvariantCulture);

            typeNode.Text = /*typeNodeValue == (Int32)NraCard.CardType.Plot
                ? ComputeStatsPlots(((NraCardTreeView)typeNode.TreeView).Cards)
                :*/ string.Format(CultureInfo.CurrentCulture, "{0} [{1}]", NraCard.GetTypeName(typeNodeValue), count);
        }

        private void UpdateStatistics()
        {
            rtbStatistics.Clear();

            //var types = new[] { NraCard.CardType.Agenda, NraCard.CardType.Hardware, 
            //    NraCard.CardType.Ice, NraCard.CardType.Identity, NraCard.CardType.Program, 
            //    NraCard.CardType.Resource, NraCard.CardType.Upgrade};

            //var cardList = _currentDeck.CardLists[1];

            //var alreadyIntroduced = false;
            //foreach (var type in types)
            //{
            //    var traits = DeckStatisticsService.GetTraitsDistribution(type, cardList);
            //    if (traits.Count == 0)
            //        continue;
            //    AddStatsIntroIfNecessary(Resource1.TraitsText, ref alreadyIntroduced);
            //    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //    rtbStatistics.SelectionColor = Color.Black;
            //    rtbStatistics.AppendText(String.Format("{0}: ", NraCard.CardTypeNames[(int)type]));
            //    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, NraCard.TraitsFormat.Style);
            //    rtbStatistics.SelectionColor = NraCard.TraitsFormat.Color;

            //    rtbStatistics.AppendText(String.Join(", ", traits.OrderBy(kv => kv.Key).ThenByDescending(kv => kv.Value).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
            //    rtbStatistics.AppendText("\n");
            //}

            //alreadyIntroduced = false;
            //foreach (var type in types)
            //{
            //    var crests = DeckStatisticsService.GetCrestsDistribution(type, cardList);
            //    if (crests.Count > 0)
            //    {
            //        AddStatsIntroIfNecessary(Resource1.CrestsText, ref alreadyIntroduced);
            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //        rtbStatistics.SelectionColor = Color.Black;
            //        rtbStatistics.AppendText(String.Format("{0}: ", NraCard.CardTypeNames[(int)type]));

            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
            //        rtbStatistics.AppendText(String.Join(", ",
            //            crests.OrderByDescending(kv => kv.Value).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
            //        rtbStatistics.AppendText("\n");
            //    }
            //}

            //alreadyIntroduced = false;
            //{
            //    var icons = DeckStatisticsService.GetIconsDistribution(cardList);
            //    if (icons.Count > 0)
            //    {
            //        AddStatsIntroIfNecessary(Resource1.IconsText, ref alreadyIntroduced);
            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //        rtbStatistics.SelectionColor = Color.Black;
            //        rtbStatistics.AppendText(String.Format("{0}:\n", NraCard.CardTypeNames[(int)NraCard.CardType.Character]));

            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
            //        rtbStatistics.AppendText(String.Join("\n",
            //            icons.OrderByDescending(kv => kv.Value * 100 + kv.Key.Length).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
            //        rtbStatistics.AppendText("\n");
            //    }
            //    var iconsStrengths = DeckStatisticsService.GetIconsStrength(cardList);
            //    if (iconsStrengths.Count > 0)
            //    {
            //        AddStatsIntroIfNecessary(Resource1.IconsText, ref alreadyIntroduced);
            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //        rtbStatistics.SelectionColor = Color.Black;
            //        rtbStatistics.AppendText(String.Format("{0}:\n", NraCard.CardTypeNames[(int)NraCard.CardType.Character]));

            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
            //        rtbStatistics.AppendText(String.Join("\n",
            //            iconsStrengths.Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
            //        rtbStatistics.AppendText("\n");
            //    }
            //}

            //alreadyIntroduced = false;
            //foreach (var type in types)
            //{
            //    var houses = DeckStatisticsService.GetHousesDistribution(type, cardList);
            //    if (houses.Count > 0)
            //    {
            //        AddStatsIntroIfNecessary(Resource1.HouseText, ref alreadyIntroduced);
            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //        rtbStatistics.SelectionColor = Color.Black;
            //        rtbStatistics.AppendText(String.Format("{0}: ", NraCard.CardTypeNames[(int)type]));

            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
            //        rtbStatistics.AppendText(String.Join(", ",
            //            houses.OrderByDescending(kv => kv.Value * 100 + kv.Key.Length).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
            //        rtbStatistics.AppendText("\n");
            //    }
            //}

            //alreadyIntroduced = false;
            //{
            //    var influenceCost = DeckStatisticsService.GetCardInfluenceCosts(cardList);
            //    if (influenceCost.Count > 0)
            //    {
            //        AddStatsIntroIfNecessary(Resource1.InfluenceCostingCardsText, ref alreadyIntroduced);
            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
            //        rtbStatistics.SelectionColor = Color.Black;

            //        rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
            //        rtbStatistics.AppendText(String.Join(", ",
            //            influenceCost.OrderByDescending(kv => kv.Key).Select(kv => String.Format("{0} influence: {1}", kv.Key == -1 ? "X" : kv.Key.ToString(CultureInfo.InvariantCulture), kv.Value)).ToArray()));
            //        rtbStatistics.AppendText("\n");
            //    }
            //}
        }

        private void AddStatsIntroIfNecessary(string text, ref bool alreadyIntroduced)
        {
            if (!alreadyIntroduced)
            {
                rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Underline);
                rtbStatistics.SelectionColor = Color.Black;
                rtbStatistics.AppendText("\n" + text + "\n");
                alreadyIntroduced = true;
            }
        }

        #endregion

        #region Controls / Versioned deck synchronization
        private void UpdateControlsWithVersionedDeck(bool updateHistory)
        {
            UpdateControlsFromFaction();
            Application.DoEvents();
            UpdateTreeViews();

            tbDeckName.Text = _versionedDeck.Name;
            tbAuthor.Text = _versionedDeck.Author;
            rtbDescription.Text = _versionedDeck.Description;
            if (updateHistory)
                UpdateHistoryFromVersionedDeck();

            // enable or disable the edit mode depending on the Editable property of the deck
            eclFaction.Enabled = _currentDeck.Editable;
            rtbDescription.Enabled = _currentDeck.Editable;
        }

        private void UpdateVersionedDeckWithControls()
        {
            UpdateFactionFromControls();
            _versionedDeck.Name = tbDeckName.Text;
            _versionedDeck.Author = tbAuthor.Text;
            _versionedDeck.Description = rtbDescription.Text;
        }

        private void UpdateFactionFromControls()
        {
            var value = 0;
            eclFaction.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                for (var i = 0; i < ecl.Items.Count; ++i)
                    if (ecl.GetItemCheckState(i) == CheckState.Checked)
                    {
                        var factionName = ((DbFilter)ecl.Items[i]).ShortName;
                        value += Int32.Parse(factionName, CultureInfo.InvariantCulture);
                    }
            });
            _currentDeck.Factions = value;
            _currentDeck.Side = rbCorp.Checked ? NraCard.CardSide.Corp : NraCard.CardSide.Runner;
            UpdateTreeViews();
        }

        private void UpdateControlsFromFaction()
        {
            var value = _currentDeck.Factions;
            eclFaction.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                for (var i = ecl.Items.Count - 1; i >= 0; --i) // factions are sorted by increasing value
                {
                    var v = Int32.Parse(((DbFilter)ecl.Items[i]).Column, CultureInfo.InvariantCulture);
                    if (value >= v)
                    {
                        ecl.SetItemCheckState(i, CheckState.Checked);
                        value -= v;
                    }
                    else
                        ecl.SetItemCheckState(i, CheckState.Unchecked);
                }
            });
            rbCorp.Checked = _currentDeck.Side == NraCard.CardSide.Corp;
            rbRunner.Checked = _currentDeck.Side == NraCard.CardSide.Runner;
        }
        #endregion

        #region Node methods
        /// <summary>
        /// Updates the tab text to reflect the card count (excluding plots)
        /// </summary>
        /// <param name="treeView"></param>
        private static void UpdateTabText(NraCardTreeView treeView)
        {
            var tabPage = (TabPage)treeView.Parent;
            var count = treeView.Cards
                .Where(card => card.Type != null)// && card.Type.Value != (Int32)NraCard.CardType.Plot)
                .Sum(card => card.Quantity);

            var index = tabPage.Text.IndexOf('[');
            if (index > 0)
                tabPage.Text = tabPage.Text.Substring(0, index - 1);
            tabPage.Text = tabPage.Text + String.Format(CultureInfo.CurrentCulture, " [{0}]", count);
        }

        private static CardNodeInfo GetCardNodeInfo(TreeNode cardNode)
        {
            var card = (NraCard)cardNode.Tag;
            var deck = ((NraCardTreeView)cardNode.TreeView).Deck;
            var result = new CardNodeInfo(String.Format(CultureInfo.CurrentCulture, "{0}× {1}", card.Quantity, card.Name.Value), card.GetSummaryInfo());
            if (card.Unique == null)
                result.Value1 += " ?";
            else if (card.Unique.Value)
                result.Value1 += String.Format(CultureInfo.CurrentCulture, " * ({0})", card.GetShortSet());
            result.ForeColor = cardNode.TreeView.ForeColor; // default value
            result.BackColor = cardNode.TreeView.BackColor; // default value
            //if (card.Shadow != null && card.Shadow.Value)
            //{
            //    result.ForeColor = Color.White;
            //    result.BackColor = Color.Gray;
            //}
            //var deckIsCorp = NraCard.CardFactionSide.Where(kvp => kvp.Value == (Int32)NraCard.CardSide.Corp).Any(kvp => (kvp.Key & deck.Factions) != 0);
            //var deckIsRunner = NraCard.CardFactionSide.Where(kvp => kvp.Value == (Int32)NraCard.CardSide.Corp).Any(kvp => (kvp.Key & deck.Factions) != 0);
            //var deckSide = deck.Factions

            if (card.Side != null && card.Side.Value != (Int32)deck.Side)
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.Red;
            }
            else if (card.Faction != null && card.Faction.Value != (int)NraCard.CardFaction.Neutral && (deck.Factions & card.Faction.Value) == 0)
            {
                result.ForeColor = Enlighten(Color.OrangeRed, result.BackColor);
            }
            if (card.Banned != null && card.Banned.Value)
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.Red;
            }
            // if the card is restricted, check there's no other card restricted in any other deck or sideboard
            if (card.Restricted != null && card.Restricted.Value
                && (deck.CardLists.Any(cl => cl.Any(c => c.Restricted != null && c.Restricted.Value && c.UniversalId != card.UniversalId)))
                    || deck.Agenda.Any(a => a.Restricted != null && a.Restricted.Value))
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.OrangeRed;
            }
            return result;
        }
        #endregion

        #region Deck tree view drawing
        //private static String ComputeStatsPlots(NraCardList cards)
        //{
        //    int maxIncome = 0, minIncome = int.MaxValue, count = 0, totalIncome = 0;
        //    foreach (NraCard card in cards)
        //    {
        //        if (card.Type != null && card.Type.Value == (Int32)NraCard.CardType.Plot)
        //        {
        //            if (!card.Income.Value.IsX)
        //            {
        //                maxIncome = Math.Max(maxIncome, card.Income.Value.Value);
        //                minIncome = Math.Min(minIncome, card.Income.Value.Value);
        //                totalIncome += (card.Income.Value.Value) * card.Quantity;
        //            }
        //            count += card.Quantity;
        //        }
        //    }
        //    return String.Format(CultureInfo.CurrentCulture, "{0} [{1}] ({2}: {3}: {4} {5}: {6} {7}: {8:F})",
        //        NraCard.GetTypeName((Int32)NraCard.CardType.Plot), count, Resource1.IncomeText, Resource1.MinStatsText, minIncome, Resource1.MaxStatsText, maxIncome, Resource1.AvgStatsText, (totalIncome / count));
        //}

        private static String ComputeStatsPlots(NraCardList cards)
        {
            //int maxIncome = 0, minIncome = int.MaxValue, count = 0, totalIncome = 0;
            //foreach (NraCard card in cards)
            //{
            //    if (card.Type != null && card.Type.Value == (Int32)NraCard.CardType.Plot)
            //    {
            //        if (!card.Income.Value.IsX)
            //        {
            //            maxIncome = Math.Max(maxIncome, card.Income.Value.Value);
            //            minIncome = Math.Min(minIncome, card.Income.Value.Value);
            //            totalIncome += (card.Income.Value.Value) * card.Quantity;
            //        }
            //        count += card.Quantity;
            //    }
            //}
            //return String.Format(CultureInfo.CurrentCulture, "{0} [{1}] ({2}: {3}: {4} {5}: {6} {7}: {8:F})",
            //    NraCard.GetTypeName((Int32)NraCard.CardType.Plot), count, Resource1.IncomeText, Resource1.MinStatsText, minIncome, Resource1.MaxStatsText, maxIncome, Resource1.AvgStatsText, (totalIncome / count));
            return null;
        }
        #endregion

        #region Export / Import
        private string CurrentDeckToText()
        {
            var text = new StringBuilder();

            text.Append(lblDeckName.Text).AppendLine(_versionedDeck.Name);
            text.Append(lblAuthor.Text).AppendLine(_versionedDeck.Author);
            text.Append(lblFaction.Text).Append(NraCard.GetFactionName(_currentDeck.Factions));
            if (_currentDeck.Agenda.Count > 0)
            {
                var agendaNames = _currentDeck.Agenda.Select(card => card.Name.Value);
                text.AppendFormat(" ({0})", string.Join(" + ", agendaNames.ToArray()));
            }

            text.AppendLine().AppendLine();
            text.Append(tabPageDescription.Text).Append(" : ").AppendLine(_versionedDeck.Description);

            text.AppendLine().AppendLine(tabPageDeck.Text);
            text.AppendLine(TreeViewToText(treeViewDeck));
            text.AppendLine(tabPageSideboard.Text);
            text.AppendLine(TreeViewToText(treeViewSide));
            text.AppendLine(tabPageStats.Text);
            text.AppendLine(rtbStatistics.Text);
            return text.ToString();
        }

        private string CurrentDeckSortedBySetToText()
        {
            var text = new StringBuilder();

            text.Append(lblDeckName.Text).AppendLine(_versionedDeck.Name);
            text.Append(lblAuthor.Text).AppendLine(_versionedDeck.Author);
            text.Append(lblFaction.Text).Append(NraCard.GetFactionName(_currentDeck.Factions));

            text.AppendLine();

            var cardsBySet = _currentDeck.Agenda
                .Union(_currentDeck.CardLists.SelectMany(cl => cl))
                .GroupBy(c =>
                {
                    var sets = c.Set.Value.Split('/');
                    var mostRecentSet = sets.Last();
                    return mostRecentSet.Split('(')[0].Trim();
                });

            foreach (var group in cardsBySet.OrderBy(g => g.Key))
            {
                text.AppendLine();
                text.AppendLine(group.Key);
                foreach (var card in group)
                    text.AppendLine(string.Format("{0}x {1}", card.Quantity, card.Name.Value));
            }

            text.AppendLine();
            text.Append(tabPageDescription.Text).Append(" : ").AppendLine(_versionedDeck.Description);

            text.AppendLine(tabPageStats.Text);
            text.AppendLine(rtbStatistics.Text);
            return text.ToString();
        }
        #endregion

        private void rbSide_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSideChoicesControl();
            UpdateVersionedDeckWithControls();
        }

        /// <summary>
        /// Updates the extended checklist box control according to the 
        /// user settings.
        /// </summary>
        private void UpdateSideChoicesControl()
        {
            var side = (Int32)(rbCorp.Checked ? NraCard.CardSide.Corp : NraCard.CardSide.Runner);
            var neutralSide = (Int32)NraCard.CardSide.None;

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
