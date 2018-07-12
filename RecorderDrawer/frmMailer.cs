using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
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
        private class MailItem
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public MailItem(string name, string address)
            {
                Name = name;
                Address = address;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        #endregion

        public frmMailer(bool advancedOption)
        {
            InitializeComponent();
            //read mail list
            try
            {
                string mailList = string.Empty;
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("RecorderDrawer.Resources.mailList.txt"), Encoding.UTF8))
                    mailList = sr.ReadToEnd();
                foreach (string item in mailList.Split(';'))
                    cboMailAddress.Items.Add(new MailItem(item, item.Substring(item.IndexOf('<') + 1, item.IndexOf('>') - item.IndexOf('<') - 1)));
            }
            catch (Exception)
            { }
            cboMailAddress.SelectedIndex = 0;
            cboBorder.SelectedIndex = 2;
            cboImageFormat.SelectedIndex = 1;
            if (!advancedOption)
                pnlAdvance.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check mail address
            if (txtMailAddress.Visible && !IsValid(txtMailAddress.Text))
            {
                MessageBox.Show(this,"郵件地址錯誤，請更正。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MailAddress = (txtMailAddress.Visible ? txtMailAddress.Text : ((MailItem)cboMailAddress.SelectedItem).Address);
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

        private void btnReadMailAddress_Click(object sender, EventArgs e)
        {
            Enabled = false;
            cboMailAddress.Items.Clear();
            MessageBox.Show(this,"將嘗試讀取Outlook中通訊錄，需要一段時間", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.AddressList addressList = outlook.Session.GetGlobalAddressList();
            for (int i = 1; i <= addressList.AddressEntries.Count - 1; i++)
            {
                Microsoft.Office.Interop.Outlook.AddressEntry addrEntry = addressList.AddressEntries[i];
                if (addrEntry.AddressEntryUserType == Microsoft.Office.Interop.Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry)
                {
                    Microsoft.Office.Interop.Outlook.ExchangeUser exchUser = addrEntry.GetExchangeUser();
                    cboMailAddress.Items.Add(new MailItem(exchUser.Name + " <" + exchUser.PrimarySmtpAddress + ">", exchUser.PrimarySmtpAddress));
                }
            }
            if (cboMailAddress.Items.Count > 0)
            {
                cboMailAddress.SelectedIndex = 0;
                txtMailAddress.Visible = false;
                cboMailAddress.Visible = true;
                MessageBox.Show(this,"讀取成功", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtMailAddress.Visible = true;
                cboMailAddress.Visible = false;
                MessageBox.Show(this,"讀取失敗，請手動輸入郵件地址", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Enabled = true;
        }
    }
}
