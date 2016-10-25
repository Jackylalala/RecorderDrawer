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
    public partial class frmTypeSelector : Form
    {
        private int type = -1;

        public int Type { get { return type; } }

        public frmTypeSelector()
        {
            InitializeComponent();
            cboType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            type = cboType.SelectedIndex - 1;
            DialogResult = DialogResult.OK;
        }
    }
}
