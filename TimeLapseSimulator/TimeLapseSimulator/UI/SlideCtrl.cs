using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeLapseSimulator.UI
{
    public partial class SlideCtrl : UserControl
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private string slideName;

        [Description("Slide Display Text"), Category("Appearance")]
        public string SlideName
        {
            get
            {
                return slideName;
            }
            set
            {
                slideName = value;
            }
        }

        public int PlateRows { get; set; }

        public int PlateColumns { get; set; }

        private void InitializePlate()
        {
            this.plate = new Plate();
            this.plate.CellsFontSize = 8F;
            this.plate.ColumHeaderFontSize = 9F;
            this.plate.Location = new System.Drawing.Point(0, 0);
            this.plate.Name = "plate";
            this.plate.PlateColumnHeadersHeight = 25;
            this.plate.PlateColumns = PlateColumns;
            this.plate.PlateRowHeadersWidth = 25;
            this.plate.PlateRows = PlateRows;
            this.plate.PlateWellsHeight = 25;
            this.plate.PlateWellsWidth = 25;
            this.plate.RowHeaderFontSize = 9F;
            this.plate.TabIndex = 0;
            this.Controls.Add(this.plate);
        }

        public SlideCtrl()
        {
            InitializeComponent();
            this.Load += SlideCtrl_Load;
        }

        public delegate void CellMouseClickDelegate(object sender, DataGridViewCellMouseEventArgs e);
        public CellMouseClickDelegate CellMouseClickHandler;
        private void Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseClickHandler?.Invoke(sender, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void SlideCtrl_Load(object sender, EventArgs e)
        {
            InitializePlate();

            this.Width = this.plate.Width;
            this.Height = this.plate.Height;
            this.plate.Grid.CellMouseClick += Grid_CellMouseClick;
        }

        public void SetWellColor(int row, int col, Color backColor)
        {
            this.plate.SetWellColor(row, col, backColor);
        }

        public void ClearColor()
        {
            this.plate.ClearWellColor();
        }

    }
}
