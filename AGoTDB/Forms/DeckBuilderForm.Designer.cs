using AGoTDB.Components;

namespace AGoTDB.Forms
{
	partial class DeckBuilderForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeckBuilderForm));
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControlDecks = new System.Windows.Forms.TabControl();
			this.tabPageDeck = new System.Windows.Forms.TabPage();
			this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miIncreaseCount = new System.Windows.Forms.ToolStripMenuItem();
			this.miDecreaseCount = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportDeckToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.miAddCardList = new System.Windows.Forms.ToolStripMenuItem();
			this.miRemoveCardList = new System.Windows.Forms.ToolStripMenuItem();
			this.miGenerateProxyPdf = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPageSideboard = new System.Windows.Forms.TabPage();
			this.tabControlLocalInfo = new System.Windows.Forms.TabControl();
			this.tabPageCardtext = new System.Windows.Forms.TabPage();
			this.splitCardText = new System.Windows.Forms.SplitContainer();
			this.rtbCardText = new System.Windows.Forms.RichTextBox();
			this.tabControlGlobalInfo = new System.Windows.Forms.TabControl();
			this.tabPageDescription = new System.Windows.Forms.TabPage();
			this.rtbDescription = new System.Windows.Forms.RichTextBox();
			this.tabPageHistory = new System.Windows.Forms.TabPage();
			this.treeViewHistory = new System.Windows.Forms.TreeView();
			this.tabPageStats = new System.Windows.Forms.TabPage();
			this.rtbStatistics = new System.Windows.Forms.RichTextBox();
			this.lblDeckName = new System.Windows.Forms.Label();
			this.tbDeckName = new System.Windows.Forms.TextBox();
			this.menuStripMain = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveDeckAstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblHouse = new System.Windows.Forms.Label();
			this.eclHouse = new Beyond.ExtendedControls.ExtendedCheckedListBox();
			this.lblAgenda = new System.Windows.Forms.Label();
			this.tbAuthor = new System.Windows.Forms.TextBox();
			this.lblAuthor = new System.Windows.Forms.Label();
			this.eclAgenda = new Beyond.ExtendedControls.ExtendedCheckedListBox();
			this.treeViewDeck = new AGoTDB.Components.AgotCardTreeView();
			this.treeViewSide = new AGoTDB.Components.AgotCardTreeView();
			this.cardPreviewControl = new AGoTDB.Forms.CardPreviewControl();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControlDecks.SuspendLayout();
			this.tabPageDeck.SuspendLayout();
			this.contextMenuStripTreeView.SuspendLayout();
			this.tabPageSideboard.SuspendLayout();
			this.tabControlLocalInfo.SuspendLayout();
			this.tabPageCardtext.SuspendLayout();
			this.splitCardText.Panel1.SuspendLayout();
			this.splitCardText.Panel2.SuspendLayout();
			this.splitCardText.SuspendLayout();
			this.tabControlGlobalInfo.SuspendLayout();
			this.tabPageDescription.SuspendLayout();
			this.tabPageHistory.SuspendLayout();
			this.tabPageStats.SuspendLayout();
			this.menuStripMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer2
			// 
			resources.ApplyResources(this.splitContainer2, "splitContainer2");
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.tabControlGlobalInfo);
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabControlDecks);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControlLocalInfo);
			// 
			// tabControlDecks
			// 
			this.tabControlDecks.Controls.Add(this.tabPageDeck);
			this.tabControlDecks.Controls.Add(this.tabPageSideboard);
			resources.ApplyResources(this.tabControlDecks, "tabControlDecks");
			this.tabControlDecks.Name = "tabControlDecks";
			this.tabControlDecks.SelectedIndex = 0;
			// 
			// tabPageDeck
			// 
			this.tabPageDeck.Controls.Add(this.treeViewDeck);
			resources.ApplyResources(this.tabPageDeck, "tabPageDeck");
			this.tabPageDeck.Name = "tabPageDeck";
			this.tabPageDeck.UseVisualStyleBackColor = true;
			// 
			// contextMenuStripTreeView
			// 
			this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIncreaseCount,
            this.miDecreaseCount,
            this.miExportDeckToClipboard,
            this.miAddCardList,
            this.miRemoveCardList,
            this.miGenerateProxyPdf});
			this.contextMenuStripTreeView.Name = "contextMenuStrip1";
			resources.ApplyResources(this.contextMenuStripTreeView, "contextMenuStripTreeView");
			this.contextMenuStripTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripTreeView_Opening);
			// 
			// miIncreaseCount
			// 
			this.miIncreaseCount.Name = "miIncreaseCount";
			resources.ApplyResources(this.miIncreaseCount, "miIncreaseCount");
			this.miIncreaseCount.Click += new System.EventHandler(this.miIncreaseCount_Click);
			// 
			// miDecreaseCount
			// 
			this.miDecreaseCount.Name = "miDecreaseCount";
			resources.ApplyResources(this.miDecreaseCount, "miDecreaseCount");
			this.miDecreaseCount.Click += new System.EventHandler(this.miDecreaseCount_Click);
			// 
			// miExportDeckToClipboard
			// 
			this.miExportDeckToClipboard.Name = "miExportDeckToClipboard";
			resources.ApplyResources(this.miExportDeckToClipboard, "miExportDeckToClipboard");
			this.miExportDeckToClipboard.Click += new System.EventHandler(this.exportDeckToClipboardToolStripMenuItem_Click);
			// 
			// miAddCardList
			// 
			this.miAddCardList.Name = "miAddCardList";
			resources.ApplyResources(this.miAddCardList, "miAddCardList");
			this.miAddCardList.Click += new System.EventHandler(this.miAddCardList_Click);
			// 
			// miRemoveCardList
			// 
			this.miRemoveCardList.Name = "miRemoveCardList";
			resources.ApplyResources(this.miRemoveCardList, "miRemoveCardList");
			// 
			// miGenerateProxyPdf
			// 
			this.miGenerateProxyPdf.Name = "miGenerateProxyPdf";
			resources.ApplyResources(this.miGenerateProxyPdf, "miGenerateProxyPdf");
			this.miGenerateProxyPdf.Click += new System.EventHandler(this.miGenerateProxyPdf_Click);
			// 
			// tabPageSideboard
			// 
			this.tabPageSideboard.Controls.Add(this.treeViewSide);
			resources.ApplyResources(this.tabPageSideboard, "tabPageSideboard");
			this.tabPageSideboard.Name = "tabPageSideboard";
			this.tabPageSideboard.UseVisualStyleBackColor = true;
			// 
			// tabControlLocalInfo
			// 
			this.tabControlLocalInfo.Controls.Add(this.tabPageCardtext);
			resources.ApplyResources(this.tabControlLocalInfo, "tabControlLocalInfo");
			this.tabControlLocalInfo.Name = "tabControlLocalInfo";
			this.tabControlLocalInfo.SelectedIndex = 0;
			// 
			// tabPageCardtext
			// 
			this.tabPageCardtext.Controls.Add(this.splitCardText);
			resources.ApplyResources(this.tabPageCardtext, "tabPageCardtext");
			this.tabPageCardtext.Name = "tabPageCardtext";
			this.tabPageCardtext.UseVisualStyleBackColor = true;
			// 
			// splitCardText
			// 
			resources.ApplyResources(this.splitCardText, "splitCardText");
			this.splitCardText.Name = "splitCardText";
			// 
			// splitCardText.Panel1
			// 
			this.splitCardText.Panel1.Controls.Add(this.rtbCardText);
			// 
			// splitCardText.Panel2
			// 
			this.splitCardText.Panel2.Controls.Add(this.cardPreviewControl);
			// 
			// rtbCardText
			// 
			this.rtbCardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtbCardText, "rtbCardText");
			this.rtbCardText.Name = "rtbCardText";
			// 
			// tabControlGlobalInfo
			// 
			this.tabControlGlobalInfo.Controls.Add(this.tabPageDescription);
			this.tabControlGlobalInfo.Controls.Add(this.tabPageHistory);
			this.tabControlGlobalInfo.Controls.Add(this.tabPageStats);
			resources.ApplyResources(this.tabControlGlobalInfo, "tabControlGlobalInfo");
			this.tabControlGlobalInfo.Name = "tabControlGlobalInfo";
			this.tabControlGlobalInfo.SelectedIndex = 0;
			// 
			// tabPageDescription
			// 
			this.tabPageDescription.Controls.Add(this.rtbDescription);
			resources.ApplyResources(this.tabPageDescription, "tabPageDescription");
			this.tabPageDescription.Name = "tabPageDescription";
			this.tabPageDescription.UseVisualStyleBackColor = true;
			// 
			// rtbDescription
			// 
			this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtbDescription, "rtbDescription");
			this.rtbDescription.Name = "rtbDescription";
			// 
			// tabPageHistory
			// 
			this.tabPageHistory.Controls.Add(this.treeViewHistory);
			resources.ApplyResources(this.tabPageHistory, "tabPageHistory");
			this.tabPageHistory.Name = "tabPageHistory";
			this.tabPageHistory.UseVisualStyleBackColor = true;
			// 
			// treeViewHistory
			// 
			this.treeViewHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.treeViewHistory, "treeViewHistory");
			this.treeViewHistory.HideSelection = false;
			this.treeViewHistory.Name = "treeViewHistory";
			this.treeViewHistory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewHistory_AfterSelect);
			// 
			// tabPageStats
			// 
			this.tabPageStats.Controls.Add(this.rtbStatistics);
			resources.ApplyResources(this.tabPageStats, "tabPageStats");
			this.tabPageStats.Name = "tabPageStats";
			this.tabPageStats.UseVisualStyleBackColor = true;
			// 
			// rtbStatistics
			// 
			this.rtbStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtbStatistics, "rtbStatistics");
			this.rtbStatistics.Name = "rtbStatistics";
			// 
			// lblDeckName
			// 
			resources.ApplyResources(this.lblDeckName, "lblDeckName");
			this.lblDeckName.Name = "lblDeckName";
			// 
			// tbDeckName
			// 
			resources.ApplyResources(this.tbDeckName, "tbDeckName");
			this.tbDeckName.Name = "tbDeckName";
			// 
			// menuStripMain
			// 
			this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			resources.ApplyResources(this.menuStripMain, "menuStripMain");
			this.menuStripMain.Name = "menuStripMain";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadDeckToolStripMenuItem,
            this.saveDeckToolStripMenuItem,
            this.saveDeckAstoolStripMenuItem,
            this.newVersionToolStripMenuItem,
            this.printDeckToolStripMenuItem,
            this.exportToClipboardToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// loadDeckToolStripMenuItem
			// 
			this.loadDeckToolStripMenuItem.Name = "loadDeckToolStripMenuItem";
			resources.ApplyResources(this.loadDeckToolStripMenuItem, "loadDeckToolStripMenuItem");
			this.loadDeckToolStripMenuItem.Click += new System.EventHandler(this.loadDeckToolStripMenuItem_Click);
			// 
			// saveDeckToolStripMenuItem
			// 
			this.saveDeckToolStripMenuItem.Name = "saveDeckToolStripMenuItem";
			resources.ApplyResources(this.saveDeckToolStripMenuItem, "saveDeckToolStripMenuItem");
			this.saveDeckToolStripMenuItem.Click += new System.EventHandler(this.saveDeckToolStripMenuItem_Click);
			// 
			// saveDeckAstoolStripMenuItem
			// 
			this.saveDeckAstoolStripMenuItem.Name = "saveDeckAstoolStripMenuItem";
			resources.ApplyResources(this.saveDeckAstoolStripMenuItem, "saveDeckAstoolStripMenuItem");
			this.saveDeckAstoolStripMenuItem.Click += new System.EventHandler(this.saveDeckAstoolStripMenuItem_Click);
			// 
			// newVersionToolStripMenuItem
			// 
			this.newVersionToolStripMenuItem.Name = "newVersionToolStripMenuItem";
			resources.ApplyResources(this.newVersionToolStripMenuItem, "newVersionToolStripMenuItem");
			this.newVersionToolStripMenuItem.Click += new System.EventHandler(this.newVersionToolStripMenuItem_Click);
			// 
			// printDeckToolStripMenuItem
			// 
			this.printDeckToolStripMenuItem.Name = "printDeckToolStripMenuItem";
			resources.ApplyResources(this.printDeckToolStripMenuItem, "printDeckToolStripMenuItem");
			this.printDeckToolStripMenuItem.Click += new System.EventHandler(this.printDeckToolStripMenuItem_Click);
			// 
			// exportToClipboardToolStripMenuItem
			// 
			this.exportToClipboardToolStripMenuItem.Name = "exportToClipboardToolStripMenuItem";
			resources.ApplyResources(this.exportToClipboardToolStripMenuItem, "exportToClipboardToolStripMenuItem");
			this.exportToClipboardToolStripMenuItem.Click += new System.EventHandler(this.exportToClipboardToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawSimulatorToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			// 
			// drawSimulatorToolStripMenuItem
			// 
			this.drawSimulatorToolStripMenuItem.Name = "drawSimulatorToolStripMenuItem";
			resources.ApplyResources(this.drawSimulatorToolStripMenuItem, "drawSimulatorToolStripMenuItem");
			this.drawSimulatorToolStripMenuItem.Click += new System.EventHandler(this.drawSimulatorToolStripMenuItem_Click);
			// 
			// lblHouse
			// 
			resources.ApplyResources(this.lblHouse, "lblHouse");
			this.lblHouse.Name = "lblHouse";
			// 
			// eclHouse
			// 
			resources.ApplyResources(this.eclHouse, "eclHouse");
			this.eclHouse.CheckOnClick = true;
			this.eclHouse.Condensed = true;
			this.eclHouse.CondensedMode = true;
			this.eclHouse.FormattingEnabled = true;
			this.eclHouse.Name = "eclHouse";
			this.eclHouse.RollDownDelay = 250;
			this.eclHouse.Summary = "House";
			this.eclHouse.ThreeState = false;
			this.eclHouse.SelectedValueChanged += new System.EventHandler(this.eclHouse_SelectedValueChanged);
			// 
			// lblAgenda
			// 
			resources.ApplyResources(this.lblAgenda, "lblAgenda");
			this.lblAgenda.Name = "lblAgenda";
			// 
			// tbAuthor
			// 
			resources.ApplyResources(this.tbAuthor, "tbAuthor");
			this.tbAuthor.Name = "tbAuthor";
			// 
			// lblAuthor
			// 
			resources.ApplyResources(this.lblAuthor, "lblAuthor");
			this.lblAuthor.Name = "lblAuthor";
			// 
			// eclAgenda
			// 
			resources.ApplyResources(this.eclAgenda, "eclAgenda");
			this.eclAgenda.CheckOnClick = true;
			this.eclAgenda.Condensed = true;
			this.eclAgenda.CondensedMode = true;
			this.eclAgenda.FormattingEnabled = true;
			this.eclAgenda.Name = "eclAgenda";
			this.eclAgenda.RollDownDelay = 250;
			this.eclAgenda.Summary = "Agenda";
			this.eclAgenda.ThreeState = false;
			this.eclAgenda.SelectedValueChanged += new System.EventHandler(this.eclAgenda_SelectedValueChanged);
			// 
			// treeViewDeck
			// 
			this.treeViewDeck.Cards = null;
			this.treeViewDeck.ContextMenuStrip = this.contextMenuStripTreeView;
			this.treeViewDeck.Deck = null;
			resources.ApplyResources(this.treeViewDeck, "treeViewDeck");
			this.treeViewDeck.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.treeViewDeck.HideSelection = false;
			this.treeViewDeck.Name = "treeViewDeck";
			this.treeViewDeck.NodeInfo = null;
			this.treeViewDeck.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeViewDeck.Nodes")))});
			this.treeViewDeck.ShowNodeToolTips = true;
			this.treeViewDeck.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewDeck_DrawNode);
			this.treeViewDeck.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDeck_AfterSelect);
			this.treeViewDeck.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeViewDeck_KeyPress);
			this.treeViewDeck.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDeck_NodeMouseClick);
			this.treeViewDeck.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewDeck_KeyDown);
			this.treeViewDeck.FontChanged += new System.EventHandler(this.treeViewDeck_FontChanged);
			// 
			// treeViewSide
			// 
			this.treeViewSide.Cards = null;
			this.treeViewSide.ContextMenuStrip = this.contextMenuStripTreeView;
			this.treeViewSide.Deck = null;
			resources.ApplyResources(this.treeViewSide, "treeViewSide");
			this.treeViewSide.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.treeViewSide.HideSelection = false;
			this.treeViewSide.Name = "treeViewSide";
			this.treeViewSide.NodeInfo = null;
			this.treeViewSide.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeViewSide.Nodes")))});
			this.treeViewSide.ShowNodeToolTips = true;
			this.treeViewSide.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewDeck_DrawNode);
			this.treeViewSide.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDeck_AfterSelect);
			this.treeViewSide.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDeck_NodeMouseClick);
			// 
			// cardPreviewControl
			// 
			this.cardPreviewControl.CardUniversalId = -1;
			resources.ApplyResources(this.cardPreviewControl, "cardPreviewControl");
			this.cardPreviewControl.Name = "cardPreviewControl";
			this.cardPreviewControl.MouseCaptureChanged += new System.EventHandler(this.cardPreviewControl1_MouseCaptureChanged);
			this.cardPreviewControl.MouseLeave += new System.EventHandler(this.cardPreviewControl1_MouseLeave);
			this.cardPreviewControl.MouseEnter += new System.EventHandler(this.cardPreviewControl1_MouseEnter);
			// 
			// DeckBuilderForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbAuthor);
			this.Controls.Add(this.lblAuthor);
			this.Controls.Add(this.eclHouse);
			this.Controls.Add(this.eclAgenda);
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.lblAgenda);
			this.Controls.Add(this.lblHouse);
			this.Controls.Add(this.tbDeckName);
			this.Controls.Add(this.lblDeckName);
			this.Controls.Add(this.menuStripMain);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStripMain;
			this.Name = "DeckBuilderForm";
			this.Shown += new System.EventHandler(this.DeckBuilderForm_Shown);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeckBuilderForm_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeckBuilderForm_FormClosing);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.tabControlDecks.ResumeLayout(false);
			this.tabPageDeck.ResumeLayout(false);
			this.contextMenuStripTreeView.ResumeLayout(false);
			this.tabPageSideboard.ResumeLayout(false);
			this.tabControlLocalInfo.ResumeLayout(false);
			this.tabPageCardtext.ResumeLayout(false);
			this.splitCardText.Panel1.ResumeLayout(false);
			this.splitCardText.Panel2.ResumeLayout(false);
			this.splitCardText.ResumeLayout(false);
			this.tabControlGlobalInfo.ResumeLayout(false);
			this.tabPageDescription.ResumeLayout(false);
			this.tabPageHistory.ResumeLayout(false);
			this.tabPageStats.ResumeLayout(false);
			this.menuStripMain.ResumeLayout(false);
			this.menuStripMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDeckName;
		private System.Windows.Forms.TextBox tbDeckName;
		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newVersionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToClipboardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawSimulatorToolStripMenuItem;
		private System.Windows.Forms.Label lblHouse;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclHouse;
    private System.Windows.Forms.Label lblAgenda;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private AgotCardTreeView treeViewDeck;
		private System.Windows.Forms.TabControl tabControlLocalInfo;
		private System.Windows.Forms.TabPage tabPageCardtext;
		private System.Windows.Forms.TabControl tabControlGlobalInfo;
		private System.Windows.Forms.TabPage tabPageDescription;
		private System.Windows.Forms.RichTextBox rtbDescription;
		private System.Windows.Forms.TabPage tabPageHistory;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeView;
		private System.Windows.Forms.ToolStripMenuItem miIncreaseCount;
		private System.Windows.Forms.ToolStripMenuItem miDecreaseCount;
		private System.Windows.Forms.TextBox tbAuthor;
		private System.Windows.Forms.Label lblAuthor;
		private System.Windows.Forms.TreeView treeViewHistory;
		private System.Windows.Forms.ToolStripMenuItem saveDeckAstoolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem miExportDeckToClipboard;
		private System.Windows.Forms.TabControl tabControlDecks;
		private System.Windows.Forms.TabPage tabPageDeck;
		private System.Windows.Forms.TabPage tabPageSideboard;
		private AgotCardTreeView treeViewSide;
		private System.Windows.Forms.ToolStripMenuItem miAddCardList;
		private System.Windows.Forms.ToolStripMenuItem miRemoveCardList;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclAgenda;
	private System.Windows.Forms.ToolStripMenuItem miGenerateProxyPdf;
	private System.Windows.Forms.RichTextBox rtbCardText;
	private System.Windows.Forms.TabPage tabPageStats;
	private System.Windows.Forms.RichTextBox rtbStatistics;
	private System.Windows.Forms.SplitContainer splitCardText;
	private CardPreviewControl cardPreviewControl;
	}
}