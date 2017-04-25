using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Summer.System.IO.ZIP
{
    public interface IZipWorker
    {
        void DoWork(object sender, DoWorkEventArgs e);

        void ProgressChanged(object sender, ProgressChangedEventArgs e);

        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);
    }
}
