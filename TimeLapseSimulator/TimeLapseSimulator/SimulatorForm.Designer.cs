namespace TimeLapseSimulator
{
    partial class SimulatorForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.logListView1 = new TimeLapseSimulator.UI.LogListView();
            this.plate3 = new TimeLapseSimulator.UI.Plate();
            this.plate2 = new TimeLapseSimulator.UI.Plate();
            this.plate1 = new TimeLapseSimulator.UI.Plate();
            this.SuspendLayout();
            // 
            // logListView1
            // 
            this.logListView1.FullRowSelect = true;
            this.logListView1.GridLines = true;
            this.logListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.logListView1.HideSelection = false;
            this.logListView1.Location = new System.Drawing.Point(12, 224);
            this.logListView1.MaxLogRecords = 300;
            this.logListView1.MultiSelect = false;
            this.logListView1.Name = "logListView1";
            this.logListView1.ShowGroups = false;
            this.logListView1.Size = new System.Drawing.Size(657, 283);
            this.logListView1.TabIndex = 2;
            this.logListView1.Timer = null;
            this.logListView1.UseCompatibleStateImageBehavior = false;
            this.logListView1.View = System.Windows.Forms.View.Details;
            // 
            // plate3
            // 
            this.plate3.CellsFontSize = 8F;
            this.plate3.ColumHeaderFontSize = 9F;
            this.plate3.Location = new System.Drawing.Point(469, 51);
            this.plate3.Name = "plate3";
            this.plate3.PlateColumnHeadersHeight = 25;
            this.plate3.PlateColumns = 3;
            this.plate3.PlateRowHeadersWidth = 25;
            this.plate3.PlateRows = 5;
            this.plate3.PlateWellsHeight = 25;
            this.plate3.PlateWellsWidth = 25;
            this.plate3.RowHeaderFontSize = 9F;
            this.plate3.Size = new System.Drawing.Size(102, 152);
            this.plate3.TabIndex = 2;
            // 
            // plate2
            // 
            this.plate2.CellsFontSize = 8F;
            this.plate2.ColumHeaderFontSize = 9F;
            this.plate2.Location = new System.Drawing.Point(255, 51);
            this.plate2.Name = "plate2";
            this.plate2.PlateColumnHeadersHeight = 25;
            this.plate2.PlateColumns = 3;
            this.plate2.PlateRowHeadersWidth = 25;
            this.plate2.PlateRows = 5;
            this.plate2.PlateWellsHeight = 25;
            this.plate2.PlateWellsWidth = 25;
            this.plate2.RowHeaderFontSize = 9F;
            this.plate2.Size = new System.Drawing.Size(102, 152);
            this.plate2.TabIndex = 1;
            // 
            // plate1
            // 
            this.plate1.CellsFontSize = 8F;
            this.plate1.ColumHeaderFontSize = 9F;
            this.plate1.Location = new System.Drawing.Point(56, 51);
            this.plate1.Name = "plate1";
            this.plate1.PlateColumnHeadersHeight = 25;
            this.plate1.PlateColumns = 3;
            this.plate1.PlateRowHeadersWidth = 25;
            this.plate1.PlateRows = 5;
            this.plate1.PlateWellsHeight = 25;
            this.plate1.PlateWellsWidth = 25;
            this.plate1.RowHeaderFontSize = 9F;
            this.plate1.Size = new System.Drawing.Size(102, 152);
            this.plate1.TabIndex = 0;
            // 
            // SimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 519);
            this.Controls.Add(this.logListView1);
            this.Controls.Add(this.plate3);
            this.Controls.Add(this.plate2);
            this.Controls.Add(this.plate1);
            this.Name = "SimulatorForm";
            this.Text = "Simulator Form";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Plate plate1;
        private UI.Plate plate2;
        private UI.Plate plate3;
        private UI.LogListView logListView1;
    }
}

