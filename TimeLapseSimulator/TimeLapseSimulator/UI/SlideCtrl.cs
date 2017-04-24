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
                if (value != slideName)
                {
                    slideName = value;
                    this.Invalidate();
                    InvokeInvalidate(value);
                }
            }
        }

        /// <summary>
        /// invoke change slide name
        /// </summary>
        /// <param name="value"></param>
        private void InvokeInvalidate(string value)
        {
            if (!IsHandleCreated)
                return;
            try
            {
                this.Invoke(
                    (MethodInvoker)delegate 
                    {
                        SlideName = value;
                        this.lblName.Text = SlideName;
                    });
            }
            catch { }
        }
        public SlideCtrl()
        {
            InitializeComponent();
            this.Load += SlideCtrl_Load;
        }

        private void SlideCtrl_Load(object sender, EventArgs e)
        {
            this.Width = this.plate.Width;
            this.Height = this.plate.Height + 30;
            this.lblName.Text = SlideName;
        }

        public void SetWellColor(int row, int col, Color backColor)
        {
            this.plate.SetWellColor(row, col, backColor);
        }
    }
}
