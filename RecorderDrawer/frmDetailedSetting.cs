using System;
using System.Drawing;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmDetailedSetting : Form
    {
        #region | Fields |
        //Controls
        private readonly TextBox[] txtTitle = new TextBox[6];
        private readonly ComboBox[] cboUnit = new ComboBox[6];
        private readonly TextBox[] txtMin = new TextBox[6];
        private readonly TextBox[] txtMax = new TextBox[6];
        private readonly TextBox[] txtInterval = new TextBox[6];
        #endregion

        #region | Property |
        //General tab
        public string TitleText { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int FluidIndex { get; private set; }
        public int ReactorSizeIndex { get; private set; }
        public string RecorderName { get; private set; }
        //Axes tab
        public AxesProp[] YProp { get; private set; } = new AxesProp[FrmMain.YProp.Length];
        public int XType { get; private set; }
        public int XInterval { get; private set; }
        public int XAngle { get; private set; }
        //Series tab
        public string[] ParamTitle { get; private set; }
        public Color[] ParamBackColor { get; private set; }
        public Color[] ParamForeColor { get; private set; }
        //Animation tab
        public int Percentage { get; private set; }
        public int Duration { get; private set; }
        //aging tab
        public bool AgingTempFix { get; private set; }
        public float AgingPDiff { get; private set; }
        public float AgingTimeHold { get; private set; }
        #endregion

        #region | Events |

        public frmDetailedSetting()
        {
            InitializeComponent();
            //Init. componment
            for (int i = 0; i < FrmMain.YProp.Length; i++)
            {
                if (FrmMain.SeriesMap[i + 1].Length == 0)
                    continue;
                txtTitle[i] = new TextBox
                {
                    Text = FrmMain.YProp[i].Title,
                    MaxLength = 4,
                    TextAlign = HorizontalAlignment.Center,
                    Dock = DockStyle.Fill
                };
                tblMain.Controls.Add(txtTitle[i], 0, i + 1);
                cboUnit[i] = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Dock = DockStyle.Fill
                };
                cboUnit[i].Items.AddRange(FrmMain.unitList);
                cboUnit[i].SelectedIndex = FrmMain.YProp[i].UnitIndex;
                tblMain.Controls.Add(cboUnit[i], 1, i + 1);
                txtMin[i] = new TextBox
                {
                    Text = FrmMain.YProp[i].Min.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                txtMin[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtMin[i], 2, i + 1);
                txtMax[i] = new TextBox
                {
                    Text = FrmMain.YProp[i].Max.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                txtMax[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtMax[i], 3, i + 1);
                txtInterval[i] = new TextBox
                {
                    Text = FrmMain.YProp[i].Interval.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                txtInterval[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtInterval[i], 4, i + 1);
            }
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            //Init. general tab
            txtMainTitle.Text = FrmMain.TitleText;
            for (int i = 0; i < FrmMain.fluidList.Length; i++)
                cboFluid.Items.Add(FrmMain.fluidList[i].Key);
            cboFluid.SelectedIndex = FrmMain.FluidIndex;
            cboReactorSize.SelectedIndex = 0;
            cboReactorSize.SelectedIndex = FrmMain.ReactorSizeIndex;
            txtRecorderName.Text = FrmMain.RecorderName;
            //Init. axes tab
            cboXAngle.Items.Clear();
            for (int i = -90; i <= 90; i += 5)
                cboXAngle.Items.Add(i);
            cboXType.SelectedIndex = FrmMain.XType;
            if (FrmMain.XInterval == 0)
                cboXInterval.SelectedIndex = 0;
            else
                cboXInterval.SelectedItem = FrmMain.XInterval.ToString();
            cboXAngle.SelectedItem = FrmMain.XAngle;
            //Init. series tab
            tblSeries.RowCount = FrmMain.ParamTitle.Length + 2;
            foreach (RowStyle row in tblSeries.RowStyles)
                row.Height = 20;
            ParamTitle = (string[])FrmMain.ParamTitle.Clone();
            ParamBackColor = (Color[])FrmMain.ParamBackColor.Clone();
            ParamForeColor = (Color[])FrmMain.ParamForeColor.Clone();
            for (int i = 0; i < FrmMain.ParamTitle.Length; i++)
            {
                tblSeries.Controls.Add(new TextBox()
                {
                    Text = ParamTitle[i],
                    Font = new System.Drawing.Font("微軟正黑體", 10),
                    TextAlign = HorizontalAlignment.Center,
                    Dock = DockStyle.Fill,
                    Tag = 100 + i
                }, 0, i + 1);
                tblSeries.Controls.Add(new Button()
                {
                    BackColor = ParamBackColor[i],
                    Dock = DockStyle.Fill,
                    Tag = 200 + i
                }, 1, i + 1);
                tblSeries.Controls.Add(new Button()
                {
                    BackColor = ParamForeColor[i],
                    Dock = DockStyle.Fill,
                    Tag = 300 + i
                }, 2, i + 1);
            }
            foreach(Control ctrl in tblSeries.Controls)
            {
                if(ctrl.GetType()==typeof(Button))
                    ((Button)ctrl).Click += BtnColorChange_Click;
            }
            //Init. time-period tab
            dtpStart.Value = FrmMain.StartTime;
            dtpEnd.Value = FrmMain.EndTime;
            //Init. animation tab
            txtPercentage.Text = FrmMain.Percentage.ToString();
            txtDuration.Text = FrmMain.Duration.ToString();
            //Init. aging tab
            chkAgingTempFix.Checked = FrmMain.AgingTempFix;
            txtAgingPDiff.Text = FrmMain.AgingPDiff.ToString();
            txtAgingTimeHold.Text = FrmMain.AgingTimeHold.ToString();
        }

        private void BtnColorChange_Click(object sender, EventArgs e)
        {
            ColorDialog crd = new ColorDialog();
            if (crd.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = crd.Color;
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

        public void InputOnlyPositiveNumber(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != 8))
                e.Handled = true;
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }

        private void InputOnlyPositiveInteger(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != 8))
                e.Handled = true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            //General tab
            TitleText = txtMainTitle.Text;
            StartTime = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Hour, dtpStart.Value.Minute, dtpStart.Value.Second);
            EndTime = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, dtpEnd.Value.Hour, dtpEnd.Value.Minute, dtpEnd.Value.Second);
            FluidIndex = cboFluid.SelectedIndex;
            ReactorSizeIndex = cboReactorSize.SelectedIndex;
            RecorderName = txtRecorderName.Text;
            //Axes tab
            for (int i = 0; i < YProp.Length; i++)
            {
                if (FrmMain.SeriesMap[i + 1].Length == 0)
                    continue;
                YProp[i] = new AxesProp(txtTitle[i].Text, cboUnit[i].SelectedIndex, float.Parse(txtMin[i].Text), float.Parse(txtMax[i].Text), float.Parse(txtInterval[i].Text));
            }
            XType = cboXType.SelectedIndex;
            if (cboXInterval.SelectedIndex == 0)
                XInterval = 0;
            else
                XInterval = int.Parse(cboXInterval.SelectedItem.ToString());
            XAngle = (int)cboXAngle.SelectedItem;
            //Series tab
            foreach(Control ctrl in tblSeries.Controls)
            {
                if(ctrl.Tag!=null && int.TryParse(ctrl.Tag.ToString(),out int index))
                {
                    switch(index /100)
                    {
                        case 1: //text
                            ParamTitle[index % 100] = ((TextBox)ctrl).Text;
                            break;
                        case 2: //back color
                            ParamBackColor[index % 100] = ((Button)ctrl).BackColor;
                            break;
                        case 3: //fore color
                            ParamForeColor[index % 100] = ((Button)ctrl).BackColor;
                            break;
                    }
                }
            }
            //Animation tab
            Percentage = int.Parse(txtPercentage.Text);
            Duration = int.Parse(txtDuration.Text);
            //aging tab
            AgingTempFix = chkAgingTempFix.Checked;
            AgingPDiff = float.Parse(txtAgingPDiff.Text);
            AgingTimeHold = float.Parse(txtAgingTimeHold.Text);
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

        #endregion
    }
}
