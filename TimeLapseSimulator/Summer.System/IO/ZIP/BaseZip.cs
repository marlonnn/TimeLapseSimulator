using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Summer.System.IO.ZIP
{
    public class BaseZip
    {
        private BackgroundWorker _loadFileTask;
        private IZipWorker _worker;

        public BackgroundWorker LoadFileTask
        {
            private set
            {
                this._loadFileTask = value;
            }
            get
            {
                return this._loadFileTask;
            }
        }

        public BaseZip(IZipWorker worker)
        {
            this._worker = worker;
            _loadFileTask = new BackgroundWorker();
            _loadFileTask.WorkerReportsProgress = true;
            _loadFileTask.WorkerSupportsCancellation = true;
            _loadFileTask.DoWork += LoadFileTask_DoWork;
            _loadFileTask.ProgressChanged += LoadFileTask_ProgressChanged;
            _loadFileTask.RunWorkerCompleted += LoadFileTask_RunWorkerCompleted;
        }

        private void LoadFileTask_DoWork(object sender, DoWorkEventArgs e)
        {
            _worker.DoWork(sender, e);
        }

        private void LoadFileTask_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _worker.ProgressChanged(sender, e);
        }

        private void LoadFileTask_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _worker.RunWorkerCompleted(sender, e);
        }
    }
}
