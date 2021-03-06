﻿// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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

        public static bool GetDownloadProgress(string filename, out long downloadedBytes, out long totalBytes, out int downloadProgress)
        {
            lock (Singleton)
            {
                if (!Singleton._downloadedFiles.ContainsKey(filename))
                {
                    downloadedBytes = -1;
                    totalBytes = -1;
                    downloadProgress = -1;
                    return false;
                }

                var info = Singleton._downloadedFiles[filename];
                downloadedBytes = info.BytesReceived;
                totalBytes = info.TotalBytesToReceive;
                downloadProgress = info.DownloadProgress;
                return true;
            }
        }

        public static void CancelDownload(string filename)
        {
            lock (Singleton)
            {
                if (!Singleton._downloadedFiles.ContainsKey(filename))
                    return;
                Singleton._downloadedFiles[filename].BackgroundWorker.CancelAsync();
            }
        }

        private void StartDownload(string url, string filename)
        {
            DownloadInfo di;
            if (!Singleton._downloadedFiles.TryGetValue(filename, out di))
            {
                di = new DownloadInfo();
                Singleton._downloadedFiles[filename] = di;
            }

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
                wc.DownloadProgressChanged += delegate(object o, DownloadProgressChangedEventArgs args)
                {
                    di.BytesReceived = args.BytesReceived;
                    di.TotalBytesToReceive = args.TotalBytesToReceive;
                    di.DownloadProgress = args.ProgressPercentage;
                };
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
        public long BytesReceived { get; set; }
        public long TotalBytesToReceive { get; set; }
        public int DownloadProgress { get; set; }
        public BackgroundWorker BackgroundWorker { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
    }
}
