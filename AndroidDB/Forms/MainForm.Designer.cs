namespace NRADB.Forms
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
            this.label13 = new System.Windows.Forms.Label();
            this.tbInfluenceHigh = new System.Windows.Forms.TextBox();
            this.tbInfluenceLow = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.rbRunnerOnly = new System.Windows.Forms.RadioButton();
            this.rbCorpOnly = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.tbTrashCostHigh = new System.Windows.Forms.TextBox();
            this.tbTrashCostLow = new System.Windows.Forms.TextBox();
            this.picTrashCost = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbLinkHigh = new System.Windows.Forms.TextBox();
            this.tbLinkLow = new System.Windows.Forms.TextBox();
            this.picLink = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbAgendaPointsHigh = new System.Windows.Forms.TextBox();
            this.tbAgendaPointsLow = new System.Windows.Forms.TextBox();
            this.picAgendaPoints = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.eclKeyword = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclTrigger = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclExpansionSet = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.btnQuickFindNext = new System.Windows.Forms.Button();
            this.tbStrengthHigh = new System.Windows.Forms.TextBox();
            this.tbStrengthLow = new System.Windows.Forms.TextBox();
            this.tbDeckSizeHigh = new System.Windows.Forms.TextBox();
            this.tbDeckSizeLow = new System.Windows.Forms.TextBox();
            this.tbMuHigh = new System.Windows.Forms.TextBox();
            this.tbMuLow = new System.Windows.Forms.TextBox();
            this.tbCostHigh = new System.Windows.Forms.TextBox();
            this.tbCostLow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCardtext = new System.Windows.Forms.TextBox();
            this.eclCardtextCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.eclNameCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.eclKeywordCheck = new Beyond.ExtendedControls.ExtendedCheckBox();
            this.eclProvides = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclIcon = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclCardtype = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.picStrength = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.picDeckSize = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.picMu = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picCost = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.eclVirtue = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclFaction = new Beyond.ExtendedControls.ExtendedCheckedListBox();
            this.eclIceType = new Beyond.ExtendedControls.ExtendedCheckedListBox();
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
            this.cardPreviewControl = new NRADB.Forms.CardPreviewControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deckBuilderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oCTGNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadOCTGNDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkNewVersionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTrashCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAgendaPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDeckSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCost)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.tbInfluenceHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbInfluenceLow);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.rbRunnerOnly);
            this.splitContainer1.Panel1.Controls.Add(this.rbCorpOnly);
            this.splitContainer1.Panel1.Controls.Add(this.rbAll);
            this.splitContainer1.Panel1.Controls.Add(this.tbTrashCostHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbTrashCostLow);
            this.splitContainer1.Panel1.Controls.Add(this.picTrashCost);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.tbLinkHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbLinkLow);
            this.splitContainer1.Panel1.Controls.Add(this.picLink);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.tbAgendaPointsHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbAgendaPointsLow);
            this.splitContainer1.Panel1.Controls.Add(this.picAgendaPoints);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.eclKeyword);
            this.splitContainer1.Panel1.Controls.Add(this.eclTrigger);
            this.splitContainer1.Panel1.Controls.Add(this.eclExpansionSet);
            this.splitContainer1.Panel1.Controls.Add(this.btnQuickFindNext);
            this.splitContainer1.Panel1.Controls.Add(this.tbStrengthHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbStrengthLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbDeckSizeHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbDeckSizeLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbMuHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbMuLow);
            this.splitContainer1.Panel1.Controls.Add(this.tbCostHigh);
            this.splitContainer1.Panel1.Controls.Add(this.tbCostLow);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.tbCardtext);
            this.splitContainer1.Panel1.Controls.Add(this.eclCardtextCheck);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.tbName);
            this.splitContainer1.Panel1.Controls.Add(this.eclNameCheck);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tbKeywords);
            this.splitContainer1.Panel1.Controls.Add(this.eclKeywordCheck);
            this.splitContainer1.Panel1.Controls.Add(this.eclProvides);
            this.splitContainer1.Panel1.Controls.Add(this.eclIcon);
            this.splitContainer1.Panel1.Controls.Add(this.eclCardtype);
            this.splitContainer1.Panel1.Controls.Add(this.picStrength);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.picDeckSize);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.picMu);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.picCost);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.tbFind);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.eclVirtue);
            this.splitContainer1.Panel1.Controls.Add(this.eclFaction);
            this.splitContainer1.Panel1.Controls.Add(this.eclIceType);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.btnReset);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.splitCardText);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // tbInfluenceHigh
            // 
            resources.ApplyResources(this.tbInfluenceHigh, "tbInfluenceHigh");
            this.tbInfluenceHigh.Name = "tbInfluenceHigh";
            this.tbInfluenceHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbInfluenceHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbInfluenceLow
            // 
            resources.ApplyResources(this.tbInfluenceLow, "tbInfluenceLow");
            this.tbInfluenceLow.Name = "tbInfluenceLow";
            this.tbInfluenceLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbInfluenceLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // rbRunnerOnly
            // 
            resources.ApplyResources(this.rbRunnerOnly, "rbRunnerOnly");
            this.rbRunnerOnly.Name = "rbRunnerOnly";
            this.rbRunnerOnly.UseVisualStyleBackColor = true;
            this.rbRunnerOnly.CheckedChanged += new System.EventHandler(this.rbSide_CheckedChanged);
            // 
            // rbCorpOnly
            // 
            resources.ApplyResources(this.rbCorpOnly, "rbCorpOnly");
            this.rbCorpOnly.Name = "rbCorpOnly";
            this.rbCorpOnly.UseVisualStyleBackColor = true;
            this.rbCorpOnly.CheckedChanged += new System.EventHandler(this.rbSide_CheckedChanged);
            // 
            // rbAll
            // 
            resources.ApplyResources(this.rbAll, "rbAll");
            this.rbAll.Checked = true;
            this.rbAll.Name = "rbAll";
            this.rbAll.TabStop = true;
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbSide_CheckedChanged);
            // 
            // tbTrashCostHigh
            // 
            resources.ApplyResources(this.tbTrashCostHigh, "tbTrashCostHigh");
            this.tbTrashCostHigh.Name = "tbTrashCostHigh";
            this.tbTrashCostHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbTrashCostHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbTrashCostLow
            // 
            resources.ApplyResources(this.tbTrashCostLow, "tbTrashCostLow");
            this.tbTrashCostLow.Name = "tbTrashCostLow";
            this.tbTrashCostLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbTrashCostLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // picTrashCost
            // 
            resources.ApplyResources(this.picTrashCost, "picTrashCost");
            this.picTrashCost.Name = "picTrashCost";
            this.picTrashCost.TabStop = false;
            this.toolTip1.SetToolTip(this.picTrashCost, resources.GetString("picTrashCost.ToolTip"));
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tbLinkHigh
            // 
            resources.ApplyResources(this.tbLinkHigh, "tbLinkHigh");
            this.tbLinkHigh.Name = "tbLinkHigh";
            this.tbLinkHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbLinkHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbLinkLow
            // 
            resources.ApplyResources(this.tbLinkLow, "tbLinkLow");
            this.tbLinkLow.Name = "tbLinkLow";
            this.tbLinkLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbLinkLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // picLink
            // 
            resources.ApplyResources(this.picLink, "picLink");
            this.picLink.Name = "picLink";
            this.picLink.TabStop = false;
            this.toolTip1.SetToolTip(this.picLink, resources.GetString("picLink.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbAgendaPointsHigh
            // 
            resources.ApplyResources(this.tbAgendaPointsHigh, "tbAgendaPointsHigh");
            this.tbAgendaPointsHigh.Name = "tbAgendaPointsHigh";
            this.tbAgendaPointsHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbAgendaPointsHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbAgendaPointsLow
            // 
            resources.ApplyResources(this.tbAgendaPointsLow, "tbAgendaPointsLow");
            this.tbAgendaPointsLow.Name = "tbAgendaPointsLow";
            this.tbAgendaPointsLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbAgendaPointsLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // picAgendaPoints
            // 
            resources.ApplyResources(this.picAgendaPoints, "picAgendaPoints");
            this.picAgendaPoints.Name = "picAgendaPoints";
            this.picAgendaPoints.TabStop = false;
            this.toolTip1.SetToolTip(this.picAgendaPoints, resources.GetString("picAgendaPoints.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            // tbDeckSizeHigh
            // 
            resources.ApplyResources(this.tbDeckSizeHigh, "tbDeckSizeHigh");
            this.tbDeckSizeHigh.Name = "tbDeckSizeHigh";
            this.tbDeckSizeHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbDeckSizeHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbDeckSizeLow
            // 
            resources.ApplyResources(this.tbDeckSizeLow, "tbDeckSizeLow");
            this.tbDeckSizeLow.Name = "tbDeckSizeLow";
            this.tbDeckSizeLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbDeckSizeLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbMuHigh
            // 
            resources.ApplyResources(this.tbMuHigh, "tbMuHigh");
            this.tbMuHigh.Name = "tbMuHigh";
            this.tbMuHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbMuHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbMuLow
            // 
            resources.ApplyResources(this.tbMuLow, "tbMuLow");
            this.tbMuLow.Name = "tbMuLow";
            this.tbMuLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbMuLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbCostHigh
            // 
            resources.ApplyResources(this.tbCostHigh, "tbCostHigh");
            this.tbCostHigh.Name = "tbCostHigh";
            this.tbCostHigh.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbCostHigh.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // tbCostLow
            // 
            resources.ApplyResources(this.tbCostLow, "tbCostLow");
            this.tbCostLow.Name = "tbCostLow";
            this.tbCostLow.TextChanged += new System.EventHandler(this.tbLowHigh_TextChanged);
            this.tbCostLow.Validated += new System.EventHandler(this.tbLowHigh_Validated);
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
            // tbKeywords
            // 
            resources.ApplyResources(this.tbKeywords, "tbKeywords");
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Validated += new System.EventHandler(this.tbLowHigh_Validated);
            // 
            // eclKeywordCheck
            // 
            resources.ApplyResources(this.eclKeywordCheck, "eclKeywordCheck");
            this.eclKeywordCheck.Name = "eclKeywordCheck";
            this.eclKeywordCheck.ThreeState = true;
            this.eclKeywordCheck.UseVisualStyleBackColor = true;
            this.eclKeywordCheck.CheckStateChanged += new System.EventHandler(this.eclCheck_CheckStateChanged);
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
            // picDeckSize
            // 
            resources.ApplyResources(this.picDeckSize, "picDeckSize");
            this.picDeckSize.Name = "picDeckSize";
            this.picDeckSize.TabStop = false;
            this.toolTip1.SetToolTip(this.picDeckSize, resources.GetString("picDeckSize.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // picMu
            // 
            resources.ApplyResources(this.picMu, "picMu");
            this.picMu.Name = "picMu";
            this.picMu.TabStop = false;
            this.toolTip1.SetToolTip(this.picMu, resources.GetString("picMu.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // picCost
            // 
            resources.ApplyResources(this.picCost, "picCost");
            this.picCost.Name = "picCost";
            this.picCost.TabStop = false;
            this.toolTip1.SetToolTip(this.picCost, resources.GetString("picCost.ToolTip"));
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
            // eclFaction
            // 
            this.eclFaction.CheckOnClick = true;
            this.eclFaction.Condensed = true;
            this.eclFaction.CondensedMode = true;
            this.eclFaction.FormattingEnabled = true;
            resources.ApplyResources(this.eclFaction, "eclFaction");
            this.eclFaction.Name = "eclFaction";
            this.eclFaction.RollDownDelay = 250;
            this.eclFaction.Summary = "House";
            this.eclFaction.ThreeState = true;
            this.eclFaction.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // eclIceType
            // 
            this.eclIceType.CheckOnClick = true;
            this.eclIceType.Condensed = true;
            this.eclIceType.CondensedMode = true;
            this.eclIceType.FormattingEnabled = true;
            resources.ApplyResources(this.eclIceType, "eclIceType");
            this.eclIceType.Name = "eclIceType";
            this.eclIceType.RollDownDelay = 250;
            this.eclIceType.Summary = "Mecanism";
            this.eclIceType.ThreeState = true;
            this.eclIceType.MouseLeave += new System.EventHandler(this.eclMouseLeave);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
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
            this.dataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
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
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            resources.ApplyResources(this.filterToolStripMenuItem, "filterToolStripMenuItem");
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
            ((System.ComponentModel.ISupportInitialize)(this.picTrashCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAgendaPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDeckSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCost)).EndInit();
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
		private System.Windows.Forms.TextBox tbKeywords;
		private Beyond.ExtendedControls.ExtendedCheckBox eclKeywordCheck;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclProvides;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclKeyword;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclIcon;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclCardtype;
		private System.Windows.Forms.PictureBox picStrength;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.PictureBox picDeckSize;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.PictureBox picMu;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbFind;
		private System.Windows.Forms.Label label11;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclVirtue;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclFaction;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclIceType;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclTrigger;
		private Beyond.ExtendedControls.ExtendedCheckedListBox eclExpansionSet;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.TextBox tbCostLow;
		private System.Windows.Forms.TextBox tbMuHigh;
		private System.Windows.Forms.TextBox tbMuLow;
		private System.Windows.Forms.TextBox tbCostHigh;
		private System.Windows.Forms.TextBox tbStrengthLow;
		private System.Windows.Forms.TextBox tbDeckSizeHigh;
		private System.Windows.Forms.TextBox tbDeckSizeLow;
		private System.Windows.Forms.TextBox tbStrengthHigh;
		private System.Windows.Forms.ContextMenuStrip popupGrid;
		private System.Windows.Forms.ToolStripMenuItem saveSelectionToTextFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveToANewWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
		private System.Windows.Forms.PictureBox picCost;
		private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deckBuilderToolStripMenuItem;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem addCardToDeckToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addCardToSideboardToolStripMenuItem;
        private System.Windows.Forms.Button btnQuickFindNext;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.ComponentModel.BackgroundWorker checkNewVersionBackgroundWorker;
		private System.Windows.Forms.SplitContainer splitCardText;
		private System.Windows.Forms.Button btnReportError;
		private CardPreviewControl cardPreviewControl;
        private System.Windows.Forms.Label lblUniversalId;
        private System.Windows.Forms.ToolStripMenuItem oCTGNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadOCTGNDataToolStripMenuItem;
        private System.Windows.Forms.TextBox tbAgendaPointsHigh;
        private System.Windows.Forms.TextBox tbAgendaPointsLow;
        private System.Windows.Forms.PictureBox picAgendaPoints;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLinkHigh;
        private System.Windows.Forms.TextBox tbLinkLow;
        private System.Windows.Forms.PictureBox picLink;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTrashCostHigh;
        private System.Windows.Forms.TextBox tbTrashCostLow;
        private System.Windows.Forms.PictureBox picTrashCost;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton rbRunnerOnly;
        private System.Windows.Forms.RadioButton rbCorpOnly;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.TextBox tbInfluenceHigh;
        private System.Windows.Forms.TextBox tbInfluenceLow;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
	}
}
