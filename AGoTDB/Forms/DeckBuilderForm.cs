// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AGoTDB.Components;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Extensions;

using AGoTDB.BusinessObjects;
using AGoTDB.Services;
using Beyond.ExtendedControls;

namespace AGoTDB.Forms
{
    /// <summary>
    /// Form displaying versioned decks.
    /// </summary>
    public partial class DeckBuilderForm : Form
    {
        #region Form events (FormShown/Closed, NodeClick or HouseValue changed, tvHistory select)
        private void DeckBuilderForm_Shown(object sender, EventArgs e)
        {
            ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclHouse, ApplicationSettings.DatabaseManager.TableNameHouse, "House", TableType.ValueId);
            eclHouse.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                ecl.Summary += " - " + ecl.Items[0];
                ecl.Items.RemoveAt(0); // remove neutral house
                ecl.UpdateSize(); // update size because neutral house has been removed
            });
            UpdateAgendaComboBox();
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
        }

        private void EclHouse_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateHouseFromControls();
        }

        private void EclAgenda_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateAgendaFromControls();
        }

        private void CbJoustMeleeChanged(object sender, EventArgs e)
        {
            UpdateDeckFormatFromControls();
        }
        #endregion

        #region Form update methods
        private static void UpdateTypeNodeText(TreeNode typeNode)
        {
            var count = 0;
            int maxWidth = 0, maxLength = 0;

            foreach (TreeNode node in typeNode.Nodes)
            {
                count += ((AgotCard)node.Tag).Quantity;
                Font font = GetNodeFont(node);
                var cardNodeInfo = GetCardNodeInfo(node);
                maxWidth = Math.Max(maxWidth, TextRenderer.MeasureText(cardNodeInfo.Value1, font).Width);
                maxLength = Math.Max(maxLength, cardNodeInfo.Value1.Length);
            }
            var currentInfo = (TypeNodeInfo)typeNode.Tag;
            currentInfo.Width = maxWidth;
            currentInfo.Length = maxLength;

            var typeNodeValue = Convert.ToInt32(typeNode.Name, CultureInfo.InvariantCulture);

            typeNode.Text = typeNodeValue == (Int32)AgotCard.CardType.Plot
                ? ComputeStatsPlots(((AgotCardTreeView)typeNode.TreeView).Cards)
                : string.Format(CultureInfo.CurrentCulture, "{0} [{1}]", AgotCard.GetTypeName(typeNodeValue), count);
        }

        /// <summary>
        /// Loads the agenda-card items in the agenda checkbox.
        /// </summary>
        private void UpdateAgendaComboBox()
        {
            eclAgenda.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                ecl.Items.Clear();
                var table = ApplicationSettings.DatabaseManager.GetAgendas();
                foreach (DataRow row in table.Rows)
                    ecl.Items.Add(new AgotCard(row));
                ecl.UpdateSize();
            });
        }

        private void UpdateStatistics()
        {
            rtbStatistics.Clear();

            var types = new[] { AgotCard.CardType.Plot, AgotCard.CardType.Character, 
                AgotCard.CardType.Location, AgotCard.CardType.Attachment, AgotCard.CardType.Event };

            var cardList = _currentDeck.CardLists[1];

            var alreadyIntroduced = false;
            foreach (var type in types)
            {
                var traits = DeckStatisticsService.GetTraitsDistribution(type, cardList);
                if (traits.Count == 0)
                    continue;
                AddStatsIntroIfNecessary(Resource1.TraitsText, ref alreadyIntroduced);
                rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                rtbStatistics.SelectionColor = Color.Black;
                rtbStatistics.AppendText(String.Format("{0}: ", AgotCard.CardTypeNames[(int)type]));
                rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, AgotCard.TraitsFormat.Style);
                rtbStatistics.SelectionColor = AgotCard.TraitsFormat.Color;

                rtbStatistics.AppendText(String.Join(", ", traits.OrderBy(kv => kv.Key).ThenByDescending(kv => kv.Value).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
                rtbStatistics.AppendText("\n");
            }

            alreadyIntroduced = false;
            foreach (var type in types)
            {
                var crests = DeckStatisticsService.GetCrestsDistribution(type, cardList);
                if (crests.Count > 0)
                {
                    AddStatsIntroIfNecessary(Resource1.CrestsText, ref alreadyIntroduced);
                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                    rtbStatistics.SelectionColor = Color.Black;
                    rtbStatistics.AppendText(String.Format("{0}: ", AgotCard.CardTypeNames[(int)type]));

                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
                    rtbStatistics.AppendText(String.Join(", ",
                        crests.OrderByDescending(kv => kv.Value).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
                    rtbStatistics.AppendText("\n");
                }
            }

            alreadyIntroduced = false;
            {
                var icons = DeckStatisticsService.GetIconsDistribution(cardList);
                if (icons.Count > 0)
                {
                    AddStatsIntroIfNecessary(Resource1.IconsText, ref alreadyIntroduced);
                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                    rtbStatistics.SelectionColor = Color.Black;
                    rtbStatistics.AppendText(String.Format("{0}:\n", AgotCard.CardTypeNames[(int)AgotCard.CardType.Character]));

                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
                    rtbStatistics.AppendText(String.Join("\n",
                        icons.OrderByDescending(kv => kv.Value * 100 + kv.Key.Length).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
                    rtbStatistics.AppendText("\n");
                }
                var iconsStrengths = DeckStatisticsService.GetIconsStrength(cardList);
                if (iconsStrengths.Count > 0)
                {
                    AddStatsIntroIfNecessary(Resource1.IconsText, ref alreadyIntroduced);
                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                    rtbStatistics.SelectionColor = Color.Black;
                    rtbStatistics.AppendText(String.Format("{0}:\n", AgotCard.CardTypeNames[(int)AgotCard.CardType.Character]));

                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
                    rtbStatistics.AppendText(String.Join("\n",
                        iconsStrengths.Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
                    rtbStatistics.AppendText("\n");
                }
            }

            alreadyIntroduced = false;
            foreach (var type in types)
            {
                var houses = DeckStatisticsService.GetHousesDistribution(type, cardList);
                if (houses.Count > 0)
                {
                    AddStatsIntroIfNecessary(Resource1.HouseText, ref alreadyIntroduced);
                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                    rtbStatistics.SelectionColor = Color.Black;
                    rtbStatistics.AppendText(String.Format("{0}: ", AgotCard.CardTypeNames[(int)type]));

                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
                    rtbStatistics.AppendText(String.Join(", ",
                        houses.OrderByDescending(kv => kv.Value * 100 + kv.Key.Length).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
                    rtbStatistics.AppendText("\n");
                }
            }

            alreadyIntroduced = false;
            {
                var influenceCost = DeckStatisticsService.GetCardInfluenceCosts(cardList);
                if (influenceCost.Count > 0)
                {
                    AddStatsIntroIfNecessary(Resource1.InfluenceCostingCardsText, ref alreadyIntroduced);
                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Regular);
                    rtbStatistics.SelectionColor = Color.Black;

                    rtbStatistics.SelectionFont = new Font(rtbStatistics.SelectionFont, FontStyle.Bold);
                    rtbStatistics.AppendText(String.Join(", ",
                        influenceCost.OrderByDescending(kv => kv.Key).Select(kv => String.Format("{0} influence: {1}", kv.Key == -1 ? "X" : kv.Key.ToString(CultureInfo.InvariantCulture), kv.Value)).ToArray()));
                    rtbStatistics.AppendText("\n");
                }
            }
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
            UpdateControlsFromHouse();
            UpdateControlsFromAgenda();
            UpdateControlsFromDeckFormat();
            Application.DoEvents();
            UpdateTreeViews();

            tbDeckName.Text = _versionedDeck.Name;
            tbAuthor.Text = _versionedDeck.Author;
            rtbDescription.Text = _versionedDeck.Description;
            if (updateHistory)
                UpdateHistoryFromVersionedDeck();

            // enable or disable the edit mode depending on the Editable property of the deck
            eclHouse.Enabled = _currentDeck.Editable;
            eclAgenda.Enabled = _currentDeck.Editable;
            rbJoust.Enabled = _currentDeck.Editable;
            rbMelee.Enabled = _currentDeck.Editable;
            rtbDescription.Enabled = _currentDeck.Editable;
        }

        private void UpdateVersionedDeckWithControls()
        {
            UpdateHouseFromControls();
            _versionedDeck.Name = tbDeckName.Text;
            _versionedDeck.Author = tbAuthor.Text;
            _versionedDeck.Description = rtbDescription.Text;
        }

        private void UpdateHouseFromControls()
        {
            var h = 0;
            eclHouse.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                for (var i = 0; i < ecl.Items.Count; ++i)
                    if (ecl.GetItemCheckState(i) == CheckState.Checked)
                        h += Int32.Parse(((DbFilter)ecl.Items[i]).Column, CultureInfo.InvariantCulture);
            });
            _currentDeck.Houses = h;
            UpdateTreeViews();
        }

        private void UpdateControlsFromHouse()
        {
            var h = _currentDeck.Houses;
            eclHouse.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                for (var i = ecl.Items.Count - 1; i >= 0; --i) // houses are sorted by increasing value
                {
                    var hv = Int32.Parse(((DbFilter)ecl.Items[i]).Column, CultureInfo.InvariantCulture);
                    if (h >= hv)
                    {
                        ecl.SetItemCheckState(i, CheckState.Checked);
                        h -= hv;
                    }
                    else
                        ecl.SetItemCheckState(i, CheckState.Unchecked);
                }
            });
        }

        private void UpdateAgendaFromControls()
        {
            var agenda = new AgotCardList();
            eclAgenda.WorkOnExpandedItems(ecl => agenda.AddRange(ecl.Items.Cast<object>()
                .Where((t, i) => ecl.GetItemCheckState(i) == CheckState.Checked)
                .Select(t => t as AgotCard)));
            _currentDeck.Agenda.Clear();
            _currentDeck.Agenda.AddRange(agenda);
        }

        private void UpdateControlsFromAgenda()
        {
            // check for agenda in the deck but missing in our list 
            var missingAgenda = new List<AgotCard>();
            var currentAgenda = new List<AgotCard>(_currentDeck.Agenda);
            missingAgenda.AddRange(_currentDeck.Agenda);

            eclAgenda.WorkOnExpandedItems(delegate(ExtendedCheckedListBox ecl)
            {
                missingAgenda.RemoveAll(a => ecl.Items.Contains(a));

                ecl.Items.AddRange(missingAgenda.ToArray());
                for (var i = ecl.Items.Count - 1; i >= 0; --i)
                {
                    var agenda = ecl.Items[i] as AgotCard;
                    ecl.SetItemCheckState(i, currentAgenda.Contains(agenda) ? CheckState.Checked : CheckState.Unchecked);
                }
            });
        }

        private void UpdateDeckFormatFromControls()
        {
            _currentDeck.DeckFormat = rbJoust.Checked ? AgotDeckFormat.Joust : AgotDeckFormat.Melee;
        }

        private void UpdateControlsFromDeckFormat()
        {
            rbJoust.Checked = _currentDeck.DeckFormat == AgotDeckFormat.Joust;
            rbMelee.Checked = _currentDeck.DeckFormat == AgotDeckFormat.Melee;
        }
        #endregion

        #region Node methods
        /// <summary>
        /// Updates the tab text to reflect the card count (excluding plots)
        /// </summary>
        /// <param name="treeView"></param>
        private static void UpdateTabText(AgotCardTreeView treeView)
        {
            var tabPage = (TabPage)treeView.Parent;
            var count = treeView.Cards
                .Where(card => card.Type != null && card.Type.Value != (Int32)AgotCard.CardType.Plot)
                .Sum(card => card.Quantity);

            var index = tabPage.Text.IndexOf('[');
            if (index > 0)
                tabPage.Text = tabPage.Text.Substring(0, index - 1);
            tabPage.Text = tabPage.Text + String.Format(CultureInfo.CurrentCulture, " [{0}]", count);
        }

        private static CardNodeInfo GetCardNodeInfo(TreeNode cardNode)
        {
            var card = (AgotCard)cardNode.Tag;
            var deck = ((AgotCardTreeView)cardNode.TreeView).Deck;
            var result = new CardNodeInfo(String.Format(CultureInfo.CurrentCulture, "{0}× {1}", card.Quantity, card.Name.Value), card.GetSummaryInfo());
            if (card.Unique == null)
                result.Value1 += " ?";
            else if (card.Unique.Value)
                result.Value1 += String.Format(CultureInfo.CurrentCulture, " * ({0})", card.GetShortSet());
            result.ForeColor = cardNode.TreeView.ForeColor; // default value
            result.BackColor = cardNode.TreeView.BackColor; // default value
            if (card.Shadow != null && card.Shadow.Value)
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.Gray;
            }
            if (card.House != null && card.House.Value != (int)AgotCard.CardHouse.Neutral && (deck.Houses & card.House.Value) == 0)
            {
                result.ForeColor = Enlighten(Color.OrangeRed, result.BackColor);
            }
            if (card.Banned != null && card.Banned.Value)
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.Red;
            }
            // if the card is restricted, check there's no other card restricted in any other deck or sideboard

            if (deck.DeckFormat == AgotDeckFormat.Joust)
            {
                if (card.RestrictedJoust != null && card.RestrictedJoust.Value
                    && (deck.CardLists.Any(cl => cl.Any(c => c.RestrictedJoust != null && c.RestrictedJoust.Value && c.UniversalId != card.UniversalId)))
                        || deck.Agenda.Any(a => a.RestrictedJoust != null && a.RestrictedJoust.Value))
                {
                    result.ForeColor = Color.White;
                    result.BackColor = Color.OrangeRed;
                }
            }
            else if (deck.DeckFormat == AgotDeckFormat.Melee)
            {
                if (card.RestrictedMelee != null && card.RestrictedMelee.Value
                    && (deck.CardLists.Any(cl => cl.Any(c => c.RestrictedMelee != null && c.RestrictedMelee.Value && c.UniversalId != card.UniversalId)))
                        || deck.Agenda.Any(a => a.RestrictedMelee != null && a.RestrictedMelee.Value))
                {
                    result.ForeColor = Color.White;
                    result.BackColor = Color.OrangeRed;
                }
            }
            return result;
        }
        #endregion

        private static String ComputeStatsPlots(AgotCardList cards)
        {
            int maxIncome = 0, minIncome = int.MaxValue, count = 0, totalIncome = 0;
            foreach (AgotCard card in cards)
            {
                if (card.Type != null && card.Type.Value == (Int32)AgotCard.CardType.Plot)
                {
                    if (!card.Income.Value.IsX)
                    {
                        maxIncome = Math.Max(maxIncome, card.Income.Value.Value);
                        minIncome = Math.Min(minIncome, card.Income.Value.Value);
                        totalIncome += (card.Income.Value.Value) * card.Quantity;
                    }
                    count += card.Quantity;
                }
            }
            return String.Format(CultureInfo.CurrentCulture, "{0} [{1}] ({2}: {3}: {4} {5}: {6} {7}: {8:F})",
                AgotCard.GetTypeName((Int32)AgotCard.CardType.Plot), count, Resource1.IncomeText, Resource1.MinStatsText, minIncome, Resource1.MaxStatsText, maxIncome, Resource1.AvgStatsText, (totalIncome / count));
        }

        #region Export / Import
        private string CurrentDeckToText()
        {
            var text = new StringBuilder();

            text.Append(lblDeckName.Text).AppendLine(_versionedDeck.Name);
            text.Append(lblAuthor.Text).AppendLine(_versionedDeck.Author);
            text.Append(lblHouse.Text).Append(AgotCard.GetHouseName(_currentDeck.Houses));
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
            text.Append(lblHouse.Text).Append(AgotCard.GetHouseName(_currentDeck.Houses));

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
    }
}
