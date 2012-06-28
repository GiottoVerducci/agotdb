using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace AGoTDB.Forms
{
	public partial class SplashScreen : Form
	{
		private  volatile bool _mustShow;
        private  int _step;

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
            {
                RefreshWaitLabel();
                Show();
                timer1.Start();
            }
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

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            _step = (_step + 1) % 4;
            RefreshWaitLabel();
        }

	    private void RefreshWaitLabel()
	    {
            label1.Text = new string('.', _step);
	    }
	}
}
