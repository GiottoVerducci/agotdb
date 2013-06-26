using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GenericDB.OCTGN
{
    public abstract class OctgnManager
    {
        private readonly IOctgnLoader _octgnLoader;

        protected OctgnManager(IOctgnLoader octgnLoader)
        {
            _octgnLoader = octgnLoader;
        }

        public void ImportImages(string imagesFolder)
        {
            var dialog = new OctgnSetSelector { IsUrl = false, Url = null, Items = "images" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.Path;
                var octgnImageWorker = new BackgroundWorker
                {
                    //WorkerReportsProgress = true,
                    //WorkerSupportsCancellation = true
                };

                octgnImageWorker.DoWork += OctgnImageImportWorkerOnDoWork;
                octgnImageWorker.RunWorkerAsync(new object[] { path, imagesFolder });
            }
        }

        private void OctgnImageImportWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var backgroundWorker = (BackgroundWorker)sender;
            var paths = (string[])doWorkEventArgs.Argument;

            _octgnLoader.ImportAllImages(paths[1], paths[0], backgroundWorker);
        }
    }
}
