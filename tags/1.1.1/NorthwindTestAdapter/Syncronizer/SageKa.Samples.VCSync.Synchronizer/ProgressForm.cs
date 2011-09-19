#region Usings

using System.Windows.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using SageKa.Samples.VcSync.SyncEngine;
using System.Runtime.InteropServices;

#endregion

namespace SageKa.Samples.VCSync.Synchronizer
{
    public partial class ProgressForm : Form
    {
        #region Class variables

        private readonly Engine _syncEngine;
        private int _nextSyncStateMessageIndex = -1;
        private Timer _t;
        private object reportLock = new object();
        private int _step = 0;  // holds the progress for preogress bar. Here it is a fake. Every log message will increment this value.

        #endregion

        #region Ctor.

        public ProgressForm()
        {
            InitializeComponent();
        }

        public ProgressForm(Engine syncEngine)
            : this()
        {
            _syncEngine = syncEngine;
        }

        #endregion

        #region UI Handlers

        private void ProgressForm_Load(object sender, System.EventArgs e)
        {
            // initialize sync engine start
            this._nextSyncStateMessageIndex = 0;

            this.btnClose.Enabled = false;
            this.richTextBox1.Clear();

            // Create a timer that will raise an event every 500 ms and start it.
            // The timer is used to check the synchronization state in a certain time period
            // and to display the current state to the user.
            _t = new Timer();
            _t.Interval = 500;
            _t.Tick += new EventHandler(timer_Tick);
            _t.Start();

            // Start the background worker, to easily run the switch between UI thread and timer thread
            bw.RunWorkerAsync();
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw.IsBusy)
                e.Cancel = true;
        }

        #endregion

        #region BACKGROUND WORKER EVENTS

        // Called when the background worker starts
        private void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // reset the progress
            bw.ReportProgress(0);

            // Start the syncEngine
            _syncEngine.Run();
        }

        // Running on the UI thread. Here we report the current sync state.
        private void bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            lock (reportLock)
            {
                SyncState syncState = e.UserState as SyncState;
                if (null != syncState)
                {
                    for (int i = _nextSyncStateMessageIndex; _nextSyncStateMessageIndex < syncState.Messages.Count; _nextSyncStateMessageIndex++)
                    {
                        this.WriteTraceLine(syncState.Messages[_nextSyncStateMessageIndex]);
                    }
                }

                this.progressBar1.Value = e.ProgressPercentage;
            }
        }
        // Running on the UI thread. Here we report the end of the synchronization.
        private void bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _t.Stop();
            lock (reportLock)
            {
                this.progressBar1.Value = 100;

                FifoStackLogger logger = _syncEngine.Logger as FifoStackLogger;

                if (null == logger)
                    this.WriteTraceLine("Invalid Logger instance!");

                SyncState syncState = logger.SyncState;
                
                if (null != syncState)
                {
                    for (int i = _nextSyncStateMessageIndex; _nextSyncStateMessageIndex < syncState.Messages.Count; _nextSyncStateMessageIndex++)
                    {
                        this.WriteTraceLine(syncState.Messages[_nextSyncStateMessageIndex]);
                    }

                    if (!string.IsNullOrEmpty(syncState.ErrorMsg))
                        this.WriteTraceLine("An error occurred: " + Environment.NewLine + syncState.ErrorMsg);
                }
            }
            this.btnClose.Enabled = true;
        }

        #endregion

        #region TIMER EVENT

        // gets and reports the current sync state
        private void timer_Tick(object sender, EventArgs e)
        {
            FifoStackLogger logger = _syncEngine.Logger as FifoStackLogger;

            if (null == logger)
                this.WriteTraceLine("Invalid Logger instance!");

            SyncState syncState = logger.SyncState;
            if (null != syncState)
            {
                if (_step < 100)
                    _step++;

                // invoke progress changed event at background worker.
                bw.ReportProgress(_step, syncState);
            }
        }

        #endregion

        #region UI MESSAGE WRITERS

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, Int32 wMsg, IntPtr wParam, IntPtr lParam);

        private void WriteTraceLine(string message)
        {
            richTextBox1.AppendText(message + Environment.NewLine);
            SendMessage(richTextBox1.Handle, 277, new IntPtr(7), IntPtr.Zero);
        }

        #endregion

    }
}
