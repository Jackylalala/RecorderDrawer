using System;
using System.ComponentModel;

namespace RecorderDrawer
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStatList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCalculation = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDifferential = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDetailedSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRawdata = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToClip = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportImg = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecorderFig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.pnlChartItems = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInformation = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProcessTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProcessingInfo = new System.Windows.Forms.Label();
            this.lblTimeDisplay = new System.Windows.Forms.Label();
            this.txtMinLimit = new System.Windows.Forms.TextBox();
            this.txtMaxLimit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkYGrid = new System.Windows.Forms.CheckBox();
            this.chkXGrid = new System.Windows.Forms.CheckBox();
            this.bgdWorkerDraw = new System.ComponentModel.BackgroundWorker();
            this.pnlChartSetting = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.chtMain = new ZedGraph.ZedGraphControl();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlChartSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuOpenFolder,
            this.mnuAnalysis,
            this.mnuStatList,
            this.mnuCalculation,
            this.mnuSetting,
            this.mnuRawdata,
            this.mnuToClip,
            this.mnuExportImg,
            this.mnuRecorderFig,
            this.mnuAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1005, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(85, 20);
            this.mnuOpen.Text = "開啟檔案(&O)";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuOpenFolder
            // 
            this.mnuOpenFolder.Name = "mnuOpenFolder";
            this.mnuOpenFolder.Size = new System.Drawing.Size(99, 20);
            this.mnuOpenFolder.Text = "讀取記憶卡(&M)";
            this.mnuOpenFolder.Click += new System.EventHandler(this.mnuOpenFolder_Click);
            // 
            // mnuAnalysis
            // 
            this.mnuAnalysis.Enabled = false;
            this.mnuAnalysis.Name = "mnuAnalysis";
            this.mnuAnalysis.Size = new System.Drawing.Size(83, 20);
            this.mnuAnalysis.Text = "重新讀取(&R)";
            this.mnuAnalysis.Click += new System.EventHandler(this.mnuAnalysis_Click);
            // 
            // mnuStatList
            // 
            this.mnuStatList.Enabled = false;
            this.mnuStatList.Name = "mnuStatList";
            this.mnuStatList.Size = new System.Drawing.Size(84, 20);
            this.mnuStatList.Text = "統計數據(&D)";
            this.mnuStatList.Click += new System.EventHandler(this.mnuStatList_Click);
            // 
            // mnuCalculation
            // 
            this.mnuCalculation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDifferential});
            this.mnuCalculation.Enabled = false;
            this.mnuCalculation.Name = "mnuCalculation";
            this.mnuCalculation.Size = new System.Drawing.Size(60, 20);
            this.mnuCalculation.Text = "計算(&U)";
            // 
            // mnuDifferential
            // 
            this.mnuDifferential.Name = "mnuDifferential";
            this.mnuDifferential.Size = new System.Drawing.Size(98, 22);
            this.mnuDifferential.Text = "微分";
            this.mnuDifferential.Click += new System.EventHandler(this.mnuDifferential_Click);
            // 
            // mnuSetting
            // 
            this.mnuSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTitle,
            this.mnuDetailedSetting});
            this.mnuSetting.Enabled = false;
            this.mnuSetting.Name = "mnuSetting";
            this.mnuSetting.Size = new System.Drawing.Size(58, 20);
            this.mnuSetting.Text = "設定(&S)";
            // 
            // mnuTitle
            // 
            this.mnuTitle.Name = "mnuTitle";
            this.mnuTitle.Size = new System.Drawing.Size(139, 22);
            this.mnuTitle.Text = "設定標題(&T)";
            this.mnuTitle.Click += new System.EventHandler(this.mnuTitle_Click);
            // 
            // mnuDetailedSetting
            // 
            this.mnuDetailedSetting.Name = "mnuDetailedSetting";
            this.mnuDetailedSetting.Size = new System.Drawing.Size(139, 22);
            this.mnuDetailedSetting.Text = "詳細設定(&D)";
            this.mnuDetailedSetting.Click += new System.EventHandler(this.mnuDetailedSetting_Click);
            // 
            // mnuRawdata
            // 
            this.mnuRawdata.Enabled = false;
            this.mnuRawdata.Name = "mnuRawdata";
            this.mnuRawdata.Size = new System.Drawing.Size(81, 20);
            this.mnuRawdata.Text = "數據列表(&L)";
            this.mnuRawdata.Click += new System.EventHandler(this.mnuRawdata_Click);
            // 
            // mnuToClip
            // 
            this.mnuToClip.Enabled = false;
            this.mnuToClip.Name = "mnuToClip";
            this.mnuToClip.Size = new System.Drawing.Size(131, 20);
            this.mnuToClip.Text = "複製圖片到剪貼簿(&C)";
            this.mnuToClip.Click += new System.EventHandler(this.mnuToClip_Click);
            // 
            // mnuExportImg
            // 
            this.mnuExportImg.Enabled = false;
            this.mnuExportImg.Name = "mnuExportImg";
            this.mnuExportImg.Size = new System.Drawing.Size(82, 20);
            this.mnuExportImg.Text = "匯出圖片(&E)";
            this.mnuExportImg.Click += new System.EventHandler(this.mnuExportImg_Click);
            // 
            // mnuRecorderFig
            // 
            this.mnuRecorderFig.Name = "mnuRecorderFig";
            this.mnuRecorderFig.Size = new System.Drawing.Size(105, 20);
            this.mnuRecorderFig.Text = "控制器編號圖(&F)";
            this.mnuRecorderFig.Click += new System.EventHandler(this.mnuRecorderFig_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(58, 20);
            this.mnuAbout.Text = "關於(&B)";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFilePath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtFilePath.Location = new System.Drawing.Point(12, 27);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(630, 25);
            this.txtFilePath.TabIndex = 3;
            // 
            // pnlChartItems
            // 
            this.pnlChartItems.AutoScroll = true;
            this.pnlChartItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlChartItems.Location = new System.Drawing.Point(10, 56);
            this.pnlChartItems.Name = "pnlChartItems";
            this.pnlChartItems.Size = new System.Drawing.Size(985, 111);
            this.pnlChartItems.TabIndex = 0;
            this.pnlChartItems.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInformation,
            this.lblProcessTime,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 786);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1005, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInformation
            // 
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(0, 17);
            // 
            // lblProcessTime
            // 
            this.lblProcessTime.Name = "lblProcessTime";
            this.lblProcessTime.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(753, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(237, 17);
            this.toolStripStatusLabel2.Text = "Mon-Wei Hsiao Copyright ©  2015-2020";
            // 
            // lblProcessingInfo
            // 
            this.lblProcessingInfo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblProcessingInfo.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProcessingInfo.ForeColor = System.Drawing.Color.White;
            this.lblProcessingInfo.Location = new System.Drawing.Point(367, 368);
            this.lblProcessingInfo.Name = "lblProcessingInfo";
            this.lblProcessingInfo.Size = new System.Drawing.Size(271, 36);
            this.lblProcessingInfo.TabIndex = 25;
            this.lblProcessingInfo.Text = "Now Processing";
            this.lblProcessingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessingInfo.Visible = false;
            // 
            // lblTimeDisplay
            // 
            this.lblTimeDisplay.BackColor = System.Drawing.Color.White;
            this.lblTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTimeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeDisplay.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTimeDisplay.Location = new System.Drawing.Point(10, 172);
            this.lblTimeDisplay.Name = "lblTimeDisplay";
            this.lblTimeDisplay.Size = new System.Drawing.Size(986, 23);
            this.lblTimeDisplay.TabIndex = 26;
            this.lblTimeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTimeDisplay.Visible = false;
            // 
            // txtMinLimit
            // 
            this.txtMinLimit.Location = new System.Drawing.Point(180, 1);
            this.txtMinLimit.MaxLength = 8;
            this.txtMinLimit.Name = "txtMinLimit";
            this.txtMinLimit.Size = new System.Drawing.Size(70, 22);
            this.txtMinLimit.TabIndex = 28;
            this.txtMinLimit.Tag = "false";
            this.txtMinLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinLimit.TextChanged += new System.EventHandler(this.txtLimit_TextChanged);
            this.txtMinLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLimit_KeyDown);
            this.txtMinLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputOnlyNumber);
            this.txtMinLimit.Leave += new System.EventHandler(this.DataLimitChage);
            // 
            // txtMaxLimit
            // 
            this.txtMaxLimit.Location = new System.Drawing.Point(280, 1);
            this.txtMaxLimit.MaxLength = 8;
            this.txtMaxLimit.Name = "txtMaxLimit";
            this.txtMaxLimit.Size = new System.Drawing.Size(70, 22);
            this.txtMaxLimit.TabIndex = 29;
            this.txtMaxLimit.Tag = "false";
            this.txtMaxLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaxLimit.TextChanged += new System.EventHandler(this.txtLimit_TextChanged);
            this.txtMaxLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLimit_KeyDown);
            this.txtMaxLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputOnlyNumber);
            this.txtMaxLimit.Leave += new System.EventHandler(this.DataLimitChage);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(256, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 17);
            this.label1.TabIndex = 30;
            this.label1.Text = "~";
            // 
            // chkYGrid
            // 
            this.chkYGrid.AutoSize = true;
            this.chkYGrid.Checked = true;
            this.chkYGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkYGrid.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkYGrid.Location = new System.Drawing.Point(65, 2);
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
            this.chkXGrid.Checked = true;
            this.chkXGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXGrid.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkXGrid.Location = new System.Drawing.Point(3, 2);
            this.chkXGrid.Name = "chkXGrid";
            this.chkXGrid.Size = new System.Drawing.Size(61, 21);
            this.chkXGrid.TabIndex = 32;
            this.chkXGrid.Text = "X格線";
            this.chkXGrid.UseVisualStyleBackColor = true;
            this.chkXGrid.CheckedChanged += new System.EventHandler(this.chkGrid_CheckedChanged);
            // 
            // bgdWorkerDraw
            // 
            this.bgdWorkerDraw.WorkerReportsProgress = true;
            this.bgdWorkerDraw.WorkerSupportsCancellation = true;
            this.bgdWorkerDraw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgdWorkerDraw_DoWork);
            this.bgdWorkerDraw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgdWorkerDraw_ProgressChanged);
            this.bgdWorkerDraw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgdWorkerDraw_RunWorkerCompleted);
            // 
            // pnlChartSetting
            // 
            this.pnlChartSetting.Controls.Add(this.label3);
            this.pnlChartSetting.Controls.Add(this.chkXGrid);
            this.pnlChartSetting.Controls.Add(this.chkYGrid);
            this.pnlChartSetting.Controls.Add(this.label1);
            this.pnlChartSetting.Controls.Add(this.txtMaxLimit);
            this.pnlChartSetting.Controls.Add(this.txtMinLimit);
            this.pnlChartSetting.Location = new System.Drawing.Point(645, 29);
            this.pnlChartSetting.Name = "pnlChartSetting";
            this.pnlChartSetting.Size = new System.Drawing.Size(356, 25);
            this.pnlChartSetting.TabIndex = 0;
            this.pnlChartSetting.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(144, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 33;
            this.label3.Text = "閥值";
            // 
            // chtMain
            // 
            this.chtMain.IsEnableHPan = false;
            this.chtMain.IsEnableHZoom = false;
            this.chtMain.IsEnableSelection = true;
            this.chtMain.IsEnableVPan = false;
            this.chtMain.IsEnableVZoom = false;
            this.chtMain.IsEnableWheelZoom = false;
            this.chtMain.IsShowContextMenu = false;
            this.chtMain.IsVerticalCursorLine = true;
            this.chtMain.LinkModifierKeys = System.Windows.Forms.Keys.None;
            this.chtMain.Location = new System.Drawing.Point(10, 199);
            this.chtMain.Name = "chtMain";
            this.chtMain.PanButtons = System.Windows.Forms.MouseButtons.None;
            this.chtMain.PanButtons2 = System.Windows.Forms.MouseButtons.None;
            this.chtMain.PanModifierKeys = System.Windows.Forms.Keys.None;
            this.chtMain.ScrollGrace = 0D;
            this.chtMain.ScrollMaxX = 0D;
            this.chtMain.ScrollMaxY = 0D;
            this.chtMain.ScrollMaxY2 = 0D;
            this.chtMain.ScrollMinX = 0D;
            this.chtMain.ScrollMinY = 0D;
            this.chtMain.ScrollMinY2 = 0D;
            this.chtMain.SelectionBoxColor = System.Drawing.Color.MistyRose;
            this.chtMain.SelectionStyle = ZedGraph.ZedGraphControl.SelectionStyles.SemiTransparentBox;
            this.chtMain.SelectModifierKeys = System.Windows.Forms.Keys.None;
            this.chtMain.Size = new System.Drawing.Size(986, 584);
            this.chtMain.TabIndex = 28;
            this.chtMain.UseExtendedPrintDialog = true;
            this.chtMain.Visible = false;
            this.chtMain.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.chtMain_MouseDown);
            this.chtMain.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.chtMain_MouseUp);
            this.chtMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseDoubleClick);
            this.chtMain.MouseEnter += new System.EventHandler(this.chtMain_MouseEnter);
            this.chtMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseMove);
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 808);
            this.Controls.Add(this.pnlChartSetting);
            this.Controls.Add(this.pnlChartItems);
            this.Controls.Add(this.lblTimeDisplay);
            this.Controls.Add(this.lblProcessingInfo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.chtMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "高壓反應器趨勢圖繪圖程式";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRecorderDrawer_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmRecorderDrawer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmRecorderDrawer_DragEnter);
            this.DragLeave += new System.EventHandler(this.frmMain_DragLeave);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlChartSetting.ResumeLayout(false);
            this.pnlChartSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuAnalysis;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.ToolStripMenuItem mnuSetting;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuExportImg;
        private System.Windows.Forms.ToolStripMenuItem mnuToClip;
        internal System.Windows.Forms.Label lblProcessingInfo;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.Label lblTimeDisplay;
        private System.Windows.Forms.TextBox txtMinLimit;
        private System.Windows.Forms.TextBox txtMaxLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem mnuStatList;
        private System.Windows.Forms.CheckBox chkYGrid;
        private System.Windows.Forms.CheckBox chkXGrid;
        private System.Windows.Forms.Panel pnlChartItems;
        private System.ComponentModel.BackgroundWorker bgdWorkerDraw;
        private System.Windows.Forms.ToolStripMenuItem mnuTitle;
        private System.Windows.Forms.ToolStripMenuItem mnuDetailedSetting;
        private System.Windows.Forms.ToolStripMenuItem mnuRecorderFig;
        private System.Windows.Forms.ToolStripMenuItem mnuRawdata;
        private System.Windows.Forms.ToolStripStatusLabel lblInformation;
        private System.Windows.Forms.Panel pnlChartSetting;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenFolder;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessTime;
        private ZedGraph.ZedGraphControl chtMain;
        private System.Windows.Forms.ToolStripMenuItem mnuCalculation;
        private System.Windows.Forms.ToolStripMenuItem mnuDifferential;
    }
}

