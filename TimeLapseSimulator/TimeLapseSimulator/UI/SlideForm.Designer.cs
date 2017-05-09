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
            this.lblSlide = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelPName = new System.Windows.Forms.Label();
            this.labelPID = new System.Windows.Forms.Label();
            this.labelCell = new System.Windows.Forms.Label();
            this.labelSLide = new System.Windows.Forms.Label();
            this.lblCell = new System.Windows.Forms.Label();
            this.lblPName = new System.Windows.Forms.Label();
            this.lblPID = new System.Windows.Forms.Label();
            this.plate = new TimeLapseSimulator.UI.Plate();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSlide
            // 
            this.lblSlide.AutoSize = true;
            this.lblSlide.Location = new System.Drawing.Point(18, 28);
            this.lblSlide.Name = "lblSlide";
            this.lblSlide.Size = new System.Drawing.Size(71, 12);
            this.lblSlide.TabIndex = 1;
            this.lblSlide.Text = "Slide Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelTime);
            this.groupBox1.Controls.Add(this.labelPName);
            this.groupBox1.Controls.Add(this.labelPID);
            this.groupBox1.Controls.Add(this.labelCell);
            this.groupBox1.Controls.Add(this.labelSLide);
            this.groupBox1.Controls.Add(this.lblCell);
            this.groupBox1.Controls.Add(this.lblPName);
            this.groupBox1.Controls.Add(this.lblPID);
            this.groupBox1.Controls.Add(this.lblSlide);
            this.groupBox1.Location = new System.Drawing.Point(12, 348);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 165);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTime.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelTime.Location = new System.Drawing.Point(18, 140);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(26, 12);
            this.labelTime.TabIndex = 10;
            // 
            // labelPName
            // 
            this.labelPName.AutoSize = true;
            this.labelPName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPName.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelPName.Location = new System.Drawing.Point(89, 112);
            this.labelPName.Name = "labelPName";
            this.labelPName.Size = new System.Drawing.Size(0, 12);
            this.labelPName.TabIndex = 9;
            // 
            // labelPID
            // 
            this.labelPID.AutoSize = true;
            this.labelPID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPID.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelPID.Location = new System.Drawing.Point(89, 83);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(0, 12);
            this.labelPID.TabIndex = 8;
            // 
            // labelCell
            // 
            this.labelCell.AutoSize = true;
            this.labelCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCell.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelCell.Location = new System.Drawing.Point(89, 59);
            this.labelCell.Name = "labelCell";
            this.labelCell.Size = new System.Drawing.Size(0, 12);
            this.labelCell.TabIndex = 7;
            // 
            // labelSLide
            // 
            this.labelSLide.AutoSize = true;
            this.labelSLide.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSLide.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelSLide.Location = new System.Drawing.Point(89, 28);
            this.labelSLide.Name = "labelSLide";
            this.labelSLide.Size = new System.Drawing.Size(0, 12);
            this.labelSLide.TabIndex = 6;
            // 
            // lblCell
            // 
            this.lblCell.AutoSize = true;
            this.lblCell.Location = new System.Drawing.Point(24, 59);
            this.lblCell.Name = "lblCell";
            this.lblCell.Size = new System.Drawing.Size(65, 12);
            this.lblCell.TabIndex = 5;
            this.lblCell.Text = "Cell   ID:";
            // 
            // lblPName
            // 
            this.lblPName.AutoSize = true;
            this.lblPName.Location = new System.Drawing.Point(6, 112);
            this.lblPName.Name = "lblPName";
            this.lblPName.Size = new System.Drawing.Size(83, 12);
            this.lblPName.TabIndex = 3;
            this.lblPName.Text = "Patient Name:";
            // 
            // lblPID
            // 
            this.lblPID.AutoSize = true;
            this.lblPID.Location = new System.Drawing.Point(18, 83);
            this.lblPID.Name = "lblPID";
            this.lblPID.Size = new System.Drawing.Size(71, 12);
            this.lblPID.TabIndex = 2;
            this.lblPID.Text = "Patient ID:";
            // 
            // plate
            // 
            this.plate.CellsFontSize = 8F;
            this.plate.ColumHeaderFontSize = 9F;
            this.plate.Location = new System.Drawing.Point(12, 12);
            this.plate.Name = "plate";
            this.plate.PlateColumnHeadersHeight = 55;
            this.plate.PlateColumns = 3;
            this.plate.PlateRowHeadersWidth = 55;
            this.plate.PlateRows = 5;
            this.plate.PlateWellsHeight = 55;
            this.plate.PlateWellsWidth = 55;
            this.plate.RowHeaderFontSize = 9F;
            this.plate.Size = new System.Drawing.Size(220, 330);
            this.plate.TabIndex = 0;
            // 
            // SlideForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 518);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.plate);
            this.Name = "SlideForm";
            this.Text = "SlideForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Plate plate;
        private System.Windows.Forms.Label lblSlide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblPID;
        private System.Windows.Forms.Label lblCell;
        private System.Windows.Forms.Label lblPName;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelPName;
        private System.Windows.Forms.Label labelPID;
        private System.Windows.Forms.Label labelCell;
        private System.Windows.Forms.Label labelSLide;
    }
}