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
      this.treeViewDeck = new AGoTDB.Components.AgotCardTreeView();
      this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.miIncreaseCount = new System.Windows.Forms.ToolStripMenuItem();
      this.miDecreaseCount = new System.Windows.Forms.ToolStripMenuItem();
      this.miExportDeckToClipboard = new System.Windows.Forms.ToolStripMenuItem();
      this.miAddCardList = new System.Windows.Forms.ToolStripMenuItem();
      this.miRemoveCardList = new System.Windows.Forms.ToolStripMenuItem();
      this.tabPageSideboard = new System.Windows.Forms.TabPage();
      this.treeViewSide = new AGoTDB.Components.AgotCardTreeView();
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
      this.lblHouse = new System.Windows.Forms.Label();
      this.eclHouse = new Beyond.ExtendedControls.ExtendedCheckedListBox();
      this.lblAgenda = new System.Windows.Forms.Label();
      this.tbAuthor = new System.Windows.Forms.TextBox();
      this.lblAuthor = new System.Windows.Forms.Label();
      this.eclAgenda = new Beyond.ExtendedControls.ExtendedCheckedListBox();
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
      this.splitContainer2.AccessibleDescription = null;
      this.splitContainer2.AccessibleName = null;
      resources.ApplyResources(this.splitContainer2, "splitContainer2");
      this.splitContainer2.BackgroundImage = null;
      this.splitContainer2.Font = null;
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.AccessibleDescription = null;
      this.splitContainer2.Panel1.AccessibleName = null;
      resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
      this.splitContainer2.Panel1.BackgroundImage = null;
      this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
      this.splitContainer2.Panel1.Font = null;
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.AccessibleDescription = null;
      this.splitContainer2.Panel2.AccessibleName = null;
      resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
      this.splitContainer2.Panel2.BackgroundImage = null;
      this.splitContainer2.Panel2.Controls.Add(this.tabControlGlobalInfo);
      this.splitContainer2.Panel2.Font = null;
      // 
      // splitContainer1
      // 
      this.splitContainer1.AccessibleDescription = null;
      this.splitContainer1.AccessibleName = null;
      resources.ApplyResources(this.splitContainer1, "splitContainer1");
      this.splitContainer1.BackgroundImage = null;
      this.splitContainer1.Font = null;
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.AccessibleDescription = null;
      this.splitContainer1.Panel1.AccessibleName = null;
      resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
      this.splitContainer1.Panel1.BackgroundImage = null;
      this.splitContainer1.Panel1.Controls.Add(this.tabControlDecks);
      this.splitContainer1.Panel1.Font = null;
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.AccessibleDescription = null;
      this.splitContainer1.Panel2.AccessibleName = null;
      resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
      this.splitContainer1.Panel2.BackgroundImage = null;
      this.splitContainer1.Panel2.Controls.Add(this.tabControlLocalInfo);
      this.splitContainer1.Panel2.Font = null;
      // 
      // tabControlDecks
      // 
      this.tabControlDecks.AccessibleDescription = null;
      this.tabControlDecks.AccessibleName = null;
      resources.ApplyResources(this.tabControlDecks, "tabControlDecks");
      this.tabControlDecks.BackgroundImage = null;
      this.tabControlDecks.Controls.Add(this.tabPageDeck);
      this.tabControlDecks.Controls.Add(this.tabPageSideboard);
      this.tabControlDecks.Font = null;
      this.tabControlDecks.Name = "tabControlDecks";
      this.tabControlDecks.SelectedIndex = 0;
      // 
      // tabPageDeck
      // 
      this.tabPageDeck.AccessibleDescription = null;
      this.tabPageDeck.AccessibleName = null;
      resources.ApplyResources(this.tabPageDeck, "tabPageDeck");
      this.tabPageDeck.BackgroundImage = null;
      this.tabPageDeck.Controls.Add(this.treeViewDeck);
      this.tabPageDeck.Font = null;
      this.tabPageDeck.Name = "tabPageDeck";
      this.tabPageDeck.UseVisualStyleBackColor = true;
      // 
      // treeViewDeck
      // 
      this.treeViewDeck.AccessibleDescription = null;
      this.treeViewDeck.AccessibleName = null;
      resources.ApplyResources(this.treeViewDeck, "treeViewDeck");
      this.treeViewDeck.BackgroundImage = null;
      this.treeViewDeck.Cards = null;
      this.treeViewDeck.ContextMenuStrip = this.contextMenuStripTreeView;
      this.treeViewDeck.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
      this.treeViewDeck.Font = null;
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
      this.contextMenuStripTreeView.AccessibleDescription = null;
      this.contextMenuStripTreeView.AccessibleName = null;
      resources.ApplyResources(this.contextMenuStripTreeView, "contextMenuStripTreeView");
      this.contextMenuStripTreeView.BackgroundImage = null;
      this.contextMenuStripTreeView.Font = null;
      this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIncreaseCount,
            this.miDecreaseCount,
            this.miExportDeckToClipboard,
            this.miAddCardList,
            this.miRemoveCardList});
      this.contextMenuStripTreeView.Name = "contextMenuStrip1";
      this.contextMenuStripTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripTreeView_Opening);
      // 
      // miIncreaseCount
      // 
      this.miIncreaseCount.AccessibleDescription = null;
      this.miIncreaseCount.AccessibleName = null;
      resources.ApplyResources(this.miIncreaseCount, "miIncreaseCount");
      this.miIncreaseCount.BackgroundImage = null;
      this.miIncreaseCount.Name = "miIncreaseCount";
      this.miIncreaseCount.ShortcutKeyDisplayString = null;
      this.miIncreaseCount.Click += new System.EventHandler(this.miIncreaseCount_Click);
      // 
      // miDecreaseCount
      // 
      this.miDecreaseCount.AccessibleDescription = null;
      this.miDecreaseCount.AccessibleName = null;
      resources.ApplyResources(this.miDecreaseCount, "miDecreaseCount");
      this.miDecreaseCount.BackgroundImage = null;
      this.miDecreaseCount.Name = "miDecreaseCount";
      this.miDecreaseCount.ShortcutKeyDisplayString = null;
      this.miDecreaseCount.Click += new System.EventHandler(this.miDecreaseCount_Click);
      // 
      // miExportDeckToClipboard
      // 
      this.miExportDeckToClipboard.AccessibleDescription = null;
      this.miExportDeckToClipboard.AccessibleName = null;
      resources.ApplyResources(this.miExportDeckToClipboard, "miExportDeckToClipboard");
      this.miExportDeckToClipboard.BackgroundImage = null;
      this.miExportDeckToClipboard.Name = "miExportDeckToClipboard";
      this.miExportDeckToClipboard.ShortcutKeyDisplayString = null;
      this.miExportDeckToClipboard.Click += new System.EventHandler(this.exportDeckToClipboardToolStripMenuItem_Click);
      // 
      // miAddCardList
      // 
      this.miAddCardList.AccessibleDescription = null;
      this.miAddCardList.AccessibleName = null;
      resources.ApplyResources(this.miAddCardList, "miAddCardList");
      this.miAddCardList.BackgroundImage = null;
      this.miAddCardList.Name = "miAddCardList";
      this.miAddCardList.ShortcutKeyDisplayString = null;
      this.miAddCardList.Click += new System.EventHandler(this.miAddCardList_Click);
      // 
      // miRemoveCardList
      // 
      this.miRemoveCardList.AccessibleDescription = null;
      this.miRemoveCardList.AccessibleName = null;
      resources.ApplyResources(this.miRemoveCardList, "miRemoveCardList");
      this.miRemoveCardList.BackgroundImage = null;
      this.miRemoveCardList.Name = "miRemoveCardList";
      this.miRemoveCardList.ShortcutKeyDisplayString = null;
      // 
      // tabPageSideboard
      // 
      this.tabPageSideboard.AccessibleDescription = null;
      this.tabPageSideboard.AccessibleName = null;
      resources.ApplyResources(this.tabPageSideboard, "tabPageSideboard");
      this.tabPageSideboard.BackgroundImage = null;
      this.tabPageSideboard.Controls.Add(this.treeViewSide);
      this.tabPageSideboard.Font = null;
      this.tabPageSideboard.Name = "tabPageSideboard";
      this.tabPageSideboard.UseVisualStyleBackColor = true;
      // 
      // treeViewSide
      // 
      this.treeViewSide.AccessibleDescription = null;
      this.treeViewSide.AccessibleName = null;
      resources.ApplyResources(this.treeViewSide, "treeViewSide");
      this.treeViewSide.BackgroundImage = null;
      this.treeViewSide.Cards = null;
      this.treeViewSide.ContextMenuStrip = this.contextMenuStripTreeView;
      this.treeViewSide.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
      this.treeViewSide.Font = null;
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
      this.tabControlLocalInfo.AccessibleDescription = null;
      this.tabControlLocalInfo.AccessibleName = null;
      resources.ApplyResources(this.tabControlLocalInfo, "tabControlLocalInfo");
      this.tabControlLocalInfo.BackgroundImage = null;
      this.tabControlLocalInfo.Controls.Add(this.tabPageCardtext);
      this.tabControlLocalInfo.Controls.Add(this.tabPageStats);
      this.tabControlLocalInfo.Font = null;
      this.tabControlLocalInfo.Name = "tabControlLocalInfo";
      this.tabControlLocalInfo.SelectedIndex = 0;
      // 
      // tabPageCardtext
      // 
      this.tabPageCardtext.AccessibleDescription = null;
      this.tabPageCardtext.AccessibleName = null;
      resources.ApplyResources(this.tabPageCardtext, "tabPageCardtext");
      this.tabPageCardtext.BackgroundImage = null;
      this.tabPageCardtext.Controls.Add(this.rtbCardText);
      this.tabPageCardtext.Font = null;
      this.tabPageCardtext.Name = "tabPageCardtext";
      this.tabPageCardtext.UseVisualStyleBackColor = true;
      // 
      // rtbCardText
      // 
      this.rtbCardText.AccessibleDescription = null;
      this.rtbCardText.AccessibleName = null;
      resources.ApplyResources(this.rtbCardText, "rtbCardText");
      this.rtbCardText.BackgroundImage = null;
      this.rtbCardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtbCardText.Font = null;
      this.rtbCardText.Name = "rtbCardText";
      // 
      // tabPageStats
      // 
      this.tabPageStats.AccessibleDescription = null;
      this.tabPageStats.AccessibleName = null;
      resources.ApplyResources(this.tabPageStats, "tabPageStats");
      this.tabPageStats.BackgroundImage = null;
      this.tabPageStats.Font = null;
      this.tabPageStats.Name = "tabPageStats";
      this.tabPageStats.UseVisualStyleBackColor = true;
      // 
      // tabControlGlobalInfo
      // 
      this.tabControlGlobalInfo.AccessibleDescription = null;
      this.tabControlGlobalInfo.AccessibleName = null;
      resources.ApplyResources(this.tabControlGlobalInfo, "tabControlGlobalInfo");
      this.tabControlGlobalInfo.BackgroundImage = null;
      this.tabControlGlobalInfo.Controls.Add(this.tabPageDescription);
      this.tabControlGlobalInfo.Controls.Add(this.tabPageHistory);
      this.tabControlGlobalInfo.Controls.Add(this.tabPage5);
      this.tabControlGlobalInfo.Font = null;
      this.tabControlGlobalInfo.Name = "tabControlGlobalInfo";
      this.tabControlGlobalInfo.SelectedIndex = 0;
      // 
      // tabPageDescription
      // 
      this.tabPageDescription.AccessibleDescription = null;
      this.tabPageDescription.AccessibleName = null;
      resources.ApplyResources(this.tabPageDescription, "tabPageDescription");
      this.tabPageDescription.BackgroundImage = null;
      this.tabPageDescription.Controls.Add(this.rtbDescription);
      this.tabPageDescription.Font = null;
      this.tabPageDescription.Name = "tabPageDescription";
      this.tabPageDescription.UseVisualStyleBackColor = true;
      // 
      // rtbDescription
      // 
      this.rtbDescription.AccessibleDescription = null;
      this.rtbDescription.AccessibleName = null;
      resources.ApplyResources(this.rtbDescription, "rtbDescription");
      this.rtbDescription.BackgroundImage = null;
      this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtbDescription.Font = null;
      this.rtbDescription.Name = "rtbDescription";
      // 
      // tabPageHistory
      // 
      this.tabPageHistory.AccessibleDescription = null;
      this.tabPageHistory.AccessibleName = null;
      resources.ApplyResources(this.tabPageHistory, "tabPageHistory");
      this.tabPageHistory.BackgroundImage = null;
      this.tabPageHistory.Controls.Add(this.treeViewHistory);
      this.tabPageHistory.Font = null;
      this.tabPageHistory.Name = "tabPageHistory";
      this.tabPageHistory.UseVisualStyleBackColor = true;
      // 
      // treeViewHistory
      // 
      this.treeViewHistory.AccessibleDescription = null;
      this.treeViewHistory.AccessibleName = null;
      resources.ApplyResources(this.treeViewHistory, "treeViewHistory");
      this.treeViewHistory.BackgroundImage = null;
      this.treeViewHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.treeViewHistory.Font = null;
      this.treeViewHistory.HideSelection = false;
      this.treeViewHistory.Name = "treeViewHistory";
      this.treeViewHistory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewHistory_AfterSelect);
      // 
      // tabPage5
      // 
      this.tabPage5.AccessibleDescription = null;
      this.tabPage5.AccessibleName = null;
      resources.ApplyResources(this.tabPage5, "tabPage5");
      this.tabPage5.BackgroundImage = null;
      this.tabPage5.Font = null;
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.UseVisualStyleBackColor = true;
      // 
      // lblDeckName
      // 
      this.lblDeckName.AccessibleDescription = null;
      this.lblDeckName.AccessibleName = null;
      resources.ApplyResources(this.lblDeckName, "lblDeckName");
      this.lblDeckName.Font = null;
      this.lblDeckName.Name = "lblDeckName";
      // 
      // tbDeckName
      // 
      this.tbDeckName.AccessibleDescription = null;
      this.tbDeckName.AccessibleName = null;
      resources.ApplyResources(this.tbDeckName, "tbDeckName");
      this.tbDeckName.BackgroundImage = null;
      this.tbDeckName.Font = null;
      this.tbDeckName.Name = "tbDeckName";
      // 
      // menuStripMain
      // 
      this.menuStripMain.AccessibleDescription = null;
      this.menuStripMain.AccessibleName = null;
      resources.ApplyResources(this.menuStripMain, "menuStripMain");
      this.menuStripMain.BackgroundImage = null;
      this.menuStripMain.Font = null;
      this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
      this.menuStripMain.Name = "menuStripMain";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.AccessibleDescription = null;
      this.fileToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
      this.fileToolStripMenuItem.BackgroundImage = null;
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
      this.fileToolStripMenuItem.ShortcutKeyDisplayString = null;
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.AccessibleDescription = null;
      this.newToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
      this.newToolStripMenuItem.BackgroundImage = null;
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // loadDeckToolStripMenuItem
      // 
      this.loadDeckToolStripMenuItem.AccessibleDescription = null;
      this.loadDeckToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.loadDeckToolStripMenuItem, "loadDeckToolStripMenuItem");
      this.loadDeckToolStripMenuItem.BackgroundImage = null;
      this.loadDeckToolStripMenuItem.Name = "loadDeckToolStripMenuItem";
      this.loadDeckToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.loadDeckToolStripMenuItem.Click += new System.EventHandler(this.loadDeckToolStripMenuItem_Click);
      // 
      // saveDeckToolStripMenuItem
      // 
      this.saveDeckToolStripMenuItem.AccessibleDescription = null;
      this.saveDeckToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.saveDeckToolStripMenuItem, "saveDeckToolStripMenuItem");
      this.saveDeckToolStripMenuItem.BackgroundImage = null;
      this.saveDeckToolStripMenuItem.Name = "saveDeckToolStripMenuItem";
      this.saveDeckToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.saveDeckToolStripMenuItem.Click += new System.EventHandler(this.saveDeckToolStripMenuItem_Click);
      // 
      // saveDeckAstoolStripMenuItem
      // 
      this.saveDeckAstoolStripMenuItem.AccessibleDescription = null;
      this.saveDeckAstoolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.saveDeckAstoolStripMenuItem, "saveDeckAstoolStripMenuItem");
      this.saveDeckAstoolStripMenuItem.BackgroundImage = null;
      this.saveDeckAstoolStripMenuItem.Name = "saveDeckAstoolStripMenuItem";
      this.saveDeckAstoolStripMenuItem.ShortcutKeyDisplayString = null;
      this.saveDeckAstoolStripMenuItem.Click += new System.EventHandler(this.saveDeckAstoolStripMenuItem_Click);
      // 
      // newVersionToolStripMenuItem
      // 
      this.newVersionToolStripMenuItem.AccessibleDescription = null;
      this.newVersionToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.newVersionToolStripMenuItem, "newVersionToolStripMenuItem");
      this.newVersionToolStripMenuItem.BackgroundImage = null;
      this.newVersionToolStripMenuItem.Name = "newVersionToolStripMenuItem";
      this.newVersionToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.newVersionToolStripMenuItem.Click += new System.EventHandler(this.newVersionToolStripMenuItem_Click);
      // 
      // printDeckToolStripMenuItem
      // 
      this.printDeckToolStripMenuItem.AccessibleDescription = null;
      this.printDeckToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.printDeckToolStripMenuItem, "printDeckToolStripMenuItem");
      this.printDeckToolStripMenuItem.BackgroundImage = null;
      this.printDeckToolStripMenuItem.Name = "printDeckToolStripMenuItem";
      this.printDeckToolStripMenuItem.ShortcutKeyDisplayString = null;
      // 
      // exportToClipboardToolStripMenuItem
      // 
      this.exportToClipboardToolStripMenuItem.AccessibleDescription = null;
      this.exportToClipboardToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.exportToClipboardToolStripMenuItem, "exportToClipboardToolStripMenuItem");
      this.exportToClipboardToolStripMenuItem.BackgroundImage = null;
      this.exportToClipboardToolStripMenuItem.Name = "exportToClipboardToolStripMenuItem";
      this.exportToClipboardToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.exportToClipboardToolStripMenuItem.Click += new System.EventHandler(this.exportToClipboardToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.AccessibleDescription = null;
      this.exitToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
      this.exitToolStripMenuItem.BackgroundImage = null;
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.AccessibleDescription = null;
      this.toolsToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
      this.toolsToolStripMenuItem.BackgroundImage = null;
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawSimulatorToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.ShortcutKeyDisplayString = null;
      // 
      // drawSimulatorToolStripMenuItem
      // 
      this.drawSimulatorToolStripMenuItem.AccessibleDescription = null;
      this.drawSimulatorToolStripMenuItem.AccessibleName = null;
      resources.ApplyResources(this.drawSimulatorToolStripMenuItem, "drawSimulatorToolStripMenuItem");
      this.drawSimulatorToolStripMenuItem.BackgroundImage = null;
      this.drawSimulatorToolStripMenuItem.Name = "drawSimulatorToolStripMenuItem";
      this.drawSimulatorToolStripMenuItem.ShortcutKeyDisplayString = null;
      this.drawSimulatorToolStripMenuItem.Click += new System.EventHandler(this.drawSimulatorToolStripMenuItem_Click);
      // 
      // lblHouse
      // 
      this.lblHouse.AccessibleDescription = null;
      this.lblHouse.AccessibleName = null;
      resources.ApplyResources(this.lblHouse, "lblHouse");
      this.lblHouse.Font = null;
      this.lblHouse.Name = "lblHouse";
      // 
      // eclHouse
      // 
      this.eclHouse.AccessibleDescription = null;
      this.eclHouse.AccessibleName = null;
      resources.ApplyResources(this.eclHouse, "eclHouse");
      this.eclHouse.BackgroundImage = null;
      this.eclHouse.CheckOnClick = true;
      this.eclHouse.Condensed = true;
      this.eclHouse.CondensedMode = true;
      this.eclHouse.Font = null;
      this.eclHouse.FormattingEnabled = true;
      this.eclHouse.Name = "eclHouse";
      this.eclHouse.RollDownDelay = 250;
      this.eclHouse.Summary = "House";
      this.eclHouse.ThreeState = false;
      this.eclHouse.SelectedValueChanged += new System.EventHandler(this.eclHouse_SelectedValueChanged);
      // 
      // lblAgenda
      // 
      this.lblAgenda.AccessibleDescription = null;
      this.lblAgenda.AccessibleName = null;
      resources.ApplyResources(this.lblAgenda, "lblAgenda");
      this.lblAgenda.Font = null;
      this.lblAgenda.Name = "lblAgenda";
      // 
      // tbAuthor
      // 
      this.tbAuthor.AccessibleDescription = null;
      this.tbAuthor.AccessibleName = null;
      resources.ApplyResources(this.tbAuthor, "tbAuthor");
      this.tbAuthor.BackgroundImage = null;
      this.tbAuthor.Font = null;
      this.tbAuthor.Name = "tbAuthor";
      // 
      // lblAuthor
      // 
      this.lblAuthor.AccessibleDescription = null;
      this.lblAuthor.AccessibleName = null;
      resources.ApplyResources(this.lblAuthor, "lblAuthor");
      this.lblAuthor.Font = null;
      this.lblAuthor.Name = "lblAuthor";
      // 
      // eclAgenda
      // 
      this.eclAgenda.AccessibleDescription = null;
      this.eclAgenda.AccessibleName = null;
      resources.ApplyResources(this.eclAgenda, "eclAgenda");
      this.eclAgenda.BackgroundImage = null;
      this.eclAgenda.CheckOnClick = true;
      this.eclAgenda.Condensed = true;
      this.eclAgenda.CondensedMode = true;
      this.eclAgenda.Font = null;
      this.eclAgenda.FormattingEnabled = true;
      this.eclAgenda.Name = "eclAgenda";
      this.eclAgenda.RollDownDelay = 250;
      this.eclAgenda.Summary = "Agenda";
      this.eclAgenda.ThreeState = false;
      this.eclAgenda.SelectedValueChanged += new System.EventHandler(this.eclAgenda_SelectedValueChanged);
      // 
      // DeckBuilderForm
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = null;
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
      this.Font = null;
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
		private System.Windows.Forms.Label lblHouse;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclHouse;
    private System.Windows.Forms.Label lblAgenda;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private AgotCardTreeView treeViewDeck;
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
		private AgotCardTreeView treeViewSide;
		private System.Windows.Forms.ToolStripMenuItem miAddCardList;
		private System.Windows.Forms.ToolStripMenuItem miRemoveCardList;
    private Beyond.ExtendedControls.ExtendedCheckedListBox eclAgenda;
	}
}