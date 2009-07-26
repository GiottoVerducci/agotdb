namespace WoMDB.Forms
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lblDbVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblVersion
			// 
			this.lblVersion.AccessibleDescription = null;
			this.lblVersion.AccessibleName = null;
			resources.ApplyResources(this.lblVersion, "lblVersion");
			this.lblVersion.Name = "lblVersion";
			// 
			// btnOk
			// 
			this.btnOk.AccessibleDescription = null;
			this.btnOk.AccessibleName = null;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackgroundImage = null;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Font = null;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// textBox1
			// 
			this.textBox1.AccessibleDescription = null;
			this.textBox1.AccessibleName = null;
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.BackgroundImage = null;
			this.textBox1.Font = null;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			// 
			// lblDbVersion
			// 
			this.lblDbVersion.AccessibleDescription = null;
			this.lblDbVersion.AccessibleName = null;
			resources.ApplyResources(this.lblDbVersion, "lblDbVersion");
			this.lblDbVersion.Name = "lblDbVersion";
			// 
			// AboutForm
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.CancelButton = this.btnOk;
			this.Controls.Add(this.lblDbVersion);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.TopMost = true;
			this.Shown += new System.EventHandler(this.AboutForm_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label lblDbVersion;
	}
}