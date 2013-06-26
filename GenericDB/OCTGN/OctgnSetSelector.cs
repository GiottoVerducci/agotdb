// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System;
using System.Threading;
using System.Windows.Forms;

using GenericDB.Services;

namespace GenericDB.OCTGN
{
    public partial class OctgnSetSelector : Form
    {
        public bool IsUrl;
        public string Url;
        public string Path;
        public string Items;

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

            rbPath.Text = string.Format(rbPath.Text, Items);
            rbUrl.Text = string.Format(rbUrl.Text, Items);

            // readjust size and position of the text boxes to accomodate the various length of the radiobuttons
            var pathPreviousRight = tbPath.Right;
            var urlPreviousRight = tbUrl.Right;
            tbPath.Left = tbUrl.Left = Math.Max(rbPath.Right, rbUrl.Right) + 10;
            tbPath.Width = pathPreviousRight - tbPath.Left;
            tbUrl.Width = urlPreviousRight - tbUrl.Left;
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
                rbPath.Checked = true;
            }
        }

        private void OctgnSetSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason != CloseReason.UserClosing)
            //    return;
            if (this.DialogResult == DialogResult.OK)
            {
                if (rbUrl.Checked)
                {
                    if (string.IsNullOrWhiteSpace(tbUrl.Text))
                    {
                        MessageBox.Show("Please enter an url.");
                        return;
                    }

                    var path = System.IO.Path.GetTempPath() + System.IO.Path.GetRandomFileName();

                    var loadingForm = new OctgnDownloadForm {Url = tbUrl.Text, Path = path};

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
                else
                {
                    Path = tbPath.Text;
                }
            }
        }

    }
}
