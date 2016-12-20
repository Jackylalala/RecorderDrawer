namespace RecorderDrawer
{
    partial class frmRecorderDrawer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecorderDrawer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.munOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.munAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.munStatList = new System.Windows.Forms.ToolStripMenuItem();
            this.munSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.munTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.munDetailedSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportImg = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportImgToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportImgToMail = new System.Windows.Forms.ToolStripMenuItem();
            this.munToClip = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportAnimation = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportAnimationToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.munExportAnimationToMail = new System.Windows.Forms.ToolStripMenuItem();
            this.munAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.pnlChartItems = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pgbProcess = new System.Windows.Forms.ToolStripProgressBar();
            this.chtMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.lblTimeDisplay = new System.Windows.Forms.Label();
            this.chkThreshold = new System.Windows.Forms.CheckBox();
            this.txtMinLimit = new System.Windows.Forms.TextBox();
            this.txtMaxLimit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkYGrid = new System.Windows.Forms.CheckBox();
            this.chkXGrid = new System.Windows.Forms.CheckBox();
            this.bgdWorkerAnimation = new System.ComponentModel.BackgroundWorker();
            this.bgdWorkerDraw = new System.ComponentModel.BackgroundWorker();
            this.bgdWorkerMail = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtMain)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munOpen,
            this.munAnalysis,
            this.munStatList,
            this.munSetting,
            this.munExportImg,
            this.munToClip,
            this.munExportAnimation,
            this.munAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1005, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // munOpen
            // 
            this.munOpen.Name = "munOpen";
            this.munOpen.Size = new System.Drawing.Size(85, 20);
            this.munOpen.Text = "開啟檔案(&O)";
            this.munOpen.Click += new System.EventHandler(this.munOpen_Click);
            // 
            // munAnalysis
            // 
            this.munAnalysis.Enabled = false;
            this.munAnalysis.Name = "munAnalysis";
            this.munAnalysis.Size = new System.Drawing.Size(83, 20);
            this.munAnalysis.Text = "重新分析(&A)";
            this.munAnalysis.Click += new System.EventHandler(this.munAnalysis_Click);
            // 
            // munStatList
            // 
            this.munStatList.Enabled = false;
            this.munStatList.Name = "munStatList";
            this.munStatList.Size = new System.Drawing.Size(84, 20);
            this.munStatList.Text = "統計數據(&D)";
            this.munStatList.Click += new System.EventHandler(this.munStatList_Click);
            // 
            // munSetting
            // 
            this.munSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munTitle,
            this.munDetailedSetting});
            this.munSetting.Enabled = false;
            this.munSetting.Name = "munSetting";
            this.munSetting.Size = new System.Drawing.Size(58, 20);
            this.munSetting.Text = "設定(&S)";
            // 
            // munTitle
            // 
            this.munTitle.Name = "munTitle";
            this.munTitle.Size = new System.Drawing.Size(139, 22);
            this.munTitle.Text = "設定標題(&T)";
            this.munTitle.Click += new System.EventHandler(this.munTitle_Click);
            // 
            // munDetailedSetting
            // 
            this.munDetailedSetting.Name = "munDetailedSetting";
            this.munDetailedSetting.Size = new System.Drawing.Size(139, 22);
            this.munDetailedSetting.Text = "詳細設定(&D)";
            this.munDetailedSetting.Click += new System.EventHandler(this.munDetailedSetting_Click);
            // 
            // munExportImg
            // 
            this.munExportImg.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munExportImgToFile,
            this.munExportImgToMail});
            this.munExportImg.Enabled = false;
            this.munExportImg.Name = "munExportImg";
            this.munExportImg.Size = new System.Drawing.Size(82, 20);
            this.munExportImg.Text = "匯出圖片(&E)";
            // 
            // munExportImgToFile
            // 
            this.munExportImgToFile.Name = "munExportImgToFile";
            this.munExportImgToFile.Size = new System.Drawing.Size(155, 22);
            this.munExportImgToFile.Text = "匯出至檔案(&F)";
            this.munExportImgToFile.Click += new System.EventHandler(this.munExportImgToFile_Click);
            // 
            // munExportImgToMail
            // 
            this.munExportImgToMail.Name = "munExportImgToMail";
            this.munExportImgToMail.Size = new System.Drawing.Size(155, 22);
            this.munExportImgToMail.Text = "以Mail寄出(&M)";
            this.munExportImgToMail.Click += new System.EventHandler(this.munExportImgToMail_Click);
            // 
            // munToClip
            // 
            this.munToClip.Enabled = false;
            this.munToClip.Name = "munToClip";
            this.munToClip.Size = new System.Drawing.Size(131, 20);
            this.munToClip.Text = "複製圖片到剪貼簿(&C)";
            this.munToClip.Click += new System.EventHandler(this.munToClip_Click);
            // 
            // munExportAnimation
            // 
            this.munExportAnimation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.munExportAnimationToFile,
            this.munExportAnimationToMail});
            this.munExportAnimation.Enabled = false;
            this.munExportAnimation.Name = "munExportAnimation";
            this.munExportAnimation.Size = new System.Drawing.Size(85, 20);
            this.munExportAnimation.Text = "匯出動畫(&N)";
            // 
            // munExportAnimationToFile
            // 
            this.munExportAnimationToFile.Name = "munExportAnimationToFile";
            this.munExportAnimationToFile.Size = new System.Drawing.Size(155, 22);
            this.munExportAnimationToFile.Text = "匯出至檔案(&F)";
            this.munExportAnimationToFile.Click += new System.EventHandler(this.munExportAnimationToFile_Click);
            // 
            // munExportAnimationToMail
            // 
            this.munExportAnimationToMail.Name = "munExportAnimationToMail";
            this.munExportAnimationToMail.Size = new System.Drawing.Size(155, 22);
            this.munExportAnimationToMail.Text = "以Mail寄出(&M)";
            this.munExportAnimationToMail.Click += new System.EventHandler(this.munExportAnimationToMail_Click);
            // 
            // munAbout
            // 
            this.munAbout.Name = "munAbout";
            this.munAbout.Size = new System.Drawing.Size(58, 20);
            this.munAbout.Text = "關於(&B)";
            this.munAbout.Click += new System.EventHandler(this.munAbout_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilePath.Enabled = false;
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFilePath.Location = new System.Drawing.Point(10, 27);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(632, 25);
            this.txtFilePath.TabIndex = 3;
            this.txtFilePath.Text = "來源檔案︰";
            // 
            // pnlChartItems
            // 
            this.pnlChartItems.AutoScroll = true;
            this.pnlChartItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlChartItems.Location = new System.Drawing.Point(10, 56);
            this.pnlChartItems.Name = "pnlChartItems";
            this.pnlChartItems.Size = new System.Drawing.Size(985, 81);
            this.pnlChartItems.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(28, 17);
            this.lblStatus.Text = "Idle";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pgbProcess,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 756);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1005, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pgbProcess
            // 
            this.pgbProcess.Name = "pgbProcess";
            this.pgbProcess.Size = new System.Drawing.Size(100, 16);
            this.pgbProcess.Visible = false;
            // 
            // chtMain
            // 
            chartArea1.Name = "ChartArea1";
            this.chtMain.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chtMain.Legends.Add(legend1);
            this.chtMain.Location = new System.Drawing.Point(10, 171);
            this.chtMain.Name = "chtMain";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtMain.Series.Add(series1);
            this.chtMain.Size = new System.Drawing.Size(986, 579);
            this.chtMain.TabIndex = 24;
            this.chtMain.Text = "chart1";
            this.chtMain.Visible = false;
            this.chtMain.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.chtMain_PostPaint);
            this.chtMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseDoubleClick);
            this.chtMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseDown);
            this.chtMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseMove);
            this.chtMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseUp);
            // 
            // lblProcessing
            // 
            this.lblProcessing.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblProcessing.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProcessing.ForeColor = System.Drawing.Color.White;
            this.lblProcessing.Location = new System.Drawing.Point(361, 323);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(271, 81);
            this.lblProcessing.TabIndex = 25;
            this.lblProcessing.Text = "Rendering, please wait...";
            this.lblProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessing.Visible = false;
            // 
            // lblTimeDisplay
            // 
            this.lblTimeDisplay.BackColor = System.Drawing.Color.White;
            this.lblTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTimeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeDisplay.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTimeDisplay.Location = new System.Drawing.Point(10, 142);
            this.lblTimeDisplay.Name = "lblTimeDisplay";
            this.lblTimeDisplay.Size = new System.Drawing.Size(986, 23);
            this.lblTimeDisplay.TabIndex = 26;
            this.lblTimeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkThreshold
            // 
            this.chkThreshold.AutoSize = true;
            this.chkThreshold.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkThreshold.Location = new System.Drawing.Point(773, 29);
            this.chkThreshold.Name = "chkThreshold";
            this.chkThreshold.Size = new System.Drawing.Size(53, 21);
            this.chkThreshold.TabIndex = 27;
            this.chkThreshold.Text = "閥值";
            this.chkThreshold.UseVisualStyleBackColor = true;
            this.chkThreshold.CheckedChanged += new System.EventHandler(this.chkThreshold_Changed);
            // 
            // txtMinLimit
            // 
            this.txtMinLimit.Location = new System.Drawing.Point(825, 28);
            this.txtMinLimit.MaxLength = 8;
            this.txtMinLimit.Name = "txtMinLimit";
            this.txtMinLimit.Size = new System.Drawing.Size(70, 22);
            this.txtMinLimit.TabIndex = 28;
            this.txtMinLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMinLimit_KeyDown);
            this.txtMinLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtThreshold_KeyPress);
            // 
            // txtMaxLimit
            // 
            this.txtMaxLimit.Location = new System.Drawing.Point(925, 28);
            this.txtMaxLimit.MaxLength = 8;
            this.txtMaxLimit.Name = "txtMaxLimit";
            this.txtMaxLimit.Size = new System.Drawing.Size(70, 22);
            this.txtMaxLimit.TabIndex = 29;
            this.txtMaxLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaxLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMaxLimit_KeyDown);
            this.txtMaxLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtThreshold_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(901, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 17);
            this.label1.TabIndex = 30;
            this.label1.Text = "~";
            // 
            // chkYGrid
            // 
            this.chkYGrid.AutoSize = true;
            this.chkYGrid.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkYGrid.Location = new System.Drawing.Point(710, 29);
            this.chkYGrid.Name = "chkYGrid";
            this.chkYGrid.Size = new System.Drawing.Size(61, 21);
            this.chkYGrid.TabIndex = 31;
            this.chkYGrid.Text = "Y格線";
            this.chkYGrid.UseVisualStyleBackColor = true;
            this.chkYGrid.CheckedChanged += new System.EventHandler(this.chkGrid_CheckedChanged);
            // 
            // chkXGrid
            // 
            this.chkXGrid.AutoSize = true;
            this.chkXGrid.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkXGrid.Location = new System.Drawing.Point(648, 29);
            this.chkXGrid.Name = "chkXGrid";
            this.chkXGrid.Size = new System.Drawing.Size(61, 21);
            this.chkXGrid.TabIndex = 32;
            this.chkXGrid.Text = "X格線";
            this.chkXGrid.UseVisualStyleBackColor = true;
            this.chkXGrid.CheckedChanged += new System.EventHandler(this.chkGrid_CheckedChanged);
            // 
            // bgdWorkerAnimation
            // 
            this.bgdWorkerAnimation.WorkerReportsProgress = true;
            this.bgdWorkerAnimation.WorkerSupportsCancellation = true;
            this.bgdWorkerAnimation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgdWorkerAnimation_DoWork);
            this.bgdWorkerAnimation.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorkerAnimation_ProgressChanged);
            this.bgdWorkerAnimation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgdWorkerAnimation_RunWorkerCompleted);
            // 
            // bgdWorkerDraw
            // 
            this.bgdWorkerDraw.WorkerReportsProgress = true;
            this.bgdWorkerDraw.WorkerSupportsCancellation = true;
            this.bgdWorkerDraw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgdWorkerDraw_DoWork);
            this.bgdWorkerDraw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorkerDraw_ProgressChanged);
            this.bgdWorkerDraw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgdWorkerDraw_RunWorkerCompleted);
            // 
            // bgdWorkerMail
            // 
            this.bgdWorkerMail.WorkerReportsProgress = true;
            this.bgdWorkerMail.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgdWorkerMail_DoWork);
            this.bgdWorkerMail.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorkerMail_ProgressChanged);
            this.bgdWorkerMail.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgdWorkerMail_RunWorkerCompleted);
            // 
            // frmRecorderDrawer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 778);
            this.Controls.Add(this.pnlChartItems);
            this.Controls.Add(this.chkXGrid);
            this.Controls.Add(this.chkYGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxLimit);
            this.Controls.Add(this.txtMinLimit);
            this.Controls.Add(this.chkThreshold);
            this.Controls.Add(this.lblTimeDisplay);
            this.Controls.Add(this.lblProcessing);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.chtMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmRecorderDrawer";
            this.Text = "反應槽繪圖器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRecorderDrawer_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmRecorderDrawer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmRecorderDrawer_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem munOpen;
        private System.Windows.Forms.ToolStripMenuItem munAnalysis;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.ToolStripMenuItem munSetting;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem munExportImg;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtMain;
        private System.Windows.Forms.ToolStripMenuItem munToClip;
        internal System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.ToolStripMenuItem munAbout;
        private System.Windows.Forms.Label lblTimeDisplay;
        private System.Windows.Forms.CheckBox chkThreshold;
        private System.Windows.Forms.TextBox txtMinLimit;
        private System.Windows.Forms.TextBox txtMaxLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem munStatList;
        private System.Windows.Forms.CheckBox chkYGrid;
        private System.Windows.Forms.CheckBox chkXGrid;
        private System.Windows.Forms.Panel pnlChartItems;
        private System.ComponentModel.BackgroundWorker bgdWorkerAnimation;
        private System.ComponentModel.BackgroundWorker bgdWorkerDraw;
        private System.Windows.Forms.ToolStripProgressBar pgbProcess;
        private System.Windows.Forms.ToolStripMenuItem munExportAnimation;
        private System.Windows.Forms.ToolStripMenuItem munExportImgToFile;
        private System.Windows.Forms.ToolStripMenuItem munExportImgToMail;
        private System.Windows.Forms.ToolStripMenuItem munExportAnimationToFile;
        private System.Windows.Forms.ToolStripMenuItem munExportAnimationToMail;
        private System.ComponentModel.BackgroundWorker bgdWorkerMail;
        private System.Windows.Forms.ToolStripMenuItem munTitle;
        private System.Windows.Forms.ToolStripMenuItem munDetailedSetting;
    }
}

