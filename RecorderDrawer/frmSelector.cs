using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RecorderDrawer
{
    public partial class frmSelector : Form
    {
        public int Index { get; private set; } = -1;

        public frmSelector(int[] itemList, string descriptionString, bool showHelpFig)
            : this(itemList.Select(m => m.ToString()).ToArray(), descriptionString, showHelpFig) { }

        public frmSelector(float[] itemList, string descriptionString, bool showHelpFig)
            : this(itemList.Select(m => m.ToString()).ToArray(), descriptionString, showHelpFig) { }

        public frmSelector(IEnumerable<string> itemList, string descriptionString, bool showHelpFig)
            : this(itemList.ToArray(), descriptionString, showHelpFig) { }

        public frmSelector(string[] itemList, string descriptionString, bool showHelpFig)
        {
            InitializeComponent();
            //Help figure button
            if (!showHelpFig)
            {
                btnRecorderFig.Visible = false;
                cboItem.Width = btnOK.Width;
            }
            //Init. description
            Text = "選擇" + descriptionString;
            lblDescription.Text= "請選擇" + descriptionString;
            lblDescription.Left = ClientSize.Width / 2 - lblDescription.Width / 2;
            //Init. comboBox
            foreach (string name in itemList)
                cboItem.Items.Add(name);
            int maxSize = 0;
            Graphics g = CreateGraphics();
            for (int i = 0; i < cboItem.Items.Count; i++)
            {
                cboItem.SelectedIndex = i;
                SizeF size = g.MeasureString(cboItem.Text, cboItem.Font);
                if (maxSize < (int)size.Width)
                    maxSize = (int)size.Width;
            }
            cboItem.DropDownWidth = cboItem.Width;
            if (cboItem.DropDownWidth < maxSize)
                cboItem.DropDownWidth = maxSize;
            cboItem.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Index = cboItem.SelectedIndex;
            DialogResult = DialogResult.OK;
        }

        private void btnRecorderFig_Click(object sender, EventArgs e)
        {
            frmRecorderFigure frmRecorderFigure = new frmRecorderFigure();
            frmRecorderFigure.StartPosition = FormStartPosition.Manual;
            frmRecorderFigure.Location = new Point(Location.X + Width / 2 - frmRecorderFigure.ClientSize.Width / 2, Location.Y + Height / 2 - frmRecorderFigure.ClientSize.Height / 2);
            frmRecorderFigure.ShowDialog(this);
        }
    }
}
