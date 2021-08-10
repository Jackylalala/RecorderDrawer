using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmFormatSelector : Form
    {
        #region | Fields |
        private static readonly AxesProp[] defaultYProp= new AxesProp[6]
            {  
            new AxesProp("內溫", 0, 0, 220, 20),
            new AxesProp("外溫", 0, 0, 220, 20),
            new AxesProp("壓力", 1, -2, 20, 2),
            new AxesProp("流速", 3, 0, 22, 2),
            new AxesProp("轉速", 4, 0, 1100, 100),
            new AxesProp("扭力", 5, 0, 220, 20)
            };
        //Controls
        private readonly TextBox[] txtTitle = new TextBox[6];
        private readonly ComboBox[] cboUnit = new ComboBox[6];
        private readonly TextBox[] txtMin = new TextBox[6];
        private readonly TextBox[] txtMax = new TextBox[6];
        private readonly TextBox[] txtInterval = new TextBox[6];
        private readonly List<Label> lblAxis = new List<Label>();
        private readonly List<ComboBox> cboAxis = new List<ComboBox>();
        private readonly List<TextBox> txtAxis = new List<TextBox>();
        private readonly int columnCount;
        #endregion

        #region | Property |
        public AxesProp[] YProp { get; private set; }
        public int XType { get; private set; }
        public int XInterval { get; private set; }
        public int XAngle { get; private set; }
        public int[][] SeriesMap { get; private set; }
        public int FirstDataRow { get; private set; }
        public List<string> ParamTitle { get; private set; } = new List<string>();
        public string RecorderName { get; private set; }
        #endregion

        public frmFormatSelector(string[] previewData)
        {
            InitializeComponent();
            //Init. componment
            for (int i = 0; i < 6; i++)
            {
                txtTitle[i] = new TextBox
                {
                    Text = defaultYProp[i].Title,
                    MaxLength = 4,
                    TextAlign = HorizontalAlignment.Center
                };
                tblMain.Controls.Add(txtTitle[i], 1, i + 1);
                cboUnit[i] = new ComboBox();
                cboUnit[i].Items.AddRange(FrmMain.unitList);
                cboUnit[i].SelectedIndex = defaultYProp[i].UnitIndex;
                cboUnit[i].DropDownStyle = ComboBoxStyle.DropDownList;
                tblMain.Controls.Add(cboUnit[i], 2, i + 1);
                txtMin[i] = new TextBox
                {
                    Text = defaultYProp[i].Min.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                ;
                txtMin[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtMin[i], 3, i + 1);
                txtMax[i] = new TextBox
                {
                    Text = defaultYProp[i].Max.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                txtMax[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtMax[i], 4, i + 1);
                txtInterval[i] = new TextBox
                {
                    Text = defaultYProp[i].Interval.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center
                };
                txtInterval[i].KeyPress += InputOnlyNumber;
                tblMain.Controls.Add(txtInterval[i], 5, i + 1);
            }
            cboXAngle.Items.Clear();
            for (int i = -90; i <= 90; i += 5)
                cboXAngle.Items.Add(i);
            cboXType.SelectedIndex = FrmMain.XType;
            if (FrmMain.XInterval == 0)
                cboXInterval.SelectedIndex = 0;
            else
                cboXInterval.SelectedItem = FrmMain.XInterval.ToString();
            cboXAngle.SelectedItem = FrmMain.XAngle;
            //set tooltip
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(lblAxe1, "請將此座標軸設定為內溫資訊");
            tooltip.SetToolTip(lblAxe2, "請將此座標軸設定為外溫資訊");
            tooltip.SetToolTip(lblAxe3, "請將此座標軸設定為壓力資訊");
            tooltip.SetToolTip(lblAxe4, "請將此座標軸設定為流速資訊");
            tooltip.SetToolTip(lblAxe5, "請將此座標軸設定為轉速資訊");
            tooltip.SetToolTip(lblAxe6, "此座標軸可設定為任意資訊");
            //preview data
            columnCount = previewData[5].Split(',', '\t').Length;
            for (int i = 0; i < columnCount; i++)
                dgvMain.Columns.Add("#" + i, "#" + i);
            foreach (DataGridViewColumn col in dgvMain.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            for (int i = 0; i < 51; i++)
            {
                dgvMain.Rows.Add(previewData[i].Split(',', '\t'));
                dgvMain.Rows[i].HeaderCell.Value = string.Format("{0}",i+1);
            }
            //channel tab
            for (int i = 0; i < columnCount; i++)
            {
                lblAxis.Add(new Label());
                lblAxis[i].Font = new Font("微軟正黑體", 10);
                lblAxis[i].AutoSize = false;
                lblAxis[i].TextAlign = ContentAlignment.MiddleCenter;
                lblAxis[i].Text = "#" + i;
                lblAxis[i].Location = new Point(7, 29 + 31 * i);
                lblAxis[i].Size = new Size(37, 25);
                pnlChannel.Controls.Add(lblAxis[i]);
                cboAxis.Add(new ComboBox());
                cboAxis[i].Font = new Font("微軟正黑體", 10);
                cboAxis[i].DropDownStyle = ComboBoxStyle.DropDownList;
                cboAxis[i].Items.AddRange(new object[] { "無", "時間", "內溫軸", "外溫軸", "壓力軸", "流速軸", "轉速軸", "任意軸" });
                cboAxis[i].Location = new Point(44, 29 + 31 * i);
                cboAxis[i].Size = new Size(80, 25);
                cboAxis[i].SelectedIndex = 0;
                cboAxis[i].Tag = i;
                cboAxis[i].SelectedIndexChanged += CboAxis_SelectedIndexChanged;
                pnlChannel.Controls.Add(cboAxis[i]);
                txtAxis.Add(new TextBox());
                txtAxis[i].Font = new Font("微軟正黑體", 10);
                txtAxis[i].TextAlign = HorizontalAlignment.Center;
                txtAxis[i].Text = i < previewData[0].Split(',', '\t').Length ? previewData[0].Split(',', '\t')[i] : "訊號" + i;
                txtAxis[i].ImeMode = ImeMode.On;
                txtAxis[i].Location = new Point(130, 29 + 31 * i);
                txtAxis[i].Size = new Size(60, 25);
                txtAxis[i].MaxLength = 6;
                pnlChannel.Controls.Add(txtAxis[i]);
            }
            cboFirstDataRow.SelectedIndex = 1;
            txtRecorderName.Text = FrmMain.RecorderName;
            cboAxis[0].SelectedIndex = 1; //default datetime
        }

        private void CboAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(ComboBox cbo in cboAxis)
            {
                if (cbo.SelectedIndex <= 1)
                    txtAxis[int.Parse(cbo.Tag.ToString())].Enabled = false;
                else
                    txtAxis[int.Parse(cbo.Tag.ToString())].Enabled = true;
            }
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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            FirstDataRow = cboFirstDataRow.SelectedIndex;
            RecorderName = txtRecorderName.Text;
            //create series map
            List<int>[] map = new List<int>[7];
            for (int i = 0; i < map.Length; i++)
                map[i] = new List<int>();
            for (int i = 0; i < columnCount; i++)
            {
                if (cboAxis[i].SelectedIndex > 0)
                    map[cboAxis[i].SelectedIndex - 1].Add(i);
                if (cboAxis[i].SelectedIndex > 1)
                    ParamTitle.Add(txtAxis[i].Text.Equals(string.Empty) ? "訊號" + i : txtAxis[i].Text);
            }
            //check
            if (map[0].Count == 0)
            {
                MessageBox.Show(this, "必須有時間通道資訊");
                return;
            }
            else if (map.Skip(1).SelectMany(m => m).Count() == 0)
            {
                MessageBox.Show(this, "至少必須有一個以上的通道資訊");
                return;
            }
            else
                SeriesMap = map.Select(m => m.ToArray()).ToArray();
            //y-axis param
            List<AxesProp> temp = new List<AxesProp>();
            int axisCount = SeriesMap.Skip(1).Where(m => m.Length > 0).Count();
            for (int i = 0; i < txtTitle.Length; i++)
                temp.Add(new AxesProp(txtTitle[i].Text, cboUnit[i].SelectedIndex, float.Parse(txtMin[i].Text), float.Parse(txtMax[i].Text), float.Parse(txtInterval[i].Text)));
            YProp = temp.ToArray();
            XType = cboXType.SelectedIndex;
            if (cboXInterval.SelectedIndex == 0)
                XInterval = 0;
            else
                XInterval = int.Parse(cboXInterval.SelectedItem.ToString());
            XAngle = (int)cboXAngle.SelectedItem;
            //commit
            DialogResult = DialogResult.OK;
        }
    }
}
