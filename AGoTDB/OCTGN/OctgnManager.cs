using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AGoTDB.OCTGN
{
    public static class OctgnManager
    {
        public static void PromptForInitialization(Action callback = null)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select path for OCTGN set files",
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var octgnLoaderWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                octgnLoaderWorker.DoWork += OctgnLoaderWorkerOnDoWork;

                var loaderForm = new OctgnLoaderForm { BackgroundWorker = octgnLoaderWorker, Path = dialog.SelectedPath, Callback = callback };
                loaderForm.ShowDialog();
            }
        }

        private static void OctgnLoaderWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var backgroundWorker = (BackgroundWorker)sender;
            var path = (string)doWorkEventArgs.Argument;
            var sets = OctgnLoader.LoadAllSets(path, backgroundWorker);
            if (sets.Count == 0 || sets.All(s => s.Value.Count == 0))
            {
                doWorkEventArgs.Result = OctgnLoader.OctgnLoaderResult.SetsNotFound;
                return;
            }
            OctgnLoader.UpdateCards(sets, backgroundWorker);
            doWorkEventArgs.Result = OctgnLoader.OctgnLoaderResult.Success;
        }
    }
}