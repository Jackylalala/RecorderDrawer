﻿namespace RecorderDrawer
{
    partial class frmMailer
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.cboBorder = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMailAddress = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReadMailAddress = new System.Windows.Forms.Button();
            this.cboMailAddress = new System.Windows.Forms.ComboBox();
            this.pnlAdvance = new System.Windows.Forms.Panel();
            this.pnlAdvance.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "檔案類型：";
            // 
            // cboImageFormat
            // 
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboImageFormat.FormattingEnabled = true;
            this.cboImageFormat.Items.AddRange(new object[] {
            "Bitmap(*.bmp)",
            "Jpeg(*.jpg,*.jpeg)",
            "GIF(*.gif)",
            "PNG(*.png)",
            "TIFF(*.tif,*.tiff)"});
            this.cboImageFormat.Location = new System.Drawing.Point(114, 8);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(291, 25);
            this.cboImageFormat.TabIndex = 9;
            // 
            // cboBorder
            // 
            this.cboBorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBorder.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboBorder.FormattingEnabled = true;
            this.cboBorder.Items.AddRange(new object[] {
            "無",
            "窄",
            "普通",
            "寬"});
            this.cboBorder.Location = new System.Drawing.Point(114, 40);
            this.cboBorder.Name = "cboBorder";
            this.cboBorder.Size = new System.Drawing.Size(291, 25);
            this.cboBorder.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(11, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "邊框留白寬度：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(14, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "郵件地址：";
            // 
            // txtMailAddress
            // 
            this.txtMailAddress.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMailAddress.Location = new System.Drawing.Point(95, 11);
            this.txtMailAddress.Name = "txtMailAddress";
            this.txtMailAddress.Size = new System.Drawing.Size(315, 25);
            this.txtMailAddress.TabIndex = 7;
            this.txtMailAddress.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOk.Location = new System.Drawing.Point(294, 154);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(114, 31);
            this.btnOk.TabIndex = 31;
            this.btnOk.Text = "確定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(159, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 31);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFileName.Location = new System.Drawing.Point(95, 43);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(315, 25);
            this.txtFileName.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(14, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 33;
            this.label5.Text = "檔案名稱：";
            // 
            // btnReadMailAddress
            // 
            this.btnReadMailAddress.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnReadMailAddress.Location = new System.Drawing.Point(17, 154);
            this.btnReadMailAddress.Name = "btnReadMailAddress";
            this.btnReadMailAddress.Size = new System.Drawing.Size(114, 31);
            this.btnReadMailAddress.TabIndex = 34;
            this.btnReadMailAddress.Text = "讀取通訊錄";
            this.btnReadMailAddress.UseVisualStyleBackColor = true;
            this.btnReadMailAddress.Click += new System.EventHandler(this.btnReadMailAddress_Click);
            // 
            // cboMailAddress
            // 
            this.cboMailAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMailAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMailAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMailAddress.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboMailAddress.FormattingEnabled = true;
            this.cboMailAddress.Location = new System.Drawing.Point(95, 11);
            this.cboMailAddress.Name = "cboMailAddress";
            this.cboMailAddress.Size = new System.Drawing.Size(315, 25);
            this.cboMailAddress.TabIndex = 35;
            // 
            // pnlAdvance
            // 
            this.pnlAdvance.Controls.Add(this.label2);
            this.pnlAdvance.Controls.Add(this.label1);
            this.pnlAdvance.Controls.Add(this.cboImageFormat);
            this.pnlAdvance.Controls.Add(this.cboBorder);
            this.pnlAdvance.Location = new System.Drawing.Point(4, 74);
            this.pnlAdvance.Name = "pnlAdvance";
            this.pnlAdvance.Size = new System.Drawing.Size(416, 73);
            this.pnlAdvance.TabIndex = 36;
            // 
            // frmMailer
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(422, 193);
            this.Controls.Add(this.pnlAdvance);
            this.Controls.Add(this.cboMailAddress);
            this.Controls.Add(this.btnReadMailAddress);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtMailAddress);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMailer";
            this.Text = "以郵件寄出";
            this.pnlAdvance.ResumeLayout(false);
            this.pnlAdvance.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboImageFormat;
        private System.Windows.Forms.ComboBox cboBorder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMailAddress;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnReadMailAddress;
        private System.Windows.Forms.ComboBox cboMailAddress;
        private System.Windows.Forms.Panel pnlAdvance;
    }
}