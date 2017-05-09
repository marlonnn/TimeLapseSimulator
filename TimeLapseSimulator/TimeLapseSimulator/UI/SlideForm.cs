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
using TimeLapseSimulator.Device;

namespace TimeLapseSimulator.UI
{
    public partial class SlideForm : Form
    {
        private DBOperate dbOperate;

        public int Row { get; set; }
        public int Colum { get; set; }

        public int CurrentIndex { get; set; }

        public int CellID { get; set; }

        public Slide Slide { get; set; }
        public SlideForm()
        {
            InitializeComponent();
            CurrentIndex = 0;
            this.Load += SlideForm_Load;
            this.FormClosing += SlideForm_FormClosing;
        }

        private void Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;    // no action needed when click on the column or row header, or top left cell
            if (IsRowColIndexRight(e.RowIndex, e.ColumnIndex))
            {
                if (e.Button == MouseButtons.Left)
                {
                    Row = e.RowIndex;
                    Colum = e.ColumnIndex;
                    CellID = Row * plate.PlateColumns + Colum + 1;
                    ClearColor();
                    this.plate.SetWellColor(Row, Colum, Color.DarkGreen);
                    this.SetInfomation();
                }
            }
        }

        bool IsRowColIndexRight(int row, int col)
        {
            return row > -1 && col > -1;
        }

        private void SlideForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CurrentIndex = 0;
        }

        private void SlideForm_Load(object sender, EventArgs e)
        {
            this.plate.Grid.CellMouseClick += Grid_CellMouseClick;
            this.plate.SetWellColor(Row, Colum, Color.DarkGreen);
        }

        public void ClearColor()
        {
            this.plate.ClearWellColor();
        }

        public void SetInfomation()
        {
            this.labelSLide.Text = Slide.Name;
            this.labelCell.Text = CellID.ToString();
            this.labelPName.Text = Slide.Patient.Name;
            this.labelPID.Text = Slide.Patient.ID.ToString();
            this.labelTime.Text = Slide.Patient.Time.ToString("yyyy-MM-dd HH:mm:ss:ms");
        }

    }
}
