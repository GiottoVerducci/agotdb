namespace AGoTDB.Forms
{
	partial class CardPreviewControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pbImage = new System.Windows.Forms.PictureBox();
			this.lblUnavailable = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
			this.SuspendLayout();
			// 
			// pbImage
			// 
			this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbImage.Location = new System.Drawing.Point(0, 0);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(150, 150);
			this.pbImage.TabIndex = 1;
			this.pbImage.TabStop = false;
			this.pbImage.MouseCaptureChanged += new System.EventHandler(this.Control_MouseCaptureChanged);
			this.pbImage.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
			this.pbImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Control_MouseClick);
			this.pbImage.MouseEnter += new System.EventHandler(this.Control_MouseEnter);
			// 
			// lblUnavailable
			// 
			this.lblUnavailable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblUnavailable.Location = new System.Drawing.Point(0, 0);
			this.lblUnavailable.Name = "lblUnavailable";
			this.lblUnavailable.Size = new System.Drawing.Size(150, 150);
			this.lblUnavailable.TabIndex = 2;
			this.lblUnavailable.Text = "label1";
			this.lblUnavailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblUnavailable.MouseCaptureChanged += new System.EventHandler(this.Control_MouseCaptureChanged);
			this.lblUnavailable.MouseLeave += new System.EventHandler(this.Control_MouseLeave);
			this.lblUnavailable.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Control_MouseClick);
			this.lblUnavailable.MouseEnter += new System.EventHandler(this.Control_MouseEnter);
			// 
			// CardPreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblUnavailable);
			this.Controls.Add(this.pbImage);
			this.Name = "CardPreviewControl";
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbImage;
		private System.Windows.Forms.Label lblUnavailable;
	}
}
