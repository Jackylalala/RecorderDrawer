namespace RecorderDrawer
{
    partial class frmAnalysis
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chtDisplay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblVariable = new System.Windows.Forms.Label();
            this.lblDeriOfVariable = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.Label();
            this.txtVariable = new System.Windows.Forms.Label();
            this.txtDeriOfVariable = new System.Windows.Forms.Label();
            this.btnCalcd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chtDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // chtDisplay
            // 
            chartArea2.Name = "ChartArea1";
            this.chtDisplay.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chtDisplay.Legends.Add(legend2);
            this.chtDisplay.Location = new System.Drawing.Point(0, 69);
            this.chtDisplay.Name = "chtDisplay";
            this.chtDisplay.Size = new System.Drawing.Size(784, 492);
            this.chtDisplay.TabIndex = 0;
            this.chtDisplay.Text = "chart1";
            this.chtDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chtDisplay_MouseMove);
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblVariable.Location = new System.Drawing.Point(277, 17);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(44, 17);
            this.lblVariable.TabIndex = 3;
            this.lblVariable.Text = "label1";
            // 
            // lblDeriOfVariable
            // 
            this.lblDeriOfVariable.AutoSize = true;
            this.lblDeriOfVariable.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDeriOfVariable.Location = new System.Drawing.Point(468, 17);
            this.lblDeriOfVariable.Name = "lblDeriOfVariable";
            this.lblDeriOfVariable.Size = new System.Drawing.Size(44, 17);
            this.lblDeriOfVariable.TabIndex = 4;
            this.lblDeriOfVariable.Text = "label2";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTime.Location = new System.Drawing.Point(118, 17);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(44, 17);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "label1";
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.White;
            this.txtTime.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtTime.Location = new System.Drawing.Point(183, 17);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(80, 17);
            this.txtTime.TabIndex = 7;
            this.txtTime.Text = "label2";
            this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtVariable
            // 
            this.txtVariable.BackColor = System.Drawing.Color.White;
            this.txtVariable.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtVariable.Location = new System.Drawing.Point(358, 17);
            this.txtVariable.Name = "txtVariable";
            this.txtVariable.Size = new System.Drawing.Size(80, 17);
            this.txtVariable.TabIndex = 8;
            this.txtVariable.Text = "label3";
            this.txtVariable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDeriOfVariable
            // 
            this.txtDeriOfVariable.BackColor = System.Drawing.Color.White;
            this.txtDeriOfVariable.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtDeriOfVariable.Location = new System.Drawing.Point(518, 17);
            this.txtDeriOfVariable.Name = "txtDeriOfVariable";
            this.txtDeriOfVariable.Size = new System.Drawing.Size(80, 17);
            this.txtDeriOfVariable.TabIndex = 9;
            this.txtDeriOfVariable.Text = "label4";
            this.txtDeriOfVariable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCalcd
            // 
            this.btnCalcd.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCalcd.Location = new System.Drawing.Point(285, 41);
            this.btnCalcd.Name = "btnCalcd";
            this.btnCalcd.Size = new System.Drawing.Size(215, 23);
            this.btnCalcd.TabIndex = 10;
            this.btnCalcd.Text = "熟成時間計算";
            this.btnCalcd.UseVisualStyleBackColor = true;
            this.btnCalcd.Click += new System.EventHandler(this.btnCalcd_Click);
            // 
            // frmAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnCalcd);
            this.Controls.Add(this.txtDeriOfVariable);
            this.Controls.Add(this.txtVariable);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDeriOfVariable);
            this.Controls.Add(this.lblVariable);
            this.Controls.Add(this.chtDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnalysis";
            this.Text = "frmAnalysis";
            ((System.ComponentModel.ISupportInitialize)(this.chtDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart chtDisplay;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.Label lblDeriOfVariable;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label txtTime;
        private System.Windows.Forms.Label txtVariable;
        private System.Windows.Forms.Label txtDeriOfVariable;
        private System.Windows.Forms.Button btnCalcd;


    }
}