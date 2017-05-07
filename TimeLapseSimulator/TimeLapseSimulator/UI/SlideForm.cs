using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeLapseSimulator.DataBase.ADO;

namespace TimeLapseSimulator.UI
{
    public partial class SlideForm : Form
    {
        private DBOperate dbOperate;

        public int Row { get; set; }
        public int Colum { get; set; }

        public int CurrentIndex { get; set; }

        public int CellID { get; set; }
        public SlideForm()
        {
            InitializeComponent();
            CurrentIndex = 0;
            this.Load += SlideForm_Load;
            this.FormClosing += SlideForm_FormClosing;
        }

        private void SlideForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CurrentIndex = 0;
            this.SlideTimer.Enabled = false;
        }

        private void SlideForm_Load(object sender, EventArgs e)
        {
            SetWellColor(Row, Colum, Color.DarkGreen);
            this.dataListView.Timer = this.SlideTimer;
            this.SlideTimer.Enabled = true;
            this.SlideTimer.Interval = 5000;
        }

        public void SetSLideName(string name)
        {
            this.slideCtrl.SlideName = name;
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                IList<TSLide> slides =  dbOperate.QuerySlides(this.slideCtrl.SlideName, CellID, CurrentIndex);
                foreach (var slide in slides)
                {
                    this.dataListView.AppendLog(new string[] { slide.Time.ToString("yyyy-MM-dd HH:mm:ss:ms"), slide.SlideName, slide.CellName, slide.FocalName});
                }
                CurrentIndex = CurrentIndex + 10;
            }
            catch (Exception ee)
            {
            }
        }

        public void SetWellColor(int row, int col, Color backColor)
        {
            slideCtrl.SetWellColor(row, col, backColor);
        }
    }
}
