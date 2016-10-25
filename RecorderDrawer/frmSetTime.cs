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
    public partial class frmSetTime : Form
    {
        #region | Field |
        private DateTime time;
        private float deltaP;
        #endregion

        #region | Properties |
        public DateTime Time { get { return time; } }
        public float DeltaP { get { return deltaP; } }
        #endregion

        public frmSetTime(DateTime time)
        {
            InitializeComponent();
            this.time = time;
            dtpTime.Value = time;
            txtDeltaP.Text = "0.01";
            deltaP = 0.01F;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            time = dtpTime.Value;
            if (!float.TryParse(txtDeltaP.Text, out deltaP))
                deltaP = 0.01F;
            DialogResult = DialogResult.OK;
        }

        private void txtDeltaP_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != 8))
                e.Handled = true;
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }
    }
}
