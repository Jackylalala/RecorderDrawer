﻿namespace RecorderDrawer
{
    partial class frmTypeSelector
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
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnRecorderFig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblType.Location = new System.Drawing.Point(34, 8);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(112, 17);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "請選擇控制器編號";
            // 
            // cboType
            // 
            this.cboType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(16, 30);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(121, 25);
            this.cboType.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.Location = new System.Drawing.Point(12, 59);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(156, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnRecorderFig
            // 
            this.btnRecorderFig.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRecorderFig.Location = new System.Drawing.Point(144, 30);
            this.btnRecorderFig.Name = "btnRecorderFig";
            this.btnRecorderFig.Size = new System.Drawing.Size(20, 24);
            this.btnRecorderFig.TabIndex = 3;
            this.btnRecorderFig.Text = "?";
            this.btnRecorderFig.UseVisualStyleBackColor = true;
            this.btnRecorderFig.Click += new System.EventHandler(this.btnRecorderFig_Click);
            // 
            // frmTypeSelector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 87);
            this.Controls.Add(this.btnRecorderFig);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.lblType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTypeSelector";
            this.Text = "控制器編號選擇";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnRecorderFig;
    }
}