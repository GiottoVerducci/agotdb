namespace AGoTDB.Forms
{
	partial class OptionsForm
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
			this.cbDisplayImages = new System.Windows.Forms.CheckBox();
			this.cbRebuildDatabase = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbDisplayMessages = new System.Windows.Forms.CheckBox();
			this.gbDisplay = new System.Windows.Forms.GroupBox();
			this.gbMisc = new System.Windows.Forms.GroupBox();
			this.lbCardPreviewSize = new System.Windows.Forms.Label();
			this.nudCardPreviewSize = new System.Windows.Forms.NumericUpDown();
			this.pnlCardPreviewSize = new System.Windows.Forms.Panel();
			this.gbDisplay.SuspendLayout();
			this.gbMisc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCardPreviewSize)).BeginInit();
			this.pnlCardPreviewSize.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbDisplayImages
			// 
			this.cbDisplayImages.AutoSize = true;
			this.cbDisplayImages.Location = new System.Drawing.Point(17, 24);
			this.cbDisplayImages.Name = "cbDisplayImages";
			this.cbDisplayImages.Size = new System.Drawing.Size(215, 17);
			this.cbDisplayImages.TabIndex = 0;
			this.cbDisplayImages.Text = "Display images (icons and card preview)";
			this.cbDisplayImages.UseVisualStyleBackColor = true;
			this.cbDisplayImages.CheckedChanged += new System.EventHandler(this.cbDisplayImages_CheckedChanged);
			// 
			// cbRebuildDatabase
			// 
			this.cbRebuildDatabase.AutoSize = true;
			this.cbRebuildDatabase.Location = new System.Drawing.Point(17, 28);
			this.cbRebuildDatabase.Name = "cbRebuildDatabase";
			this.cbRebuildDatabase.Size = new System.Drawing.Size(170, 17);
			this.cbRebuildDatabase.TabIndex = 1;
			this.cbRebuildDatabase.Text = "Rebuild database on next start";
			this.cbRebuildDatabase.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(197, 202);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(107, 202);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// cbDisplayMessages
			// 
			this.cbDisplayMessages.AutoSize = true;
			this.cbDisplayMessages.Location = new System.Drawing.Point(17, 73);
			this.cbDisplayMessages.Name = "cbDisplayMessages";
			this.cbDisplayMessages.Size = new System.Drawing.Size(123, 17);
			this.cbDisplayMessages.TabIndex = 4;
			this.cbDisplayMessages.Text = "Display all messages";
			this.cbDisplayMessages.ThreeState = true;
			this.cbDisplayMessages.UseVisualStyleBackColor = true;
			// 
			// gbDisplay
			// 
			this.gbDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbDisplay.Controls.Add(this.pnlCardPreviewSize);
			this.gbDisplay.Controls.Add(this.cbDisplayImages);
			this.gbDisplay.Controls.Add(this.cbDisplayMessages);
			this.gbDisplay.Location = new System.Drawing.Point(12, 12);
			this.gbDisplay.Name = "gbDisplay";
			this.gbDisplay.Size = new System.Drawing.Size(260, 101);
			this.gbDisplay.TabIndex = 5;
			this.gbDisplay.TabStop = false;
			this.gbDisplay.Text = "Display";
			// 
			// gbMisc
			// 
			this.gbMisc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbMisc.Controls.Add(this.cbRebuildDatabase);
			this.gbMisc.Location = new System.Drawing.Point(12, 124);
			this.gbMisc.Name = "gbMisc";
			this.gbMisc.Size = new System.Drawing.Size(260, 63);
			this.gbMisc.TabIndex = 6;
			this.gbMisc.TabStop = false;
			this.gbMisc.Text = "Miscellaneous";
			// 
			// lbCardPreviewSize
			// 
			this.lbCardPreviewSize.AutoSize = true;
			this.lbCardPreviewSize.Location = new System.Drawing.Point(3, 2);
			this.lbCardPreviewSize.Name = "lbCardPreviewSize";
			this.lbCardPreviewSize.Size = new System.Drawing.Size(110, 13);
			this.lbCardPreviewSize.TabIndex = 5;
			this.lbCardPreviewSize.Text = "Card preview size (%):";
			// 
			// nudCardPreviewSize
			// 
			this.nudCardPreviewSize.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.nudCardPreviewSize.Location = new System.Drawing.Point(119, 0);
			this.nudCardPreviewSize.Name = "nudCardPreviewSize";
			this.nudCardPreviewSize.Size = new System.Drawing.Size(38, 20);
			this.nudCardPreviewSize.TabIndex = 6;
			// 
			// pnlCardPreviewSize
			// 
			this.pnlCardPreviewSize.Controls.Add(this.lbCardPreviewSize);
			this.pnlCardPreviewSize.Controls.Add(this.nudCardPreviewSize);
			this.pnlCardPreviewSize.Location = new System.Drawing.Point(32, 47);
			this.pnlCardPreviewSize.Name = "pnlCardPreviewSize";
			this.pnlCardPreviewSize.Size = new System.Drawing.Size(155, 20);
			this.pnlCardPreviewSize.TabIndex = 7;
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 237);
			this.Controls.Add(this.gbMisc);
			this.Controls.Add(this.gbDisplay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "OptionsForm";
			this.Text = "OptionsForm";
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			this.Shown += new System.EventHandler(this.OptionsForm_Shown);
			this.gbDisplay.ResumeLayout(false);
			this.gbDisplay.PerformLayout();
			this.gbMisc.ResumeLayout(false);
			this.gbMisc.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCardPreviewSize)).EndInit();
			this.pnlCardPreviewSize.ResumeLayout(false);
			this.pnlCardPreviewSize.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox cbDisplayImages;
		private System.Windows.Forms.CheckBox cbRebuildDatabase;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox cbDisplayMessages;
		private System.Windows.Forms.GroupBox gbDisplay;
		private System.Windows.Forms.GroupBox gbMisc;
		private System.Windows.Forms.NumericUpDown nudCardPreviewSize;
		private System.Windows.Forms.Label lbCardPreviewSize;
		private System.Windows.Forms.Panel pnlCardPreviewSize;
	}
}