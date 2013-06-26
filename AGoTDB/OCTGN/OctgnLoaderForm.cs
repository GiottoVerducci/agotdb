using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AGoTDB.BusinessObjects;
using GenericDB.OCTGN;

namespace AGoTDB.OCTGN
{
    public partial class OctgnLoaderForm : Form
    {
        public BackgroundWorker BackgroundWorker { get; set; }
        public string Path { get; set; }
        public Action Callback { get; set; }
        private bool _isLoadCompleted;

        public OctgnLoaderForm()
        {
            InitializeComponent();
            Shown += OctgnLoaderForm_Shown;
        }

        private void OctgnLoaderForm_Shown(object sender, EventArgs e)
        {
            BackgroundWorker.ProgressChanged += OctgnLoaderWorkerOnProgressChanged;
            BackgroundWorker.RunWorkerCompleted += OctgnLoaderWorkerOnRunWorkerCompleted;
            BackgroundWorker.RunWorkerAsync(this.Path); // @"C:\Users\vripoll\Downloads\Sets"); //
        }

        private void OctgnLoaderWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            var task = (AgotOctgnLoader.OctgnLoaderTask)progressChangedEventArgs.UserState;
            switch (task)
            {
                case AgotOctgnLoader.OctgnLoaderTask.LoadSet:
                    pbLoadSet.Value = progressChangedEventArgs.ProgressPercentage;
                    break;
                case AgotOctgnLoader.OctgnLoaderTask.MatchSet:
                    pbMatchSet.Value = progressChangedEventArgs.ProgressPercentage;
                    break;
                case AgotOctgnLoader.OctgnLoaderTask.FindCard:
                    pbFindCard.Value = progressChangedEventArgs.ProgressPercentage;
                    break;
                case AgotOctgnLoader.OctgnLoaderTask.UpdateDatabase:
                    pbUpdateDatabase.Value = progressChangedEventArgs.ProgressPercentage;
                    btAbort.Enabled = false;
                    break;
            }
            EnforceLabelStyle(lblLoadSet, task == AgotOctgnLoader.OctgnLoaderTask.LoadSet);
            EnforceLabelStyle(lblMatchSet, task == AgotOctgnLoader.OctgnLoaderTask.MatchSet);
            EnforceLabelStyle(lblFindCard, task == AgotOctgnLoader.OctgnLoaderTask.FindCard);
            EnforceLabelStyle(lblUpdateDatabase, task == AgotOctgnLoader.OctgnLoaderTask.UpdateDatabase);
        }

        private static void EnforceLabelStyle(Label label, bool indicatorIsVisible)
        {
            const string indicator = "\u25B6 "; // BLACK RIGHT-POINTING TRIANGLE
            if (label.Text.StartsWith(indicator))
            {
                if (!indicatorIsVisible)
                {
                    label.Text = label.Text.Substring(indicator.Length);
                    label.Font = new Font(label.Font, FontStyle.Regular);
                    label.ForeColor = SystemColors.ControlText;
                }
            }
            else if (indicatorIsVisible)
            {
                label.Text = indicator + label.Text;
                label.Font = new Font(label.Font, FontStyle.Bold);
                label.ForeColor = Color.Green;
            }
        }

        private void OctgnLoaderWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            _isLoadCompleted = true;
            var result = (OctgnLoaderResultAndValue)runWorkerCompletedEventArgs.Result;
            if (result.Result == OctgnLoaderResult.NoSetsFounds)
            {
                MessageBox.Show(Resource1.ErrOctgnNotFound, Resource1.ErrOctgnNotFoundTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (result.Result == OctgnLoaderResult.SetNotDefinedInDatabase)
            {
                var octgnSetNotFound = (AgotOctgnLoader.OctgnSetData)result.Value;
                MessageBox.Show(string.Format(Resource1.ErrOctgnSetNotDefinedInDatabase, octgnSetNotFound.Name, octgnSetNotFound.Id),
                    Resource1.ErrOctgnSetNotDefinedInDatabaseTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!runWorkerCompletedEventArgs.Cancelled)
                ApplicationSettings.IsOctgnReady = true; // ApplicationSettings.DatabaseManager.HasOctgnData();
            this.Close();
            if (this.Callback != null && ApplicationSettings.IsOctgnReady)
                this.Callback();
        }

        private void btAbort_Click(object sender, EventArgs e)
        {
            BackgroundWorker.CancelAsync();
        }

        private void OctgnLoaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var canClose = _isLoadCompleted || btAbort.Enabled;
            e.Cancel = !canClose;
        }

        private void OctgnLoaderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BackgroundWorker.CancelAsync();
        }
    }
}
