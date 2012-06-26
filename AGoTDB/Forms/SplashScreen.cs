using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AGoTDB.Forms
{
	public partial class SplashScreen : Form
	{
		public volatile bool _mustShow;
		public SplashScreen()
		{
			InitializeComponent();
			backgroundWorker1.DoWork += backgroundWorker1_DoWork;
			backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			var delay = (int) e.Argument;
			Thread.Sleep(delay);
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
		{
			if (_mustShow)
				Show();
		}

		public void Show(int delay)
		{
			_mustShow = true;
			backgroundWorker1.RunWorkerAsync(delay);
		}

		public void CancelAndClose()
		{
			_mustShow = false;
			Close();
		}
	}
}
