namespace TimeLapseSimulator
{
    partial class Form1
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
            this.plate1 = new TimeLapseSimulator.UI.Plate();
            this.plate2 = new TimeLapseSimulator.UI.Plate();
            this.plate3 = new TimeLapseSimulator.UI.Plate();
            this.SuspendLayout();
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
            // plate2
            // 
            this.plate2.CellsFontSize = 8F;
            this.plate2.ColumHeaderFontSize = 9F;
            this.plate2.Location = new System.Drawing.Point(201, 51);
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
            // plate3
            // 
            this.plate3.CellsFontSize = 8F;
            this.plate3.ColumHeaderFontSize = 9F;
            this.plate3.Location = new System.Drawing.Point(348, 51);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 262);
            this.Controls.Add(this.plate3);
            this.Controls.Add(this.plate2);
            this.Controls.Add(this.plate1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Plate plate1;
        private UI.Plate plate2;
        private UI.Plate plate3;
    }
}

