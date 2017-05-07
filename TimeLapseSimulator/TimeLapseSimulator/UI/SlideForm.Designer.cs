namespace TimeLapseSimulator.UI
{
    partial class SlideForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataListView = new TimeLapseSimulator.UI.LogListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.slideCtrl = new TimeLapseSimulator.UI.SlideCtrl();
            this.SlideTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // dataListView
            // 
            this.dataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.dataListView.FullRowSelect = true;
            this.dataListView.GridLines = true;
            this.dataListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataListView.HideSelection = false;
            this.dataListView.Location = new System.Drawing.Point(120, 12);
            this.dataListView.MaxLogRecords = 300;
            this.dataListView.MultiSelect = false;
            this.dataListView.Name = "dataListView";
            this.dataListView.ShowGroups = false;
            this.dataListView.Size = new System.Drawing.Size(566, 385);
            this.dataListView.TabIndex = 3;
            this.dataListView.Timer = null;
            this.dataListView.UseCompatibleStateImageBehavior = false;
            this.dataListView.View = System.Windows.Forms.View.Details;
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
            // slideCtrl
            // 
            this.slideCtrl.Flashing = false;
            this.slideCtrl.FlickCount = 185;
            this.slideCtrl.ID = 0;
            this.slideCtrl.Location = new System.Drawing.Point(12, 12);
            this.slideCtrl.Name = "slideCtrl";
            this.slideCtrl.Size = new System.Drawing.Size(100, 195);
            this.slideCtrl.SlideName = null;
            this.slideCtrl.TabIndex = 4;
            // 
            // SlideTimer
            // 
            this.SlideTimer.Tick += new System.EventHandler(this.SlideTimer_Tick);
            // 
            // SlideForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 409);
            this.Controls.Add(this.slideCtrl);
            this.Controls.Add(this.dataListView);
            this.Name = "SlideForm";
            this.Text = "SlideForm";
            this.ResumeLayout(false);

        }

        #endregion
        private LogListView dataListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private SlideCtrl slideCtrl;
        private System.Windows.Forms.Timer SlideTimer;
    }
}