using System;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmDetailedSetting : Form
    {
        #region | Fields |
        //Controls
        private TextBox[] txtTitle = new TextBox[6];
        private ComboBox[] cboUnit = new ComboBox[6];
        private TextBox[] txtMin = new TextBox[6];
        private TextBox[] txtMax = new TextBox[6];
        private TextBox[] txtInterval = new TextBox[6];
        #endregion

        #region | Events |

        public frmDetailedSetting()
        {
            InitializeComponent();
            //Init. componment
            for (int i = 0; i < 6; i++)
            {
                txtTitle[i] = new TextBox();
                txtTitle[i].Text = frmRecorderDrawer.YProp[i].Title;
                txtTitle[i].MaxLength = 4;
                txtTitle[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtTitle[i], 0, i + 1);
                cboUnit[i] = new ComboBox();
                cboUnit[i].Items.AddRange(frmRecorderDrawer.UNIT_TABLE);
                cboUnit[i].SelectedIndex = frmRecorderDrawer.YProp[i].Unit;
                cboUnit[i].DropDownStyle = ComboBoxStyle.DropDownList;
                tblMain.Controls.Add(cboUnit[i], 1, i + 1);
                txtMin[i] = new TextBox();
                txtMin[i].Text = frmRecorderDrawer.YProp[i].Min.ToString(); ;
                txtMin[i].KeyPress += InputOnlyNumber;
                txtMin[i].Dock = DockStyle.Fill;
                txtMin[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMin[i], 2, i + 1);
                txtMax[i] = new TextBox();
                txtMax[i].Text = frmRecorderDrawer.YProp[i].Max.ToString();
                txtMax[i].KeyPress += InputOnlyNumber;
                txtMax[i].Dock = DockStyle.Fill;
                txtMax[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMax[i], 3, i + 1);
                txtInterval[i] = new TextBox();
                txtInterval[i].Text = frmRecorderDrawer.YProp[i].Interval.ToString();
                txtInterval[i].KeyPress += InputOnlyNumber;
                txtInterval[i].Dock = DockStyle.Fill;
                txtInterval[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtInterval[i], 4, i + 1);
            }
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            //Init. general tab
            txtMainTitle.Text = frmRecorderDrawer.TitleText;
            //Init. axes tab
            cboXAngle.Items.Clear();
            for (int i = -90; i <= 90; i += 5)
                cboXAngle.Items.Add(i);
            cboXType.SelectedIndex = frmRecorderDrawer.XType;
            if (frmRecorderDrawer.XInterval == 0)
                cboXInterval.SelectedIndex = 0;
            else
                cboXInterval.SelectedItem = frmRecorderDrawer.XInterval.ToString();
            cboXAngle.SelectedItem = frmRecorderDrawer.XAngle;
            //Init. time-period tab
            dtpStart.Value = frmRecorderDrawer.StartTime;
            dtpEnd.Value = frmRecorderDrawer.EndTime;
            //Init. animation tab
            txtPercentage.Text = frmRecorderDrawer.Percentage.ToString();
            txtDuration.Text = frmRecorderDrawer.Duration.ToString();
        }

        public void InputOnlyNumber(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != 8))
                e.Handled = true;
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
            // only allow sign symbol at first char
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
                e.Handled = true;
            if ((e.KeyChar == '-') && !((sender as TextBox).Text.IndexOf('-') > -1) && ((sender as TextBox).SelectionStart != 0))
                e.Handled = true;
        }

        private void InputOnlyPositiveInteger(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != 8))
                e.Handled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //General tab
            frmRecorderDrawer.TitleText = txtMainTitle.Text;
            if (frmRecorderDrawer.LimitedTimePeriod)
            {
                frmRecorderDrawer.StartTime = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Hour, dtpStart.Value.Minute, dtpStart.Value.Second);
                frmRecorderDrawer.EndTime = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, dtpEnd.Value.Hour, dtpEnd.Value.Minute, dtpEnd.Value.Second);
            }
            //Axes tab
            for (int i = 0; i < 6; i++)
                frmRecorderDrawer.YProp[i] = new AxesProp(txtTitle[i].Text, cboUnit[i].SelectedIndex, float.Parse(txtMin[i].Text), float.Parse(txtMax[i].Text), float.Parse(txtInterval[i].Text));
            frmRecorderDrawer.XType = cboXType.SelectedIndex;
            if (cboXInterval.SelectedIndex == 0)
                frmRecorderDrawer.XInterval = 0;
            else
                frmRecorderDrawer.XInterval = int.Parse(cboXInterval.SelectedItem.ToString());
            frmRecorderDrawer.XAngle = (int)cboXAngle.SelectedItem;
            //Animation tab
            frmRecorderDrawer.Percentage = int.Parse(txtPercentage.Text);
            frmRecorderDrawer.Duration = int.Parse(txtDuration.Text);
            //Commit
            DialogResult = DialogResult.OK;
        }

        private void cboXType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboXType.SelectedIndex)
            {
                case 0: //Text
                    lblXInterval.Text = "時間軸間距(筆數)";
                    cboXInterval.Items.Clear();
                    cboXInterval.Items.AddRange(new string[] { "自動", "100", "200", "300", "400", "500", "600", "700", "800", "900", "1000", "2500", "5000", "10000", "20000", "50000"});
                    cboXInterval.SelectedIndex = 0;
                    break;
                case 1: //DateTime
                    lblXInterval.Text = "時間軸間距(分)";
                    cboXInterval.Items.Clear();
                    cboXInterval.Items.AddRange(new string[] { "自動", "5", "10", "15", "30", "60", "90", "120", "180"});
                    cboXInterval.SelectedIndex = 0;
                    break;
            }
        }

        private void TimePeriod_Changed(object sender, EventArgs e)
        {
            frmRecorderDrawer.LimitedTimePeriod = true;
        }

        #endregion

    }
}
