using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmSetAxes : Form
    {
        #region | Fields |
        //Controls
        private TextBox[] txtTitle = new TextBox[6];
        private ComboBox[] cboUnit = new ComboBox[6];
        private TextBox[] txtMin = new TextBox[6];
        private TextBox[] txtMax = new TextBox[6];
        private TextBox[] txtInterval = new TextBox[6];
        //Variables
        private AxesProp[] yProp = new AxesProp[6];
        private int xType; //0: Text, 1: DateTime
        private int xInterval;
        private int xAngle;
        #endregion

        #region | Properties |
        public AxesProp[] YProp { get { return (AxesProp[])yProp.Clone(); } }
        public int XType { get { return xType; } }
        public int XInterval { get { return xInterval; } }
        public int XAngle { get { return xAngle; } }
        #endregion

        #region | Events |

        public frmSetAxes(AxesProp[] yProp, int xType, int xInterval, int xAngle)
        {
            InitializeComponent();
            //Init. componment
            for (int i = 0; i < 6; i++)
            {
                txtTitle[i] = new TextBox();
                txtTitle[i].Text = yProp[i].Title;
                txtTitle[i].MaxLength = 4;
                txtTitle[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtTitle[i], 0, i + 1);
                cboUnit[i] = new ComboBox();
                cboUnit[i].Items.AddRange(frmRecorderDrawer.UNIT_TABLE);
                cboUnit[i].SelectedIndex = yProp[i].Unit;
                cboUnit[i].DropDownStyle = ComboBoxStyle.DropDownList;
                tblMain.Controls.Add(cboUnit[i], 1, i + 1);
                txtMin[i] = new TextBox();
                txtMin[i].Text = yProp[i].Min.ToString(); ;
                txtMin[i].KeyPress += txtProp_KeyPress;
                txtMin[i].Dock = DockStyle.Fill;
                txtMin[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMin[i], 2, i + 1);
                txtMax[i] = new TextBox();
                txtMax[i].Text = yProp[i].Max.ToString();
                txtMax[i].KeyPress += txtProp_KeyPress;
                txtMax[i].Dock = DockStyle.Fill;
                txtMax[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtMax[i], 3, i + 1);
                txtInterval[i] = new TextBox();
                txtInterval[i].Text = yProp[i].Interval.ToString();
                txtInterval[i].KeyPress += txtProp_KeyPress;
                txtInterval[i].Dock = DockStyle.Fill;
                txtInterval[i].TextAlign = HorizontalAlignment.Center;
                tblMain.Controls.Add(txtInterval[i], 4, i + 1);
            }
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            //Init. cboXAngle
            cboXAngle.Items.Clear();
            for (int i = -90; i <= 90; i += 5)
                cboXAngle.Items.Add(i);
            this.yProp = (AxesProp[])yProp.Clone();
            this.xType = xType;
            this.xInterval = xInterval;
            this.xAngle = xAngle;
            cboXType.SelectedIndex = xType;
            if (xInterval == 0)
                cboXInterval.SelectedIndex = 0;
            else
                cboXInterval.SelectedItem = (object)xInterval.ToString();
            cboXAngle.SelectedItem = (object)xAngle;
        }

        public void txtProp_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
                this.yProp[i] = new AxesProp(txtTitle[i].Text, cboUnit[i].SelectedIndex, float.Parse(txtMin[i].Text), float.Parse(txtMax[i].Text), float.Parse(txtInterval[i].Text));
            this.xType = cboXType.SelectedIndex;
            if (cboXInterval.SelectedIndex == 0)
                this.xInterval = 0;
            else
                this.xInterval = int.Parse(cboXInterval.SelectedItem.ToString());
            this.xAngle = (int)cboXAngle.SelectedItem;
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
