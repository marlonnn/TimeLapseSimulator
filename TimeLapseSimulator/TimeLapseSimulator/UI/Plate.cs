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
    public partial class Plate : UserControl
    {
        private sealed class WellValue
        {
            private Color _color;
            public Color BackColor
            {
                set
                {
                    _color = value;
                }
                get
                {
                    return _color;
                }
            }

            private Color _fontColor = Color.Black;
            public Color FontColor
            {
                set
                {
                    _fontColor = value;
                }
                get
                {
                    return _fontColor;
                }
            }
        }

        private DataGridView grid;

        private int plateRows = 5;
        private int plateColumns = 3;
        private int plateWellsWidth = 25;
        private int plateWellsHeight = 25;
        private int plateColumnHeadersHeight = 25;
        private int plateRowHeadersWidth = 25;
        private float rowHeaderFontSize = 9;
        private float columHeaderFontSize = 9;
        private float cellsFontSize = 8;

        WellValue[,] wellvalues;

        public int PlateRows
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set Plate Rows to zero !");
                }
                plateRows = value;
                InitialPlate();
            }
            get
            {
                return plateRows;
            }
        }
        public int PlateColumns
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set Plate Columns to zero !");
                }
                plateColumns = value;
                InitialPlate();
            }
            get
            {
                return plateColumns;
            }
        }

        public int PlateWellsWidth
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set PlateWellsWidth to zero !");
                }
                plateWellsWidth = value;
                InitialPlate();
            }
            get
            {
                return plateWellsWidth;
            }
        }
        public int PlateWellsHeight
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set Plate Wells Height to zero !");
                }
                plateWellsHeight = value;
                InitialPlate();
            }
            get
            {
                return plateWellsHeight;
            }
        }

        public int PlateColumnHeadersHeight
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set PlateColumnHeadersHeight to zero !");
                }
                plateColumnHeadersHeight = value;
                InitialPlate();
            }
            get
            {
                return plateColumnHeadersHeight;
            }
        }
        public int PlateRowHeadersWidth
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Can not set PlateColumnHeadersHeight to zero !");
                }
                plateRowHeadersWidth = value;
                InitialPlate();
            }
            get
            {
                return plateRowHeadersWidth;
            }
        }

        public float RowHeaderFontSize
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The number must beyond 0 ! ");
                }
                rowHeaderFontSize = value;
                InitialPlate();
            }
            get
            {
                return rowHeaderFontSize;
            }
        }
        public float ColumHeaderFontSize
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The number must beyond 0 ! ");
                }
                columHeaderFontSize = value;
                InitialPlate();
            }
            get
            {
                return columHeaderFontSize;
            }
        }
        public float CellsFontSize
        {
            set
            {
                if (value <= 0)
                {
                    throw new Exception("The number must beyond 0 ! ");
                }
                cellsFontSize = value;
                InitialPlate();
            }
            get
            {
                return cellsFontSize;
            }
        }

        private void InitialPlate()
        {
            if (this.grid == null) return;
            this.grid.Rows.Clear();
            this.grid.Columns.Clear();
            SetWells();
            SetFontSize();
            SetWellsValue();
        }

        private void SetPlate()
        {
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeColumns = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid.ReadOnly = true;
            this.grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.grid.GridColor = System.Drawing.Color.DarkGray;
            this.grid.BorderStyle = BorderStyle.FixedSingle;
            this.grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.grid.EnableHeadersVisualStyles = false;
            this.grid.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 214, 231);
            this.grid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 214, 231);
            this.grid.RowHeadersDefaultCellStyle.Padding = new Padding(this.grid.RowHeadersWidth);
        }

        private void SetWellsValue()
        {
            //_rightClickWell = new WellID();
            //_rightClickWell.InitialWellID();
            wellvalues = new WellValue[PlateRows, PlateColumns];
            for (int i = 0; i < wellvalues.GetLength(0); i++)
            {
                for (int j = 0; j < wellvalues.GetLength(1); j++)
                {
                    wellvalues[i, j] = new WellValue();
                }
            }
            Draw(this.grid[0, 0]);
            //MultiSelectChanged();
        }

        private void SetFontSize()
        {
            this.grid.RowHeadersDefaultCellStyle.Font = new Font("Arial", RowHeaderFontSize);
            this.grid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", ColumHeaderFontSize);
            this.grid.DefaultCellStyle.Font = new Font("Arial", CellsFontSize);
        }

        private void SetWells()
        {
            for (int i = 1; i <= PlateColumns; i++)
            {
                DataGridViewImageColumn imgcol = new DataGridViewImageColumn();
                imgcol.HeaderText = i.ToString();
                imgcol.Width = PlateWellsWidth;
                this.grid.Columns.Add(imgcol);
            }
            System.Drawing.Image image = new System.Drawing.Bitmap(PlateWellsWidth, PlateWellsHeight);
            using (Graphics graphhic = System.Drawing.Graphics.FromImage(image))
            {
                using (Brush brush = new SolidBrush(Color.White))
                {
                    graphhic.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));
                }
            }
            for (int i = 0; i < PlateRows; i++)
            {
                this.grid.Rows.Add();
                this.grid.Rows[i].Height = PlateWellsHeight;
                for (int j = 0; j < PlateColumns; j++)
                {
                    this.grid[j, i].Value = image;
                }
            }
            this.grid.ColumnHeadersHeight = PlateColumnHeadersHeight;
            this.grid.RowHeadersWidth = PlateRowHeadersWidth;
            SetSize();
        }

        private void SetSize()
        {
            this.Width = PlateRowHeadersWidth + PlateWellsWidth * PlateColumns;
            this.Height = PlateColumnHeadersHeight + PlateWellsHeight * PlateRows;
        }

        public Plate()
        {
            InitializeComponent();
            this.Load += Plate_Load;
        }

        private void Plate_Load(object sender, EventArgs e)
        {
            NewAPlate();
            InitialPlate();
        }

        private void NewAPlate()
        {
            this.grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "Plate";
            SetPlate();
            this.grid.TabIndex = 0;
            this.grid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(Plate_RowPostPaint);
            //this.grid.CellStateChanged += new DataGridViewCellStateChangedEventHandler(Plate_CellStateChanged);
            //this.grid.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(Plate_RowHeaderMouseClick);
            //this.grid.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(Plate_ColumnHeaderMouseClick);
            //this.grid.CellMouseClick += new DataGridViewCellMouseEventHandler(Plate_CellMouseClick);
            //this.grid.MouseDown += new MouseEventHandler(Plate_MouseDown);
            //this.grid.CurrentCellChanged += new EventHandler(Plate_CurrentCellChanged);
            //this.grid.CellDoubleClick += new DataGridViewCellEventHandler(Plate_CellDoubleClick);
            this.Controls.Add(this.grid);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }

        private void Plate_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
              Convert.ToInt32(e.RowBounds.Location.Y + (e.RowBounds.Height - this.grid.RowHeadersDefaultCellStyle.Font.Size) / 2),
              this.grid.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, string.Format("{0}", (char)('A' + e.RowIndex)), this.grid.RowHeadersDefaultCellStyle.Font,
                rectangle, this.grid.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.Right);
        }

        //画背景色
        private void DrawColor(DataGridViewCell dvc)
        {
            Image image = new Bitmap(dvc.Value as Image);
            if (image == null) return;
            using (Graphics graphhic = System.Drawing.Graphics.FromImage(image))
            {
                using (Brush brush = new SolidBrush(wellvalues[dvc.RowIndex, dvc.ColumnIndex].BackColor))
                {
                    if (wellvalues[dvc.RowIndex, dvc.ColumnIndex].BackColor == Color.Gray
                        || wellvalues[dvc.RowIndex, dvc.ColumnIndex].BackColor == Color.White)
                    {
                        graphhic.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));
                    }
                    else
                    {
                        graphhic.FillRectangle(brush, new Rectangle(2, 2, image.Width - 5, image.Height - 5));
                    }
                    dvc.Value = (System.Drawing.Bitmap)(image);
                }
            }
        }

        private void Draw(DataGridViewCell dvc)
        {
            if (dvc.Value == null) return;
            DrawColor(dvc);
        }

        /// <summary>
        /// 画Item背景色
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="backColor"></param>
        public void SetWellColor(int row, int col, Color backColor)
        {
            wellvalues[row, col].BackColor = backColor;
            Draw(this.grid[col, row]);
        }
    }
}
