using System;
using AGoTDB.BusinessObjects;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace AGoTDB.Services
{
	public sealed class DownloadService
	{
		private readonly Dictionary<string, DownloadInfo> fDownloadedFiles = new Dictionary<string, DownloadInfo>();

		private static readonly DownloadService fDownloadService = new DownloadService();
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
			get { return fDownloadService; }
		}

		public static void DownloadFile(string url, string filename)
		{
			lock (Singleton)
			{
				if (Singleton.fDownloadedFiles.ContainsKey(filename)
					&& Singleton.fDownloadedFiles[filename].DownloadStatus == DownloadStatus.InProgress)
					return;
				Singleton.StartDownload(url, filename);
			}
		}

		public static bool IsDownloadAvailable(string filename)
		{
			lock (Singleton)
			{
				return Singleton.fDownloadedFiles.ContainsKey(filename)
					&& Singleton.fDownloadedFiles[filename].DownloadStatus == DownloadStatus.Success;
			}
		}

		private void StartDownload(string url, string filename)
		{
			var di = Singleton.fDownloadedFiles.ContainsKey(filename)
				? Singleton.fDownloadedFiles[filename]
				: new DownloadInfo();

			di.Filename = filename;
			di.Url = url;
			di.DownloadStatus = DownloadStatus.InProgress;
			di.BackgroundWorker = new BackgroundWorker();
			di.BackgroundWorker.WorkerSupportsCancellation = true;
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
				wc.DownloadFileCompleted += delegate(object s, AsyncCompletedEventArgs ee) { completed = true; };
				wc.DownloadFileAsync(new Uri(di.Url), di.Filename, di);

				// wait until the download is completed or cancelled
				while (!completed && !di.BackgroundWorker.CancellationPending)
				{
					System.Threading.Thread.Sleep(200);
				}

				if (completed)
					di.DownloadStatus = DownloadStatus.Success;
				else
					di.DownloadStatus = DownloadStatus.Fail;
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
