using System;
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
            foreach (string name in frmRecorderDrawer.REACTOR_LIST)
                cboType.Items.Add(name);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            type = cboType.SelectedIndex - 1;
            DialogResult = DialogResult.OK;
        }
    }
}
