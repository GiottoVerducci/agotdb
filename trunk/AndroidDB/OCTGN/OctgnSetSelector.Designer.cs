namespace NRADB.OCTGN
{
    partial class OctgnSetSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OctgnSetSelector));
            this.rbPath = new System.Windows.Forms.RadioButton();
            this.rbUrl = new System.Windows.Forms.RadioButton();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btBrowsePath = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbPath
            // 
            this.rbPath.AutoSize = true;
            this.rbPath.Location = new System.Drawing.Point(12, 12);
            this.rbPath.Name = "rbPath";
            this.rbPath.Size = new System.Drawing.Size(119, 17);
            this.rbPath.TabIndex = 0;
            this.rbPath.Text = "Load sets from disk:";
            this.rbPath.UseVisualStyleBackColor = true;
            this.rbPath.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbUrl
            // 
            this.rbUrl.AutoSize = true;
            this.rbUrl.Checked = true;
            this.rbUrl.Location = new System.Drawing.Point(12, 35);
            this.rbUrl.Name = "rbUrl";
            this.rbUrl.Size = new System.Drawing.Size(135, 17);
            this.rbUrl.TabIndex = 1;
            this.rbUrl.TabStop = true;
            this.rbUrl.Text = "Load sets from internet:";
            this.rbUrl.UseVisualStyleBackColor = true;
            this.rbUrl.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // tbUrl
            // 
            this.tbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUrl.Location = new System.Drawing.Point(153, 34);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(246, 20);
            this.tbUrl.TabIndex = 2;
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(153, 11);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(215, 20);
            this.tbPath.TabIndex = 3;
            // 
            // btBrowsePath
            // 
            this.btBrowsePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowsePath.Location = new System.Drawing.Point(374, 11);
            this.btBrowsePath.Name = "btBrowsePath";
            this.btBrowsePath.Size = new System.Drawing.Size(25, 20);
            this.btBrowsePath.TabIndex = 4;
            this.btBrowsePath.Text = "...";
            this.btBrowsePath.UseVisualStyleBackColor = true;
            this.btBrowsePath.Click += new System.EventHandler(this.btBrowsePath_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(324, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoad.Location = new System.Drawing.Point(243, 86);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // OctgnSetSelector
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(411, 121);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btBrowsePath);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.rbUrl);
            this.Controls.Add(this.rbPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OctgnSetSelector";
            this.Text = "Select path or url";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OctgnSetSelector_FormClosing);
            this.Load += new System.EventHandler(this.OctgnSetSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbPath;
        private System.Windows.Forms.RadioButton rbUrl;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btBrowsePath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLoad;
    }
}