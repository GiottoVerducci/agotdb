namespace NRADB.OCTGN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OctgnLoaderForm));
            this.lblLoadSet = new System.Windows.Forms.Label();
            this.pbLoadSet = new System.Windows.Forms.ProgressBar();
            this.pbImportSet = new System.Windows.Forms.ProgressBar();
            this.lblImportSet = new System.Windows.Forms.Label();
            this.pbImportCard = new System.Windows.Forms.ProgressBar();
            this.lblImportCard = new System.Windows.Forms.Label();
            this.pbUpdateDatabase = new System.Windows.Forms.ProgressBar();
            this.lblUpdateDatabase = new System.Windows.Forms.Label();
            this.btAbort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLoadSet
            // 
            resources.ApplyResources(this.lblLoadSet, "lblLoadSet");
            this.lblLoadSet.Name = "lblLoadSet";
            // 
            // pbLoadSet
            // 
            resources.ApplyResources(this.pbLoadSet, "pbLoadSet");
            this.pbLoadSet.Name = "pbLoadSet";
            // 
            // pbImportSet
            // 
            resources.ApplyResources(this.pbImportSet, "pbImportSet");
            this.pbImportSet.Name = "pbImportSet";
            // 
            // lblImportSet
            // 
            resources.ApplyResources(this.lblImportSet, "lblImportSet");
            this.lblImportSet.Name = "lblImportSet";
            // 
            // pbImportCard
            // 
            resources.ApplyResources(this.pbImportCard, "pbImportCard");
            this.pbImportCard.Name = "pbImportCard";
            // 
            // lblImportCard
            // 
            resources.ApplyResources(this.lblImportCard, "lblImportCard");
            this.lblImportCard.Name = "lblImportCard";
            // 
            // pbUpdateDatabase
            // 
            resources.ApplyResources(this.pbUpdateDatabase, "pbUpdateDatabase");
            this.pbUpdateDatabase.Name = "pbUpdateDatabase";
            // 
            // lblUpdateDatabase
            // 
            resources.ApplyResources(this.lblUpdateDatabase, "lblUpdateDatabase");
            this.lblUpdateDatabase.Name = "lblUpdateDatabase";
            // 
            // btAbort
            // 
            resources.ApplyResources(this.btAbort, "btAbort");
            this.btAbort.Name = "btAbort";
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // OctgnLoaderForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btAbort);
            this.Controls.Add(this.pbUpdateDatabase);
            this.Controls.Add(this.lblUpdateDatabase);
            this.Controls.Add(this.pbImportCard);
            this.Controls.Add(this.lblImportCard);
            this.Controls.Add(this.pbImportSet);
            this.Controls.Add(this.lblImportSet);
            this.Controls.Add(this.pbLoadSet);
            this.Controls.Add(this.lblLoadSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OctgnLoaderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OctgnLoaderForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OctgnLoaderForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoadSet;
        private System.Windows.Forms.ProgressBar pbLoadSet;
        private System.Windows.Forms.ProgressBar pbImportSet;
        private System.Windows.Forms.Label lblImportSet;
        private System.Windows.Forms.ProgressBar pbImportCard;
        private System.Windows.Forms.Label lblImportCard;
        private System.Windows.Forms.ProgressBar pbUpdateDatabase;
        private System.Windows.Forms.Label lblUpdateDatabase;
        private System.Windows.Forms.Button btAbort;
    }
}