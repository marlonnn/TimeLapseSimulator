using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeLapseSimulator.DataBase.ADO;
using TimeLapseSimulator.UI;

namespace TimeLapseSimulator
{
    public partial class SimulatorForm : Form
    {
        private TimeLapseSimulator.Device.Device device;
        private OperationFactory operationFactory;
        private Thread OperationThread;
        private DBOperate dbOperate;

        public SimulatorForm()
        {
            InitializeComponent();
            InitializeTimer();
            this.Load += SimulatorForm_Load;
            this.FormClosing += SimulatorForm_FormClosing;
            operationFactory = new OperationFactory();
            operationFactory.AppendLogHandler += AppendLogHandler;
            operationFactory.SetWellColorHandler += SetWellColorHandler;
            OperationThread = new Thread(new ThreadStart(operationFactory.ExecuteInternal));
        }

        private void SimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.LogViewTimer.Stop();
            operationFactory.ThreadRun = false;
            operationFactory.Execute = false;
            OperationThread.Abort();
        }

        private void SetWellColorHandler(int slideID, int row, int col, Color backColor)
        {
            SlideCtrl slideCtrl = GetSlideByID(slideID);
            if (slideCtrl != null)
            {
                slideCtrl.SetWellColor(row, col, backColor);
            }
        }

        private SlideCtrl GetSlideByID(int slideID)
        {
            SlideCtrl s = null;
            foreach (Control ctrl in this.Controls)
            {
                SlideCtrl slidectrl = ctrl as SlideCtrl;
                if (slidectrl != null && slidectrl.ID == slideID)
                {
                    s = slidectrl;
                    break;
                }
            }
            return s;
        }

        private void AppendLogHandler(string[] log)
        {
            this.logListView.AppendLog(log);
        }

        private void InitializeTimer()
        {
            this.logListView.Timer = this.LogViewTimer;
            this.logListView.Timer.Start();
        }

        private void SimulatorForm_Load(object sender, EventArgs e)
        {
            operationFactory.Device = device;
            operationFactory.DBOperate = dbOperate;
            OperationThread.Start();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            bool canExecute = !operationFactory.Execute;
            operationFactory.Execute = canExecute;
            btnStartStop.Text = canExecute ? "Stop" : "Start";

        }
    }
}
