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
    public partial class frmTimePeriod : Form
    {
        #region | Fields |
        bool limitedTimePeriod;
        DateTime startTime;
        DateTime endTime;
        #endregion

        #region | Properties |
        public bool LimitedTimePeriod { get { return limitedTimePeriod; } }
        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get { return endTime; } }
        #endregion

        #region | Events |
        public frmTimePeriod(bool limitedTimePeriod, DateTime startTime, DateTime endTime)
        {
            InitializeComponent();
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            this.limitedTimePeriod = limitedTimePeriod;
            this.startTime = startTime;
            this.endTime = endTime;
            dtpStart.Value = startTime;
            dtpEnd.Value = endTime;
            if (limitedTimePeriod)
            {
                dtpStart.Visible = true;
                dtpEnd.Visible = true;
                lblStartTime.Visible = false;
                lblEndTime.Visible = false;
            }
            else
            {
                dtpStart.Visible = false;
                dtpEnd.Visible = false;
                lblStartTime.Visible = true;
                lblEndTime.Visible = true;
            }
        }
        #endregion

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (!limitedTimePeriod)
            {
                limitedTimePeriod = true;
                dtpStart.Visible = true;
                dtpEnd.Visible = true;
                lblStartTime.Visible = false;
                lblEndTime.Visible = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (limitedTimePeriod)
            {
                startTime = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, dtpStart.Value.Day, dtpStart.Value.Hour, dtpStart.Value.Minute, dtpStart.Value.Second);
                endTime = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, dtpEnd.Value.Day, dtpEnd.Value.Hour, dtpEnd.Value.Minute, dtpEnd.Value.Second);
            }
            DialogResult = DialogResult.OK;
        }
    }
}
