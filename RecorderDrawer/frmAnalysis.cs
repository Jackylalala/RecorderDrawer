using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RecorderDrawer
{
    public partial class frmAnalysis : Form
    {
        #region | Field |

        private Series seriesVariable = new Series();
        private Series seriesDeriOfVariable = new Series();
        private int deltaTime;
        //Selection
        private bool select = false;
        private int indexSelectionStart;
        private int indexSelectionEnd;
        //Cursor index
        private int cursorIndex;

        #endregion

        #region | Events |

        public frmAnalysis(Series seriesVariable, Series seriesDeriOfVariable, AxesProp scaleVariable, int deltaTime)
        {
            InitializeComponent();
            this.seriesVariable = seriesVariable;
            this.seriesDeriOfVariable = seriesDeriOfVariable;
            this.deltaTime = deltaTime;
            chtDisplay.Series.Add(seriesVariable);
            chtDisplay.Series.Add(seriesDeriOfVariable);
            this.Text = "The change of " + seriesVariable.Name.ToLower().Substring(0, seriesVariable.Name.IndexOf("(")).Replace(" ", "") + " in " + deltaTime + " miunte(s)";
            chtDisplay.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chtDisplay.ChartAreas[0].AxisX.MinorTickMark.Enabled = true;
            chtDisplay.ChartAreas[0].AxisX.MajorTickMark.Size = 2;
            chtDisplay.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd HH:mm:ss";
            chtDisplay.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Auto;
            chtDisplay.Legends.Add("");
            chtDisplay.Legends[0].IsTextAutoFit = true;
            chtDisplay.Legends[0].Docking = Docking.Top;
            chtDisplay.Legends[0].Alignment = StringAlignment.Center;
            chtDisplay.ChartAreas[0].AxisY.Maximum = scaleVariable.Max;
            chtDisplay.ChartAreas[0].AxisY.Minimum = scaleVariable.Min;
            chtDisplay.ChartAreas[0].AxisY.Interval = scaleVariable.Interval;
            chtDisplay.ChartAreas[0].AxisY.Title = seriesVariable.Name;
            chtDisplay.ChartAreas[0].AxisY.TitleFont = new Font("Calibri", 10);
            chtDisplay.ChartAreas[0].AxisY2.Title = seriesDeriOfVariable.Name;
            chtDisplay.ChartAreas[0].AxisY2.TitleFont = new Font("Calibri", 10);
            chtDisplay.ChartAreas[0].CursorX.SelectionColor = Color.FromArgb(244, 157, 157);
            chtDisplay.ChartAreas[0].CursorX.IsUserSelectionEnabled=true;
            //Set label content and position
            lblTime.Text = "Time";
            lblVariable.Text = seriesVariable.Name;
            lblDeriOfVariable.Text = seriesDeriOfVariable.Name;
            txtTime.Text = "";
            txtVariable.Text = "";
            txtDeriOfVariable.Text = "";
            lblTime.Left = (this.Width - (lblTime.Width + 10 + txtTime.Width + 30 + lblVariable.Width + 10 + txtVariable.Width + 30 + lblDeriOfVariable.Width + 10 + txtDeriOfVariable.Width)) / 2;
            txtTime.Left = lblTime.Left + lblTime.Width + 10;
            lblVariable.Left = txtTime.Left + txtTime.Width + 30;
            txtVariable.Left = lblVariable.Left + lblVariable.Width + 10;
            lblDeriOfVariable.Left = txtVariable.Left + txtVariable.Width + 30;
            txtDeriOfVariable.Left = lblDeriOfVariable.Left + lblDeriOfVariable.Width + 10;
        }

        private void chtDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                chtDisplay.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Seconds;
                chtDisplay.ChartAreas[0].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
                if (select)
                {
                    chtDisplay.ChartAreas[0].CursorX.SelectionEnd = chtDisplay.ChartAreas[0].CursorX.Position;
                    indexSelectionStart = CursorPositionToIndex(chtDisplay.ChartAreas[0].CursorX.SelectionStart);
                    indexSelectionEnd = CursorPositionToIndex(chtDisplay.ChartAreas[0].CursorX.SelectionEnd);
                }
                cursorIndex = CursorPositionToIndex(chtDisplay.ChartAreas[0].CursorX.Position);
                ShowParameter(cursorIndex);
            }
            catch (Exception)
            { }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right)
            {
                try
                {
                    cursorIndex += keyData == Keys.Right ? 1 : -1;
                    //Set limit
                    if (cursorIndex < 0)
                        cursorIndex = 0;
                    else if (cursorIndex >= seriesVariable.Points.Count)
                        cursorIndex = seriesVariable.Points.Count;
                    chtDisplay.ChartAreas[0].CursorX.Position = IndexToCursorPosition(cursorIndex);
                    ShowParameter(cursorIndex);
                }
                catch (Exception)
                { }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region | Methods |

        private int CursorPositionToIndex(double position)
        {
            DateTime date = DateTime.FromOADate(position);
            int index = -1;
            //Try to find the closet index
            TimeSpan diff = DateTime.FromOADate(seriesVariable.Points[seriesVariable.Points.Count - 1].XValue) - DateTime.FromOADate(seriesVariable.Points[0].XValue);
            Parallel.For(0, seriesVariable.Points.Count, i =>
            {
                TimeSpan temp = date - DateTime.FromOADate(seriesVariable.Points[i].XValue);
                temp = temp.Duration();
                if (diff.CompareTo(temp) >= 0)
                {
                    diff = temp;
                    index = i;
                }
            });
            chtDisplay.ChartAreas[0].CursorX.Position = IndexToCursorPosition(index);
            return index;
        }

        private double IndexToCursorPosition(int index)
        {
            return seriesVariable.Points[index].XValue;
        }

        private void ShowParameter(int index)
        {
            if (index < 0 || index >= seriesVariable.Points.Count)
            {
                txtVariable.Text = "";
                txtDeriOfVariable.Text = "";
            }
            else
            {
                txtTime.Text = DateTime.FromOADate(seriesVariable.Points[index].XValue).ToString("HH:mm:ss");
                txtVariable.Text = seriesVariable.Points[index].YValues[0].ToString("0.00");
                //Do not forget, derivative points is LESS THAN origin points
                if ((index - (seriesVariable.Points.Count - seriesDeriOfVariable.Points.Count)) >= 0)
                    txtDeriOfVariable.Text = seriesDeriOfVariable.Points[index - (seriesVariable.Points.Count - seriesDeriOfVariable.Points.Count)].YValues[0].ToString("0.00");
                else
                    txtDeriOfVariable.Text = "";
            }
        }

        #endregion

        private void btnCalcd_Click(object sender, EventArgs e)
        {
            frmSetTime frmSetTime = new frmSetTime(DateTime.FromOADate(seriesVariable.Points[0].XValue));
            frmSetTime.StartPosition = FormStartPosition.Manual;
            frmSetTime.Location = new Point(this.Location.X + this.Width / 2 - frmSetTime.ClientSize.Width / 2, this.Location.Y + this.Height / 2 - frmSetTime.ClientSize.Height / 2);
            DateTime timeEnd = DateTime.FromOADate(seriesVariable.Points[0].XValue);
            if (frmSetTime.ShowDialog(this) == DialogResult.OK)
            {
                for (int i = 0; i < seriesDeriOfVariable.Points.Count; i++)
                {
                    if (DateTime.FromOADate(seriesDeriOfVariable.Points[i].XValue).CompareTo(frmSetTime.Time) > 0 && Math.Abs(seriesDeriOfVariable.Points[i].YValues[0]) <= frmSetTime.DeltaP)
                    {
                        timeEnd = DateTime.FromOADate(seriesDeriOfVariable.Points[i].XValue);
                        break;
                    }
                }
                if (timeEnd.CompareTo(frmSetTime.Time) > 0)
                {
                    MessageBox.Show(this, "*****壓力每" + deltaTime + "分鐘變化量絕對值小於等於"+frmSetTime.DeltaP.ToString("0.00")+"視為熟成結束*****" + Environment.NewLine +
                        "熟成開始時間：" + frmSetTime.Time.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                        "熟成結束時間：" + timeEnd.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                        "熟成花費時間：" + (timeEnd - frmSetTime.Time).TotalMinutes.ToString("0.00") + "分",
                        "熟成時間計算");
                }
                else
                    MessageBox.Show(this, "熟成未完成", "熟成時間計算");
            }
        }







    }
}
