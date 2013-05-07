using System;
using System.Windows.Forms;
using GenericDB.Services;

namespace NRADB.OCTGN
{
    public partial class OctgnDownloadForm : Form
    {
        public string Url;
        public string Path;

        public OctgnDownloadForm()
        {
            InitializeComponent();
        }

        private void OctgnDownloadForm_Load(object sender, EventArgs e)
        {
            DownloadService.DownloadFile(Url, Path);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DownloadService.CancelDownload(Path);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            long bytes, totalBytes; int progress;
            if (DownloadService.GetDownloadProgress(Path, out bytes, out totalBytes, out progress))
            {
                progressBar.Value = progress;
                lblProgress.Text = string.Format("{0}/{1} KB downloaded", bytes / 1024, totalBytes / 1024);
            }
            if (DownloadService.IsDownloadAvailable(Path))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
