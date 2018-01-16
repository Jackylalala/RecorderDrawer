using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmRawData : Form
    {
        public frmRawData()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "匯出Excel檔";
            sfd.Filter = "逗號分隔值的文字檔案(*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite), Encoding.Default))
                    WriteCSV(sw);
                MessageBox.Show("匯出完成！", "匯出Excel檔");
            }

        }

        private void btnToExcelByMail_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (frmRecorderDrawer.InputBox("Export Excel by mail","請輸入檔案名稱：",ref filename)==DialogResult.OK)
            {
                //Check file name
                if (!System.Text.RegularExpressions.Regex.IsMatch(filename, @"^[\w\-. ]+$"))
                {
                    MessageBox.Show("檔案名稱錯誤，請更正。", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Input mail address
                string mailAddress = "";
                if (frmRecorderDrawer.InputBox("Export Excel by mail", "請輸入郵件地址：", ref mailAddress) == DialogResult.OK)
                {
                    //Check mail address
                    if (!IsValid(mailAddress))
                    {
                        MessageBox.Show("郵件地址錯誤，請更正。", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MemoryStream ms = new MemoryStream();
                    StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
                    WriteCSV(sw);
                    //Rewinding position
                    ms.Position = 0;
                    //Making mail
                    Attachment attach = new Attachment(ms, new ContentType("text/csv"));
                    attach.Name = filename + ".csv";
                    MailMessage myMail = new MailMessage();
                    myMail.Subject = filename;
                    myMail.From = new MailAddress("jackylalala9527@gmail.com", "Raw Data Mailer");
                    myMail.To.Add(mailAddress);
                    myMail.SubjectEncoding = Encoding.UTF8;
                    myMail.IsBodyHtml = true;
                    myMail.BodyEncoding = Encoding.UTF8;
                    myMail.Body = filename + Environment.NewLine + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    myMail.Attachments.Add(attach);
                    bgdMailer.RunWorkerAsync(myMail);
                }
            }
        }

        private void WriteCSV(StreamWriter sw)
        {
            sw.AutoFlush = true;
            //Write type and version
            sw.Write("RecorderDrawer " + Application.ProductVersion + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ",");
            sw.Write(frmRecorderDrawer.Type);
            sw.Write(Environment.NewLine);
            //Write header
            sw.Write("Date" + ",");
            for (int i = 0; i < frmRecorderDrawer.ParaTitle.Length; i++)
            {
                if (i == (frmRecorderDrawer.ParaTitle.Length - 1))
                    sw.Write(frmRecorderDrawer.ParaTitle[i]);
                else
                    sw.Write(frmRecorderDrawer.ParaTitle[i] + ",");
            }
            sw.Write(Environment.NewLine);
            foreach (DataRow line in ((DataTable)dgvDisplay.DataSource).Rows)
            {
                DateTime date;
                DateTime.TryParseExact(line.ItemArray[0].ToString(), frmRecorderDrawer.dateTimeList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date);
                sw.Write("=\"" + date.ToString("MM/dd HH:mm:ss") + "\", ");
                for (int i = 1; i < line.ItemArray.Length; i++)
                {
                    if (i == (line.ItemArray.Length - 1))
                        sw.Write(string.Format("{0,5}", float.Parse(line.ItemArray[i].ToString())));
                    else
                        sw.Write(string.Format("{0,5}", float.Parse(line.ItemArray[i].ToString())) + ",");
                }
                sw.Write(Environment.NewLine);
            }
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

        private void bgdMailer_DoWork(object sender, DoWorkEventArgs e)
        {
            MailMessage myMail = (MailMessage)(e.Argument as object);
            try
            {
                using (SmtpClient mySmtp = new SmtpClient())
                {
                    mySmtp.Port = 587;
                    mySmtp.Credentials = new NetworkCredential("jackylalala9527@gmail.com", "7qvt6t2738");
                    mySmtp.Host = "smtp.gmail.com";
                    mySmtp.EnableSsl = true;
                    mySmtp.Send(myMail);
                }
                MessageBox.Show("成功寄出檔案到" + myMail.To[0].Address, "Alert");
            }
            catch (Exception)
            {
                MessageBox.Show("寄出檔案失敗！", "Alert");
            }
            finally
            {
                myMail.Dispose();
            }
        }

        private void dgvDisplay_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvDisplay[e.ColumnIndex, e.RowIndex].Value.ToString().Equals(string.Empty))
                dgvDisplay[e.ColumnIndex, e.RowIndex].Value = "0";
            if (dgvDisplay.SelectedCells.Count > 1)
            {
                dgvDisplay.CellValidating -= dgvDisplay_CellValidating;
                string temp = dgvDisplay[e.ColumnIndex, e.RowIndex].Value.ToString();
                foreach (DataGridViewCell cell in dgvDisplay.SelectedCells)
                    cell.Value = temp;
                dgvDisplay.CellValidating += dgvDisplay_CellValidating;
            }
        }
    }
}
