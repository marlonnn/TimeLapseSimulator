using Summer.System.Core;
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
using TimeLapseSimulator.Device;
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
            this.KeyDown += SimulatorForm_KeyDown;
            InitializeClickHander();
            operationFactory = new OperationFactory();
            operationFactory.AppendLogHandler += AppendLogHandler;
            operationFactory.SetWellColorHandler += SetWellColorHandler;
            operationFactory.ClearWellColorHandler += ClearWellColorHandler;
            operationFactory.FlashHandler += FlashHandler;
            OperationThread = new Thread(new ThreadStart(operationFactory.ExecuteInternal));
        }

        private void InitializeClickHander()
        {
            this.slideCtrl1.CellMouseClickHandler += CellMouseClickHandler;
            this.slideCtrl2.CellMouseClickHandler += CellMouseClickHandler;
            this.slideCtrl3.CellMouseClickHandler += CellMouseClickHandler;
            this.slideCtrl4.CellMouseClickHandler += CellMouseClickHandler;
        }


        bool IsRowColIndexRight(int row, int col)
        {
            return row > -1 && col > -1;
        }

        private void CellMouseClickHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;    // no action needed when click on the column or row header, or top left cell

            if (IsRowColIndexRight(e.RowIndex, e.ColumnIndex))
            {
                if (e.Button == MouseButtons.Left)
                {
                    int rowIndex = e.RowIndex;
                    int columnIndex = e.ColumnIndex;
                    var grid = sender as DataGridView;
                    if (grid != null && grid.Parent != null)
                    {
                        Plate plate = (Plate)grid.Parent;
                        SlideCtrl slideCtrl = (SlideCtrl)plate.Parent;
                        if (slideCtrl != null)
                        {
                            string slideName = slideCtrl.SlideName;
                            //table: slideName
                            //Cell: 
                            int current = rowIndex * plate.PlateColumns + columnIndex + 1;
                            Slide slide = device.Slides[slideCtrl.ID -1];
                            SlideForm slideForm = SpringHelper.GetObject<SlideForm>("slideForm");
                            slideForm.Row = rowIndex;
                            slideForm.Colum = columnIndex;
                            slideForm.CellID = current;
                            slideForm.Slide = slide;
                            slideForm.SetInfomation();
                            slideForm.ShowDialog();
                        }
                    }
                }
            }
        }

        private void SimulatorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.F5)
            {
                DatabaseForm databaseForm = SpringHelper.GetObject<DatabaseForm>("dbForm");
                databaseForm.ShowDialog();
            }
        }

        private void ClearWellColorHandler()
        {
            this.slideCtrl1.ClearColor();
            this.slideCtrl2.ClearColor();
            this.slideCtrl3.ClearColor();
            this.slideCtrl4.ClearColor();
        }

        private void FlashHandler(int slideID)
        {
            foreach (var ctrl in this.Controls)
            {
                SlideCtrl sc = ctrl as SlideCtrl;
                if (sc != null)
                {
                    if (sc.ID == slideID)
                    {
                        sc.Flashing = true;
                    }
                    else
                    {
                        sc.Flashing = false;
                    }
                }
            }
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
            operationFactory.Execute = true;
            OperationThread.Start();
        }
    }
}
