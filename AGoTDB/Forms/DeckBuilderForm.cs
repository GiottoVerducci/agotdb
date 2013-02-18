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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AGoTDB.BusinessObjects;
using AGoTDB.Components;
using AGoTDB.OCTGN;
using AGoTDB.Services;
using Beyond.ExtendedControls;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Extensions;
using GenericDB.Forms;

namespace AGoTDB.Forms
{
    /// <summary>
    /// Form displaying versioned decks.
    /// </summary>
    public partial class DeckBuilderForm : Form
    {
        private static readonly Object _singletonLock = new Object();
        private static DeckBuilderForm _singleton;
        private AgotVersionedDeck _versionedDeck;
        private AgotVersionedDeck _lastLoadedDeck; // used to detect changes in the versioned deck
        private AgotDeck _currentDeck; // deck currently displayed
        private String _currentFilename; // filename of the deck (non-empty if the deck was loaded or saved)
        private readonly List<AgotCardTreeView> _treeViews;
        private readonly CardPreviewForm _cardPreviewForm = new CardPreviewForm();

        private int _deckTreeViewSpaceWidth; // width of a space using the deck tree view font (used to expand the lines)
        private readonly DeckTreeNodeSorter _deckTreeNodeSorter; // not fully used, we keep our sorting algorithm when inserting new nodes

        protected internal class TypeNodeInfo
        {
            public int Width { get; set; }
            public int Length { get; set; }
            public int Type { get; set; }
            public TypeNodeInfo(int type) { Type = type; }
        }

        /// <summary>
        /// Gets the unique shared singleton instance of this class.
        /// </summary>
        public static DeckBuilderForm Singleton
        {
            get
            {
                lock (_singletonLock)
                {
                    return _singleton ?? (_singleton = new DeckBuilderForm());
                }
            }
        }

        public static bool SingletonExists()
        {
            return _singleton != null;
        }

        /// <summary>
        /// Default form constructor.
        /// </summary>
        private DeckBuilderForm()
        {
            InitializeComponent();
            _treeViews = new List<AgotCardTreeView> { treeViewSide, treeViewDeck };
            foreach (AgotCardTreeView t in _treeViews)
                t.NodeInfo = t.Nodes[0]; // must have been added during design time
            NewVersionedDeck(false);
            for (var i = 0; i < _treeViews.Count; ++i)
            {
                _treeViews[i].Cards = _currentDeck.CardLists[i];
                _treeViews[i].Deck = _currentDeck;
            }
            _deckTreeNodeSorter = new DeckTreeNodeSorter(
                UserSettings.TypeOrder,
                IsCardNode,
                x => ((TypeNodeInfo)x.Tag).Type
            );
        }

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

        private void DeckBuilderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (_singletonLock)
            {
                _singleton = null;
            }
        }

        private void treeViewDeck_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && IsCardNode(e.Node))
                ((TreeView)sender).SelectedNode = e.Node;
        }

        private void treeViewDeck_KeyDown(object sender, KeyEventArgs e)
        {
            var node = ((TreeView)sender).SelectedNode;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (IsCardNode(node))
                        SubtractCardFromCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                case Keys.Insert:
                    if (IsCardNode(node))
                        AddCardToCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
            }
        }

        private void treeViewDeck_KeyPress(object sender, KeyPressEventArgs e)
        {
            var node = ((TreeView)sender).SelectedNode;

            e.Handled = true;
            switch (e.KeyChar)
            {
                case '-':
                    if (IsCardNode(node))
                        SubtractCardFromCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                case '+':
                    if (IsCardNode(node))
                        AddCardToCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                default: e.Handled = false; break;
            }
        }

        private void eclHouse_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateHouseFromControls();
        }

        private void eclAgenda_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateAgendaFromControls();
        }

        /// <summary>
        /// Updates the card detail textbox to match the selected node in the deck tree view.
        /// If the selected node is a card, the description of the card is displayed. Otherwise,
        /// the textbox is cleared.
        /// </summary>
        private void treeViewDeck_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearCardInformations();

            if (!IsCardNode(e.Node) || (e.Node.Tag == null))
                return;
            UpdateCardInformations((AgotCard)e.Node.Tag);
        }

        private void DeckBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckDeckChanges();
        }

        private void treeViewHistory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _currentDeck = _versionedDeck[Int32.Parse(e.Node.Name, CultureInfo.InvariantCulture)];
            UpdateControlsWithVersionedDeck(false);
        }
        #endregion

        #region Main menu Items (load/save/new...)
        private void saveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVersionedDeck(false);
        }

        private void saveDeckAstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVersionedDeck(true);
        }

        private void loadDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckDeckChanges())
                LoadVersionedDeck();
        }

        private void newVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserSettings.ShowNewVersionMessage)
            {
                using (var form = new FormNewVersionInfo())
                {
                    if (form.ShowDialog() != DialogResult.OK)
                        return;
                    UserSettings.ShowNewVersionMessage = form.DisplayFormNextTime();
                    UserSettings.Save();
                }
            }
            using (var form = new RevisionCommentInputForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                _versionedDeck.AddNewVersion(form.RevisionComment());
                UpdateHistoryFromVersionedDeck();
            }
        }

        private void exportToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckToText());
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckDeckChanges())
                NewVersionedDeck(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Deck tree view context menu events (increase/decrease count...)
        private void contextMenuStripTreeView_Opening(object sender, CancelEventArgs e)
        {
            var cms = (ContextMenuStrip)sender;
            TreeNode node = ((TreeView)cms.SourceControl).SelectedNode;
            // disable items if the deck isn't editable or if the current node isn't a card node
            foreach (ToolStripMenuItem mi in contextMenuStripTreeView.Items)
                mi.Enabled = _currentDeck.Editable && (node != null) && IsCardNode(node);
            // always enabled items
            miExportDeckToClipboard.Enabled = true;
            miGenerateProxyPdf.Enabled = true;
        }

        private void miIncreaseCount_Click(object sender, EventArgs e)
        {
            var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            TreeNode node = tv.SelectedNode;
            if (IsCardNode(node)) // menuitem should be disabled for other nodes anyway
            {
                AddCardToCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
            }
        }

        private void miDecreaseCount_Click(object sender, EventArgs e)
        {
            var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            TreeNode node = tv.SelectedNode;
            if (IsCardNode(node)) // menuitem should be disabled for other nodes anyway
            {
                SubtractCardFromCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
            }
        }

        private void exportDeckToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckToText());
        }
        #endregion

        #region Form update methods

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

        private void UpdateCardNodeText(TreeNode cardNode)
        {
            if (_deckTreeViewSpaceWidth == 0)
                ComputeDeckTreeViewSpaceWidth(cardNode.TreeView);
            var cardNodeInfo = GetCardNodeInfo(cardNode);
            Font font = GetNodeFont(cardNode);
            var textWidth = TextRenderer.MeasureText(cardNodeInfo.Value1, font).Width;
            var text2Width = TextRenderer.MeasureText(cardNodeInfo.Value2, GetNodeInfoFont(cardNode)).Width;
            var nbSpacesToAdd = 1 + (((TypeNodeInfo)cardNode.Parent.Tag).Width - textWidth + text2Width) / _deckTreeViewSpaceWidth;
            cardNode.Text = cardNodeInfo.Value1 + " ".PadRight(nbSpacesToAdd);
            cardNode.ForeColor = cardNodeInfo.ForeColor;
            cardNode.BackColor = cardNodeInfo.BackColor;
        }

        /// <summary>
        /// Clears the card detail textbox and image preview.
        /// </summary>
        private void ClearCardInformations()
        {
            rtbCardText.Clear();
            cardPreviewControl.Visible = false;
        }

        /// <summary>
        /// Updates the card detail textbox and image preview with a given card.
        /// </summary>
        private void UpdateCardInformations(AgotCard card)
        {
            UpdateCardText(card);
            if (UserSettings.DisplayImages)
                UpdateCardImage(card);
        }

        /// <summary>
        /// Updates the card detail textbox with a given card.
        /// </summary>
        private void UpdateCardText(AgotCard card)
        {
            rtbCardText.Clear();
            foreach (FormattedText ft in card.ToFormattedString())
            {
                rtbCardText.SelectionFont = new Font(rtbCardText.SelectionFont, ft.Format.Style);
                rtbCardText.SelectionColor = ft.Format.Color;
                rtbCardText.AppendText(ft.Text);
            }
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

                rtbStatistics.AppendText(String.Join(", ", traits.OrderBy(kv => kv.Key).OrderByDescending(kv => kv.Value).Select(kv => String.Format("{0} {1}", kv.Value, kv.Key)).ToArray()));
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
                        influenceCost.OrderByDescending(kv => kv.Key).Select(kv => String.Format("{0} influence: {1}", kv.Key == -1 ? "X" : kv.Key.ToString(), kv.Value)).ToArray()));
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
            rtbDescription.Enabled = _currentDeck.Editable;
        }

        private void UpdateTreeViews()
        {
            rtbStatistics.BeginUpdate();
            UpdateTreeViewWithCards(treeViewDeck, _currentDeck.CardLists[1], _currentDeck); // TODO : modify to handle multiple deck lists
            UpdateTreeViewWithCards(treeViewSide, _currentDeck.CardLists[0], _currentDeck); // TODO : modify to handle multiple deck lists
            rtbStatistics.EndUpdate();
        }

        private void UpdateTreeViewWithCards(AgotCardTreeView treeView, AgotCardList cards, AgotDeck deck)
        {
            treeView.BeginUpdate();
            treeView.Cards = cards;
            treeView.Nodes.Clear();
            treeView.Nodes.Add(treeView.NodeInfo);
            foreach (AgotCard card in cards)
                AddCardToTreeView(treeView, card, false);
            treeView.Deck = deck;
            treeView.EndUpdate();
            UpdateTabText(treeView);
            UpdateStatistics();
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

        private void UpdateHistoryFromVersionedDeck()
        {
            treeViewHistory.Nodes.Clear();
            var lastVersionIndex = _versionedDeck.Count - 1;
            for (var i = 0; i < lastVersionIndex; ++i)
                treeViewHistory.Nodes.Insert(0, i.ToString(), String.Format(CultureInfo.CurrentCulture, "v{0} {1} {2}: {3}", i, _versionedDeck[i].LastModifiedDate.ToShortDateString(), _versionedDeck[i].LastModifiedDate.ToShortTimeString(), _versionedDeck[i].RevisionComments));
            treeViewHistory.Nodes.Insert(0, lastVersionIndex.ToString(), String.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Resource1.CurrentRevision, _versionedDeck.LastVersion.LastModifiedDate.ToShortDateString(), _versionedDeck.LastVersion.LastModifiedDate.ToShortTimeString()));
        }
        #endregion

        #region Node methods
        protected internal static bool IsCardNode(TreeNode node)
        {
            return node.Level == 1;
        }

        private TreeNode AddTypeNode(TreeView treeView, Int32 type)
        {
            TreeNode typeNode;

            var order = _deckTreeNodeSorter.GetTypeOrder(type);
            if (order == -1) // type not found in the order list
                typeNode = treeView.Nodes.Add(type.ToString(CultureInfo.InvariantCulture), AgotCard.GetTypeName(type)); // add it to the end
            else
            {
                // get the index of the greatest node below the node to add
                var i = 0;
                while (i < treeView.Nodes.Count)
                {
                    var nodeOrder = _deckTreeNodeSorter.GetTypeOrder(Int32.Parse(treeView.Nodes[i].Name, CultureInfo.InvariantCulture));
                    if ((nodeOrder == -1) || (nodeOrder > order))
                        break;
                    ++i;
                }
                typeNode = treeView.Nodes.Insert(i, type.ToString(CultureInfo.InvariantCulture), AgotCard.GetTypeName(type));
            }
            typeNode.Tag = new TypeNodeInfo(type);
            return typeNode;
        }

        private void AddCardToTreeView(AgotCardTreeView treeView, AgotCard card, bool alreadyPresent)
        {
            treeView.BeginUpdate();
            if (treeView.Nodes.Count == 1) // information node
            {
                if (treeView.NodeInfo == null)
                    treeView.NodeInfo = treeView.Nodes[0]; // must have been added during design time
                treeView.Nodes.Remove(treeView.NodeInfo);
            }
            TreeNode root, cardNode;
            if (!alreadyPresent)
            {
                var cardType = card.Type != null ? card.Type.Value : (Int32)AgotCard.CardType.Unknown; // we may have no type for unknown cards
                var rootIndex = treeView.Nodes.IndexOfKey(cardType.ToString(CultureInfo.InvariantCulture));
                root = rootIndex == -1
                    ? AddTypeNode(treeView, cardType)
                    : treeView.Nodes[rootIndex];

                var i = 0;
                while ((i < root.Nodes.Count) && (card.CompareTo((AgotCard)root.Nodes[i].Tag) < 0))
                    ++i;

                cardNode = root.Nodes.Insert(i, card.UniversalId.ToString(CultureInfo.InvariantCulture), card.Name.Value);
                cardNode.Tag = card;
            }
            else
            {
                // we assume that all the nodes that we're going to use exist
                root = treeView.Nodes[treeView.Nodes.IndexOfKey(card.Type.Value.ToString(CultureInfo.InvariantCulture))];
                cardNode = root.Nodes[root.Nodes.IndexOfKey(card.UniversalId.ToString(CultureInfo.InvariantCulture))];
            }
            // we must set the ToolTipText property here because the ShowNodeToolTips property of the treeview is ignored
            // when the treeView isn't large enough to display the whole node (a tooltip is displayed to show the whole node)
            var cardNodeInfo = GetCardNodeInfo(cardNode);
            cardNode.ToolTipText = cardNodeInfo.Value1 + cardNodeInfo.Value2;
            UpdateTypeNodeText(root); // update count and column size
            //UpdateCardNodeText(cardNode);
            // we must redraw each node in order to redisplay the nodes - is there another method?
            foreach (TreeNode node in root.Nodes)
                UpdateCardNodeText(node);
            root.Expand();
            treeView.EndUpdate();
            UpdateTabText(treeView); // updates the card count (excluding plots)
            UpdateStatistics();
        }

        private void SubtractCardFromTreeView(AgotCardTreeView treeView, AgotCard card, bool wasLastCopy)
        {
            treeView.BeginUpdate();
            var rootIndex = treeView.Nodes.IndexOfKey(card.Type.Value.ToString(CultureInfo.InvariantCulture));
            if (rootIndex == -1)
                return; // could not find its root

            TreeNode root = treeView.Nodes[rootIndex];

            var i = 0;
            while ((i < root.Nodes.Count) && (card.UniversalId != ((AgotCard)root.Nodes[i].Tag).UniversalId))
                ++i;

            if (i == root.Nodes.Count)
                return; // could not find the card

            TreeNode cardNode = root.Nodes[i];
            if (!wasLastCopy)
                cardNode.Tag = card;
            else
            {
                cardNode.Remove();
                if (root.Nodes.Count == 0)
                {
                    root.Remove();
                    root = null;
                }
            }
            if (!wasLastCopy)
            {
                //UpdateCardNodeText(cardNode);
                // we must set the ToolTipText property here because the ShowNodeToolTips property of the treeview is ignored
                // when the treeView isn't large enough to display the whole node (a tooltip is displayed to show the whole node)
                var cardNodeInfo = GetCardNodeInfo(cardNode);
                cardNode.ToolTipText = cardNodeInfo.Value1 + cardNodeInfo.Value2;
            }
            if (root != null)
            {
                UpdateTypeNodeText(root); // update count and column size
                foreach (TreeNode node in root.Nodes)
                    UpdateCardNodeText(node);
                root.Expand();
            }

            if (treeView.Nodes.Count == 0) // add the information node
                treeView.Nodes.Add(treeView.NodeInfo);
            treeView.EndUpdate();
            UpdateTabText(treeView); // updates the card count (excluding plots)
            UpdateStatistics();
        }

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

        /// <summary>
        /// Returns the font used to display the node in the treeview.
        /// </summary>
        /// <param name="node">The font</param>
        private static Font GetNodeFont(TreeNode node)
        {
            return node.NodeFont ?? node.TreeView.Font;
        }

        /// <summary>
        /// Returns the font used to display the extra data about the node in the treeview.
        /// The font is monospace.
        /// </summary>
        /// <returns>The monospace font</returns>
        private static Font GetNodeInfoFont(TreeNode node)
        {
            return new Font(FontFamily.GenericMonospace, GetNodeFont(node).Size);
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
            if (card.Restricted != null && card.Restricted.Value
                && (deck.CardLists.Any(cl => cl.Any(c => c.Restricted != null && c.Restricted.Value && c.UniversalId != card.UniversalId)))
                    || deck.Agenda.Any(a => a.Restricted != null && a.Restricted.Value))
            {
                result.ForeColor = Color.White;
                result.BackColor = Color.OrangeRed;
            }
            return result;
        }

        private static Color Enlighten(Color color, Color backgroundColor)
        {
            if (backgroundColor.GetBrightness() * color.GetBrightness() < 0.5f)
            {
                return Color.FromArgb(color.A,
                    Math.Min(color.R * 2, 255),
                    Math.Min(color.G * 2, 255),
                    Math.Min(color.B * 2, 255));
            }
            return color;
        }

        #endregion

        #region Deck tree view drawing
        private void treeViewDeck_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var treeView = (AgotCardTreeView)sender;
            if (treeView.IsBeingUpdated()) // no redraw when updating
                return;
            if (IsCardNode(e.Node))
            {
                Font font = GetNodeFont(e.Node);

                var cardNodeInfo = GetCardNodeInfo(e.Node);
                Color foreColor = cardNodeInfo.ForeColor;
                Color backColor = cardNodeInfo.BackColor;

                bool isFocused = treeView.Focused;
                bool isSelected = e.Node.IsSelected;

                if (isSelected)
                {
                    foreColor = isFocused ? SystemColors.HighlightText : SystemColors.WindowText;
                    backColor = isFocused ? SystemColors.Highlight : SystemColors.Control;
                }

                Brush backBrush = new SolidBrush(backColor);
                e.Graphics.FillRectangle(backBrush, e.Node.Bounds);

                var x = e.Bounds.Left + ((TypeNodeInfo)e.Node.Parent.Tag).Width; // x of the second column (in pixel)
                // e.Graphics.DrawString produit un affichage trop large (plus large que la taille mesurée avec TextRenderer.MeasureText
                // et plus large également que le rendu standard du treeview. Mais :
                //The text rendering offered by the TextRenderer class is based on GDI text rendering and is not supported for printing from Windows Forms. Instead, use the DrawString methods of the Graphics class.
                TextRenderer.DrawText(e.Graphics, cardNodeInfo.Value1, font, new Point(e.Bounds.Left, e.Bounds.Top), foreColor);
                TextRenderer.DrawText(e.Graphics, cardNodeInfo.Value2, GetNodeInfoFont(e.Node), new Point(x, e.Bounds.Top), foreColor);
            }
            else
                e.DrawDefault = true;
        }
        #endregion

        public bool AddCardToDeck(AgotCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            AgotCard c = _currentDeck.CardLists[1].AddCard(card); // TODO : modify to handle multiple deck lists
            AddCardToTreeView(treeViewDeck, c, c.Quantity > 1);
            return true;
        }

        private bool SubtractCardFromDeck(AgotCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            AgotCard c = _currentDeck.CardLists[1].SubtractCard(card); // TODO : modify to handle multiple deck lists
            SubtractCardFromTreeView(treeViewDeck, card, c == null);
            return true;
        }

        public bool AddCardToSide(AgotCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            AgotCard c = _currentDeck.CardLists[0].AddCard(card); // TODO : modify to handle multiple deck lists
            AddCardToTreeView(treeViewSide, c, c.Quantity > 1);
            return true;
        }

        private bool SubtractCardFromSide(AgotCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            AgotCard c = _currentDeck.CardLists[0].SubtractCard(card); // TODO : modify to handle multiple deck lists
            SubtractCardFromTreeView(treeViewSide, card, c == null);
            return true;
        }

        public bool AddCardToCurrent(AgotCard card)
        {
            return tabControlDecks.SelectedTab == tabPageDeck
                ? AddCardToDeck(card)
                : AddCardToSide(card);
        }

        private bool SubtractCardFromCurrent(AgotCard card)
        {
            return tabControlDecks.SelectedTab == tabPageDeck
                ? SubtractCardFromDeck(card)
                : SubtractCardFromSide(card);
        }

        private bool SaveVersionedDeck(bool forceCallToSaveDialog)
        {
            UpdateVersionedDeckWithControls();
            //_currentFilename = "toto.xml"; // for quick debug purpose
            String previousFilename = _currentFilename;
            if (string.IsNullOrEmpty(_currentFilename) || forceCallToSaveDialog)
            {
                using (var fd = new SaveFileDialog())
                {
                    fd.Filter = Resource1.DeckFileFilter;
                    fd.ValidateNames = true;
                    fd.FileName = _versionedDeck.Name + ".xml"; // use deck name as default name. Todo: remove special characters
                    if (fd.ShowDialog() != DialogResult.OK)
                        return false;
                    _currentFilename = fd.FileName;
                }
            }
            int? databaseVersion = (ApplicationSettings.DatabaseManager.DatabaseInfos.Count > 0)
                ? (int?)ApplicationSettings.DatabaseManager.DatabaseInfos[0].VersionId
                : null;

            var result = _versionedDeck.SaveToXmlFile(_currentFilename, ApplicationSettings.ApplicationName, ApplicationSettings.ApplicationVersion, databaseVersion);
            if (result == DeckSaveResult.Success)
                _lastLoadedDeck = (AgotVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
            else
            {
                MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrSaveXmlDeck, _currentFilename), Resource1.ErrDeckSaveTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _currentFilename = previousFilename; // restore filename to avoid confusion ('cause the deck wasn't saved under the new filename)
            }
            UpdateFormTitle();
            return result == DeckSaveResult.Success;
        }

        private void LoadVersionedDeck()
        {
            string oldFilename = _currentFilename;
            //_versionedDeck.LoadFromXMLFile("toto.xml"); // for quick debug purpose
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = Resource1.DeckFileFilter;
                if (fd.ShowDialog() != DialogResult.OK)
                    return;
                _currentFilename = fd.FileName;
            }
            var versionedDeck = new AgotVersionedDeck();
            DeckLoadResult result = versionedDeck.LoadFromXmlFile(_currentFilename);
            if (result == DeckLoadResult.Success)
            {
                _versionedDeck = versionedDeck;
                _currentDeck = _versionedDeck.LastVersion; // get reference on the latest deck
                UpdateControlsWithVersionedDeck(true);
                //_lastLoadedDeck = new VersionedDeck(_versionedDeck); // performs a deep copy
                _lastLoadedDeck = (AgotVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
            }
            else
            {
                switch (result)
                {
                    case DeckLoadResult.FileNotFound:
                        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrXmlDeckFileNotFound, _currentFilename),
                            Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case DeckLoadResult.InvalidContent:
                        MessageBox.Show(Resource1.ErrInvalidXmlDeckFile, Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                _currentFilename = oldFilename;
            }
            UpdateFormTitle();
        }

        private void NewVersionedDeck(bool cleanControls)
        {
            _versionedDeck = new AgotVersionedDeck();
            _currentDeck = _versionedDeck.LastVersion;
            _currentFilename = "";
            //_lastLoadedDeck = new VersionedDeck(_versionedDeck); // performs a deep copy
            _lastLoadedDeck = (AgotVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
            if (cleanControls)
                UpdateControlsWithVersionedDeck(true);
            UpdateFormTitle();
        }

        private bool CheckDeckChanges()
        {
            //if (VersionedDeck.AreEqual(_versionedDeck, _lastLoadedDeck))
            if (StaticComparer.AreEqual(_versionedDeck, _lastLoadedDeck))
                return true;

            bool result = true;
            switch (MessageBox.Show(Resource1.WarnDeckModified, Resource1.WarnDeckModifiedTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop))
            {
                case DialogResult.Yes: result = SaveVersionedDeck(false); break; // sauvegarde
                case DialogResult.No: return true;
                case DialogResult.Cancel: return false;
            }
            if (!result)
                MessageBox.Show(Resource1.CantSaveDeckErr, Resource1.ErrorTitle, MessageBoxButtons.OK);
            return result;
        }

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

        private static String TreeViewToText(AgotCardTreeView treeView)
        {
            var text = new StringBuilder();
            if ((treeView.Nodes.Count > 1) || (treeView.Nodes[0] != treeView.NodeInfo)) // not the node info alone
            {
                foreach (TreeNode typeNode in treeView.Nodes)
                {
                    text.AppendLine(typeNode.Text);
                    var currentInfo = (TypeNodeInfo)typeNode.Tag;
                    foreach (TreeNode cardNode in typeNode.Nodes)
                    {
                        var cardNodeInfo = GetCardNodeInfo(cardNode);
                        String curLine = cardNodeInfo.Value1;
                        curLine = String.Format("\t{0}{1}", curLine.PadRight(currentInfo.Length + 1), cardNodeInfo.Value2);
                        text.AppendLine(curLine);
                    }
                    text.AppendLine();
                }
            }
            return text.ToString();
        }

        private void UpdateFormTitle()
        {
            var header = Text.Substring(0, Text.IndexOf('-') + 1);
            Text = string.Format("{0} {1}", header, System.IO.Path.GetFileName(_currentFilename));
        }

        /// <summary>
        /// Computes the width in pixel of a space using the deck tree view font.
        /// The width is used to compute the number of spaces to use to enlarge each tree node.
        /// </summary>
        private void ComputeDeckTreeViewSpaceWidth(TreeView treeView)
        {
            // we have to subtract two strings to remove the constant part
            var spaceWidth = TextRenderer.MeasureText(" ", treeView.Font).Width;
            var spaceWidth2 = TextRenderer.MeasureText("  ", treeView.Font).Width;
            _deckTreeViewSpaceWidth = spaceWidth2 - spaceWidth;
        }

        private void treeViewDeck_FontChanged(object sender, EventArgs e)
        {
            ComputeDeckTreeViewSpaceWidth((TreeView)sender);
        }

        private void drawSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawSimulatorForm.Singleton.SetDeck(_currentDeck);
            DrawSimulatorForm.Singleton.Show();
            DrawSimulatorForm.Singleton.Activate();
        }

        private void miAddCardList_Click(object sender, EventArgs e)
        {
            var ctv = new AgotCardTreeView();
            ctv.Nodes.Add(new TreeNode(_treeViews[1].NodeInfo.Text));
            ctv.NodeInfo = ctv.Nodes[0];
            var tabPage = new TabPage(tabPageDeck.Text + "+");
            tabControlDecks.TabPages.Add(tabPage);
            tabPage.Controls.Add(ctv);
        }

        private void miGenerateProxyPdf_Click(object sender, EventArgs e)
        {
            try
            {
                var unprintedProxies = ProxyPdfGenerator.CreateProxiesPdf("proxy.pdf", _currentDeck);
                if (unprintedProxies.Count > 0)
                    MessageBox.Show(String.Format("{0}\n{1}",
                        Resource1.ErrUnprintedProxies,
                        String.Join("\n", unprintedProxies.ToArray())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cardPreviewControl1_MouseEnter(object sender, EventArgs e)
        {
            ShowCardPreviewForm(cardPreviewControl.CardUniversalId);
        }

        private void cardPreviewControl1_MouseLeave(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        private void cardPreviewControl1_MouseCaptureChanged(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        private void printDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportDeckToOCTGNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ApplicationSettings.IsOctgnReady)
            {
                var dialogResult = MessageBox.Show(Resource1.WarnOctgnNotLoaded, Resource1.WarnOctgnNotLoadedTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Cancel)
                    return;
                OctgnManager.PromptForInitialization(ExportDeckToOctgn);
                return;
            }
            ExportDeckToOctgn();
        }

        private void importDeckFromOCTGNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ApplicationSettings.IsOctgnReady)
            {
                var dialogResult = MessageBox.Show(Resource1.WarnOctgnNotLoaded, Resource1.WarnOctgnNotLoadedTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Cancel)
                    return;
                OctgnManager.PromptForInitialization(ExportDeckToOctgn);
                return;
            }
            ImportDeckFromOctgn();
        }

        #region Export / Import
        private void ExportDeckToOctgn()
        {
        }

        private void ImportDeckFromOctgn()
        {
            throw new NotImplementedException();
        }

        private void exportDeckToClipboardSortedBySetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckSortedBySetToText());
        }

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

            var cardsBySet = _currentDeck.Agenda
                .Union(_currentDeck.CardLists.SelectMany(cl => cl))
                .GroupBy(c => c.Set.Value); // zonk

            foreach (var group in cardsBySet)
            {
                text.AppendLine(group.Key);
                foreach (var card in group)
                    text.AppendLine(card.Name.Value);
                text.AppendLine();
            }

            text.AppendLine();
            text.Append(tabPageDescription.Text).Append(" : ").AppendLine(_versionedDeck.Description);

            text.AppendLine(tabPageStats.Text);
            text.AppendLine(rtbStatistics.Text);
            return text.ToString();
        }


        #endregion
    }

    internal delegate bool CardOperation(AgotCard card);

    internal class CardNodeInfo : StringPair
    {
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public CardNodeInfo(string value1, string value2)
            : base(value1, value2)
        {
            ForeColor = Color.Empty;
            BackColor = Color.Empty;
        }
    }
}
