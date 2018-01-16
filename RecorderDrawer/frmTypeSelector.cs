using System;
using System.Drawing;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmTypeSelector : Form
    {
        private int type = -1;

        public int Type { get { return type; } }

        public frmTypeSelector(string[] typeList, string typeString, bool showHelpFig)
        {
            InitializeComponent();
            //Help figure button
            if (!showHelpFig)
            {
                btnRecorderFig.Visible = false;
                cboType.Width = btnOK.Width;
            }
            //Init. description
            Text = "選擇" + typeString;
            lblType.Text= "請選擇" + typeString;
            lblType.Left = ClientSize.Width / 2 - lblType.Width / 2;
            //Init. comboBox
            foreach (string name in typeList)
                cboType.Items.Add(name);
            int maxSize = 0;
            Graphics g = CreateGraphics();

            for (int i = 0; i < cboType.Items.Count; i++)
            {
                cboType.SelectedIndex = i;
                SizeF size = g.MeasureString(cboType.Text, cboType.Font);
                if (maxSize < (int)size.Width)
                    maxSize = (int)size.Width;
            }
            cboType.DropDownWidth = cboType.Width;
            if (cboType.DropDownWidth < maxSize)
                cboType.DropDownWidth = maxSize;
            cboType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            type = cboType.SelectedIndex;
            DialogResult = DialogResult.OK;
        }

        private void btnRecorderFig_Click(object sender, EventArgs e)
        {
            frmRecorderFigure frmRecorderFigure = new frmRecorderFigure();
            frmRecorderFigure.StartPosition = FormStartPosition.Manual;
            frmRecorderFigure.Location = new Point(Location.X + Width / 2 - frmRecorderFigure.ClientSize.Width / 2, Location.Y + Height / 2 - frmRecorderFigure.ClientSize.Height / 2);
            frmRecorderFigure.ShowDialog();
        }
    }
}
