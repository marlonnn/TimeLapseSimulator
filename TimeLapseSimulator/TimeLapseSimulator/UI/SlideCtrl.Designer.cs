namespace TimeLapseSimulator.UI
{
    partial class SlideCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.plate = new TimeLapseSimulator.UI.Plate();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(29, 164);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 12);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Side 1";
            // 
            // plate
            // 
            this.plate.CellsFontSize = 8F;
            this.plate.ColumHeaderFontSize = 9F;
            this.plate.Location = new System.Drawing.Point(0, 0);
            this.plate.Name = "plate";
            this.plate.PlateColumnHeadersHeight = 25;
            this.plate.PlateColumns = 3;
            this.plate.PlateRowHeadersWidth = 25;
            this.plate.PlateRows = 5;
            this.plate.PlateWellsHeight = 25;
            this.plate.PlateWellsWidth = 25;
            this.plate.RowHeaderFontSize = 9F;
            this.plate.TabIndex = 0;
            // 
            // SlideCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.plate);
            this.Name = "SlideCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Plate plate;
        private System.Windows.Forms.Label lblName;
    }
}
