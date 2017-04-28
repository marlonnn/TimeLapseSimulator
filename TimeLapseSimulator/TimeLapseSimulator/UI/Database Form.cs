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
    public partial class DatabaseForm : Form
    {
        private DBOperate dbOperate;

        public DatabaseForm()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            this.dbListView.Timer = this.timer;
            this.dbListView.Timer.Start();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            IList<TSLide> slides = dbOperate.QueryAllSlide();
            foreach (TSLide slide in slides)
            {
                dbListView.AppendLog(new string[] { slide.Time.ToString("yyyy-MM-dd HH:mm:ss:ms"), slide.SlideID.ToString(), slide.SlideName,
                slide.CellID.ToString(), slide.CellName, slide.FocalName});
            }
        }
    }
}
