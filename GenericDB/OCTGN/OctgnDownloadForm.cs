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
using System.Windows.Forms;
using GenericDB.Services;

namespace GenericDB.OCTGN
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
