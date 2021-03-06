using WoMDB.Components;

namespace WoMDB.Forms
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
			this.treeViewDeck = new WoMDB.Components.WomCardTreeView();
			this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miIncreaseCount = new System.Windows.Forms.ToolStripMenuItem();
			this.miDecreaseCount = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportDeckToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.miAddCardList = new System.Windows.Forms.ToolStripMenuItem();
			this.miRemoveCardList = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPageSideboard = new System.Windows.Forms.TabPage();
			this.treeViewSide = new WoMDB.Components.WomCardTreeView();
			this.tabControlLocalInfo = new System.Windows.Forms.TabControl();
			this.tabPageCardtext = new System.Windows.Forms.TabPage();
			this.rtbCardText = new System.Windows.Forms.RichTextBox();
			this.tabPageStats = new System.Windows.Forms.TabPage();
			this.tabControlGlobalInfo = new System.Windows.Forms.TabControl();
			this.tabPageDescription = new System.Windows.Forms.TabPage();
			this.rtbDescription = new System.Windows.Forms.RichTextBox();
			this.tabPageHistory = new System.Windows.Forms.TabPage();
			this.treeViewHistory = new System.Windows.Forms.TreeView();
			this.tabPage5 = new System.Windows.Forms.TabPage();
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
			this.tbAuthor = new System.Windows.Forms.TextBox();
			this.lblAuthor = new System.Windows.Forms.Label();
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
			this.tabControlGlobalInfo.SuspendLayout();
			this.tabPageDescription.SuspendLayout();
			this.tabPageHistory.SuspendLayout();
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
			// treeViewDeck
			// 
			this.treeViewDeck.Cards = null;
			this.treeViewDeck.ContextMenuStrip = this.contextMenuStripTreeView;
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
			this.treeViewDeck.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewDeck_NodeMouseClick);
			this.treeViewDeck.FontChanged += new System.EventHandler(this.treeViewDeck_FontChanged);
			// 
			// contextMenuStripTreeView
			// 
			this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.miIncreaseCount,
				this.miDecreaseCount,
				this.miExportDeckToClipboard,
				this.miAddCardList,
				this.miRemoveCardList});
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
			// tabPageSideboard
			// 
			this.tabPageSideboard.Controls.Add(this.treeViewSide);
			resources.ApplyResources(this.tabPageSideboard, "tabPageSideboard");
			this.tabPageSideboard.Name = "tabPageSideboard";
			this.tabPageSideboard.UseVisualStyleBackColor = true;
			// 
			// treeViewSide
			// 
			this.treeViewSide.Cards = null;
			this.treeViewSide.ContextMenuStrip = this.contextMenuStripTreeView;
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
			// tabControlLocalInfo
			// 
			this.tabControlLocalInfo.Controls.Add(this.tabPageCardtext);
			this.tabControlLocalInfo.Controls.Add(this.tabPageStats);
			resources.ApplyResources(this.tabControlLocalInfo, "tabControlLocalInfo");
			this.tabControlLocalInfo.Name = "tabControlLocalInfo";
			this.tabControlLocalInfo.SelectedIndex = 0;
			// 
			// tabPageCardtext
			// 
			this.tabPageCardtext.Controls.Add(this.rtbCardText);
			resources.ApplyResources(this.tabPageCardtext, "tabPageCardtext");
			this.tabPageCardtext.Name = "tabPageCardtext";
			this.tabPageCardtext.UseVisualStyleBackColor = true;
			// 
			// rtbCardText
			// 
			this.rtbCardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.rtbCardText, "rtbCardText");
			this.rtbCardText.Name = "rtbCardText";
			this.rtbCardText.Text = "";
			// 
			// tabPageStats
			// 
			resources.ApplyResources(this.tabPageStats, "tabPageStats");
			this.tabPageStats.Name = "tabPageStats";
			this.tabPageStats.UseVisualStyleBackColor = true;
			// 
			// tabControlGlobalInfo
			// 
			this.tabControlGlobalInfo.Controls.Add(this.tabPageDescription);
			this.tabControlGlobalInfo.Controls.Add(this.tabPageHistory);
			this.tabControlGlobalInfo.Controls.Add(this.tabPage5);
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
			this.rtbDescription.Text = "";
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
			// tabPage5
			// 
			resources.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
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
			// DeckBuilderForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.tbDeckName);
			this.Controls.Add(this.tbAuthor);
			this.Controls.Add(this.lblAuthor);
			this.Controls.Add(this.lblDeckName);
			this.Controls.Add(this.menuStripMain);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStripMain;
			this.Name = "DeckBuilderForm";
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
			this.tabControlGlobalInfo.ResumeLayout(false);
			this.tabPageDescription.ResumeLayout(false);
			this.tabPageHistory.ResumeLayout(false);
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
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private WomCardTreeView treeViewDeck;
		private System.Windows.Forms.TabControl tabControlLocalInfo;
		private System.Windows.Forms.TabPage tabPageCardtext;
		private System.Windows.Forms.TabPage tabPageStats;
		private System.Windows.Forms.TabControl tabControlGlobalInfo;
		private System.Windows.Forms.TabPage tabPageDescription;
		private System.Windows.Forms.RichTextBox rtbDescription;
		private System.Windows.Forms.TabPage tabPageHistory;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeView;
		private System.Windows.Forms.ToolStripMenuItem miIncreaseCount;
		private System.Windows.Forms.ToolStripMenuItem miDecreaseCount;
		private System.Windows.Forms.RichTextBox rtbCardText;
		private System.Windows.Forms.TextBox tbAuthor;
		private System.Windows.Forms.Label lblAuthor;
		private System.Windows.Forms.TreeView treeViewHistory;
		private System.Windows.Forms.ToolStripMenuItem saveDeckAstoolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem miExportDeckToClipboard;
		private System.Windows.Forms.TabControl tabControlDecks;
		private System.Windows.Forms.TabPage tabPageDeck;
		private System.Windows.Forms.TabPage tabPageSideboard;
		private WomCardTreeView treeViewSide;
		private System.Windows.Forms.ToolStripMenuItem miAddCardList;
		private System.Windows.Forms.ToolStripMenuItem miRemoveCardList;
	}
}