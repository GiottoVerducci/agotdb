// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright � 2007, 2008, 2009, 2010 Vincent Ripoll
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
// � A Game of Thrones 2005 George R. R. Martin
// � A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// � A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// � Le Tr�ne de Fer JCC 2005-2007 Stratag�mes �ditions / X�nomorphe S�rl
// � Le Tr�ne de Fer JCE 2008 Edge Entertainment


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
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Forms;

namespace AGoTDB.Forms
{
	/// <summary>
	/// Form displaying versioned decks.
	/// </summary>
	public partial class DeckBuilderForm : Form
	{
		private static readonly Object SingletonLock = new Object();
		private static DeckBuilderForm fSingleton;
		private AgotVersionedDeck fVersionedDeck;
		private AgotVersionedDeck fLastLoadedDeck; // used to detect changes in the versioned deck
		private AgotDeck fCurrentDeck; // deck currently displayed
		private String fCurrentFilename; // filename of the deck (non-empty if the deck was loaded or saved)
		private readonly List<AgotCardTreeView> fTreeViews;

		private int fDeckTreeViewSpaceWidth; // width of a space using the deck tree view font (used to expand the lines)
		private readonly DeckTreeNodeSorter fDeckTreeNodeSorter; // not fully used, we keep our sorting algorithm when inserting new nodes

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
				lock (SingletonLock)
				{
					if (fSingleton == null)
						fSingleton = new DeckBuilderForm();
					return fSingleton;
				}
			}
		}

		public static bool SingletonExists()
		{
			return fSingleton != null;
		}

		/// <summary>
		/// Default form constructor.
		/// </summary>
		private DeckBuilderForm()
		{
			InitializeComponent();
			fTreeViews = new List<AgotCardTreeView>();
			fTreeViews.Add(treeViewSide);
			fTreeViews.Add(treeViewDeck);
			for (var i = 0; i < fTreeViews.Count; ++i)
				fTreeViews[i].NodeInfo = fTreeViews[i].Nodes[0]; // must have been added during design time
			NewVersionedDeck(false);
			for (var i = 0; i < fTreeViews.Count; ++i)
			{
				fTreeViews[i].Cards = fCurrentDeck.CardLists[i];
				fTreeViews[i].Deck = fCurrentDeck;
			}
			fDeckTreeNodeSorter = new DeckTreeNodeSorter(
				UserSettings.Singleton.ReadString("DeckBuilder", "TypeOrder", ""),
				IsCardNode,
				x => ((TypeNodeInfo)x.Tag).Type
			);

			ApplicationSettings.DatabaseManager.UpdateExtendedCheckedListBox(eclHouse, ApplicationSettings.DatabaseManager.TableNameHouse, "House", TableType.ValueId);
			eclHouse.Summary += " - " + eclHouse.Items[0].ToString();
			eclHouse.Items.RemoveAt(0); // remove neutral house
			UpdateAgendaComboBox();
			UpdateHistoryFromVersionedDeck();
			//treeViewDeck.TreeViewNodeSorter = deckTreeNodeSorter; // we don't use it because it's slower than inserting directly at the right place (visible when loading decks)
		}

		#region Form events (FormShown/Closed, NodeClick or HouseValue changed, tvHistory select)
		private void DeckBuilderForm_Shown(object sender, EventArgs e)
		{
			eclHouse.UpdateSize(); // update size because neutral house has been removed
			eclAgenda.UpdateSize();
		}

		private void DeckBuilderForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			lock (SingletonLock)
			{
				fSingleton = null;
			}
		}

		private void treeViewDeck_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if ((e.Button == MouseButtons.Right) && IsCardNode(e.Node))
				((TreeView)sender).SelectedNode = e.Node;
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
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeViewDeck_AfterSelect(object sender, TreeViewEventArgs e)
		{
			rtbCardText.Clear();
			if (!IsCardNode(e.Node) || (e.Node.Tag == null))
				return;

			var card = (AgotCard)e.Node.Tag;
			foreach (FormattedText ft in card.ToFormattedString())
			{
				rtbCardText.SelectionFont = new Font(rtbCardText.SelectionFont, ft.Format.Style);
				rtbCardText.SelectionColor = ft.Format.Color;
				rtbCardText.AppendText(ft.Text);
			}
		}

		private void DeckBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !CheckDeckChanges();
		}

		private void treeViewHistory_AfterSelect(object sender, TreeViewEventArgs e)
		{
			fCurrentDeck = fVersionedDeck[Int32.Parse(e.Node.Name, CultureInfo.InvariantCulture)];
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
			if (!UserSettings.Singleton.ReadBool("Deckbuilder", "ShowNewVersionMessage", true))
				return;
			using (var form = new FormNewVersionInfo())
			{
				if (form.ShowDialog() != DialogResult.OK)
					return;
				UserSettings.Singleton.WriteBool("Deckbuilder", "ShowNewVersionMessage", form.DisplayFormNextTime());
			}
			using (var form = new RevisionCommentInputForm())
			{
				if (form.ShowDialog() != DialogResult.OK)
					return;
				fVersionedDeck.AddNewVersion(form.RevisionComment());
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
				mi.Enabled = fCurrentDeck.Editable && (node != null) && IsCardNode(node);
			miExportDeckToClipboard.Enabled = true; // always true
		}

		private void miIncreaseCount_Click(object sender, EventArgs e)
		{
			var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
			TreeNode node = tv.SelectedNode;
			if (node.Level == 1) // card node (menuitem should be disabled for other nodes anyway)
			{
				AddCardToCurrent((AgotCard)node.Tag); // keeps the coherence between the versioned deck and its display
			}
		}

		private void miDecreaseCount_Click(object sender, EventArgs e)
		{
			var tv = (TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
			TreeNode node = tv.SelectedNode;
			if (node.Level == 1) // card node (menuitem should be disabled for other nodes anyway)
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
			eclAgenda.Items.Clear();
			var table = ApplicationSettings.DatabaseManager.GetAgendas();
			foreach (DataRow row in table.Rows)
				eclAgenda.Items.Add(new AgotCard(row));
			eclAgenda.UpdateSize();
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
			if (fDeckTreeViewSpaceWidth == 0)
				ComputeDeckTreeViewSpaceWidth(cardNode.TreeView);
			var cardNodeInfo = GetCardNodeInfo(cardNode);
			Font font = GetNodeFont(cardNode);
			var textWidth = TextRenderer.MeasureText(cardNodeInfo.Value1, font).Width;
			var text2Width = TextRenderer.MeasureText(cardNodeInfo.Value2, GetNodeInfoFont(cardNode)).Width;
			var nbSpacesToAdd = 1 + (((TypeNodeInfo)cardNode.Parent.Tag).Width - textWidth + text2Width) / fDeckTreeViewSpaceWidth;
			cardNode.Text = cardNodeInfo.Value1 + " ".PadRight(nbSpacesToAdd);
			cardNode.ForeColor = cardNodeInfo.ForeColor;
			cardNode.BackColor = cardNodeInfo.BackColor;
		}
		#endregion

		#region Controls / Versioned deck synchronization
		private void UpdateControlsWithVersionedDeck(bool updateHistory)
		{
			UpdateControlsFromHouse();
			UpdateControlsFromAgenda();
			UpdateTreeViews();

			tbDeckName.Text = fVersionedDeck.Name;
			tbAuthor.Text = fVersionedDeck.Author;
			rtbDescription.Text = fVersionedDeck.Description;
			if (updateHistory)
				UpdateHistoryFromVersionedDeck();

			// enable or disable the edit mode depending on the Editable property of the deck
			eclHouse.Enabled = fCurrentDeck.Editable;
			eclAgenda.Enabled = fCurrentDeck.Editable;
			rtbDescription.Enabled = fCurrentDeck.Editable;
		}

		private void UpdateTreeViews()
		{
			UpdateTreeViewWithCards(treeViewDeck, fCurrentDeck.CardLists[1], fCurrentDeck); // TODO : modify to handle multiple deck lists
			UpdateTreeViewWithCards(treeViewSide, fCurrentDeck.CardLists[0], fCurrentDeck); // TODO : modify to handle multiple deck lists
		}

		private void UpdateTreeViewWithCards(AgotCardTreeView treeView, AgotCardList cards, AgotDeck deck)
		{
			treeView.BeginUpdate();
			treeView.Cards = cards;
			treeView.Nodes.Clear();
			treeView.Nodes.Add(treeView.NodeInfo);
			for (var i = 0; i < cards.Count; ++i)
				AddCardToTreeView(treeView, cards[i], false);
			treeView.Deck = deck;
			treeView.EndUpdate();
			UpdateTabText(treeView);
		}

		private void UpdateVersionedDeckWithControls()
		{
			UpdateHouseFromControls();
			fVersionedDeck.Name = tbDeckName.Text;
			fVersionedDeck.Author = tbAuthor.Text;
			fVersionedDeck.Description = rtbDescription.Text;
		}

		private void UpdateHouseFromControls()
		{
			var h = 0;
			bool condensed = eclHouse.Condensed; // we'll use it to restore its previous state
			if (condensed)
				eclHouse.Condensed = false; // expand ecl to get access to the items
			for (var i = 0; i < eclHouse.Items.Count; ++i)
				if (eclHouse.GetItemCheckState(i) == CheckState.Checked)
					h += Int32.Parse(((DbFilter)eclHouse.Items[i]).Column, CultureInfo.InvariantCulture);
			fCurrentDeck.Houses = h;
			if (condensed)
				eclHouse.Condensed = true;
			UpdateTreeViews();
		}

		private void UpdateControlsFromHouse()
		{
			var h = fCurrentDeck.Houses;
			bool condensed = eclHouse.Condensed; // we'll use it to restore its previous state
			eclHouse.Condensed = false; // expand ecl to get access to the items

			for (var i = eclHouse.Items.Count - 1; i >= 0; --i) // houses are sorted by increasing value
			{
				var hv = Int32.Parse(((DbFilter)eclHouse.Items[i]).Column, CultureInfo.InvariantCulture);
				if (h >= hv)
				{
					eclHouse.SetItemCheckState(i, CheckState.Checked);
					h -= hv;
				}
				else
					eclHouse.SetItemCheckState(i, CheckState.Unchecked);
			}
			eclHouse.Condensed = condensed;
		}

		private void UpdateAgendaFromControls()
		{
			fCurrentDeck.Agenda.Clear();
			bool condensed = eclAgenda.Condensed; // we'll use it to restore its previous state
			if (condensed)
				eclAgenda.Condensed = false; // expand ecl to get access to the items
			for (var i = 0; i < eclAgenda.Items.Count; ++i)
				if (eclAgenda.GetItemCheckState(i) == CheckState.Checked)
				{
					fCurrentDeck.Agenda.Add(eclAgenda.Items[i] as AgotCard);
				}
			if (condensed)
				eclAgenda.Condensed = true;
		}

		private void UpdateControlsFromAgenda()
		{
			bool condensed = eclAgenda.Condensed; // we'll use it to restore its previous state
			eclAgenda.Condensed = false; // expand ecl to get access to the items

			// check for agenda in the deck but missing in our list 
			var missingAgenda = new List<AgotCard>();
			missingAgenda.AddRange(fCurrentDeck.Agenda);
			missingAgenda.RemoveAll(a => eclAgenda.Items.Contains(a));

			eclAgenda.Items.AddRange(missingAgenda.ToArray());
			for (var i = eclAgenda.Items.Count - 1; i >= 0; --i)
			{
				var agenda = eclAgenda.Items[i] as AgotCard;
				if (fCurrentDeck.Agenda.Contains(agenda))
					eclAgenda.SetItemCheckState(i, CheckState.Checked);
				else
					eclAgenda.SetItemCheckState(i, CheckState.Unchecked);
			}

			eclAgenda.Condensed = condensed;
		}

		private void UpdateHistoryFromVersionedDeck()
		{
			treeViewHistory.Nodes.Clear();
			var lastVersionIndex = fVersionedDeck.Count - 1;
			for (var i = 0; i < lastVersionIndex; ++i)
				treeViewHistory.Nodes.Insert(0, i.ToString(), String.Format(CultureInfo.CurrentCulture, "v{0} {1} {2}: {3}", i, fVersionedDeck[i].LastModifiedDate.ToShortDateString(), fVersionedDeck[i].LastModifiedDate.ToShortTimeString(), fVersionedDeck[i].RevisionComments));
			treeViewHistory.Nodes.Insert(0, lastVersionIndex.ToString(), String.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Resource1.CurrentRevision, fVersionedDeck.LastVersion.LastModifiedDate.ToShortDateString(), fVersionedDeck.LastVersion.LastModifiedDate.ToShortTimeString()));
		}
		#endregion

		#region Node methods
		protected internal static bool IsCardNode(TreeNode node)
		{
			return (node.Level == 1);
		}

		private TreeNode AddTypeNode(TreeView treeView, Int32 type)
		{
			TreeNode typeNode;

			var order = fDeckTreeNodeSorter.GetTypeOrder(type);
			if (order == -1) // type not found in the order list
				typeNode = treeView.Nodes.Add(type.ToString(CultureInfo.InvariantCulture), AgotCard.GetTypeName(type)); // add it to the end
			else
			{
				// get the index of the greatest node below the node to add
				var i = 0;
				while (i < treeView.Nodes.Count)
				{
					var nodeOrder = fDeckTreeNodeSorter.GetTypeOrder(Int32.Parse(treeView.Nodes[i].Name, CultureInfo.InvariantCulture));
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
				if (rootIndex == -1)
					root = AddTypeNode(treeView, cardType);
				else
					root = treeView.Nodes[rootIndex];

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
		}

		/// <summary>
		/// Updates the tab text to reflect the card count (excluding plots)
		/// </summary>
		/// <param name="treeView"></param>
		private static void UpdateTabText(AgotCardTreeView treeView)
		{
			var tabPage = (TabPage)treeView.Parent;
			var count = 0;
			foreach (AgotCard card in treeView.Cards)
			{
				if (card.Type != null && card.Type.Value != (Int32)AgotCard.CardType.Plot)
					count += card.Quantity;
			}

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
			var result = new CardNodeInfo(String.Format(CultureInfo.CurrentCulture, "{0}� {1}", card.Quantity, card.Name.Value), card.GetSummaryInfo());
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
			if (card.House.Value != (int)AgotCard.CardHouse.Neutral && (deck.Houses & card.House.Value) == 0)
				result.ForeColor = Color.OrangeRed;
			return result;
		}
		#endregion

		#region Deck tree view drawing
		private void treeViewDeck_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			if (IsCardNode(e.Node))
			{
				var treeView = (AgotCardTreeView)sender;
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
				// e.Graphics.DrawString produit un affichage trop large (plus large que la taille mesur�e avec TextRenderer.MeasureText
				// et plus large �galement que le rendu standard du treeview. Mais :
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
			if (!fCurrentDeck.Editable)
				return false;

			AgotCard c = fCurrentDeck.CardLists[1].AddCard(card); // TODO : modify to handle multiple deck lists
			AddCardToTreeView(treeViewDeck, c, c.Quantity > 1);
			return true;
		}

		private bool SubtractCardFromDeck(AgotCard card)
		{
			if (!fCurrentDeck.Editable)
				return false;

			AgotCard c = fCurrentDeck.CardLists[1].SubtractCard(card); // TODO : modify to handle multiple deck lists
			SubtractCardFromTreeView(treeViewDeck, card, c == null);
			return true;
		}

		public bool AddCardToSide(AgotCard card)
		{
			if (!fCurrentDeck.Editable)
				return false;

			AgotCard c = fCurrentDeck.CardLists[0].AddCard(card); // TODO : modify to handle multiple deck lists
			AddCardToTreeView(treeViewSide, c, c.Quantity > 1);
			return true;
		}

		private bool SubtractCardFromSide(AgotCard card)
		{
			if (!fCurrentDeck.Editable)
				return false;

			AgotCard c = fCurrentDeck.CardLists[0].SubtractCard(card); // TODO : modify to handle multiple deck lists
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
			//fCurrentFilename = "toto.xml"; // for quick debug purpose
			String previousFilename = fCurrentFilename;
			if (string.IsNullOrEmpty(fCurrentFilename) || forceCallToSaveDialog)
			{
				using (var fd = new SaveFileDialog())
				{
					fd.Filter = Resource1.DeckFileFilter;
					fd.ValidateNames = true;
					fd.FileName = fVersionedDeck.Name + ".xml"; // use deck name as default name. Todo: remove special characters
					if (fd.ShowDialog() != DialogResult.OK)
						return false;
					fCurrentFilename = fd.FileName;
				}
			}
			int? databaseVersion = (ApplicationSettings.DatabaseManager.DatabaseInfos.Count > 0)
				? (int?)ApplicationSettings.DatabaseManager.DatabaseInfos[0].VersionId
				: null;

			var result = fVersionedDeck.SaveToXmlFile(fCurrentFilename, ApplicationSettings.ApplicationName, ApplicationSettings.ApplicationVersion, databaseVersion);
			if (result == DeckSaveResult.Success)
				fLastLoadedDeck = (AgotVersionedDeck)fVersionedDeck.Clone(); // performs a deep copy
			else
			{
				MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrSaveXmlDeck, fCurrentFilename), Resource1.ErrDeckSaveTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
				fCurrentFilename = previousFilename; // restore filename to avoid confusion ('cause the deck wasn't saved under the new filename)
			}
			UpdateFormTitle();
			return result == DeckSaveResult.Success;
		}

		private void LoadVersionedDeck()
		{
			string oldFilename = fCurrentFilename;
			//fVersionedDeck.LoadFromXMLFile("toto.xml"); // for quick debug purpose
			using (var fd = new OpenFileDialog())
			{
				fd.Filter = Resource1.DeckFileFilter;
				if (fd.ShowDialog() != DialogResult.OK)
					return;
				fCurrentFilename = fd.FileName;
			}
			var versionedDeck = new AgotVersionedDeck();
			DeckLoadResult result = versionedDeck.LoadFromXmlFile(fCurrentFilename);
			if (result == DeckLoadResult.Success)
			{
				fVersionedDeck = versionedDeck;
				fCurrentDeck = fVersionedDeck.LastVersion; // get reference on the latest deck
				UpdateControlsWithVersionedDeck(true);
				//fLastLoadedDeck = new VersionedDeck(fVersionedDeck); // performs a deep copy
				fLastLoadedDeck = (AgotVersionedDeck)fVersionedDeck.Clone(); // performs a deep copy
			}
			else
			{
				switch (result)
				{
					case DeckLoadResult.FileNotFound:
						MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrXmlDeckFileNotFound, fCurrentFilename),
							Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case DeckLoadResult.InvalidContent:
						MessageBox.Show(Resource1.ErrInvalidXmlDeckFile, Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
				}
				fCurrentFilename = oldFilename;
			}
			UpdateFormTitle();
		}

		private void NewVersionedDeck(bool cleanControls)
		{
			fVersionedDeck = new AgotVersionedDeck();
			fCurrentDeck = fVersionedDeck.LastVersion;
			fCurrentFilename = "";
			//fLastLoadedDeck = new VersionedDeck(fVersionedDeck); // performs a deep copy
			fLastLoadedDeck = (AgotVersionedDeck)fVersionedDeck.Clone(); // performs a deep copy
			if (cleanControls)
				UpdateControlsWithVersionedDeck(true);
			UpdateFormTitle();
		}

		private bool CheckDeckChanges()
		{
			//if (VersionedDeck.AreEqual(fVersionedDeck, fLastLoadedDeck))
			if (StaticComparer.AreEqual(fVersionedDeck, fLastLoadedDeck))
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

		private string CurrentDeckToText()
		{
			var text = new StringBuilder();

			text.Append(lblDeckName.Text).AppendLine(fVersionedDeck.Name);
			text.Append(lblAuthor.Text).AppendLine(fVersionedDeck.Author);
			text.Append(lblHouse.Text).Append(AgotCard.GetHouseName(fCurrentDeck.Houses));
			if (fCurrentDeck.Agenda.Count > 0)
			{
				var agendaNames = fCurrentDeck.Agenda.Select(card => card.Name.Value);
				text.AppendFormat(" ({0})", string.Join(" + ", agendaNames.ToArray()));
			}

			text.AppendLine();
			text.Append(tabPageDescription.Text).Append(" : ").AppendLine(fVersionedDeck.Description);

			text.AppendLine().AppendLine(tabPageDeck.Text);
			text.Append(TreeViewToText(treeViewDeck));
			text.AppendLine().AppendLine(tabPageSideboard.Text);
			text.Append(TreeViewToText(treeViewSide));

			return text.ToString();
		}

		private static String TreeViewToText(AgotCardTreeView treeView)
		{
			String text = "";
			if ((treeView.Nodes.Count > 1) || (treeView.Nodes[0] != treeView.NodeInfo)) // not the node info alone
			{
				foreach (TreeNode typeNode in treeView.Nodes)
				{
					text += typeNode.Text + "\n";
					var currentInfo = (TypeNodeInfo)typeNode.Tag;
					foreach (TreeNode cardNode in typeNode.Nodes)
					{
						var cardNodeInfo = GetCardNodeInfo(cardNode);
						String curLine = cardNodeInfo.Value1;
						curLine = "\t" + curLine.PadRight(currentInfo.Length + 1);
						curLine += cardNodeInfo.Value2;
						text += curLine + "\n";
					}
				}
			}
			return text;
		}

		private void UpdateFormTitle()
		{
			String header = Text.Substring(0, Text.IndexOf('-') + 1);
			Text = header + " " + System.IO.Path.GetFileName(fCurrentFilename);
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
			fDeckTreeViewSpaceWidth = spaceWidth2 - spaceWidth;
		}

		private void treeViewDeck_FontChanged(object sender, EventArgs e)
		{
			ComputeDeckTreeViewSpaceWidth((TreeView)sender);
		}

		private void drawSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DrawSimulatorForm.Singleton.SetDeck(fCurrentDeck);
			DrawSimulatorForm.Singleton.Show();
			DrawSimulatorForm.Singleton.Activate();
		}

		private void miAddCardList_Click(object sender, EventArgs e)
		{
			var ctv = new AgotCardTreeView();
			ctv.Nodes.Add(new TreeNode(fTreeViews[1].NodeInfo.Text));
			ctv.NodeInfo = ctv.Nodes[0];
			var tabPage = new TabPage(tabPageDeck.Text + "+");
			tabControlDecks.TabPages.Add(tabPage);
			tabPage.Controls.Add(ctv);
		}
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