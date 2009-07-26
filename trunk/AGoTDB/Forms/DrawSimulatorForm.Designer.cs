namespace AGoT.AGoTDB.Forms
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
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
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
      this.splitContainer1.Panel1.Controls.Add(this.lbHand);
      this.splitContainer1.Panel1.Font = null;
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.AccessibleDescription = null;
      this.splitContainer1.Panel2.AccessibleName = null;
      resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
      this.splitContainer1.Panel2.BackgroundImage = null;
      this.splitContainer1.Panel2.Controls.Add(this.lbDeck);
      this.splitContainer1.Panel2.Font = null;
      // 
      // lbHand
      // 
      this.lbHand.AccessibleDescription = null;
      this.lbHand.AccessibleName = null;
      resources.ApplyResources(this.lbHand, "lbHand");
      this.lbHand.BackgroundImage = null;
      this.lbHand.Font = null;
      this.lbHand.FormattingEnabled = true;
      this.lbHand.Name = "lbHand";
      this.lbHand.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      // 
      // lbDeck
      // 
      this.lbDeck.AccessibleDescription = null;
      this.lbDeck.AccessibleName = null;
      resources.ApplyResources(this.lbDeck, "lbDeck");
      this.lbDeck.BackgroundImage = null;
      this.lbDeck.Font = null;
      this.lbDeck.FormattingEnabled = true;
      this.lbDeck.Name = "lbDeck";
      this.lbDeck.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      // 
      // btnClose
      // 
      this.btnClose.AccessibleDescription = null;
      this.btnClose.AccessibleName = null;
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.BackgroundImage = null;
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Font = null;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnDiscard
      // 
      this.btnDiscard.AccessibleDescription = null;
      this.btnDiscard.AccessibleName = null;
      resources.ApplyResources(this.btnDiscard, "btnDiscard");
      this.btnDiscard.BackgroundImage = null;
      this.btnDiscard.Font = null;
      this.btnDiscard.Name = "btnDiscard";
      this.btnDiscard.UseVisualStyleBackColor = true;
      this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
      // 
      // btnMoveRight
      // 
      this.btnMoveRight.AccessibleDescription = null;
      this.btnMoveRight.AccessibleName = null;
      resources.ApplyResources(this.btnMoveRight, "btnMoveRight");
      this.btnMoveRight.BackgroundImage = null;
      this.btnMoveRight.Font = null;
      this.btnMoveRight.Name = "btnMoveRight";
      this.btnMoveRight.UseVisualStyleBackColor = true;
      this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
      // 
      // btnMoveLeft
      // 
      this.btnMoveLeft.AccessibleDescription = null;
      this.btnMoveLeft.AccessibleName = null;
      resources.ApplyResources(this.btnMoveLeft, "btnMoveLeft");
      this.btnMoveLeft.BackgroundImage = null;
      this.btnMoveLeft.Font = null;
      this.btnMoveLeft.Name = "btnMoveLeft";
      this.btnMoveLeft.UseVisualStyleBackColor = true;
      this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
      // 
      // btnShuffle
      // 
      this.btnShuffle.AccessibleDescription = null;
      this.btnShuffle.AccessibleName = null;
      resources.ApplyResources(this.btnShuffle, "btnShuffle");
      this.btnShuffle.BackgroundImage = null;
      this.btnShuffle.Font = null;
      this.btnShuffle.Name = "btnShuffle";
      this.btnShuffle.UseVisualStyleBackColor = true;
      this.btnShuffle.Click += new System.EventHandler(this.btnShuffle_Click);
      // 
      // btnRestart
      // 
      this.btnRestart.AccessibleDescription = null;
      this.btnRestart.AccessibleName = null;
      resources.ApplyResources(this.btnRestart, "btnRestart");
      this.btnRestart.BackgroundImage = null;
      this.btnRestart.Font = null;
      this.btnRestart.Name = "btnRestart";
      this.btnRestart.UseVisualStyleBackColor = true;
      this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
      // 
      // DrawSimulatorForm
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = null;
      this.Controls.Add(this.btnRestart);
      this.Controls.Add(this.btnShuffle);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.btnMoveLeft);
      this.Controls.Add(this.btnMoveRight);
      this.Controls.Add(this.btnDiscard);
      this.Controls.Add(this.btnClose);
      this.DoubleBuffered = true;
      this.Font = null;
      this.Icon = null;
      this.Name = "DrawSimulatorForm";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DrawSimulatorForm_FormClosed);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
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