// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace GenericDB.Services
{
    public sealed class DownloadService
    {
        private readonly Dictionary<string, DownloadInfo> _downloadedFiles = new Dictionary<string, DownloadInfo>();

        private static readonly DownloadService _downloadService = new DownloadService();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit (for singleton template implementation)
        static DownloadService()
        {
        }

        private DownloadService()
        {
        }

        /// <summary>
        /// Gets the unique shared singleton instance of this class.
        /// </summary>
        private static DownloadService Singleton
        {
            get { return _downloadService; }
        }

        public static void DownloadFile(string url, string filename)
        {
            lock (Singleton)
            {
                if (Singleton._downloadedFiles.ContainsKey(filename)
                    && Singleton._downloadedFiles[filename].DownloadStatus == DownloadStatus.InProgress)
                    return;
                Singleton.StartDownload(url, filename);
            }
        }

        public static bool IsDownloadAvailable(string filename)
        {
            lock (Singleton)
            {
                return Singleton._downloadedFiles.ContainsKey(filename)
                    && Singleton._downloadedFiles[filename].DownloadStatus == DownloadStatus.Success;
            }
        }

        private void StartDownload(string url, string filename)
        {
            var di = Singleton._downloadedFiles.ContainsKey(filename)
                ? Singleton._downloadedFiles[filename]
                : new DownloadInfo();

            di.Filename = filename;
            di.Url = url;
            di.DownloadStatus = DownloadStatus.InProgress;
            di.BackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            di.BackgroundWorker.DoWork += DoDownload;
            di.BackgroundWorker.RunWorkerCompleted += RunDownloadCompleted;
            di.BackgroundWorker.RunWorkerAsync(di);
        }

        private void RunDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void DoDownload(object sender, DoWorkEventArgs e)
        {
            bool completed = false;
            var di = (DownloadInfo)e.Argument;
            using (var wc = new WebClient())
            {
                wc.DownloadFileCompleted += delegate(object s, AsyncCompletedEventArgs ee)
                    {
                        di.DownloadStatus = ee.Error != null ? DownloadStatus.Fail : DownloadStatus.Success;
                        completed = true;
                    };
                wc.DownloadFileAsync(new Uri(di.Url), di.Filename, di);

                // wait until the download is completed or cancelled
                while (!completed && !di.BackgroundWorker.CancellationPending)
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
        }
    }

    internal enum DownloadStatus
    {
        Undefined,
        InProgress,
        Success,
        Fail
    }

    internal class DownloadInfo
    {
        public DownloadStatus DownloadStatus { get; set; }
        public BackgroundWorker BackgroundWorker { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
    }
}
