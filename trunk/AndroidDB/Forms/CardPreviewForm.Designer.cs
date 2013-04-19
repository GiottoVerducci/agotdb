namespace NRADB.Forms
{
	partial class CardPreviewForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardPreviewForm));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.cardPreviewControl = new NRADB.Forms.CardPreviewControl();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "unavailable.png");
			// 
			// cardPreviewControl1
			// 
			this.cardPreviewControl.CardUniversalId = -1;
			this.cardPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cardPreviewControl.Location = new System.Drawing.Point(0, 0);
			this.cardPreviewControl.Name = "cardPreviewControl1";
			this.cardPreviewControl.Size = new System.Drawing.Size(263, 213);
			this.cardPreviewControl.TabIndex = 0;
			// 
			// CardPreviewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(263, 213);
			this.ControlBox = false;
			this.Controls.Add(this.cardPreviewControl);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CardPreviewForm";
			this.Opacity = 0.9;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "CardPreviewForm";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private CardPreviewControl cardPreviewControl;
	}
}