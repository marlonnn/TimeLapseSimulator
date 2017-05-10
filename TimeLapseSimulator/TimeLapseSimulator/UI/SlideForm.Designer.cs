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
            this.labelFocus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FocusSlider = new DevComponents.DotNetBar.Controls.Slider();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.labelInfo = new System.Windows.Forms.Label();
            this.plate = new TimeLapseSimulator.UI.Plate();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
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
            this.groupBox1.Controls.Add(this.labelFocus);
            this.groupBox1.Controls.Add(this.label2);
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
            this.groupBox1.Size = new System.Drawing.Size(220, 197);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTime.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelTime.Location = new System.Drawing.Point(18, 173);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(40, 12);
            this.labelTime.TabIndex = 10;
            this.labelTime.Text = "label";
            // 
            // labelPName
            // 
            this.labelPName.AutoSize = true;
            this.labelPName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPName.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelPName.Location = new System.Drawing.Point(89, 145);
            this.labelPName.Name = "labelPName";
            this.labelPName.Size = new System.Drawing.Size(40, 12);
            this.labelPName.TabIndex = 9;
            this.labelPName.Text = "label";
            // 
            // labelPID
            // 
            this.labelPID.AutoSize = true;
            this.labelPID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPID.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelPID.Location = new System.Drawing.Point(89, 116);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(40, 12);
            this.labelPID.TabIndex = 8;
            this.labelPID.Text = "label";
            // 
            // labelCell
            // 
            this.labelCell.AutoSize = true;
            this.labelCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCell.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelCell.Location = new System.Drawing.Point(89, 59);
            this.labelCell.Name = "labelCell";
            this.labelCell.Size = new System.Drawing.Size(40, 12);
            this.labelCell.TabIndex = 7;
            this.labelCell.Text = "label";
            // 
            // labelSLide
            // 
            this.labelSLide.AutoSize = true;
            this.labelSLide.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSLide.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelSLide.Location = new System.Drawing.Point(89, 28);
            this.labelSLide.Name = "labelSLide";
            this.labelSLide.Size = new System.Drawing.Size(40, 12);
            this.labelSLide.TabIndex = 6;
            this.labelSLide.Text = "label";
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
            this.lblPName.Location = new System.Drawing.Point(6, 145);
            this.lblPName.Name = "lblPName";
            this.lblPName.Size = new System.Drawing.Size(83, 12);
            this.lblPName.TabIndex = 3;
            this.lblPName.Text = "Patient Name:";
            // 
            // lblPID
            // 
            this.lblPID.AutoSize = true;
            this.lblPID.Location = new System.Drawing.Point(18, 116);
            this.lblPID.Name = "lblPID";
            this.lblPID.Size = new System.Drawing.Size(71, 12);
            this.lblPID.TabIndex = 2;
            this.lblPID.Text = "Patient ID:";
            // 
            // labelFocus
            // 
            this.labelFocus.AutoSize = true;
            this.labelFocus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFocus.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelFocus.Location = new System.Drawing.Point(89, 87);
            this.labelFocus.Name = "labelFocus";
            this.labelFocus.Size = new System.Drawing.Size(40, 12);
            this.labelFocus.TabIndex = 12;
            this.labelFocus.Text = "label";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "Focus   ID:";
            // 
            // FocusSlider
            // 
            // 
            // 
            // 
            this.FocusSlider.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.FocusSlider.LabelVisible = false;
            this.FocusSlider.Location = new System.Drawing.Point(711, 12);
            this.FocusSlider.Maximum = 7;
            this.FocusSlider.Minimum = 1;
            this.FocusSlider.Name = "FocusSlider";
            this.FocusSlider.Size = new System.Drawing.Size(33, 493);
            this.FocusSlider.SliderOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.FocusSlider.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.FocusSlider.TabIndex = 3;
            this.FocusSlider.TrackMarker = false;
            this.FocusSlider.Value = 1;
            this.FocusSlider.ValueChanged += new System.EventHandler(this.FocusSlider_ValueChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(238, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(467, 493);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(238, 511);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelInfo.Location = new System.Drawing.Point(328, 516);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(187, 12);
            this.labelInfo.TabIndex = 10;
            this.labelInfo.Text = "Click start button to play";
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
            this.ClientSize = new System.Drawing.Size(756, 557);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.FocusSlider);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.plate);
            this.Name = "SlideForm";
            this.Text = "SlideForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label labelFocus;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.Slider FocusSlider;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label labelInfo;
    }
}