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

        private IList<TSLide> slides;

        private int currentIndex;

        private bool updateValueChanged = true;
        public SlideForm()
        {
            InitializeComponent();
            CurrentIndex = 0;
            currentIndex = 0;
            this.Load += SlideForm_Load;
            this.FormClosing += SlideForm_FormClosing;
            slides = new List<TSLide>();
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
                    this.timer.Enabled = false;
                    this.labelInfo.Text = string.Format("Manual stoped!");
                    this.plate.SetWellColor(Row, Colum, Color.DarkGreen);
                    this.updateValueChanged = false;
                    this.FocusSlider.Value = 1;
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
            this.labelFocus.Text = "1";
            this.labelPName.Text = Slide.Patient.Name;
            this.labelPID.Text = Slide.Patient.ID.ToString();
            this.labelTime.Text = Slide.Patient.Time.ToString("yyyy-MM-dd HH:mm:ss:ms");
            this.slides = dbOperate.QuerySlides(Slide.Name, CellID, 1);
        }

        private void FocusSlider_ValueChanged(object sender, EventArgs e)
        {
            if (updateValueChanged)
            {
                this.timer.Enabled = false;
                int focalID = this.FocusSlider.Value;
                this.labelFocus.Text = focalID.ToString();
                this.slides = dbOperate.QuerySlides(Slide.Name, CellID, focalID);
            }
            updateValueChanged = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.timer.Enabled = !this.timer.Enabled;
            if (!this.timer.Enabled)
                this.labelInfo.Text = string.Format("Manual stoped!");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.slides != null)
            {
                if (this.slides.Count == 0)
                {
                    this.timer.Enabled = false;
                    this.labelInfo.Text = "Database image is empty!";
                }
                else
                {
                    Image image = null;
                    switch (dbOperate.queryMode)
                    {
                        case "Binary":
                            image = Camera.ByteArrayToImage(this.slides[currentIndex].Image);
                            this.pictureBox.Image = image;
                            break;
                        case "ImagePath":
                            this.pictureBox.Image = Image.FromFile(this.slides[currentIndex].ImagePath);
                            break;
                    }
                    this.labelInfo.Text = string.Format("Starting...Current Image is {0} of amount {1}", currentIndex + 1, this.slides.Count);
                    currentIndex++;
                    if (currentIndex == this.slides.Count)
                    {
                        currentIndex = 0;
                        this.timer.Enabled = false;
                        this.labelInfo.Text = string.Format("Stop...Current Image is {0} of amount {1}", this.slides.Count, this.slides.Count);
                    }
                }
            }
        }

        private void DisposeImages()
        {
        }
    }
}
