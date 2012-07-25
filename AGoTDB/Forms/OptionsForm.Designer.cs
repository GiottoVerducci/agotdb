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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
			this.cbDisplayImages = new System.Windows.Forms.CheckBox();
			this.cbRebuildDatabase = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbDisplayMessages = new System.Windows.Forms.CheckBox();
			this.gbDisplay = new System.Windows.Forms.GroupBox();
			this.lbCardPreviewSize = new System.Windows.Forms.Label();
			this.nudCardPreviewSize = new System.Windows.Forms.NumericUpDown();
			this.gbMisc = new System.Windows.Forms.GroupBox();
			this.cbCheckForDatabaseUpdateOnStartup = new System.Windows.Forms.CheckBox();
			this.gbDisplay.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCardPreviewSize)).BeginInit();
			this.gbMisc.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbDisplayImages
			// 
			resources.ApplyResources(this.cbDisplayImages, "cbDisplayImages");
			this.cbDisplayImages.Name = "cbDisplayImages";
			this.cbDisplayImages.UseVisualStyleBackColor = true;
			this.cbDisplayImages.CheckedChanged += new System.EventHandler(this.cbDisplayImages_CheckedChanged);
			// 
			// cbRebuildDatabase
			// 
			resources.ApplyResources(this.cbRebuildDatabase, "cbRebuildDatabase");
			this.cbRebuildDatabase.Name = "cbRebuildDatabase";
			this.cbRebuildDatabase.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// cbDisplayMessages
			// 
			resources.ApplyResources(this.cbDisplayMessages, "cbDisplayMessages");
			this.cbDisplayMessages.Name = "cbDisplayMessages";
			this.cbDisplayMessages.ThreeState = true;
			this.cbDisplayMessages.UseVisualStyleBackColor = true;
			// 
			// gbDisplay
			// 
			resources.ApplyResources(this.gbDisplay, "gbDisplay");
			this.gbDisplay.Controls.Add(this.lbCardPreviewSize);
			this.gbDisplay.Controls.Add(this.nudCardPreviewSize);
			this.gbDisplay.Controls.Add(this.cbDisplayImages);
			this.gbDisplay.Controls.Add(this.cbDisplayMessages);
			this.gbDisplay.Name = "gbDisplay";
			this.gbDisplay.TabStop = false;
			// 
			// lbCardPreviewSize
			// 
			resources.ApplyResources(this.lbCardPreviewSize, "lbCardPreviewSize");
			this.lbCardPreviewSize.Name = "lbCardPreviewSize";
			// 
			// nudCardPreviewSize
			// 
			this.nudCardPreviewSize.Increment = new decimal(new int[] {
			25,
			0,
			0,
			0});
			resources.ApplyResources(this.nudCardPreviewSize, "nudCardPreviewSize");
			this.nudCardPreviewSize.Name = "nudCardPreviewSize";
			// 
			// gbMisc
			// 
			resources.ApplyResources(this.gbMisc, "gbMisc");
			this.gbMisc.Controls.Add(this.cbCheckForDatabaseUpdateOnStartup);
			this.gbMisc.Controls.Add(this.cbRebuildDatabase);
			this.gbMisc.Name = "gbMisc";
			this.gbMisc.TabStop = false;
			// 
			// cbCheckForDatabaseUpdateOnStartup
			// 
			resources.ApplyResources(this.cbCheckForDatabaseUpdateOnStartup, "cbCheckForDatabaseUpdateOnStartup");
			this.cbCheckForDatabaseUpdateOnStartup.Name = "cbCheckForDatabaseUpdateOnStartup";
			this.cbCheckForDatabaseUpdateOnStartup.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbMisc);
			this.Controls.Add(this.gbDisplay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			this.Shown += new System.EventHandler(this.OptionsForm_Shown);
			this.gbDisplay.ResumeLayout(false);
			this.gbDisplay.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCardPreviewSize)).EndInit();
			this.gbMisc.ResumeLayout(false);
			this.gbMisc.PerformLayout();
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
		private System.Windows.Forms.CheckBox cbCheckForDatabaseUpdateOnStartup;
	}
}