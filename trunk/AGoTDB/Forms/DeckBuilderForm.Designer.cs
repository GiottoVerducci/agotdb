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
            this.miGenerateProxyPdf = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageSideboard = new System.Windows.Forms.TabPage();
            this.treeViewSide = new AGoTDB.Components.AgotCardTreeView();
            this.tabControlLocalInfo = new System.Windows.Forms.TabControl();
            this.tabPageCardtext = new System.Windows.Forms.TabPage();
            this.splitCardText = new System.Windows.Forms.SplitContainer();
            this.rtbCardText = new System.Windows.Forms.RichTextBox();
            this.cardPreviewControl = new AGoTDB.Forms.CardPreviewControl();
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
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sdsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDeckToClipboardSortedByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDeckToClipboardSortedBySetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDeckToOCTGNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDeckFromOCTGNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblHouse = new System.Windows.Forms.Label();
            this.eclHouse = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.lblAgenda = new System.Windows.Forms.Label();
            this.tbAuthor = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.eclAgenda = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.rbJoust = new System.Windows.Forms.RadioButton();
            this.rbMelee = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlDecks.SuspendLayout();
            this.tabPageDeck.SuspendLayout();
            this.contextMenuStripTreeView.SuspendLayout();
            this.tabPageSideboard.SuspendLayout();
            this.tabControlLocalInfo.SuspendLayout();
            this.tabPageCardtext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCardText)).BeginInit();
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
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.tabControlGlobalInfo);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.tabControlDecks);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.tabControlLocalInfo);
            // 
            // tabControlDecks
            // 
            resources.ApplyResources(this.tabControlDecks, "tabControlDecks");
            this.tabControlDecks.Controls.Add(this.tabPageDeck);
            this.tabControlDecks.Controls.Add(this.tabPageSideboard);
            this.tabControlDecks.Name = "tabControlDecks";
            this.tabControlDecks.SelectedIndex = 0;
            // 
            // tabPageDeck
            // 
            resources.ApplyResources(this.tabPageDeck, "tabPageDeck");
            this.tabPageDeck.Controls.Add(this.treeViewDeck);
            this.tabPageDeck.Name = "tabPageDeck";
            this.tabPageDeck.UseVisualStyleBackColor = true;
            // 
            // treeViewDeck
            // 
            resources.ApplyResources(this.treeViewDeck, "treeViewDeck");
            this.treeViewDeck.Cards = null;
            this.treeViewDeck.ContextMenuStrip = this.contextMenuStripTreeView;
            this.treeViewDeck.Deck = null;
            this.treeViewDeck.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewDeck.HideSelection = false;
            this.treeViewDeck.Name = "treeViewDeck";
            this.treeViewDeck.NodeInfo = null;
            this.treeViewDeck.ShowNodeToolTips = true;
            this.treeViewDeck.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeViewDeck_DrawNode);
            this.treeViewDeck.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewDeck_AfterSelect);
            this.treeViewDeck.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewDeck_NodeMouseClick);
            this.treeViewDeck.FontChanged += new System.EventHandler(this.TreeViewDeck_FontChanged);
            this.treeViewDeck.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewDeck_KeyDown);
            this.treeViewDeck.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TreeViewDeck_KeyPress);
            // 
            // contextMenuStripTreeView
            // 
            resources.ApplyResources(this.contextMenuStripTreeView, "contextMenuStripTreeView");
            this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miIncreaseCount,
            this.miDecreaseCount,
            this.miExportDeckToClipboard,
            this.miAddCardList,
            this.miRemoveCardList,
            this.miGenerateProxyPdf});
            this.contextMenuStripTreeView.Name = "contextMenuStrip1";
            this.contextMenuStripTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripTreeView_Opening);
            // 
            // miIncreaseCount
            // 
            resources.ApplyResources(this.miIncreaseCount, "miIncreaseCount");
            this.miIncreaseCount.Name = "miIncreaseCount";
            this.miIncreaseCount.Click += new System.EventHandler(this.MiIncreaseCount_Click);
            // 
            // miDecreaseCount
            // 
            resources.ApplyResources(this.miDecreaseCount, "miDecreaseCount");
            this.miDecreaseCount.Name = "miDecreaseCount";
            this.miDecreaseCount.Click += new System.EventHandler(this.MiDecreaseCount_Click);
            // 
            // miExportDeckToClipboard
            // 
            resources.ApplyResources(this.miExportDeckToClipboard, "miExportDeckToClipboard");
            this.miExportDeckToClipboard.Name = "miExportDeckToClipboard";
            this.miExportDeckToClipboard.Click += new System.EventHandler(this.ExportDeckToClipboardToolStripMenuItem_Click);
            // 
            // miAddCardList
            // 
            resources.ApplyResources(this.miAddCardList, "miAddCardList");
            this.miAddCardList.Name = "miAddCardList";
            this.miAddCardList.Click += new System.EventHandler(this.MiAddCardList_Click);
            // 
            // miRemoveCardList
            // 
            resources.ApplyResources(this.miRemoveCardList, "miRemoveCardList");
            this.miRemoveCardList.Name = "miRemoveCardList";
            // 
            // miGenerateProxyPdf
            // 
            resources.ApplyResources(this.miGenerateProxyPdf, "miGenerateProxyPdf");
            this.miGenerateProxyPdf.Name = "miGenerateProxyPdf";
            this.miGenerateProxyPdf.Click += new System.EventHandler(this.MiGenerateProxyPdf_Click);
            // 
            // tabPageSideboard
            // 
            resources.ApplyResources(this.tabPageSideboard, "tabPageSideboard");
            this.tabPageSideboard.Controls.Add(this.treeViewSide);
            this.tabPageSideboard.Name = "tabPageSideboard";
            this.tabPageSideboard.UseVisualStyleBackColor = true;
            // 
            // treeViewSide
            // 
            resources.ApplyResources(this.treeViewSide, "treeViewSide");
            this.treeViewSide.Cards = null;
            this.treeViewSide.ContextMenuStrip = this.contextMenuStripTreeView;
            this.treeViewSide.Deck = null;
            this.treeViewSide.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewSide.HideSelection = false;
            this.treeViewSide.Name = "treeViewSide";
            this.treeViewSide.NodeInfo = null;
            this.treeViewSide.ShowNodeToolTips = true;
            this.treeViewSide.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeViewDeck_DrawNode);
            this.treeViewSide.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewDeck_AfterSelect);
            this.treeViewSide.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewDeck_NodeMouseClick);
            // 
            // tabControlLocalInfo
            // 
            resources.ApplyResources(this.tabControlLocalInfo, "tabControlLocalInfo");
            this.tabControlLocalInfo.Controls.Add(this.tabPageCardtext);
            this.tabControlLocalInfo.Name = "tabControlLocalInfo";
            this.tabControlLocalInfo.SelectedIndex = 0;
            // 
            // tabPageCardtext
            // 
            resources.ApplyResources(this.tabPageCardtext, "tabPageCardtext");
            this.tabPageCardtext.Controls.Add(this.splitCardText);
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
            resources.ApplyResources(this.splitCardText.Panel1, "splitCardText.Panel1");
            this.splitCardText.Panel1.Controls.Add(this.rtbCardText);
            // 
            // splitCardText.Panel2
            // 
            resources.ApplyResources(this.splitCardText.Panel2, "splitCardText.Panel2");
            this.splitCardText.Panel2.Controls.Add(this.cardPreviewControl);
            // 
            // rtbCardText
            // 
            resources.ApplyResources(this.rtbCardText, "rtbCardText");
            this.rtbCardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCardText.Name = "rtbCardText";
            // 
            // cardPreviewControl
            // 
            resources.ApplyResources(this.cardPreviewControl, "cardPreviewControl");
            this.cardPreviewControl.CardUniversalId = -1;
            this.cardPreviewControl.Name = "cardPreviewControl";
            this.cardPreviewControl.MouseCaptureChanged += new System.EventHandler(this.CardPreviewControl1_MouseCaptureChanged);
            this.cardPreviewControl.MouseEnter += new System.EventHandler(this.CardPreviewControl1_MouseEnter);
            this.cardPreviewControl.MouseLeave += new System.EventHandler(this.CardPreviewControl1_MouseLeave);
            // 
            // tabControlGlobalInfo
            // 
            resources.ApplyResources(this.tabControlGlobalInfo, "tabControlGlobalInfo");
            this.tabControlGlobalInfo.Controls.Add(this.tabPageDescription);
            this.tabControlGlobalInfo.Controls.Add(this.tabPageHistory);
            this.tabControlGlobalInfo.Controls.Add(this.tabPageStats);
            this.tabControlGlobalInfo.Name = "tabControlGlobalInfo";
            this.tabControlGlobalInfo.SelectedIndex = 0;
            // 
            // tabPageDescription
            // 
            resources.ApplyResources(this.tabPageDescription, "tabPageDescription");
            this.tabPageDescription.Controls.Add(this.rtbDescription);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // rtbDescription
            // 
            resources.ApplyResources(this.rtbDescription, "rtbDescription");
            this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDescription.Name = "rtbDescription";
            // 
            // tabPageHistory
            // 
            resources.ApplyResources(this.tabPageHistory, "tabPageHistory");
            this.tabPageHistory.Controls.Add(this.treeViewHistory);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.UseVisualStyleBackColor = true;
            // 
            // treeViewHistory
            // 
            resources.ApplyResources(this.treeViewHistory, "treeViewHistory");
            this.treeViewHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewHistory.HideSelection = false;
            this.treeViewHistory.Name = "treeViewHistory";
            this.treeViewHistory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewHistory_AfterSelect);
            // 
            // tabPageStats
            // 
            resources.ApplyResources(this.tabPageStats, "tabPageStats");
            this.tabPageStats.Controls.Add(this.rtbStatistics);
            this.tabPageStats.Name = "tabPageStats";
            this.tabPageStats.UseVisualStyleBackColor = true;
            // 
            // rtbStatistics
            // 
            resources.ApplyResources(this.rtbStatistics, "rtbStatistics");
            this.rtbStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            resources.ApplyResources(this.menuStripMain, "menuStripMain");
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.sdsToolStripMenuItem});
            this.menuStripMain.Name = "menuStripMain";
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadDeckToolStripMenuItem,
            this.saveDeckToolStripMenuItem,
            this.saveDeckAstoolStripMenuItem,
            this.newVersionToolStripMenuItem,
            this.printDeckToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // newToolStripMenuItem
            // 
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // loadDeckToolStripMenuItem
            // 
            resources.ApplyResources(this.loadDeckToolStripMenuItem, "loadDeckToolStripMenuItem");
            this.loadDeckToolStripMenuItem.Name = "loadDeckToolStripMenuItem";
            this.loadDeckToolStripMenuItem.Click += new System.EventHandler(this.LoadDeckToolStripMenuItem_Click);
            // 
            // saveDeckToolStripMenuItem
            // 
            resources.ApplyResources(this.saveDeckToolStripMenuItem, "saveDeckToolStripMenuItem");
            this.saveDeckToolStripMenuItem.Name = "saveDeckToolStripMenuItem";
            this.saveDeckToolStripMenuItem.Click += new System.EventHandler(this.SaveDeckToolStripMenuItem_Click);
            // 
            // saveDeckAstoolStripMenuItem
            // 
            resources.ApplyResources(this.saveDeckAstoolStripMenuItem, "saveDeckAstoolStripMenuItem");
            this.saveDeckAstoolStripMenuItem.Name = "saveDeckAstoolStripMenuItem";
            this.saveDeckAstoolStripMenuItem.Click += new System.EventHandler(this.SaveDeckAsToolStripMenuItem_Click);
            // 
            // newVersionToolStripMenuItem
            // 
            resources.ApplyResources(this.newVersionToolStripMenuItem, "newVersionToolStripMenuItem");
            this.newVersionToolStripMenuItem.Name = "newVersionToolStripMenuItem";
            this.newVersionToolStripMenuItem.Click += new System.EventHandler(this.NewVersionToolStripMenuItem_Click);
            // 
            // printDeckToolStripMenuItem
            // 
            resources.ApplyResources(this.printDeckToolStripMenuItem, "printDeckToolStripMenuItem");
            this.printDeckToolStripMenuItem.Name = "printDeckToolStripMenuItem";
            this.printDeckToolStripMenuItem.Click += new System.EventHandler(this.PrintDeckToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawSimulatorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            // 
            // drawSimulatorToolStripMenuItem
            // 
            resources.ApplyResources(this.drawSimulatorToolStripMenuItem, "drawSimulatorToolStripMenuItem");
            this.drawSimulatorToolStripMenuItem.Name = "drawSimulatorToolStripMenuItem";
            this.drawSimulatorToolStripMenuItem.Click += new System.EventHandler(this.DrawSimulatorToolStripMenuItem_Click);
            // 
            // sdsToolStripMenuItem
            // 
            resources.ApplyResources(this.sdsToolStripMenuItem, "sdsToolStripMenuItem");
            this.sdsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToClipboardToolStripMenuItem,
            this.exportDeckToClipboardSortedByToolStripMenuItem,
            this.exportDeckToOCTGNToolStripMenuItem,
            this.importDeckFromOCTGNToolStripMenuItem});
            this.sdsToolStripMenuItem.Name = "sdsToolStripMenuItem";
            // 
            // exportToClipboardToolStripMenuItem
            // 
            resources.ApplyResources(this.exportToClipboardToolStripMenuItem, "exportToClipboardToolStripMenuItem");
            this.exportToClipboardToolStripMenuItem.Name = "exportToClipboardToolStripMenuItem";
            this.exportToClipboardToolStripMenuItem.Click += new System.EventHandler(this.ExportToClipboardToolStripMenuItem_Click);
            // 
            // exportDeckToClipboardSortedByToolStripMenuItem
            // 
            resources.ApplyResources(this.exportDeckToClipboardSortedByToolStripMenuItem, "exportDeckToClipboardSortedByToolStripMenuItem");
            this.exportDeckToClipboardSortedByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportDeckToClipboardSortedBySetToolStripMenuItem});
            this.exportDeckToClipboardSortedByToolStripMenuItem.Name = "exportDeckToClipboardSortedByToolStripMenuItem";
            // 
            // exportDeckToClipboardSortedBySetToolStripMenuItem
            // 
            resources.ApplyResources(this.exportDeckToClipboardSortedBySetToolStripMenuItem, "exportDeckToClipboardSortedBySetToolStripMenuItem");
            this.exportDeckToClipboardSortedBySetToolStripMenuItem.Name = "exportDeckToClipboardSortedBySetToolStripMenuItem";
            this.exportDeckToClipboardSortedBySetToolStripMenuItem.Click += new System.EventHandler(this.ExportDeckToClipboardSortedBySetToolStripMenuItem_Click);
            // 
            // exportDeckToOCTGNToolStripMenuItem
            // 
            resources.ApplyResources(this.exportDeckToOCTGNToolStripMenuItem, "exportDeckToOCTGNToolStripMenuItem");
            this.exportDeckToOCTGNToolStripMenuItem.Name = "exportDeckToOCTGNToolStripMenuItem";
            this.exportDeckToOCTGNToolStripMenuItem.Click += new System.EventHandler(this.ExportDeckToOctgnToolStripMenuItem_Click);
            // 
            // importDeckFromOCTGNToolStripMenuItem
            // 
            resources.ApplyResources(this.importDeckFromOCTGNToolStripMenuItem, "importDeckFromOCTGNToolStripMenuItem");
            this.importDeckFromOCTGNToolStripMenuItem.Name = "importDeckFromOCTGNToolStripMenuItem";
            this.importDeckFromOCTGNToolStripMenuItem.Click += new System.EventHandler(this.ImportDeckFromOctgnToolStripMenuItem_Click);
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
            this.eclHouse.SelectedValueChanged += new System.EventHandler(this.EclHouse_SelectedValueChanged);
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
            this.eclAgenda.SelectedValueChanged += new System.EventHandler(this.EclAgenda_SelectedValueChanged);
            // 
            // rbJoust
            // 
            resources.ApplyResources(this.rbJoust, "rbJoust");
            this.rbJoust.Checked = true;
            this.rbJoust.Name = "rbJoust";
            this.rbJoust.TabStop = true;
            this.rbJoust.UseVisualStyleBackColor = true;
            this.rbJoust.CheckedChanged += new System.EventHandler(this.CbJoustMeleeChanged);
            // 
            // rbMelee
            // 
            resources.ApplyResources(this.rbMelee, "rbMelee");
            this.rbMelee.Name = "rbMelee";
            this.rbMelee.UseVisualStyleBackColor = true;
            // 
            // DeckBuilderForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbMelee);
            this.Controls.Add(this.rbJoust);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeckBuilderForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeckBuilderForm_FormClosed);
            this.Shown += new System.EventHandler(this.DeckBuilderForm_Shown);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlDecks.ResumeLayout(false);
            this.tabPageDeck.ResumeLayout(false);
            this.contextMenuStripTreeView.ResumeLayout(false);
            this.tabPageSideboard.ResumeLayout(false);
            this.tabControlLocalInfo.ResumeLayout(false);
            this.tabPageCardtext.ResumeLayout(false);
            this.splitCardText.Panel1.ResumeLayout(false);
            this.splitCardText.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCardText)).EndInit();
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
	private System.Windows.Forms.ToolStripMenuItem sdsToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem exportDeckToOCTGNToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importDeckFromOCTGNToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportDeckToClipboardSortedByToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportDeckToClipboardSortedBySetToolStripMenuItem;
    private System.Windows.Forms.RadioButton rbJoust;
    private System.Windows.Forms.RadioButton rbMelee;
	}
}