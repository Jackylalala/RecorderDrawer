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

        #region | Property |
        //General tab
        public string TitleText { get; private set; }
        public bool LimitedTimePeriod { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int FluidIndex { get; private set; }
        public int ReactorSizeIndex { get; private set; }
        //Axes tab
        public AxesProp[] YProp { get; private set; } = new AxesProp[6];
        public int XType { get; private set; }
        public int XInterval { get; private set; }
        public int XAngle { get; private set; }
        //Animation tab
        public int Percentage { get; private set; }
        public int Duration { get; private set; }
        #endregion

        #region | Events |

        public frmDetailedSetting()
        {
            InitializeComponent();
            //Init. componment
            for (int i = 0; i < 6; i++)
            {
                txtTitle[i] = new TextBox();
                txtTitle[i].Text = frmMain.YProp[i].Title;
                txtTitle[i].MaxLength = 4;
                txtTitle[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtTitle[i], 0, i + 1);
                cboUnit[i] = new ComboBox();
                cboUnit[i].Items.AddRange(frmMain.unitList);
                cboUnit[i].SelectedIndex = frmMain.YProp[i].UnitIndex;
                cboUnit[i].DropDownStyle = ComboBoxStyle.DropDownList;
                tblMain.Controls.Add(cboUnit[i], 1, i + 1);
                txtMin[i] = new TextBox();
                txtMin[i].Text = frmMain.YProp[i].Min.ToString(); ;
                txtMin[i].KeyPress += InputOnlyNumber;
                txtMin[i].Dock = DockStyle.Fill;
                txtMin[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMin[i], 2, i + 1);
                txtMax[i] = new TextBox();
                txtMax[i].Text = frmMain.YProp[i].Max.ToString();
                txtMax[i].KeyPress += InputOnlyNumber;
                txtMax[i].Dock = DockStyle.Fill;
                txtMax[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMax[i], 3, i + 1);
                txtInterval[i] = new TextBox();
                txtInterval[i].Text = frmMain.YProp[i].Interval.ToString();
                txtInterval[i].KeyPress += InputOnlyNumber;
                txtInterval[i].Dock = DockStyle.Fill;
                txtInterval[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtInterval[i], 4, i + 1);
            }
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            //Init. general tab
            txtMainTitle.Text = frmMain.TitleText;
            for (int i = 0; i < frmMain.fluidList.Length; i++)
                cboFluid.Items.Add(frmMain.fluidList[i].Key);
            cboFluid.SelectedIndex = frmMain.FluidIndex;
            cboReactorSize.SelectedIndex = 0;
            cboReactorSize.SelectedIndex = frmMain.ReactorSizeIndex;
            //Init. axes tab
            cboXAngle.Items.Clear();
            for (int i = -90; i <= 90; i += 5)
                cboXAngle.Items.Add(i);
            cboXType.SelectedIndex = frmMain.XType;
            if (frmMain.XInterval == 0)
                cboXInterval.SelectedIndex = 0;
            else
                cboXInterval.SelectedItem = frmMain.XInterval.ToString();
            cboXAngle.SelectedItem = frmMain.XAngle;
            //Init. time-period tab
            dtpStart.Value = frmMain.StartTime;
            dtpEnd.Value = frmMain.EndTime;
            //Init. animation tab
            txtPercentage.Text = frmMain.Percentage.ToString();
            txtDuration.Text = frmMain.Duration.ToString();
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
            TitleText = txtMainTitle.Text;
            StartTime = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Hour, dtpStart.Value.Minute, dtpStart.Value.Second);
            EndTime = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, dtpEnd.Value.Hour, dtpEnd.Value.Minute, dtpEnd.Value.Second);
            FluidIndex = cboFluid.SelectedIndex;
            ReactorSizeIndex = cboReactorSize.SelectedIndex;
            //Axes tab
            for (int i = 0; i < YProp.Length; i++)
                YProp[i] = new AxesProp(txtTitle[i].Text, cboUnit[i].SelectedIndex, float.Parse(txtMin[i].Text), float.Parse(txtMax[i].Text), float.Parse(txtInterval[i].Text));
            XType = cboXType.SelectedIndex;
            if (cboXInterval.SelectedIndex == 0)
                XInterval = 0;
            else
                XInterval = int.Parse(cboXInterval.SelectedItem.ToString());
            XAngle = (int)cboXAngle.SelectedItem;
            //Animation tab
            Percentage = int.Parse(txtPercentage.Text);
            Duration = int.Parse(txtDuration.Text);
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
            LimitedTimePeriod = true;
        }

        #endregion
    }
}
