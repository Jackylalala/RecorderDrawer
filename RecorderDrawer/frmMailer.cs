using System;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmMailer : Form
    {
        #region | Field |
        private string mailAddress;
        private string fileName;
        private int resolutionType;
        private int borderType;
        private ImageFormat format;
        #endregion

        #region | Properties |
        public string MailAddress { get { return mailAddress; } }
        public string FileName { get { return fileName; } }
        public int ResolutionType { get { return resolutionType; } }
        public int BorderType { get { return borderType; } }
        public ImageFormat Format { get { return format; } }
        #endregion

        public frmMailer(bool advancedOption)
        {
            InitializeComponent();
            cboBorder.SelectedIndex = 2;
            cboImageFormat.SelectedIndex = 1;
            cboResolution.SelectedIndex = 3;
            txtMailAddress.Focus();
            if (!advancedOption)
            {
                cboBorder.Enabled = false;
                cboImageFormat.Enabled = false;
                cboResolution.Enabled = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check mail address
            if (!IsValid(txtMailAddress.Text))
            {
                MessageBox.Show("郵件地址錯誤，請更正。", "Alert");
                return;
            }
            //Check file name
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtFileName.Text, @"^[\w\-. ]+$"))
            {
                MessageBox.Show("檔案名稱錯誤，請更正。", "Alert");
                return;
            }
            mailAddress = txtMailAddress.Text;
            fileName = txtFileName.Text;
            resolutionType = cboResolution.SelectedIndex;
            borderType = cboBorder.SelectedIndex;
            switch (cboImageFormat.SelectedIndex)
            {
                case 0:
                    format = ImageFormat.Bmp;
                    break;
                case 1:
                    format = ImageFormat.Jpeg;
                    break;
                case 2:
                    format = ImageFormat.Gif;
                    break;
                case 3:
                    format = ImageFormat.Png;
                    break;
                case 4:
                    format = ImageFormat.Tiff;
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
