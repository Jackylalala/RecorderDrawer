namespace RecorderDrawer
{
    partial class frmDifferential
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
            this.chtMain = new ZedGraph.ZedGraphControl();
            this.lblTimeDisplay = new System.Windows.Forms.Label();
            this.lblOri = new System.Windows.Forms.Label();
            this.lblOriText = new System.Windows.Forms.Label();
            this.lblD1Text = new System.Windows.Forms.Label();
            this.lblD1 = new System.Windows.Forms.Label();
            this.lblD2Text = new System.Windows.Forms.Label();
            this.lblD2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chtMain
            // 
            this.chtMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chtMain.IsAutoScrollRange = true;
            this.chtMain.IsEnableHPan = false;
            this.chtMain.IsEnableHZoom = false;
            this.chtMain.IsEnableSelection = true;
            this.chtMain.IsEnableVPan = false;
            this.chtMain.IsEnableVZoom = false;
            this.chtMain.IsEnableWheelZoom = false;
            this.chtMain.IsShowContextMenu = false;
            this.chtMain.IsVerticalCursorLine = true;
            this.chtMain.LinkModifierKeys = System.Windows.Forms.Keys.None;
            this.chtMain.Location = new System.Drawing.Point(0, 124);
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
            this.chtMain.Size = new System.Drawing.Size(1001, 584);
            this.chtMain.TabIndex = 29;
            this.chtMain.UseExtendedPrintDialog = true;
            this.chtMain.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.chtMain_MouseDownEvent);
            this.chtMain.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.chtMain_MouseUpEvent);
            this.chtMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseDoubleClick);
            this.chtMain.MouseEnter += new System.EventHandler(this.chtMain_MouseEnter);
            this.chtMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chtMain_MouseMove);
            // 
            // lblTimeDisplay
            // 
            this.lblTimeDisplay.BackColor = System.Drawing.Color.White;
            this.lblTimeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTimeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeDisplay.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTimeDisplay.Location = new System.Drawing.Point(0, 93);
            this.lblTimeDisplay.Name = "lblTimeDisplay";
            this.lblTimeDisplay.Size = new System.Drawing.Size(1001, 23);
            this.lblTimeDisplay.TabIndex = 30;
            this.lblTimeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOri
            // 
            this.lblOri.BackColor = System.Drawing.Color.White;
            this.lblOri.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOri.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblOri.Location = new System.Drawing.Point(319, 60);
            this.lblOri.Name = "lblOri";
            this.lblOri.Size = new System.Drawing.Size(105, 23);
            this.lblOri.TabIndex = 31;
            this.lblOri.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOriText
            // 
            this.lblOriText.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblOriText.Location = new System.Drawing.Point(319, 9);
            this.lblOriText.Name = "lblOriText";
            this.lblOriText.Size = new System.Drawing.Size(105, 45);
            this.lblOriText.TabIndex = 33;
            this.lblOriText.Text = "P";
            this.lblOriText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblD1Text
            // 
            this.lblD1Text.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblD1Text.Location = new System.Drawing.Point(444, 9);
            this.lblD1Text.Name = "lblD1Text";
            this.lblD1Text.Size = new System.Drawing.Size(105, 45);
            this.lblD1Text.TabIndex = 35;
            this.lblD1Text.Text = "dP/dt";
            this.lblD1Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblD1
            // 
            this.lblD1.BackColor = System.Drawing.Color.White;
            this.lblD1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblD1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblD1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblD1.Location = new System.Drawing.Point(444, 60);
            this.lblD1.Name = "lblD1";
            this.lblD1.Size = new System.Drawing.Size(105, 23);
            this.lblD1.TabIndex = 34;
            this.lblD1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblD2Text
            // 
            this.lblD2Text.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblD2Text.Location = new System.Drawing.Point(576, 9);
            this.lblD2Text.Name = "lblD2Text";
            this.lblD2Text.Size = new System.Drawing.Size(105, 45);
            this.lblD2Text.TabIndex = 37;
            this.lblD2Text.Text = "d\\u00b2P/dt\\u00b2";
            this.lblD2Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblD2
            // 
            this.lblD2.BackColor = System.Drawing.Color.White;
            this.lblD2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblD2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblD2.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblD2.Location = new System.Drawing.Point(576, 60);
            this.lblD2.Name = "lblD2";
            this.lblD2.Size = new System.Drawing.Size(105, 23);
            this.lblD2.TabIndex = 36;
            this.lblD2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmDifferential
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 708);
            this.Controls.Add(this.lblD2Text);
            this.Controls.Add(this.lblD2);
            this.Controls.Add(this.lblD1Text);
            this.Controls.Add(this.lblD1);
            this.Controls.Add(this.lblOriText);
            this.Controls.Add(this.lblOri);
            this.Controls.Add(this.lblTimeDisplay);
            this.Controls.Add(this.chtMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmDifferential";
            this.Text = "微分計算";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblTimeDisplay;
        private System.Windows.Forms.Label lblOri;
        private System.Windows.Forms.Label lblOriText;
        private System.Windows.Forms.Label lblD1Text;
        private System.Windows.Forms.Label lblD1;
        private System.Windows.Forms.Label lblD2Text;
        private System.Windows.Forms.Label lblD2;
        internal ZedGraph.ZedGraphControl chtMain;
    }
}