namespace AGoTDB.OCTGN
{
    partial class OctgnLoaderForm
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
            this.lblLoadSet = new System.Windows.Forms.Label();
            this.pbLoadSet = new System.Windows.Forms.ProgressBar();
            this.pbMatchSet = new System.Windows.Forms.ProgressBar();
            this.lblMatchSet = new System.Windows.Forms.Label();
            this.pbFindCard = new System.Windows.Forms.ProgressBar();
            this.lblFindCard = new System.Windows.Forms.Label();
            this.pbUpdateDatabase = new System.Windows.Forms.ProgressBar();
            this.lblUpdateDatabase = new System.Windows.Forms.Label();
            this.btAbort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLoadSet
            // 
            this.lblLoadSet.AutoSize = true;
            this.lblLoadSet.Location = new System.Drawing.Point(12, 9);
            this.lblLoadSet.Name = "lblLoadSet";
            this.lblLoadSet.Size = new System.Drawing.Size(67, 13);
            this.lblLoadSet.TabIndex = 0;
            this.lblLoadSet.Text = "Loading sets";
            // 
            // pbLoadSet
            // 
            this.pbLoadSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLoadSet.Location = new System.Drawing.Point(15, 25);
            this.pbLoadSet.Name = "pbLoadSet";
            this.pbLoadSet.Size = new System.Drawing.Size(383, 23);
            this.pbLoadSet.TabIndex = 1;
            // 
            // pbMatchSet
            // 
            this.pbMatchSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMatchSet.Location = new System.Drawing.Point(15, 67);
            this.pbMatchSet.Name = "pbMatchSet";
            this.pbMatchSet.Size = new System.Drawing.Size(383, 23);
            this.pbMatchSet.TabIndex = 3;
            // 
            // lblMatchSet
            // 
            this.lblMatchSet.AutoSize = true;
            this.lblMatchSet.Location = new System.Drawing.Point(12, 51);
            this.lblMatchSet.Name = "lblMatchSet";
            this.lblMatchSet.Size = new System.Drawing.Size(73, 13);
            this.lblMatchSet.TabIndex = 2;
            this.lblMatchSet.Text = "Matching sets";
            // 
            // pbFindCard
            // 
            this.pbFindCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFindCard.Location = new System.Drawing.Point(15, 109);
            this.pbFindCard.Name = "pbFindCard";
            this.pbFindCard.Size = new System.Drawing.Size(383, 23);
            this.pbFindCard.TabIndex = 5;
            // 
            // lblFindCard
            // 
            this.lblFindCard.AutoSize = true;
            this.lblFindCard.Location = new System.Drawing.Point(12, 93);
            this.lblFindCard.Name = "lblFindCard";
            this.lblFindCard.Size = new System.Drawing.Size(70, 13);
            this.lblFindCard.TabIndex = 4;
            this.lblFindCard.Text = "Finding cards";
            // 
            // pbUpdateDatabase
            // 
            this.pbUpdateDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbUpdateDatabase.Location = new System.Drawing.Point(15, 151);
            this.pbUpdateDatabase.Name = "pbUpdateDatabase";
            this.pbUpdateDatabase.Size = new System.Drawing.Size(383, 23);
            this.pbUpdateDatabase.TabIndex = 7;
            // 
            // lblUpdateDatabase
            // 
            this.lblUpdateDatabase.AutoSize = true;
            this.lblUpdateDatabase.Location = new System.Drawing.Point(12, 135);
            this.lblUpdateDatabase.Name = "lblUpdateDatabase";
            this.lblUpdateDatabase.Size = new System.Drawing.Size(97, 13);
            this.lblUpdateDatabase.TabIndex = 6;
            this.lblUpdateDatabase.Text = "Updating database";
            // 
            // btAbort
            // 
            this.btAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAbort.Location = new System.Drawing.Point(323, 180);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(75, 23);
            this.btAbort.TabIndex = 8;
            this.btAbort.Text = "Abort";
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // OctgnLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 209);
            this.Controls.Add(this.btAbort);
            this.Controls.Add(this.pbUpdateDatabase);
            this.Controls.Add(this.lblUpdateDatabase);
            this.Controls.Add(this.pbFindCard);
            this.Controls.Add(this.lblFindCard);
            this.Controls.Add(this.pbMatchSet);
            this.Controls.Add(this.lblMatchSet);
            this.Controls.Add(this.pbLoadSet);
            this.Controls.Add(this.lblLoadSet);
            this.Name = "OctgnLoaderForm";
            this.Text = "Loading OCTGN data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoadSet;
        private System.Windows.Forms.ProgressBar pbLoadSet;
        private System.Windows.Forms.ProgressBar pbMatchSet;
        private System.Windows.Forms.Label lblMatchSet;
        private System.Windows.Forms.ProgressBar pbFindCard;
        private System.Windows.Forms.Label lblFindCard;
        private System.Windows.Forms.ProgressBar pbUpdateDatabase;
        private System.Windows.Forms.Label lblUpdateDatabase;
        private System.Windows.Forms.Button btAbort;
    }
}