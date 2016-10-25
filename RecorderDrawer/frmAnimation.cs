using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmAnimation : Form
    {
        #region | Field |
        private int percentage = 2;
        private int duration = 2000;
        #endregion

        #region | Properties |
        public int Percentage { get { return percentage; } }
        public int Duration { get { return duration; } }
        #endregion
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
            percentage = int.Parse(txtPercentage.Text);
            duration = int.Parse(txtDuration.Text);
            DialogResult = DialogResult.OK;
        }
    }
}
