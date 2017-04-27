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
            this.components = new System.ComponentModel.Container();
            this.LogViewTimer = new System.Windows.Forms.Timer(this.components);
            this.slideCtrl4 = new TimeLapseSimulator.UI.SlideCtrl();
            this.slideCtrl3 = new TimeLapseSimulator.UI.SlideCtrl();
            this.slideCtrl2 = new TimeLapseSimulator.UI.SlideCtrl();
            this.slideCtrl1 = new TimeLapseSimulator.UI.SlideCtrl();
            this.logListView = new TimeLapseSimulator.UI.LogListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // LogViewTimer
            // 
            this.LogViewTimer.Interval = 300;
            // 
            // slideCtrl4
            // 
            this.slideCtrl4.ID = 4;
            this.slideCtrl4.Location = new System.Drawing.Point(511, 13);
            this.slideCtrl4.Name = "slideCtrl4";
            this.slideCtrl4.Size = new System.Drawing.Size(100, 195);
            this.slideCtrl4.SlideName = "Slide 4";
            this.slideCtrl4.TabIndex = 6;
            // 
            // slideCtrl3
            // 
            this.slideCtrl3.ID = 3;
            this.slideCtrl3.Location = new System.Drawing.Point(357, 13);
            this.slideCtrl3.Name = "slideCtrl3";
            this.slideCtrl3.Size = new System.Drawing.Size(100, 195);
            this.slideCtrl3.SlideName = "Slide 3";
            this.slideCtrl3.TabIndex = 5;
            // 
            // slideCtrl2
            // 
            this.slideCtrl2.ID = 2;
            this.slideCtrl2.Location = new System.Drawing.Point(203, 13);
            this.slideCtrl2.Name = "slideCtrl2";
            this.slideCtrl2.Size = new System.Drawing.Size(100, 195);
            this.slideCtrl2.SlideName = "Slide 2";
            this.slideCtrl2.TabIndex = 4;
            // 
            // slideCtrl1
            // 
            this.slideCtrl1.ID = 1;
            this.slideCtrl1.Location = new System.Drawing.Point(47, 13);
            this.slideCtrl1.Name = "slideCtrl1";
            this.slideCtrl1.Size = new System.Drawing.Size(100, 195);
            this.slideCtrl1.SlideName = "Slide 1";
            this.slideCtrl1.TabIndex = 3;
            // 
            // logListView
            // 
            this.logListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.logListView.FullRowSelect = true;
            this.logListView.GridLines = true;
            this.logListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.logListView.HideSelection = false;
            this.logListView.Location = new System.Drawing.Point(12, 224);
            this.logListView.MaxLogRecords = 300;
            this.logListView.MultiSelect = false;
            this.logListView.Name = "logListView";
            this.logListView.ShowGroups = false;
            this.logListView.Size = new System.Drawing.Size(657, 284);
            this.logListView.TabIndex = 2;
            this.logListView.Timer = null;
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Slide Name";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Cell Name";
            this.columnHeader3.Width = 99;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Focal";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Image";
            this.columnHeader5.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Status";
            // 
            // SimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 519);
            this.Controls.Add(this.slideCtrl4);
            this.Controls.Add(this.slideCtrl3);
            this.Controls.Add(this.slideCtrl2);
            this.Controls.Add(this.slideCtrl1);
            this.Controls.Add(this.logListView);
            this.Name = "SimulatorForm";
            this.Text = "Simulator Form";
            this.ResumeLayout(false);

        }

        #endregion
        private UI.LogListView logListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private UI.SlideCtrl slideCtrl1;
        private UI.SlideCtrl slideCtrl2;
        private UI.SlideCtrl slideCtrl3;
        private System.Windows.Forms.Timer LogViewTimer;
        private UI.SlideCtrl slideCtrl4;
    }
}

