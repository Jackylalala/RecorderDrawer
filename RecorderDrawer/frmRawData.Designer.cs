namespace RecorderDrawer
{
    partial class frmRawData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgvDisplay = new System.Windows.Forms.DataGridView();
            this.chkMail = new System.Windows.Forms.CheckBox();
            this.lblProcessingInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOk.Location = new System.Drawing.Point(260, 481);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 31);
            this.btnOk.TabIndex = 29;
            this.btnOk.Text = "確定修改";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(127, 481);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 31);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnExport.Location = new System.Drawing.Point(393, 481);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(127, 31);
            this.btnExport.TabIndex = 31;
            this.btnExport.Text = "匯出檔案";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgvDisplay
            // 
            this.dgvDisplay.AllowUserToAddRows = false;
            this.dgvDisplay.AllowUserToDeleteRows = false;
            this.dgvDisplay.AllowUserToResizeColumns = false;
            this.dgvDisplay.AllowUserToResizeRows = false;
            this.dgvDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDisplay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDisplay.ColumnHeadersHeight = 24;
            this.dgvDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDisplay.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDisplay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDisplay.Location = new System.Drawing.Point(-1, -1);
            this.dgvDisplay.Name = "dgvDisplay";
            this.dgvDisplay.RowHeadersVisible = false;
            this.dgvDisplay.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDisplay.RowTemplate.Height = 24;
            this.dgvDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDisplay.Size = new System.Drawing.Size(780, 476);
            this.dgvDisplay.TabIndex = 32;
            this.dgvDisplay.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDisplay_CellValueChanged);
            // 
            // chkMail
            // 
            this.chkMail.AutoSize = true;
            this.chkMail.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkMail.Location = new System.Drawing.Point(526, 486);
            this.chkMail.Name = "chkMail";
            this.chkMail.Size = new System.Drawing.Size(92, 21);
            this.chkMail.TabIndex = 48;
            this.chkMail.Text = "以Mail寄出";
            this.chkMail.UseVisualStyleBackColor = true;
            // 
            // lblProcessingInfo
            // 
            this.lblProcessingInfo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblProcessingInfo.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProcessingInfo.ForeColor = System.Drawing.Color.White;
            this.lblProcessingInfo.Location = new System.Drawing.Point(255, 241);
            this.lblProcessingInfo.Name = "lblProcessingInfo";
            this.lblProcessingInfo.Size = new System.Drawing.Size(271, 36);
            this.lblProcessingInfo.TabIndex = 49;
            this.lblProcessingInfo.Text = "Now Processing";
            this.lblProcessingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessingInfo.Visible = false;
            // 
            // frmRawData
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(780, 519);
            this.Controls.Add(this.lblProcessingInfo);
            this.Controls.Add(this.chkMail);
            this.Controls.Add(this.dgvDisplay);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRawData";
            this.ShowIcon = false;
            this.Text = "原始數據";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        public System.Windows.Forms.DataGridView dgvDisplay;
        private System.Windows.Forms.CheckBox chkMail;
        internal System.Windows.Forms.Label lblProcessingInfo;
    }
}