using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GenericDB.BusinessObjects;
using GenericDB.Extensions;
using GenericDB.Forms;

using AGoTDB.BusinessObjects;
using AGoTDB.OCTGN;
using AGoTDB.Services;
using TCard = AGoTDB.BusinessObjects.AgotCard;
using TCardTreeView = AGoTDB.Components.AgotCardTreeView;
using TDeck = AGoTDB.BusinessObjects.AgotDeck;
using TVersionedDeck = AGoTDB.BusinessObjects.AgotVersionedDeck;
using TCardList = AGoTDB.BusinessObjects.AgotCardList;

namespace AGoTDB.Forms
{
    partial class DeckBuilderForm
    {
        private static readonly Object _singletonLock = new Object();
        private static DeckBuilderForm _singleton;
        private TVersionedDeck _versionedDeck;
        private TVersionedDeck _lastLoadedDeck; // used to detect changes in the versioned deck
        private TDeck _currentDeck; // deck currently displayed
        private String _currentFilename; // filename of the deck (non-empty if the deck was loaded or saved)
        private readonly List<TCardTreeView> _treeViews;
        private readonly CardPreviewForm _cardPreviewForm = new CardPreviewForm { ApplicationSettings = ApplicationSettings.Instance };

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
            cardPreviewControl.Settings = ApplicationSettings.Instance;
            _treeViews = new List<TCardTreeView> { treeViewSide, treeViewDeck };
            foreach (TCardTreeView t in _treeViews)
            {
                var rootNode = new TreeNode(Resource1.TreeNodeAddCard);
                rootNode.NodeFont = new Font(t.Font, FontStyle.Italic);
                t.Nodes.Add(rootNode);
                t.NodeInfo = rootNode;
            }
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

        #region Form events
        private void DeckBuilderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (_singletonLock)
            {
                _singleton = null;
            }
        }

        private void TreeViewDeck_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && IsCardNode(e.Node))
                ((TreeView)sender).SelectedNode = e.Node;
        }

        private void TreeViewDeck_KeyDown(object sender, KeyEventArgs e)
        {
            var node = ((TreeView)sender).SelectedNode;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (IsCardNode(node))
                        SubtractCardFromCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                case Keys.Insert:
                    if (IsCardNode(node))
                        AddCardToCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
            }
        }

        private void TreeViewDeck_KeyPress(object sender, KeyPressEventArgs e)
        {
            var node = ((TreeView)sender).SelectedNode;

            e.Handled = true;
            switch (e.KeyChar)
            {
                case '-':
                    if (IsCardNode(node))
                        SubtractCardFromCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                case '+':
                    if (IsCardNode(node))
                        AddCardToCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
                    break;
                default: e.Handled = false; break;
            }
        }

        /// <summary>
        /// Updates the card detail textbox to match the selected node in the deck tree view.
        /// If the selected node is a card, the description of the card is displayed. Otherwise,
        /// the textbox is cleared.
        /// </summary>
        private void TreeViewDeck_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearCardInformations();

            if (!IsCardNode(e.Node) || (e.Node.Tag == null))
            {
                RootNodeSelected(e.Node);
                return;
            }

            UpdateCardInformations((TCard)e.Node.Tag);
        }

        private void DeckBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckDeckChanges();
        }

        private void TreeViewHistory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _currentDeck = _versionedDeck[Int32.Parse(e.Node.Name, CultureInfo.InvariantCulture)];
            UpdateControlsWithVersionedDeck(false);
        }
        #endregion

        #region Main menu Items (load/save/new...)
        private void SaveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVersionedDeck(false);
        }

        private void SaveDeckAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVersionedDeck(true);
        }

        private void LoadDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckDeckChanges())
                LoadVersionedDeck();
        }

        private void NewVersionToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ExportToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckToText());
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckDeckChanges())
                NewVersionedDeck(true);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Deck tree view context menu events (increase/decrease count...)
        private void ContextMenuStripTreeView_Opening(object sender, CancelEventArgs e)
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

        private void MiIncreaseCount_Click(object sender, EventArgs e)
        {
            var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            TreeNode node = tv.SelectedNode;
            if (IsCardNode(node)) // menuitem should be disabled for other nodes anyway
            {
                AddCardToCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
            }
        }

        private void MiDecreaseCount_Click(object sender, EventArgs e)
        {
            var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            TreeNode node = tv.SelectedNode;
            if (IsCardNode(node)) // menuitem should be disabled for other nodes anyway
            {
                SubtractCardFromCurrent((TCard)node.Tag); // keeps the coherence between the versioned deck and its display
            }
        }

        private void ExportDeckToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckToText());
        }
        #endregion

        #region Form update methods
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
        private void UpdateCardInformations(TCard card)
        {
            UpdateCardText(card);
            if (UserSettings.DisplayImages)
                UpdateCardImage(card);
        }

        /// <summary>
        /// Updates the card detail textbox with a given card.
        /// </summary>
        private void UpdateCardText(TCard card)
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
        private void UpdateCardImage(TCard card)
        {
            cardPreviewControl.Visible = true;
            cardPreviewControl.SetIds(card.UniversalId, card.OctgnIds);
        }

        /// <summary>
        /// Shows the card preview form for given card id.
        /// </summary>
        /// <param name="universalId">The card id.</param>
        /// <param name="octgnId">The octgn id of the card.</param>
        private void ShowCardPreviewForm(int universalId, Guid[] octgnIds)
        {
            if (!ApplicationSettings.Instance.ImagesFolderExists || !UserSettings.DisplayImages)
                return;
            _cardPreviewForm.ImagePreviewSize = UserSettings.ImagePreviewSize;
            _cardPreviewForm.SetIds(universalId, octgnIds);
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
        #endregion

        #region Controls / Versioned deck synchronization
        private void UpdateTreeViews()
        {
            rtbStatistics.BeginUpdate();
            UpdateTreeViewWithCards(treeViewDeck, _currentDeck.CardLists[1], _currentDeck); // TODO : modify to handle multiple deck lists
            UpdateTreeViewWithCards(treeViewSide, _currentDeck.CardLists[0], _currentDeck); // TODO : modify to handle multiple deck lists
            rtbStatistics.EndUpdate();
        }

        private void UpdateTreeViewWithCards(TCardTreeView treeView, TCardList cards, TDeck deck)
        {
            treeView.BeginUpdate();
            treeView.Cards = cards;
            treeView.Nodes.Clear();
            treeView.Nodes.Add(treeView.NodeInfo);
            foreach (TCard card in cards)
                AddCardToTreeView(treeView, card, false);
            treeView.Deck = deck;
            treeView.EndUpdate();
            UpdateTabText(treeView);
            UpdateStatistics();
        }

        private void UpdateHistoryFromVersionedDeck()
        {
            treeViewHistory.Nodes.Clear();
            var lastVersionIndex = _versionedDeck.Count - 1;
            for (var i = 0; i < lastVersionIndex; ++i)
                treeViewHistory.Nodes.Insert(0, i.ToString(CultureInfo.InvariantCulture), String.Format(CultureInfo.CurrentCulture, "v{0} {1} {2}: {3}", i, _versionedDeck[i].LastModifiedDate.ToShortDateString(), _versionedDeck[i].LastModifiedDate.ToShortTimeString(), _versionedDeck[i].RevisionComments));
            treeViewHistory.Nodes.Insert(0, lastVersionIndex.ToString(CultureInfo.InvariantCulture), String.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Resource1.CurrentRevision, _versionedDeck.LastVersion.LastModifiedDate.ToShortDateString(), _versionedDeck.LastVersion.LastModifiedDate.ToShortTimeString()));
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
                typeNode = treeView.Nodes.Add(type.ToString(CultureInfo.InvariantCulture), TCard.GetTypeName(type)); // add it to the end
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
                typeNode = treeView.Nodes.Insert(i, type.ToString(CultureInfo.InvariantCulture), TCard.GetTypeName(type));
            }
            typeNode.Tag = new TypeNodeInfo(type);
            return typeNode;
        }

        private void AddCardToTreeView(TCardTreeView treeView, TCard card, bool alreadyPresent)
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
                var cardType = card.Type != null ? card.Type.Value : (Int32)TCard.CardType.Unknown; // we may have no type for unknown cards
                var rootIndex = treeView.Nodes.IndexOfKey(cardType.ToString(CultureInfo.InvariantCulture));
                root = rootIndex == -1
                    ? AddTypeNode(treeView, cardType)
                    : treeView.Nodes[rootIndex];

                var i = 0;
                while ((i < root.Nodes.Count) && (card.CompareTo((TCard)root.Nodes[i].Tag) < 0))
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

            if (card.Type != null)
            {
                UpdateDependentTypeNodeTexts(card.Type.Value, treeView);
            }

            root.Expand();
            treeView.EndUpdate();
            UpdateTabText(treeView); // updates the card count (excluding plots)
            UpdateStatistics();
        }

        private void SubtractCardFromTreeView(TCardTreeView treeView, TCard card, bool wasLastCopy)
        {
            treeView.BeginUpdate();
            var rootIndex = treeView.Nodes.IndexOfKey(card.Type.Value.ToString(CultureInfo.InvariantCulture));
            if (rootIndex == -1)
                return; // could not find its root

            TreeNode root = treeView.Nodes[rootIndex];

            var i = 0;
            while ((i < root.Nodes.Count) && (card.UniversalId != ((TCard)root.Nodes[i].Tag).UniversalId))
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
                UpdateDependentTypeNodeTexts(card.Type.Value, treeView);
            }

            if (treeView.Nodes.Count == 0) // add the information node
                treeView.Nodes.Add(treeView.NodeInfo);
            treeView.EndUpdate();
            UpdateTabText(treeView); // updates the card count (excluding plots)
            UpdateStatistics();
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
        private void TreeViewDeck_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var treeView = (TCardTreeView)sender;
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

        public bool AddCardToDeck(TCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            TCard c = _currentDeck.CardLists[1].AddCard(card); // TODO : modify to handle multiple deck lists
            AddCardToTreeView(treeViewDeck, c, c.Quantity > 1);
            return true;
        }

        private bool SubtractCardFromDeck(TCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            TCard c = _currentDeck.CardLists[1].SubtractCard(card); // TODO : modify to handle multiple deck lists
            SubtractCardFromTreeView(treeViewDeck, card, c == null);
            return true;
        }

        public bool AddCardToSide(TCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            TCard c = _currentDeck.CardLists[0].AddCard(card); // TODO : modify to handle multiple deck lists
            AddCardToTreeView(treeViewSide, c, c.Quantity > 1);
            return true;
        }

        private bool SubtractCardFromSide(TCard card)
        {
            if (!_currentDeck.Editable)
                return false;

            TCard c = _currentDeck.CardLists[0].SubtractCard(card); // TODO : modify to handle multiple deck lists
            SubtractCardFromTreeView(treeViewSide, card, c == null);
            return true;
        }

        public bool AddCardToCurrent(TCard card)
        {
            return tabControlDecks.SelectedTab == tabPageDeck
                ? AddCardToDeck(card)
                : AddCardToSide(card);
        }

        private bool SubtractCardFromCurrent(TCard card)
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
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = Resource1.DeckFileFilter;
                    dialog.ValidateNames = true;
                    dialog.FileName = _versionedDeck.Name + ".xml"; // use deck name as default name. Todo: remove special characters
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return false;
                    _currentFilename = dialog.FileName;
                }
            }
            int? databaseVersion = (ApplicationSettings.Instance.DatabaseManager.DatabaseInfos.Count > 0)
                ? (int?)ApplicationSettings.Instance.DatabaseManager.DatabaseInfos[0].VersionId
                : null;

            var result = _versionedDeck.SaveToXmlFile(_currentFilename, ApplicationSettings.Instance.ApplicationName, ApplicationSettings.Instance.ApplicationVersion, databaseVersion);
            if (result == DeckSaveResult.Success)
                _lastLoadedDeck = (TVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
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
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resource1.DeckFileFilter;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                _currentFilename = dialog.FileName;
            }
            var versionedDeck = new TVersionedDeck();
            DeckLoadResult result = versionedDeck.LoadFromXmlFile(_currentFilename);
            if (result == DeckLoadResult.Success)
            {
                _versionedDeck = versionedDeck;
                _currentDeck = _versionedDeck.LastVersion; // get reference on the latest deck
                UpdateControlsWithVersionedDeck(true);
                //_lastLoadedDeck = new VersionedDeck(_versionedDeck); // performs a deep copy
                _lastLoadedDeck = (TVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
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

        private void LoadOctgnDeck()
        {
            string oldFilename = _currentFilename;
            try
            {
                var versionedDeck = AgotOctgnManager.LoadOctgnDeck();
                if (versionedDeck == null)
                    return;

                _versionedDeck = versionedDeck;
                _currentDeck = _versionedDeck.LastVersion; // get reference on the latest deck
                UpdateControlsWithVersionedDeck(true);
                _lastLoadedDeck = (TVersionedDeck)_versionedDeck.Clone(); // performs a deep copy

            }
            catch
            {
                _currentFilename = oldFilename;
                throw;
            }
            UpdateFormTitle();
        }

        private void NewVersionedDeck(bool cleanControls)
        {
            _versionedDeck = new TVersionedDeck();
            _currentDeck = _versionedDeck.LastVersion;
            _currentFilename = "";
            //_lastLoadedDeck = new VersionedDeck(_versionedDeck); // performs a deep copy
            _lastLoadedDeck = (TVersionedDeck)_versionedDeck.Clone(); // performs a deep copy
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

        private static String TreeViewToText(TCardTreeView treeView)
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

        private void TreeViewDeck_FontChanged(object sender, EventArgs e)
        {
            ComputeDeckTreeViewSpaceWidth((TreeView)sender);
        }

        private void DrawSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawSimulatorForm.Singleton.SetDeck(_currentDeck);
            DrawSimulatorForm.Singleton.Show();
            DrawSimulatorForm.Singleton.Activate();
        }

        private void MiAddCardList_Click(object sender, EventArgs e)
        {
            var ctv = new TCardTreeView();
            ctv.Nodes.Add(new TreeNode(_treeViews[1].NodeInfo.Text));
            ctv.NodeInfo = ctv.Nodes[0];
            var tabPage = new TabPage(tabPageDeck.Text + "+");
            tabControlDecks.TabPages.Add(tabPage);
            tabPage.Controls.Add(ctv);
        }

        private void MiGenerateProxyPdf_Click(object sender, EventArgs e)
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

        private void CardPreviewControl1_MouseEnter(object sender, EventArgs e)
        {
            ShowCardPreviewForm(cardPreviewControl.CardUniversalId, cardPreviewControl.CardOctgnIds);
        }

        private void CardPreviewControl1_MouseLeave(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        private void CardPreviewControl1_MouseCaptureChanged(object sender, EventArgs e)
        {
            HideCardPreviewForm();
        }

        private void PrintDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExportDeckToOctgnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ApplicationSettings.Instance.IsOctgnReady)
            {
                var dialogResult = MessageBox.Show(Resource1.WarnOctgnNotLoaded, Resource1.WarnOctgnNotLoadedTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Cancel)
                    return;
                AgotOctgnManager.PromptForInitialization(ExportDeckToOctgn);
                return;
            }
            ExportDeckToOctgn();
        }

        private void ImportDeckFromOctgnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ApplicationSettings.Instance.IsOctgnReady)
            {
                var dialogResult = MessageBox.Show(Resource1.WarnOctgnNotLoaded, Resource1.WarnOctgnNotLoadedTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Cancel)
                    return;
                AgotOctgnManager.PromptForInitialization(ImportDeckFromOctgn);
                return;
            }
            ImportDeckFromOctgn();
        }

        #region Export / Import
        private void ExportDeckToOctgn()
        {
            AgotOctgnManager.SaveOctgnDeck(_versionedDeck, _currentDeck);
        }

        private void ImportDeckFromOctgn()
        {
            LoadOctgnDeck();
        }

        private void ExportDeckToClipboardSortedBySetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CurrentDeckSortedBySetToText());
        }
        #endregion

        private void RootNodeSelected(TreeNode node)
        {
            var typeNodeInfo = node.Tag as TypeNodeInfo;
            if (typeNodeInfo == null)
                return;
            // nothing to do
        }
    }

    internal delegate bool CardOperation(TCard card);
}
