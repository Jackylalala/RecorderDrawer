using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmAnimation : Form
    {
        public frmAnimation()
        {
            InitializeComponent();
        }

        private void txtPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != 8))
                e.Handled = true;
        }

        private void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != 8))
                e.Handled = true;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            frmRecorderDrawer.Percentage = int.Parse(txtPercentage.Text);
            frmRecorderDrawer.Duration = int.Parse(txtDuration.Text);
            DialogResult = DialogResult.OK;
        }
    }
}
