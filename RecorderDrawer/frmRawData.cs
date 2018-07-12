using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RecorderDrawer
{
    public partial class frmRawData : Form
    {
        private DataSet dt = new DataSet();

        public frmRawData()
        {
            InitializeComponent();
            dt.Tables.Add();
            dt.Tables[0].Columns.Add("時間", typeof(string));
            for (int i = 0; i < frmMain.ParamTitle.Length; i++)
                dt.Tables[0].Columns.Add(frmMain.ParamTitle[i], typeof(float));
            foreach (RecordData item in frmMain.Data)
                dt.Tables[0].Rows.Add();
            for (int i = 0; i < frmMain.Data.Count; i++)
            {
                dt.Tables[0].Rows[i][0] = frmMain.Data[i].Time.ToString("MM/dd HH:mm:ss");
                for (int j = 0; j < frmMain.ParamTitle.Length; j++)
                    dt.Tables[0].Rows[i][j + 1] = frmMain.Data[i].Parameter[j];
            }
            dgvDisplay.DataSource = dt.Tables[0];
            dgvDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvDisplay.Columns[0].ReadOnly = true;
            dgvDisplay.ScrollBars = ScrollBars.Vertical;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            frmMain.Data.Clear();
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                DateTime date = new DateTime();
                DateTime.TryParseExact(dt.Tables[0].Rows[i][0].ToString(), frmMain.datetimeFormatList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date);
                List<float> dataNum = new List<float>();
                for (int j = 1; j < dt.Tables[0].Rows[i].ItemArray.Length; j++)
                    dataNum.Add(float.Parse(dt.Tables[0].Rows[i][j].ToString()));
                frmMain.Data.Add(new RecordData(date, dataNum));
            }
            frmMain.Data.Sort();
            DialogResult = DialogResult.OK;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (chkMail.Checked) //via mail
            {
                MessageBox.Show(this,"將使用本機Outlook帳戶寄送郵件，若需要權限請點選[允許]", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmMailer frmMailer = new frmMailer(false);
                frmMailer.StartPosition = FormStartPosition.Manual;
                frmMailer.Location = new Point(Location.X + Width / 2 - frmMailer.ClientSize.Width / 2, Location.Y + Height / 2 - frmMailer.ClientSize.Height / 2);
                //send mail
                if (frmMailer.ShowDialog(this) == DialogResult.OK)
                    bgdMain.RunWorkerAsync(new object[] { 1, frmMailer.FileName, frmMailer.MailAddress });
            }
            else //save as file
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "匯出檔案";
                sfd.Filter = "匯出檔案(*.dat)|*.dat";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                    bgdMain.RunWorkerAsync(new object[] { 0, sfd.FileName, "" });
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

        private void bgdMain_DoWork(object sender, DoWorkEventArgs e)
        {
            bgdMain.ReportProgress(0, "");
            int oType = int.Parse((e.Argument as object[])[0].ToString());
            string filename = (e.Argument as object[])[1].ToString();
            string address = (e.Argument as object[])[2].ToString();
            switch (oType)
            {
                case 0: //file
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                    {
                        BinaryWriter writer = new BinaryWriter(fs, Encoding.UTF8);
                        writer.Write(Assembly.GetExecutingAssembly().GetName().Version.ToString()); //version
                        writer.Write(DateTime.Now.Ticks); //date
                        if (frmMain.Type == frmMain.recorderList.Length - 1) //custom data type
                        {
                            writer.Write(int.MaxValue); //means custom type
                            //write series map
                            writer.Write(frmMain.SeriesMap.Length);
                            for (int i = 0; i < frmMain.SeriesMap.Length; i++)
                            {
                                writer.Write(frmMain.SeriesMap[i].Length);
                                for (int j = 0; j < frmMain.SeriesMap[i].Length; j++)
                                    writer.Write(frmMain.SeriesMap[i][j]);
                            }
                            //write first row
                            writer.Write(frmMain.FirstDataRow);
                            //write y-axis parameter
                            writer.Write(frmMain.YProp.Length);
                            writer.Flush();
                            for (int i = 0; i < frmMain.YProp.Length; i++)
                                frmMain.YProp[i].Serialize(fs);
                            //write parameter title
                            writer.Write(frmMain.ParamTitle.Length);
                            for (int i = 0; i < frmMain.ParamTitle.Length; i++)
                                writer.Write(frmMain.ParamTitle[i]);
                        }
                        else
                            writer.Write(frmMain.Type); //recorder type
                        writer.Write(frmMain.Data.Count);
                        writer.Flush();
                        foreach(RecordData item in frmMain.Data)
                            item.Serialize(fs);
                    }
                    MessageBox.Show(this,"匯出檔案完成！");
                    break;
                case 1: //mail
                    try
                    {
                        //create temp file
                        string tempDoc = Path.GetTempPath() + Guid.NewGuid().ToString() + ".dat";
                        using (FileStream fs = new FileStream(tempDoc, FileMode.Create, FileAccess.ReadWrite))
                        {
                            //add type information to header and then serialize
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fs, frmMain.Data,
                                new System.Runtime.Remoting.Messaging.Header[]
                                { new System.Runtime.Remoting.Messaging.Header("type", frmMain.Type) });
                        }
                        // send via outlook
                        Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
                        Microsoft.Office.Interop.Outlook.MailItem mailItem = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                        mailItem.Subject = filename + " - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        mailItem.To = address;
                        mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                        mailItem.Body = "Sent at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        mailItem.Attachments.Add(tempDoc);
                        mailItem.Send();
                        Marshal.FinalReleaseComObject(outlook);
                        //try to del temp file
                        File.Delete(tempDoc);
                        MessageBox.Show(this,"成功寄出檔案到" + address);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this,ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void bgdMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProcessingInfo.Visible = true;
        }

        private void bgdMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProcessingInfo.Visible = false;
        }

        private void lblProcessingInfo_VisibleChanged(object sender, EventArgs e)
        {
            if (lblProcessingInfo.Visible)
            {
                lblProcessingInfo.Text = "Now processing";
                dgvDisplay.Enabled = false;
                btnCancel.Enabled = false;
                btnOk.Enabled = false;
                btnExport.Enabled = false;
                chkMail.Enabled = false;
            }
            else
            {
                dgvDisplay.Enabled = true;
                btnCancel.Enabled = true;
                btnOk.Enabled = true;
                btnExport.Enabled = true;
                chkMail.Enabled = true;
            }
        }
    }
}
