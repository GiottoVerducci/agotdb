using System;
using System.Threading;
using System.Windows.Forms;

using AndroidDB;
using GenericDB.Services;

namespace NRADB.OCTGN
{
    public partial class OctgnSetSelector : Form
    {
        public bool IsUrl;
        public string Url;
        public string Path;

        public OctgnSetSelector()
        {
            InitializeComponent();
        }

        private void OctgnSetSelector_Load(object sender, EventArgs e)
        {
            tbPath.Text = Path;
            tbUrl.Text = Url;
            rbPath.Checked = !IsUrl;
            rbUrl.Checked = IsUrl;
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            tbPath.Enabled = rbPath.Checked;
            tbUrl.Enabled = rbUrl.Checked;
            btBrowsePath.Enabled = rbPath.Checked;
        }

        private void btBrowsePath_Click(object sender, EventArgs e)
        {
            //var dialog = new FolderBrowserDialog
            //{
            //    Description = Resource1.OctgnFolderSelectPromptMessage,
            //    ShowNewFolderButton = false
            //};

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Title = Resource1.OctgnFileSelectPromptMessage,
                Filter = "OCTGN definition files|*.o8c",
                Multiselect = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = dialog.FileName;
            }
        }

        private void OctgnSetSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason != CloseReason.UserClosing)
            //    return;
            if (this.DialogResult == DialogResult.OK && rbUrl.Checked)
            {
                if (string.IsNullOrWhiteSpace(tbUrl.Text))
                {
                    MessageBox.Show("Please enter an url.");
                    return;
                }

                var path = System.IO.Path.GetTempPath() + System.IO.Path.GetRandomFileName();

                var loadingForm = new OctgnDownloadForm { Url = tbUrl.Text, Path = path };

                if (loadingForm.ShowDialog() == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Path = path;
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
        }

    }
}
