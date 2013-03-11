namespace AGoTDB.Forms
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
            this.eclKeyword = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclTrigger = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclExpansionSet = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.btnQuickFindNext = new System.Windows.Forms.Button();
            this.tbStrengthHigh = new System.Windows.Forms.TextBox();
            this.tbStrengthLow = new System.Windows.Forms.TextBox();
            this.tbClaimHigh = new System.Windows.Forms.TextBox();
            this.tbClaimLow = new System.Windows.Forms.TextBox();
            this.tbInitiativeHigh = new System.Windows.Forms.TextBox();
            this.tbInitiativeLow = new System.Windows.Forms.TextBox();
            this.tbGoldHigh = new System.Windows.Forms.TextBox();
            this.tbGoldLow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCardtext = new System.Windows.Forms.TextBox();
            this.eclCardtextCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.eclNameCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTraits = new System.Windows.Forms.TextBox();
            this.eclTraitCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.eclProvides = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclIcon = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclCardtype = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.picStrength = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.picClaim = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.picInit = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picGold = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.eclVirtue = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclHouse = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclMecanism = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.popupGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveSelectionToTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToANewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCardToDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCardToSideboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.Button();
            this.splitCardText = new System.Windows.Forms.SplitContainer();
            this.lblUniversalId = new System.Windows.Forms.Label();
            this.btnReportError = new System.Windows.Forms.Button();
            this.rtbCardDetails = new System.Windows.Forms.RichTextBox();
            this.cardPreviewControl = new AGoTDB.Forms.CardPreviewControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miLcgSetsOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deckBuilderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oCTGNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadOCTGNDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNewVersionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClaim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.popupGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCardText)).BeginInit();
            this.splitCardText.Panel1.SuspendLayout();
            this.splitCardText.Panel2.SuspendLayout();
            this.splitCardText.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.eclKeyword);
            this.splitContainer1.Panel1.Controls.Add(this.eclTrigger);
            this.splitContainer1.Panel1.Controls.Add(this.eclExpansionSet);
            this.splitContainer1.Panel1.Controls.Add(this.btnQuickFindNext);
            this.splitContainer1.Panel1.Controls.Add(this.tbStrengthHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbStrengthLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbClaimHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbClaimLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbInitiativeHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbInitiativeLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbGoldHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbGoldLow);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.tbCardtext);
            this.splitContainer1.Panel1.Controls.Add(this.eclCardtextCheck);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.tbName);
            this.splitContainer1.Panel1.Controls.Add(this.eclNameCheck);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tbTraits);
            this.splitContainer1.Panel1.Controls.Add(this.eclTraitCheck);
            this.splitContainer1.Panel1.Controls.Add(this.eclProvides);
            this.splitContainer1.Panel1.Controls.Add(this.eclIcon);
            this.splitContainer1.Panel1.Controls.Add(this.eclCardtype);
            this.splitContainer1.Panel1.Controls.Add(this.picStrength);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.picClaim);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.picInit);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.picGold);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.tbFind);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.eclVirtue);
            this.splitContainer1.Panel1.Controls.Add(this.eclHouse);
            this.splitContainer1.Panel1.Controls.Add(this.eclMecanism);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.btnReset);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.splitCardText);
            // 
            // eclKeyword
            // 
            this.eclKeyword.CheckOnClick = true;
            this.eclKeyword.Condensed = true;
            this.eclKeyword.CondensedMode = true;
            this.eclKeyword.FormattingEnabled = true;
            resources.ApplyResources(this.eclKeyword, "eclKeyword");
            this.eclKeyword.Name = "eclKeyword";
            this.eclKeyword.RollDownDelay = 250;
            this.eclKeyword.Summary = "Keywords";
            this.eclKeyword.ThreeState = true;
            this.eclKeyword.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclTrigger
            // 
            this.eclTrigger.CheckOnClick = true;
            this.eclTrigger.Condensed = true;
            this.eclTrigger.CondensedMode = true;
            this.eclTrigger.FormattingEnabled = true;
            resources.ApplyResources(this.eclTrigger, "eclTrigger");
            this.eclTrigger.Name = "eclTrigger";
            this.eclTrigger.RollDownDelay = 250;
            this.eclTrigger.Summary = "Trigger";
            this.eclTrigger.ThreeState = true;
            this.eclTrigger.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclExpansionSet
            // 
            this.eclExpansionSet.CheckOnClick = true;
            this.eclExpansionSet.Condensed = true;
            this.eclExpansionSet.CondensedMode = true;
            this.eclExpansionSet.FormattingEnabled = true;
            resources.ApplyResources(this.eclExpansionSet, "eclExpansionSet");
            this.eclExpansionSet.Name = "eclExpansionSet";
            this.eclExpansionSet.RollDownDelay = 250;
            this.eclExpansionSet.Summary = "Expansion set";
            this.eclExpansionSet.ThreeState = true;
            this.eclExpansionSet.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // btnQuickFindNext
            // 
            resources.ApplyResources(this.btnQuickFindNext, "btnQuickFindNext");
            this.btnQuickFindNext.FlatAppearance.BorderSize = 0;
            this.btnQuickFindNext.Name = "btnQuickFindNext";
            this.toolTip1.SetToolTip(this.btnQuickFindNext, resources.GetString("btnQuickFindNext.ToolTip"));
            this.btnQuickFindNext.UseVisualStyleBackColor = true;
            this.btnQuickFindNext.Click += new System.EventHandler(this.btnQuickFindNext_Click);
            // 
            // tbStrengthHigh
            // 
            resources.ApplyResources(this.tbStrengthHigh, "tbStrengthHigh");
            this.tbStrengthHigh.Name = "tbStrengthHigh";
            this.tbStrengthHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbStrengthHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbStrengthLow
            // 
            resources.ApplyResources(this.tbStrengthLow, "tbStrengthLow");
            this.tbStrengthLow.Name = "tbStrengthLow";
            this.tbStrengthLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbStrengthLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbClaimHigh
            // 
            resources.ApplyResources(this.tbClaimHigh, "tbClaimHigh");
            this.tbClaimHigh.Name = "tbClaimHigh";
            this.tbClaimHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbClaimHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbClaimLow
            // 
            resources.ApplyResources(this.tbClaimLow, "tbClaimLow");
            this.tbClaimLow.Name = "tbClaimLow";
            this.tbClaimLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbClaimLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbInitiativeHigh
            // 
            resources.ApplyResources(this.tbInitiativeHigh, "tbInitiativeHigh");
            this.tbInitiativeHigh.Name = "tbInitiativeHigh";
            this.tbInitiativeHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbInitiativeHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbInitiativeLow
            // 
            resources.ApplyResources(this.tbInitiativeLow, "tbInitiativeLow");
            this.tbInitiativeLow.Name = "tbInitiativeLow";
            this.tbInitiativeLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbInitiativeLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbGoldHigh
            // 
            resources.ApplyResources(this.tbGoldHigh, "tbGoldHigh");
            this.tbGoldHigh.Name = "tbGoldHigh";
            this.tbGoldHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbGoldHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbGoldLow
            // 
            resources.ApplyResources(this.tbGoldLow, "tbGoldLow");
            this.tbGoldLow.Name = "tbGoldLow";
            this.tbGoldLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbGoldLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbCardtext
            // 
            resources.ApplyResources(this.tbCardtext, "tbCardtext");
            this.tbCardtext.Name = "tbCardtext";
            this.tbCardtext.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // eclCardtextCheck
            // 
            resources.ApplyResources(this.eclCardtextCheck, "eclCardtextCheck");
            this.eclCardtextCheck.Name = "eclCardtextCheck";
            this.eclCardtextCheck.ThreeState = true;
            this.eclCardtextCheck.UseVisualStyleBackColor = true;
            this.eclCardtextCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbName
            // 
            resources.ApplyResources(this.tbName, "tbName");
            this.tbName.Name = "tbName";
            this.tbName.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // eclNameCheck
            // 
            resources.ApplyResources(this.eclNameCheck, "eclNameCheck");
            this.eclNameCheck.Name = "eclNameCheck";
            this.eclNameCheck.ThreeState = true;
            this.eclNameCheck.UseVisualStyleBackColor = true;
            this.eclNameCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbTraits
            // 
            resources.ApplyResources(this.tbTraits, "tbTraits");
            this.tbTraits.Name = "tbTraits";
            this.tbTraits.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // eclTraitCheck
            // 
            resources.ApplyResources(this.eclTraitCheck, "eclTraitCheck");
            this.eclTraitCheck.Name = "eclTraitCheck";
            this.eclTraitCheck.ThreeState = true;
            this.eclTraitCheck.UseVisualStyleBackColor = true;
            this.eclTraitCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
            // 
            // eclProvides
            // 
            this.eclProvides.CheckOnClick = true;
            this.eclProvides.Condensed = true;
            this.eclProvides.CondensedMode = true;
            this.eclProvides.FormattingEnabled = true;
            resources.ApplyResources(this.eclProvides, "eclProvides");
            this.eclProvides.Name = "eclProvides";
            this.eclProvides.RollDownDelay = 250;
            this.eclProvides.Summary = "Provides...";
            this.eclProvides.ThreeState = true;
            this.eclProvides.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclIcon
            // 
            this.eclIcon.CheckOnClick = true;
            this.eclIcon.Condensed = true;
            this.eclIcon.CondensedMode = true;
            this.eclIcon.FormattingEnabled = true;
            resources.ApplyResources(this.eclIcon, "eclIcon");
            this.eclIcon.Name = "eclIcon";
            this.eclIcon.RollDownDelay = 250;
            this.eclIcon.Summary = "Icons";
            this.eclIcon.ThreeState = true;
            this.eclIcon.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclCardtype
            // 
            this.eclCardtype.CheckOnClick = true;
            this.eclCardtype.Condensed = true;
            this.eclCardtype.CondensedMode = true;
            this.eclCardtype.FormattingEnabled = true;
            resources.ApplyResources(this.eclCardtype, "eclCardtype");
            this.eclCardtype.Name = "eclCardtype";
            this.eclCardtype.RollDownDelay = 250;
            this.eclCardtype.Summary = "Card type";
            this.eclCardtype.ThreeState = true;
            this.eclCardtype.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // picStrength
            // 
            resources.ApplyResources(this.picStrength, "picStrength");
            this.picStrength.Name = "picStrength";
            this.picStrength.TabStop = false;
            this.toolTip1.SetToolTip(this.picStrength, resources.GetString("picStrength.ToolTip"));
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // picClaim
            // 
            resources.ApplyResources(this.picClaim, "picClaim");
            this.picClaim.Name = "picClaim";
            this.picClaim.TabStop = false;
            this.toolTip1.SetToolTip(this.picClaim, resources.GetString("picClaim.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // picInit
            // 
            resources.ApplyResources(this.picInit, "picInit");
            this.picInit.Name = "picInit";
            this.picInit.TabStop = false;
            this.toolTip1.SetToolTip(this.picInit, resources.GetString("picInit.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // picGold
            // 
            resources.ApplyResources(this.picGold, "picGold");
            this.picGold.Name = "picGold";
            this.picGold.TabStop = false;
            this.toolTip1.SetToolTip(this.picGold, resources.GetString("picGold.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbFind
            // 
            resources.ApplyResources(this.tbFind, "tbFind");
            this.tbFind.Name = "tbFind";
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // eclVirtue
            // 
            this.eclVirtue.CheckOnClick = true;
            this.eclVirtue.Condensed = true;
            this.eclVirtue.CondensedMode = true;
            this.eclVirtue.FormattingEnabled = true;
            resources.ApplyResources(this.eclVirtue, "eclVirtue");
            this.eclVirtue.Name = "eclVirtue";
            this.eclVirtue.RollDownDelay = 250;
            this.eclVirtue.Summary = "Virtues";
            this.eclVirtue.ThreeState = true;
            this.eclVirtue.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclHouse
            // 
            this.eclHouse.CheckOnClick = true;
            this.eclHouse.Condensed = true;
            this.eclHouse.CondensedMode = true;
            this.eclHouse.FormattingEnabled = true;
            resources.ApplyResources(this.eclHouse, "eclHouse");
            this.eclHouse.Name = "eclHouse";
            this.eclHouse.RollDownDelay = 250;
            this.eclHouse.Summary = "House";
            this.eclHouse.ThreeState = true;
            this.eclHouse.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclMecanism
            // 
            this.eclMecanism.CheckOnClick = true;
            this.eclMecanism.Condensed = true;
            this.eclMecanism.CondensedMode = true;
            this.eclMecanism.FormattingEnabled = true;
            resources.ApplyResources(this.eclMecanism, "eclMecanism");
            this.eclMecanism.Name = "eclMecanism";
            this.eclMecanism.RollDownDelay = 250;
            this.eclMecanism.Summary = "Mecanism";
            this.eclMecanism.ThreeState = true;
            this.eclMecanism.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridView, "dataGridView");
            this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ContextMenuStrip = this.popupGrid;
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView.DoubleClick += new System.EventHandler(this.dataGridView_DoubleClick);
            // 
            // popupGrid
            // 
            this.popupGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSelectionToTextFileToolStripMenuItem,
            this.moveToANewWindowToolStripMenuItem,
            this.addCardToDeckToolStripMenuItem,
            this.addCardToSideboardToolStripMenuItem});
            this.popupGrid.Name = "popupGrid";
            resources.ApplyResources(this.popupGrid, "popupGrid");
            // 
            // saveSelectionToTextFileToolStripMenuItem
            // 
            this.saveSelectionToTextFileToolStripMenuItem.Name = "saveSelectionToTextFileToolStripMenuItem";
            resources.ApplyResources(this.saveSelectionToTextFileToolStripMenuItem, "saveSelectionToTextFileToolStripMenuItem");
            this.saveSelectionToTextFileToolStripMenuItem.Click += new System.EventHandler(this.saveSelectionToTextFileToolStripMenuItem_Click);
            // 
            // moveToANewWindowToolStripMenuItem
            // 
            this.moveToANewWindowToolStripMenuItem.Name = "moveToANewWindowToolStripMenuItem";
            resources.ApplyResources(this.moveToANewWindowToolStripMenuItem, "moveToANewWindowToolStripMenuItem");
            this.moveToANewWindowToolStripMenuItem.Click += new System.EventHandler(this.moveToANewWindowToolStripMenuItem_Click);
            // 
            // addCardToDeckToolStripMenuItem
            // 
            this.addCardToDeckToolStripMenuItem.Name = "addCardToDeckToolStripMenuItem";
            resources.ApplyResources(this.addCardToDeckToolStripMenuItem, "addCardToDeckToolStripMenuItem");
            this.addCardToDeckToolStripMenuItem.Click += new System.EventHandler(this.addCardToDeckToolStripMenuItem_Click);
            // 
            // addCardToSideboardToolStripMenuItem
            // 
            this.addCardToSideboardToolStripMenuItem.Name = "addCardToSideboardToolStripMenuItem";
            resources.ApplyResources(this.addCardToSideboardToolStripMenuItem, "addCardToSideboardToolStripMenuItem");
            this.addCardToSideboardToolStripMenuItem.Click += new System.EventHandler(this.addCardToSideboardToolStripMenuItem_Click);
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // splitCardText
            // 
            this.splitCardText.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.splitCardText, "splitCardText");
            this.splitCardText.Name = "splitCardText";
            // 
            // splitCardText.Panel1
            // 
            this.splitCardText.Panel1.Controls.Add(this.lblUniversalId);
            this.splitCardText.Panel1.Controls.Add(this.btnReportError);
            this.splitCardText.Panel1.Controls.Add(this.rtbCardDetails);
            // 
            // splitCardText.Panel2
            // 
            this.splitCardText.Panel2.Controls.Add(this.cardPreviewControl);
            // 
            // lblUniversalId
            // 
            resources.ApplyResources(this.lblUniversalId, "lblUniversalId");
            this.lblUniversalId.Name = "lblUniversalId";
            this.lblUniversalId.Click += new System.EventHandler(this.lblUniversalId_Click);
            // 
            // btnReportError
            // 
            resources.ApplyResources(this.btnReportError, "btnReportError");
            this.btnReportError.Name = "btnReportError";
            this.btnReportError.UseVisualStyleBackColor = true;
            this.btnReportError.Click += new System.EventHandler(this.btnReportError_Click);
            // 
            // rtbCardDetails
            // 
            resources.ApplyResources(this.rtbCardDetails, "rtbCardDetails");
            this.rtbCardDetails.Name = "rtbCardDetails";
            // 
            // cardPreviewControl
            // 
            this.cardPreviewControl.BackColor = System.Drawing.Color.Transparent;
            this.cardPreviewControl.CardUniversalId = -1;
            resources.ApplyResources(this.cardPreviewControl, "cardPreviewControl");
            this.cardPreviewControl.Name = "cardPreviewControl";
            this.cardPreviewControl.MouseCaptureChanged += new System.EventHandler(this.cardPreviewControl_MouseCaptureChanged);
            this.cardPreviewControl.MouseEnter += new System.EventHandler(this.cardPreviewControl_MouseEnter);
            this.cardPreviewControl.MouseLeave += new System.EventHandler(this.cardPreviewControl_MouseLeave);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.oCTGNToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLcgSetsOnly});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            resources.ApplyResources(this.filterToolStripMenuItem, "filterToolStripMenuItem");
            // 
            // miLcgSetsOnly
            // 
            this.miLcgSetsOnly.CheckOnClick = true;
            this.miLcgSetsOnly.Name = "miLcgSetsOnly";
            resources.ApplyResources(this.miLcgSetsOnly, "miLcgSetsOnly");
            this.miLcgSetsOnly.CheckedChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
            this.miLcgSetsOnly.Click += new System.EventHandler(this.miLcgSetsOnly_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deckBuilderToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // deckBuilderToolStripMenuItem
            // 
            this.deckBuilderToolStripMenuItem.Name = "deckBuilderToolStripMenuItem";
            resources.ApplyResources(this.deckBuilderToolStripMenuItem, "deckBuilderToolStripMenuItem");
            this.deckBuilderToolStripMenuItem.Click += new System.EventHandler(this.deckBuilderToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // oCTGNToolStripMenuItem
            // 
            this.oCTGNToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadOCTGNDataToolStripMenuItem});
            this.oCTGNToolStripMenuItem.Name = "oCTGNToolStripMenuItem";
            resources.ApplyResources(this.oCTGNToolStripMenuItem, "oCTGNToolStripMenuItem");
            // 
            // loadOCTGNDataToolStripMenuItem
            // 
            this.loadOCTGNDataToolStripMenuItem.Name = "loadOCTGNDataToolStripMenuItem";
            resources.ApplyResources(this.loadOCTGNDataToolStripMenuItem, "loadOCTGNDataToolStripMenuItem");
            this.loadOCTGNDataToolStripMenuItem.Click += new System.EventHandler(this.loadOCTGNDataToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClaim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.popupGrid.ResumeLayout(false);
            this.splitCardText.Panel1.ResumeLayout(false);
            this.splitCardText.Panel1.PerformLayout();
            this.splitCardText.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCardText)).EndInit();
            this.splitCardText.ResumeLayout(false);
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
		private System.Windows.Forms.TextBox tbTraits;
		private Beyond.ExtendedControls.ExtendedCheckBox eclTraitCheck;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclProvides;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclKeyword;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclIcon;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclCardtype;
		private System.Windows.Forms.PictureBox picStrength;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.PictureBox picClaim;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.PictureBox picInit;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbFind;
		private System.Windows.Forms.Label label11;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclVirtue;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclHouse;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclMecanism;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclTrigger;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclExpansionSet;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.TextBox tbGoldLow;
		private System.Windows.Forms.TextBox tbInitiativeHigh;
		private System.Windows.Forms.TextBox tbInitiativeLow;
		private System.Windows.Forms.TextBox tbGoldHigh;
		private System.Windows.Forms.TextBox tbStrengthLow;
		private System.Windows.Forms.TextBox tbClaimHigh;
		private System.Windows.Forms.TextBox tbClaimLow;
		private System.Windows.Forms.TextBox tbStrengthHigh;
		private System.Windows.Forms.ContextMenuStrip popupGrid;
		private System.Windows.Forms.ToolStripMenuItem saveSelectionToTextFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveToANewWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
		private System.Windows.Forms.PictureBox picGold;
		private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deckBuilderToolStripMenuItem;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem addCardToDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addCardToSideboardToolStripMenuItem;
		private System.Windows.Forms.Button btnQuickFindNext;
		private System.Windows.Forms.ToolStripMenuItem miLcgSetsOnly;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.ComponentModel.BackgroundWorker checkNewVersionBackgroundWorker;
		private System.Windows.Forms.SplitContainer splitCardText;
		private System.Windows.Forms.Button btnReportError;
		private CardPreviewControl cardPreviewControl;
        private System.Windows.Forms.Label lblUniversalId;
        private System.Windows.Forms.ToolStripMenuItem oCTGNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadOCTGNDataToolStripMenuItem;
	}
}
