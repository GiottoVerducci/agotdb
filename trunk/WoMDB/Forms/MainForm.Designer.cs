namespace WoMDB.Forms
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnQuickFindNext = new System.Windows.Forms.Button();
			this.tbPowerHigh = new System.Windows.Forms.TextBox();
			this.tbPowerLow = new System.Windows.Forms.TextBox();
			this.eclExpansionSet = new Beyond.ExtendedControls.ExtendedCheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbCardtext = new System.Windows.Forms.TextBox();
			this.eclCardtextCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.eclNameCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbTeam = new System.Windows.Forms.TextBox();
			this.eclTeamCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
			this.eclCardtype = new Beyond.ExtendedControls.ExtendedCheckedListBox();
			this.picPower = new System.Windows.Forms.PictureBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbFind = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.eclColor = new Beyond.ExtendedControls.ExtendedCheckedListBox();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.popupGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.saveSelectionToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveToANewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addCardToDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addCardToSideboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnReset = new System.Windows.Forms.Button();
			this.rtbCardDetails = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deckBuilderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPower)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.popupGrid.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.btnQuickFindNext);
			this.splitContainer1.Panel1.Controls.Add(this.tbPowerHigh);
			this.splitContainer1.Panel1.Controls.Add(this.tbPowerLow);
			this.splitContainer1.Panel1.Controls.Add(this.eclExpansionSet);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.tbCardtext);
			this.splitContainer1.Panel1.Controls.Add(this.eclCardtextCheck);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.tbName);
			this.splitContainer1.Panel1.Controls.Add(this.eclNameCheck);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.tbTeam);
			this.splitContainer1.Panel1.Controls.Add(this.eclTeamCheck);
			this.splitContainer1.Panel1.Controls.Add(this.eclCardtype);
			this.splitContainer1.Panel1.Controls.Add(this.picPower);
			this.splitContainer1.Panel1.Controls.Add(this.label8);
			this.splitContainer1.Panel1.Controls.Add(this.tbFind);
			this.splitContainer1.Panel1.Controls.Add(this.label11);
			this.splitContainer1.Panel1.Controls.Add(this.eclColor);
			this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
			this.splitContainer1.Panel1.Controls.Add(this.btnReset);
			this.splitContainer1.Panel1.Font = null;
			this.toolTip1.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AccessibleDescription = null;
			this.splitContainer1.Panel2.AccessibleName = null;
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.BackgroundImage = null;
			this.splitContainer1.Panel2.Controls.Add(this.rtbCardDetails);
			this.splitContainer1.Panel2.Font = null;
			this.toolTip1.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
			this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
			// 
			// btnQuickFindNext
			// 
			this.btnQuickFindNext.AccessibleDescription = null;
			this.btnQuickFindNext.AccessibleName = null;
			resources.ApplyResources(this.btnQuickFindNext, "btnQuickFindNext");
			this.btnQuickFindNext.BackgroundImage = null;
			this.btnQuickFindNext.FlatAppearance.BorderSize = 0;
			this.btnQuickFindNext.Name = "btnQuickFindNext";
			this.toolTip1.SetToolTip(this.btnQuickFindNext, resources.GetString("btnQuickFindNext.ToolTip"));
			this.btnQuickFindNext.UseVisualStyleBackColor = true;
			this.btnQuickFindNext.Click += new System.EventHandler(this.btnQuickFindNext_Click);
			// 
			// tbPowerHigh
			// 
			this.tbPowerHigh.AccessibleDescription = null;
			this.tbPowerHigh.AccessibleName = null;
			resources.ApplyResources(this.tbPowerHigh, "tbPowerHigh");
			this.tbPowerHigh.BackgroundImage = null;
			this.tbPowerHigh.Font = null;
			this.tbPowerHigh.Name = "tbPowerHigh";
			this.toolTip1.SetToolTip(this.tbPowerHigh, resources.GetString("tbPowerHigh.ToolTip"));
			this.tbPowerHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
			this.tbPowerHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
			// 
			// tbPowerLow
			// 
			this.tbPowerLow.AccessibleDescription = null;
			this.tbPowerLow.AccessibleName = null;
			resources.ApplyResources(this.tbPowerLow, "tbPowerLow");
			this.tbPowerLow.BackgroundImage = null;
			this.tbPowerLow.Font = null;
			this.tbPowerLow.Name = "tbPowerLow";
			this.toolTip1.SetToolTip(this.tbPowerLow, resources.GetString("tbPowerLow.ToolTip"));
			this.tbPowerLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
			this.tbPowerLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
			// 
			// eclExpansionSet
			// 
			this.eclExpansionSet.AccessibleDescription = null;
			this.eclExpansionSet.AccessibleName = null;
			resources.ApplyResources(this.eclExpansionSet, "eclExpansionSet");
			this.eclExpansionSet.BackgroundImage = null;
			this.eclExpansionSet.CheckOnClick = true;
			this.eclExpansionSet.Condensed = true;
			this.eclExpansionSet.CondensedMode = true;
			this.eclExpansionSet.Font = null;
			this.eclExpansionSet.FormattingEnabled = true;
			this.eclExpansionSet.Name = "eclExpansionSet";
			this.eclExpansionSet.RollDownDelay = 250;
			this.eclExpansionSet.Summary = "Expansion set";
			this.eclExpansionSet.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclExpansionSet, resources.GetString("eclExpansionSet.ToolTip"));
			this.eclExpansionSet.MouseLeave += new System.EventHandler(this.eclMouseLeave);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			// 
			// tbCardtext
			// 
			this.tbCardtext.AccessibleDescription = null;
			this.tbCardtext.AccessibleName = null;
			resources.ApplyResources(this.tbCardtext, "tbCardtext");
			this.tbCardtext.BackgroundImage = null;
			this.tbCardtext.Font = null;
			this.tbCardtext.Name = "tbCardtext";
			this.toolTip1.SetToolTip(this.tbCardtext, resources.GetString("tbCardtext.ToolTip"));
			this.tbCardtext.Validated += new System.EventHandler(this.tbLowHigh_Validated);
			// 
			// eclCardtextCheck
			// 
			this.eclCardtextCheck.AccessibleDescription = null;
			this.eclCardtextCheck.AccessibleName = null;
			resources.ApplyResources(this.eclCardtextCheck, "eclCardtextCheck");
			this.eclCardtextCheck.BackgroundImage = null;
			this.eclCardtextCheck.Font = null;
			this.eclCardtextCheck.Name = "eclCardtextCheck";
			this.eclCardtextCheck.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclCardtextCheck, resources.GetString("eclCardtextCheck.ToolTip"));
			this.eclCardtextCheck.UseVisualStyleBackColor = true;
			this.eclCardtextCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
			// 
			// label3
			// 
			this.label3.AccessibleDescription = null;
			this.label3.AccessibleName = null;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Font = null;
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
			// 
			// tbName
			// 
			this.tbName.AccessibleDescription = null;
			this.tbName.AccessibleName = null;
			resources.ApplyResources(this.tbName, "tbName");
			this.tbName.BackgroundImage = null;
			this.tbName.Font = null;
			this.tbName.Name = "tbName";
			this.toolTip1.SetToolTip(this.tbName, resources.GetString("tbName.ToolTip"));
			this.tbName.Validated += new System.EventHandler(this.tbLowHigh_Validated);
			// 
			// eclNameCheck
			// 
			this.eclNameCheck.AccessibleDescription = null;
			this.eclNameCheck.AccessibleName = null;
			resources.ApplyResources(this.eclNameCheck, "eclNameCheck");
			this.eclNameCheck.BackgroundImage = null;
			this.eclNameCheck.Font = null;
			this.eclNameCheck.Name = "eclNameCheck";
			this.eclNameCheck.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclNameCheck, resources.GetString("eclNameCheck.ToolTip"));
			this.eclNameCheck.UseVisualStyleBackColor = true;
			this.eclNameCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
			// 
			// tbTeam
			// 
			this.tbTeam.AccessibleDescription = null;
			this.tbTeam.AccessibleName = null;
			resources.ApplyResources(this.tbTeam, "tbTeam");
			this.tbTeam.BackgroundImage = null;
			this.tbTeam.Font = null;
			this.tbTeam.Name = "tbTeam";
			this.toolTip1.SetToolTip(this.tbTeam, resources.GetString("tbTeam.ToolTip"));
			this.tbTeam.Validated += new System.EventHandler(this.tbLowHigh_Validated);
			// 
			// eclTeamCheck
			// 
			this.eclTeamCheck.AccessibleDescription = null;
			this.eclTeamCheck.AccessibleName = null;
			resources.ApplyResources(this.eclTeamCheck, "eclTeamCheck");
			this.eclTeamCheck.BackgroundImage = null;
			this.eclTeamCheck.Font = null;
			this.eclTeamCheck.Name = "eclTeamCheck";
			this.eclTeamCheck.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclTeamCheck, resources.GetString("eclTeamCheck.ToolTip"));
			this.eclTeamCheck.UseVisualStyleBackColor = true;
			this.eclTeamCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
			// 
			// eclCardtype
			// 
			this.eclCardtype.AccessibleDescription = null;
			this.eclCardtype.AccessibleName = null;
			resources.ApplyResources(this.eclCardtype, "eclCardtype");
			this.eclCardtype.BackgroundImage = null;
			this.eclCardtype.CheckOnClick = true;
			this.eclCardtype.Condensed = true;
			this.eclCardtype.CondensedMode = true;
			this.eclCardtype.Font = null;
			this.eclCardtype.FormattingEnabled = true;
			this.eclCardtype.Name = "eclCardtype";
			this.eclCardtype.RollDownDelay = 250;
			this.eclCardtype.Summary = "Card type";
			this.eclCardtype.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclCardtype, resources.GetString("eclCardtype.ToolTip"));
			this.eclCardtype.MouseLeave += new System.EventHandler(this.eclMouseLeave);
			// 
			// picPower
			// 
			this.picPower.AccessibleDescription = null;
			this.picPower.AccessibleName = null;
			resources.ApplyResources(this.picPower, "picPower");
			this.picPower.BackgroundImage = null;
			this.picPower.Font = null;
			this.picPower.ImageLocation = null;
			this.picPower.Name = "picPower";
			this.picPower.TabStop = false;
			this.toolTip1.SetToolTip(this.picPower, resources.GetString("picPower.ToolTip"));
			// 
			// label8
			// 
			this.label8.AccessibleDescription = null;
			this.label8.AccessibleName = null;
			resources.ApplyResources(this.label8, "label8");
			this.label8.Font = null;
			this.label8.Name = "label8";
			this.toolTip1.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
			// 
			// tbFind
			// 
			this.tbFind.AccessibleDescription = null;
			this.tbFind.AccessibleName = null;
			resources.ApplyResources(this.tbFind, "tbFind");
			this.tbFind.BackgroundImage = null;
			this.tbFind.Font = null;
			this.tbFind.Name = "tbFind";
			this.toolTip1.SetToolTip(this.tbFind, resources.GetString("tbFind.ToolTip"));
			this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
			// 
			// label11
			// 
			this.label11.AccessibleDescription = null;
			this.label11.AccessibleName = null;
			resources.ApplyResources(this.label11, "label11");
			this.label11.Font = null;
			this.label11.Name = "label11";
			this.toolTip1.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
			// 
			// eclColor
			// 
			this.eclColor.AccessibleDescription = null;
			this.eclColor.AccessibleName = null;
			resources.ApplyResources(this.eclColor, "eclColor");
			this.eclColor.BackgroundImage = null;
			this.eclColor.CheckOnClick = true;
			this.eclColor.Condensed = true;
			this.eclColor.CondensedMode = true;
			this.eclColor.Font = null;
			this.eclColor.FormattingEnabled = true;
			this.eclColor.Name = "eclColor";
			this.eclColor.RollDownDelay = 250;
			this.eclColor.Summary = "Color";
			this.eclColor.ThreeState = true;
			this.toolTip1.SetToolTip(this.eclColor, resources.GetString("eclColor.ToolTip"));
			this.eclColor.MouseLeave += new System.EventHandler(this.eclMouseLeave);
			// 
			// dataGridView
			// 
			this.dataGridView.AccessibleDescription = null;
			this.dataGridView.AccessibleName = null;
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToResizeRows = false;
			resources.ApplyResources(this.dataGridView, "dataGridView");
			this.dataGridView.BackgroundImage = null;
			this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.ContextMenuStrip = this.popupGrid;
			this.dataGridView.Font = null;
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dataGridView.RowHeadersVisible = false;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.toolTip1.SetToolTip(this.dataGridView, resources.GetString("dataGridView.ToolTip"));
			this.dataGridView.DoubleClick += new System.EventHandler(this.dataGridView_DoubleClick);
			this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
			// 
			// popupGrid
			// 
			this.popupGrid.AccessibleDescription = null;
			this.popupGrid.AccessibleName = null;
			resources.ApplyResources(this.popupGrid, "popupGrid");
			this.popupGrid.BackgroundImage = null;
			this.popupGrid.Font = null;
			this.popupGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.saveSelectionToTextFileToolStripMenuItem,
				this.moveToANewWindowToolStripMenuItem,
				this.addCardToDeckToolStripMenuItem,
				this.addCardToSideboardToolStripMenuItem});
			this.popupGrid.Name = "popupGrid";
			this.toolTip1.SetToolTip(this.popupGrid, resources.GetString("popupGrid.ToolTip"));
			// 
			// saveSelectionToTextFileToolStripMenuItem
			// 
			this.saveSelectionToTextFileToolStripMenuItem.AccessibleDescription = null;
			this.saveSelectionToTextFileToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.saveSelectionToTextFileToolStripMenuItem, "saveSelectionToTextFileToolStripMenuItem");
			this.saveSelectionToTextFileToolStripMenuItem.BackgroundImage = null;
			this.saveSelectionToTextFileToolStripMenuItem.Name = "saveSelectionToTextFileToolStripMenuItem";
			this.saveSelectionToTextFileToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.saveSelectionToTextFileToolStripMenuItem.Click += new System.EventHandler(this.saveSelectionToTextFileToolStripMenuItem_Click);
			// 
			// moveToANewWindowToolStripMenuItem
			// 
			this.moveToANewWindowToolStripMenuItem.AccessibleDescription = null;
			this.moveToANewWindowToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.moveToANewWindowToolStripMenuItem, "moveToANewWindowToolStripMenuItem");
			this.moveToANewWindowToolStripMenuItem.BackgroundImage = null;
			this.moveToANewWindowToolStripMenuItem.Name = "moveToANewWindowToolStripMenuItem";
			this.moveToANewWindowToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.moveToANewWindowToolStripMenuItem.Click += new System.EventHandler(this.moveToANewWindowToolStripMenuItem_Click);
			// 
			// addCardToDeckToolStripMenuItem
			// 
			this.addCardToDeckToolStripMenuItem.AccessibleDescription = null;
			this.addCardToDeckToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.addCardToDeckToolStripMenuItem, "addCardToDeckToolStripMenuItem");
			this.addCardToDeckToolStripMenuItem.BackgroundImage = null;
			this.addCardToDeckToolStripMenuItem.Name = "addCardToDeckToolStripMenuItem";
			this.addCardToDeckToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.addCardToDeckToolStripMenuItem.Click += new System.EventHandler(this.addCardToDeckToolStripMenuItem_Click);
			// 
			// addCardToSideboardToolStripMenuItem
			// 
			this.addCardToSideboardToolStripMenuItem.AccessibleDescription = null;
			this.addCardToSideboardToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.addCardToSideboardToolStripMenuItem, "addCardToSideboardToolStripMenuItem");
			this.addCardToSideboardToolStripMenuItem.BackgroundImage = null;
			this.addCardToSideboardToolStripMenuItem.Name = "addCardToSideboardToolStripMenuItem";
			this.addCardToSideboardToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.addCardToSideboardToolStripMenuItem.Click += new System.EventHandler(this.addCardToSideboardToolStripMenuItem_Click);
			// 
			// btnReset
			// 
			this.btnReset.AccessibleDescription = null;
			this.btnReset.AccessibleName = null;
			resources.ApplyResources(this.btnReset, "btnReset");
			this.btnReset.BackgroundImage = null;
			this.btnReset.Font = null;
			this.btnReset.Name = "btnReset";
			this.toolTip1.SetToolTip(this.btnReset, resources.GetString("btnReset.ToolTip"));
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// rtbCardDetails
			// 
			this.rtbCardDetails.AccessibleDescription = null;
			this.rtbCardDetails.AccessibleName = null;
			resources.ApplyResources(this.rtbCardDetails, "rtbCardDetails");
			this.rtbCardDetails.BackgroundImage = null;
			this.rtbCardDetails.Font = null;
			this.rtbCardDetails.Name = "rtbCardDetails";
			this.toolTip1.SetToolTip(this.rtbCardDetails, resources.GetString("rtbCardDetails.ToolTip"));
			// 
			// menuStrip1
			// 
			this.menuStrip1.AccessibleDescription = null;
			this.menuStrip1.AccessibleName = null;
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.BackgroundImage = null;
			this.menuStrip1.Font = null;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.fileToolStripMenuItem,
				this.filterToolStripMenuItem,
				this.toolsToolStripMenuItem,
				this.windowToolStripMenuItem,
				this.helpToolStripMenuItem});
			this.menuStrip1.Name = "menuStrip1";
			this.toolTip1.SetToolTip(this.menuStrip1, resources.GetString("menuStrip1.ToolTip"));
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.AccessibleDescription = null;
			this.fileToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			this.fileToolStripMenuItem.BackgroundImage = null;
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.toolStripMenuItem1,
				this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.ShortcutKeyDisplayString = null;
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.AccessibleDescription = null;
			this.toolStripMenuItem1.AccessibleName = null;
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
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
			// filterToolStripMenuItem
			// 
			this.filterToolStripMenuItem.AccessibleDescription = null;
			this.filterToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.filterToolStripMenuItem, "filterToolStripMenuItem");
			this.filterToolStripMenuItem.BackgroundImage = null;
			this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
			this.filterToolStripMenuItem.ShortcutKeyDisplayString = null;
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.AccessibleDescription = null;
			this.toolsToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			this.toolsToolStripMenuItem.BackgroundImage = null;
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.deckBuilderToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.ShortcutKeyDisplayString = null;
			// 
			// deckBuilderToolStripMenuItem
			// 
			this.deckBuilderToolStripMenuItem.AccessibleDescription = null;
			this.deckBuilderToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.deckBuilderToolStripMenuItem, "deckBuilderToolStripMenuItem");
			this.deckBuilderToolStripMenuItem.BackgroundImage = null;
			this.deckBuilderToolStripMenuItem.Name = "deckBuilderToolStripMenuItem";
			this.deckBuilderToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.deckBuilderToolStripMenuItem.Click += new System.EventHandler(this.deckBuilderToolStripMenuItem_Click);
			// 
			// windowToolStripMenuItem
			// 
			this.windowToolStripMenuItem.AccessibleDescription = null;
			this.windowToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
			this.windowToolStripMenuItem.BackgroundImage = null;
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			this.windowToolStripMenuItem.ShortcutKeyDisplayString = null;
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.AccessibleDescription = null;
			this.helpToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			this.helpToolStripMenuItem.BackgroundImage = null;
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.ShortcutKeyDisplayString = null;
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.AccessibleDescription = null;
			this.aboutToolStripMenuItem.AccessibleName = null;
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.BackgroundImage = null;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.ShortcutKeyDisplayString = null;
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.Font = null;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPower)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.popupGrid.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.RichTextBox rtbCardDetails;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbCardtext;
		private Beyond.ExtendedControls.ExtendedCheckBox eclCardtextCheck;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbName;
		private Beyond.ExtendedControls.ExtendedCheckBox eclNameCheck;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbTeam;
		private Beyond.ExtendedControls.ExtendedCheckBox eclTeamCheck;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclCardtype;
		private System.Windows.Forms.PictureBox picPower;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox tbFind;
		private System.Windows.Forms.Label label11;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclColor;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclExpansionSet;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.TextBox tbPowerLow;
		private System.Windows.Forms.TextBox tbPowerHigh;
		private System.Windows.Forms.ContextMenuStrip popupGrid;
		private System.Windows.Forms.ToolStripMenuItem saveSelectionToTextFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveToANewWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deckBuilderToolStripMenuItem;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem addCardToDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addCardToSideboardToolStripMenuItem;
		private System.Windows.Forms.Button btnQuickFindNext;
	}
}