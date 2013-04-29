namespace NRADB.Forms
{
	partial class DrawSimulatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawSimulatorForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbHand = new System.Windows.Forms.ListBox();
            this.lbDeck = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnShuffle = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.lbHand);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.lbDeck);
            // 
            // lbHand
            // 
            resources.ApplyResources(this.lbHand, "lbHand");
            this.lbHand.FormattingEnabled = true;
            this.lbHand.Name = "lbHand";
            this.lbHand.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // lbDeck
            // 
            resources.ApplyResources(this.lbDeck, "lbDeck");
            this.lbDeck.FormattingEnabled = true;
            this.lbDeck.Name = "lbDeck";
            this.lbDeck.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDiscard
            // 
            resources.ApplyResources(this.btnDiscard, "btnDiscard");
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.UseVisualStyleBackColor = true;
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnMoveRight
            // 
            resources.ApplyResources(this.btnMoveRight, "btnMoveRight");
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveLeft
            // 
            resources.ApplyResources(this.btnMoveLeft, "btnMoveLeft");
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnShuffle
            // 
            resources.ApplyResources(this.btnShuffle, "btnShuffle");
            this.btnShuffle.Name = "btnShuffle";
            this.btnShuffle.UseVisualStyleBackColor = true;
            this.btnShuffle.Click += new System.EventHandler(this.btnShuffle_Click);
            // 
            // btnRestart
            // 
            resources.ApplyResources(this.btnRestart, "btnRestart");
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // DrawSimulatorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnShuffle);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnMoveLeft);
            this.Controls.Add(this.btnMoveRight);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.Name = "DrawSimulatorForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DrawSimulatorForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDiscard;
		private System.Windows.Forms.Button btnMoveRight;
		private System.Windows.Forms.Button btnMoveLeft;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox lbHand;
		private System.Windows.Forms.ListBox lbDeck;
		private System.Windows.Forms.Button btnShuffle;
		private System.Windows.Forms.Button btnRestart;
	}
}