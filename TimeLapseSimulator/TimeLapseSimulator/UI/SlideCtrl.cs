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

        //private Timer FlashTimer;

        private int flickCount;

        public int FlickCount
        {
            get { return this.flickCount; }
            set { this.flickCount = value; }
        }
        private bool flashing;
        public bool Flashing
        {
            get { return this.flashing; }
            set
            {
                if (value != this.flashing)
                {
                    flickCount = 0;
                    this.flashing = value;
                    this.Invalidate();
                }

            }
        }

        public SlideCtrl()
        {
            InitializeComponent();
            this.Load += SlideCtrl_Load;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            flickCount++;
            if (Flashing)
            {
                FlickerColor(flickCount);
            }
            else
            {
                FlickerColor(0);
            }
        }

        private void SlideCtrl_Load(object sender, EventArgs e)
        {
            this.Width = this.plate.Width;
            this.Height = this.plate.Height + 45;
            this.lblName.Text = SlideName;
        }

        public void SetWellColor(int row, int col, Color backColor)
        {
            this.plate.SetWellColor(row, col, backColor);
        }

        public void ClearColor()
        {
            this.plate.ClearWellColor();
        }

        public void FlickerColor(int cycle)
        {
            this.lblName.ForeColor = cycle % 2 == 0 ? this.ForeColor : Color.Red;
            this.lblName.Invalidate();
        }
    }
}
