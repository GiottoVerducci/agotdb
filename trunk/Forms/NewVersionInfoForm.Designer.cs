namespace AGoT.AGoTDB.Forms
{
  partial class FormNewVersionInfo
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewVersionInfo));
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnOk = new System.Windows.Forms.Button();
      this.cbDoNotDisplay = new System.Windows.Forms.CheckBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      //
      // richTextBox1
      //
      resources.ApplyResources(this.richTextBox1, "richTextBox1");
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      //
      // panel1
      //
      this.panel1.Controls.Add(this.btnOk);
      this.panel1.Controls.Add(this.cbDoNotDisplay);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
      //
      // btnOk
      //
      resources.ApplyResources(this.btnOk, "btnOk");
      this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      //
      // cbDoNotDisplay
      //
      resources.ApplyResources(this.cbDoNotDisplay, "cbDoNotDisplay");
      this.cbDoNotDisplay.Name = "cbDoNotDisplay";
      this.cbDoNotDisplay.UseVisualStyleBackColor = true;
      //
      // FormNewVersionInfo
      //
      this.AcceptButton = this.btnOk;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.richTextBox1);
      this.Name = "FormNewVersionInfo";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.CheckBox cbDoNotDisplay;
  }
}