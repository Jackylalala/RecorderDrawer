using System;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmMailer : Form
    {

        #region | Properties |
        public string MailAddress { get; set; }
        public string FileName { get; set; }
        public int BorderType { get; set; }
        public ImageFormat Format { get; set; }
        #endregion

        public frmMailer(bool advancedOption)
        {
            InitializeComponent();
            cboBorder.SelectedIndex = 2;
            cboImageFormat.SelectedIndex = 1;
            txtMailAddress.Focus();
            if (!advancedOption)
            {
                cboBorder.Enabled = false;
                cboImageFormat.Enabled = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check mail address
            if (!IsValid(txtMailAddress.Text))
            {
                MessageBox.Show("郵件地址錯誤，請更正。", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Check file name
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtFileName.Text, @"^[\w\-. ]+$"))
            {
                MessageBox.Show("檔案名稱錯誤，請更正。", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MailAddress = txtMailAddress.Text;
            FileName = txtFileName.Text;
            BorderType = cboBorder.SelectedIndex;
            switch (cboImageFormat.SelectedIndex)
            {
                case 0:
                    Format = ImageFormat.Bmp;
                    break;
                case 1:
                    Format = ImageFormat.Jpeg;
                    break;
                case 2:
                    Format = ImageFormat.Gif;
                    break;
                case 3:
                    Format = ImageFormat.Png;
                    break;
                case 4:
                    Format = ImageFormat.Tiff;
                    break;
            }
            DialogResult = DialogResult.OK;
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
