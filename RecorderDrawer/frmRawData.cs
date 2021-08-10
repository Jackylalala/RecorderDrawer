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
using ZedGraph;

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
            for (int i = 0; i < FrmMain.ParamTitle.Length; i++)
                dt.Tables[0].Columns.Add(FrmMain.ParamTitle[i], typeof(float));
            for (int i = 0; i < FrmMain.TrendSeries[0].Count; i++)
            {
                dt.Tables[0].Rows.Add();
                dt.Tables[0].Rows[i][0] = new XDate(FrmMain.TrendSeries[0][i].X).ToString("MM/dd HH:mm:ss");
                for (int j = 0; j < FrmMain.TrendSeries.Length; j++)
                    dt.Tables[0].Rows[i][j + 1] = FrmMain.TrendSeries[j][i].Y.ToString("0.00");
            }
            dgvDisplay.DataSource = dt.Tables[0];
            dgvDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvDisplay.Columns[0].ReadOnly = true;
            dgvDisplay.ScrollBars = ScrollBars.Vertical;
         }
  
        private void btnOk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < FrmMain.TrendSeries.Length; j++)
                    FrmMain.TrendSeries[j][i].Y = double.Parse(dt.Tables[0].Rows[i][j+1].ToString());
            }
            DialogResult = DialogResult.OK;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            lblProcessingInfo.Visible = true;
            if (chkMail.Checked) //via mail
            {
                MessageBox.Show(this,"將使用本機Outlook帳戶寄送郵件，若需要權限請點選[允許]", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //create temp file
                string tempDoc = Path.GetTempPath() + Guid.NewGuid().ToString() + ".dat";
                ExportFile(tempDoc);
                // send via outlook
                Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                mailItem.Body = "Sent at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                mailItem.Attachments.Add(tempDoc);
                mailItem.Display();
            }
            else //save as file
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "匯出檔案";
                sfd.Filter = "匯出檔案(*.dat)|*.dat";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    ExportFile(sfd.FileName);
                    MessageBox.Show("匯出檔案完成！");
                }
                    //bgdMain.RunWorkerAsync(new object[] { 0, sfd.FileName, "" });
            }
            lblProcessingInfo.Visible = false;
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

        private void FunctionSwitch(bool state)
        {
            lblProcessingInfo.Visible = !state;
            dgvDisplay.Enabled = state;
            btnCancel.Enabled = state;
            btnOk.Enabled = state;
            btnExport.Enabled = state;
            chkMail.Enabled = state;
        }

        private void ExportFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryWriter writer = new BinaryWriter(fs, Encoding.UTF8);
                writer.Write(Assembly.GetExecutingAssembly().GetName().Version.ToString()); //version
                writer.Write(DateTime.Now.Ticks); //date
                if (FrmMain.Type == FrmMain.recorderList.Length - 1) //custom data type
                {
                    writer.Write(int.MaxValue); //means custom type
                    //write series map
                    writer.Write(FrmMain.SeriesMap.Length);
                    for (int i = 0; i < FrmMain.SeriesMap.Length; i++)
                    {
                        writer.Write(FrmMain.SeriesMap[i].Length);
                        for (int j = 0; j < FrmMain.SeriesMap[i].Length; j++)
                            writer.Write(FrmMain.SeriesMap[i][j]);
                    }
                    //write first row
                    writer.Write(FrmMain.FirstDataRow);
                    //write y-axis parameter
                    writer.Write(FrmMain.YProp.Length);
                    writer.Flush();
                    for (int i = 0; i < FrmMain.YProp.Length; i++)
                        FrmMain.YProp[i].Serialize(fs);
                    //write parameter title
                    writer.Write(FrmMain.ParamTitle.Length);
                    for (int i = 0; i < FrmMain.ParamTitle.Length; i++)
                        writer.Write(FrmMain.ParamTitle[i]);
                }
                else
                    writer.Write(FrmMain.Type); //recorder type
                writer.Write(FrmMain.RecorderName); //recorder name
                writer.Write(FrmMain.TrendSeries.Length);
                foreach (PointPairList item in FrmMain.TrendSeries)
                {
                    writer.Write(item.Count);
                    foreach(PointPair data in item)
                    {
                        writer.Write(data.X);
                        writer.Write(data.Y);
                        writer.Write(data.Tag.ToString());
                    }
                }
            }
        }

        private void dgvDisplay_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDisplay[e.ColumnIndex, e.RowIndex].Value.ToString().Equals(string.Empty))
                dgvDisplay[e.ColumnIndex, e.RowIndex].Value = "0";
            if (dgvDisplay.SelectedCells.Count > 1)
            {
                dgvDisplay.CellValueChanged -= dgvDisplay_CellValueChanged;
                string temp = dgvDisplay[e.ColumnIndex, e.RowIndex].Value.ToString();
                foreach (DataGridViewCell cell in dgvDisplay.SelectedCells)
                {
                    if (cell.ColumnIndex != 0)
                        cell.Value = temp;
                }
                dgvDisplay.CellValueChanged += dgvDisplay_CellValueChanged;
            }
        }
    }
}
