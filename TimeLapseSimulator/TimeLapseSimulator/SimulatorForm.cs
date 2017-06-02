using Summer.System.Core;
using Summer.System.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public List<SlideCtrl> SlideCtrls;

        public const int ColumnCount = 4;

        public SimulatorForm()
        {
            InitializeComponent();
            InitializeTimer();
            this.Load += SimulatorForm_Load;
            this.FormClosing += SimulatorForm_FormClosing;
            this.KeyDown += SimulatorForm_KeyDown;
            operationFactory = new OperationFactory();
            operationFactory.AppendLogHandler += AppendLogHandler;
            operationFactory.SetWellColorHandler += SetWellColorHandler;
            operationFactory.ClearWellColorHandler += ClearWellColorHandler;
            OperationThread = new Thread(new ThreadStart(operationFactory.ExecuteInternal));
        }

        public void InitializeSlideCtrls()
        {
            SlideCtrls = new List<SlideCtrl>();
            for (int i = 0; i < device.Slides.Count; i++)
            {
                SlideCtrl slideCtrl = new SlideCtrl();
                slideCtrl.ID = device.Slides[i].ID;
                slideCtrl.SlideName = device.Slides[i].Name;
                slideCtrl.PlateRows = device.Slides[i].PlateRows;
                slideCtrl.PlateColumns = device.Slides[i].PlateColumns;
                slideCtrl.Location = new System.Drawing.Point(45 + (i % ColumnCount) * slideCtrl.Width, 13 + (i / ColumnCount) * slideCtrl.Height);
                slideCtrl.Name = "slideCtrl" + i + 1;
                slideCtrl.Size = new System.Drawing.Size(100, 195);
                //slideCtrl.SlideName = "slide2";
                slideCtrl.TabIndex = i;
                slideCtrl.CellMouseClickHandler += CellMouseClickHandler;
                this.Controls.Add(slideCtrl);
                SlideCtrls.Add(slideCtrl);
            }
            this.logListView.Location = new Point(0, (13 + SlideCtrls[0].Height) * (1 + device.Slides.Count % ColumnCount) + 13);
            this.Width = (45 + SlideCtrls[0].Width) * ColumnCount + 95;
            this.Height = (13 + SlideCtrls[0].Height) * (1 + device.Slides.Count % ColumnCount) + this.logListView.Height + 13;
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
                            Slide slide = device.Slides[slideCtrl.ID - 1];
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
            foreach (var slideCtrl in SlideCtrls)
            {
                slideCtrl.ClearColor();
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
            InitializeSlideCtrls();
            InitializeImages();
            operationFactory.Device = device;
            operationFactory.DBOperate = dbOperate;
            operationFactory.Execute = true;
            OperationThread.Start();
        }

        private void InitializeImages()
        {
            try
            {
                operationFactory.Images = new List<string>();
                string fileFolder = string.Format("{0}\\Images", System.Environment.CurrentDirectory);
                DirectoryInfo folder = new DirectoryInfo(fileFolder);
                FileInfo[] fileInfos = folder.GetFiles();
                Array.Sort(fileInfos, delegate (FileInfo x, FileInfo y)
                {
                    return Int32.Parse(Path.GetFileNameWithoutExtension(x.Name)).CompareTo
                    (Int32.Parse(Path.GetFileNameWithoutExtension(y.Name)));
                });
                foreach (FileInfo info in fileInfos)
                {
                    operationFactory.Images.Add(info.FullName);
                }
            }
            catch (Exception e)
            {
                LogHelper.GetLogger<SimulatorForm>().Error(e.Message);
            }
        }
    }
}
