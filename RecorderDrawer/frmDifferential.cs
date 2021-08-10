using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace RecorderDrawer
{
    public partial class frmDifferential : Form
    {
        private static PointPairList trendSeriesRaw = new PointPairList();
        private static PointPairList trendSeriesRefined = new PointPairList();
        private static PointPairList trendSeriesD1 = new PointPairList();
        private static PointPairList trendSeriesD2 = new PointPairList();
        private static bool dataRefined = false;
        private int timeUnit;
        //Cursor index
        private int cursorIndex = -1;
        private Point oldPoint = new Point(0, 0);
        //Selection
        private bool select = false;
        private int indexSelectionStart;
        private int indexSelectionEnd;

        public static DateTime StartTime { get; private set; } = DateTime.MinValue;
        public static DateTime EndTime { get; private set; } = DateTime.MaxValue;
        public static PointPairList TrendSeries { get { if (dataRefined) return trendSeriesRefined; else return trendSeriesRaw; } }

        public frmDifferential(PointPairList data, int timeUnit)
        {
            trendSeriesRaw = data;
            this.timeUnit = timeUnit;
            StartTime = new XDate(TrendSeries[0].X);
            EndTime = new XDate(TrendSeries[TrendSeries.Count - 1].X);
            InitializeComponent();
            DrawChart();
            lblOriText.Text = TrendSeries[0].Tag.ToString();
            lblD1Text.Text = TrendSeries[0].Tag.ToString() + Environment.NewLine + "(一次微分)";
            lblD2Text.Text = TrendSeries[0].Tag.ToString() + Environment.NewLine + "(二次微分)";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData == Keys.Left || keyData == Keys.Right) &&
                chtMain.PointToClient(Cursor.Position).Y >= chtMain.GraphPane.Chart.Rect.Top &&
                chtMain.PointToClient(Cursor.Position).Y <= chtMain.GraphPane.Chart.Rect.Bottom &&
                !chtMain.Isbusy)
            {
                cursorIndex += keyData == Keys.Left ? -1 : 1;
                if (cursorIndex < 0)
                    cursorIndex = 0;
                else if (cursorIndex > TrendSeries.Count - 1)
                    cursorIndex = TrendSeries.Count - 1;
                else
                {
                    Point newPoint = chtMain.PointToScreen(new Point(
                        (int)Math.Round(chtMain.GraphPane.Chart.Rect.Width * ((float)cursorIndex / (TrendSeries.Count - 1)) + chtMain.GraphPane.Chart.Rect.X, MidpointRounding.AwayFromZero),
                        chtMain.PointToClient(Cursor.Position).Y)
                        );
                    if (Cursor.Position.X != newPoint.X)
                    {
                        oldPoint = newPoint;
                        Cursor.Position = newPoint;
                    }
                    ShowParameter(cursorIndex);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void chtMain_MouseEnter(object sender, EventArgs e)
        {
            chtMain.Invalidate();
        }

        private void chtMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataRefined = false;
                StartTime = new XDate(TrendSeries[0].X);
                EndTime = new XDate(TrendSeries[TrendSeries.Count - 1].X);
                DrawChart();
            }
        }

        private void chtMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (chtMain.Isbusy)
                return;
            if (e.Location.Equals(oldPoint)) //if no exactly move, do nothing
                return;
            oldPoint = e.Location;
            cursorIndex = CursorPositionToIndex(e.Location);
            if (cursorIndex == -1)
                return;
            else
                ShowParameter(cursorIndex);
        }

        private bool chtMain_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (chtMain.Isbusy)
                return false;
            if (!select &&
                e.Button == MouseButtons.Left &&
                chtMain.GraphPane.Chart.Rect.Contains(e.Location))
            {
                select = true;
                indexSelectionStart = CursorPositionToIndex(e.Location);
            }
            return false; //retuen false in order to show dash line by zedgraphcontrol itself
        }

        private bool chtMain_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (chtMain.Isbusy)
                return false;
            if (select)
            {
                select = false;
                if (e.Button == MouseButtons.Left)
                {
                    indexSelectionEnd = CursorPositionToIndex(e.Location, true);
                    if (indexSelectionStart != indexSelectionEnd)
                    {
                        if (indexSelectionEnd < indexSelectionStart)
                        {
                            int temp = indexSelectionStart;
                            indexSelectionStart = indexSelectionEnd;
                            indexSelectionEnd = temp;
                        }
                        StartTime = new XDate(TrendSeries[indexSelectionStart].X);
                        EndTime = new XDate(TrendSeries[indexSelectionEnd].X);
                        //Draw chart
                        RefineData();
                        DrawChart();
                    }
                }
            }
            return false; //retuen false in order to show dash line by zedgraphcontrol itself
        }

        private void RefineData()
        {
            //set time and value limit
            if (!(StartTime == new XDate(trendSeriesRaw[0].X) &&
                EndTime == new XDate(trendSeriesRaw[trendSeriesRaw.Count - 1].X)))
            {
                trendSeriesRefined = new PointPairList();
                for (int j = 0; j < trendSeriesRaw.Count; j++)
                {
                    //filtration
                    if (new XDate(trendSeriesRaw[j].X).CompareTo(new XDate(StartTime)) >= 0 && new XDate(trendSeriesRaw[j].X).CompareTo(new XDate(EndTime)) <= 0)
                        trendSeriesRefined.Add(
                            trendSeriesRaw[j].X,
                            trendSeriesRaw[j].Y,
                            trendSeriesRaw[j].Tag.ToString()
                        );
                }
                dataRefined = true;
            }
            else
                dataRefined = false; //no change to raw data
        }

        private int CursorPositionToIndex(Point mousePosition, bool ignoreBoundary = false)
        {
            int index = -1;
            //should inside the chart(between top and bottom of GrpahPane.Chart)
            if (!ignoreBoundary && (mousePosition.Y < chtMain.GraphPane.Chart.Rect.Y || mousePosition.Y > chtMain.GraphPane.Chart.Rect.Bottom))
                return -1;
            //if in the left side of left-boundary, set to 0; in the right side of right-boundary, set to max
            if (mousePosition.X < chtMain.GraphPane.Chart.Rect.X)
                index = 0;
            else if (mousePosition.X > chtMain.GraphPane.Chart.Rect.Right)
                index = TrendSeries.Count - 1;
            else
                index = (int)((TrendSeries.Count - 1) * ((mousePosition.X - chtMain.GraphPane.Chart.Rect.X) / chtMain.GraphPane.Chart.Rect.Width));
            return index;
        }

        private void ShowParameter(int index)
        {
            if (index < 0 || index >= TrendSeries.Count) //out of range
            {
                lblTimeDisplay.Text = "";
                lblOri.Text = "";
                lblD1.Text = "";
                lblD2.Text = "";
            }
            else
            {
                try
                {
                    lblTimeDisplay.Text = new XDate(TrendSeries[index].X).ToString("MM/dd HH:mm:ss");
                    lblOri.Text = TrendSeries[index].Y.ToString("0.00");
                    lblD1.Text = trendSeriesD1[index].Y.ToString("0.0000");
                    lblD2.Text = trendSeriesD2[index].Y.ToString("0.0000");
                }
                catch (Exception)
                {
                    lblTimeDisplay.Text = "";
                    lblOri.Text = "";
                    lblD1.Text = "";
                    lblD2.Text = "";
                }
            }
        }

        private void DrawChart()
        {
            GraphPane panel = chtMain.GraphPane;
            panel.Border.IsVisible = false;
            panel.Chart.IsRectAuto = true;
            panel.Chart.Rect = new RectangleF(156.96917f, 125.655365f, 680f, 369.721436f);
            //Clear series
            panel.CurveList.Clear();
            panel.GraphObjList.Clear();
            trendSeriesD1.Clear();
            trendSeriesD2.Clear();
            //Create title
            panel.Title.Text = "";
            //add first point
            trendSeriesD1.Add(new XDate(TrendSeries[0].X), 0);
            trendSeriesD2.Add(new XDate(TrendSeries[0].X), 0);
            for (int i = 1; i < TrendSeries.Count; i++)
            {
                double dt; 
                switch (timeUnit)
                {
                    default:
                    case 0: //second
                        dt = (new XDate(TrendSeries[i].X).DateTime - new XDate(TrendSeries[i - 1].X).DateTime).TotalSeconds;
                        break;
                    case 1: //minute
                        dt = (new XDate(TrendSeries[i].X).DateTime - new XDate(TrendSeries[i - 1].X).DateTime).TotalMinutes;
                        break;
                    case 2: //hour
                        dt = (new XDate(TrendSeries[i].X).DateTime - new XDate(TrendSeries[i - 1].X).DateTime).TotalHours;
                        break;
                }
                trendSeriesD1.Add(
                    new XDate(TrendSeries[i].X),
                    (TrendSeries[i].Y - TrendSeries[i - 1].Y)
                    );
                trendSeriesD2.Add(
                    new XDate(TrendSeries[i].X),
                    (trendSeriesD1[i].Y - trendSeriesD1[i - 1].Y)
                    );
            }
            LineItem ori = new LineItem(TrendSeries[0].Tag.ToString(), TrendSeries, Color.Black, SymbolType.None);
            LineItem d1 = new LineItem(TrendSeries[0].Tag.ToString() + "(一次微分)", trendSeriesD1, Color.Blue, SymbolType.None);
            LineItem d2 = new LineItem(TrendSeries[0].Tag.ToString() + "(二次微分)", trendSeriesD2, Color.Red, SymbolType.None);
            //Set x axis
            panel.YAxisList.Clear();
            panel.Y2AxisList.Clear();
            panel.X2Axis.IsVisible = false;
            panel.XAxis.Title.Text = "";
            panel.XAxis.MajorGrid.Color = Color.Gray;
            panel.XAxis.MajorGrid.DashOff = 0;
            panel.XAxis.MajorGrid.IsVisible = true;
            panel.XAxis.MajorTic.IsOutside = true;
            panel.XAxis.MajorTic.IsOpposite = false;
            panel.XAxis.MinorTic.IsOutside = false;
            panel.XAxis.MinorTic.IsOpposite = false;
            panel.XAxis.Scale.FontSpec = new FontSpec("Calibri", 10, Color.Black, true, false, false);
            panel.XAxis.Scale.FontSpec.Border.IsVisible = false;
            panel.XAxis.Scale.MaxAuto = false;
            panel.XAxis.Scale.MinAuto = false;
            panel.XAxis.Scale.MajorUnit = DateUnit.Hour;
            panel.XAxis.Scale.MinorUnit = DateUnit.Hour;
            panel.XAxis.Scale.Min = new XDate(StartTime);
            panel.XAxis.Scale.Max = new XDate(EndTime);
            panel.XAxis.Scale.MajorStep = Math.Ceiling((EndTime - StartTime).TotalMinutes / (5f * 15f)) * 15f / 60f;
            panel.XAxis.Type = AxisType.Date;
            panel.XAxis.Scale.Format = "MM/dd HH:mm:ss";
            //set y axis
            for (int i = 0; i < 3; i++)
            {
                //create axis
                Axis axis;
                if (i == 0)
                    axis = new YAxis();
                else
                    axis = new Y2Axis();
                axis.Title.Text = "";
                axis.IsVisible = true;
                axis.AxisGap = 10;
                axis.MajorGrid.Color = Color.Gray;
                axis.MajorGrid.DashOff = 0;
                axis.MajorGrid.IsVisible = true;
                axis.MajorTic.IsInside = false;
                axis.MajorTic.IsOpposite = false;
                axis.Scale.FontSpec = new FontSpec("Calibri", 10, Color.Black, true, false, false);
                axis.Scale.FontSpec.Border.IsVisible = false;
                axis.Scale.FontSpec.Angle = i == 0 ? 90 : -90;
                axis.Scale.LabelGap = 0f;
                axis.MinorTic.IsAllTics = false;
                switch (i)
                {
                    case 0:
                        axis.Scale.Max = TrendSeries.Select(m => m.Y).Max() > 0 ? TrendSeries.Select(m => m.Y).Max() * 1.2 : TrendSeries.Select(m => m.Y).Max() * 0.8;
                        axis.Scale.Min = TrendSeries.Select(m => m.Y).Min() > 0 ? TrendSeries.Select(m => m.Y).Min() * 0.8 : TrendSeries.Select(m => m.Y).Min() * 1.2;
                        panel.YAxisList.Add((YAxis)axis);
                        break;
                    case 1:
                        axis.Scale.Max = trendSeriesD1.Select(m => m.Y).Max() > 0 ? trendSeriesD1.Select(m => m.Y).Max() * 1.2 : trendSeriesD1.Select(m => m.Y).Max() * 0.8;
                        axis.Scale.Min = trendSeriesD1.Select(m => m.Y).Min() > 0 ? trendSeriesD1.Select(m => m.Y).Min() * 0.8 : trendSeriesD1.Select(m => m.Y).Min() * 1.2;
                        panel.Y2AxisList.Add((Y2Axis)axis);
                        break;
                    case 2:
                        axis.Scale.Max = trendSeriesD2.Select(m => m.Y).Max() > 0 ? trendSeriesD2.Select(m => m.Y).Max() * 1.2 : trendSeriesD2.Select(m => m.Y).Max() * 0.8;
                        axis.Scale.Min = trendSeriesD2.Select(m => m.Y).Min() > 0 ? trendSeriesD2.Select(m => m.Y).Min() * 0.8 : trendSeriesD2.Select(m => m.Y).Min() * 1.2;
                        panel.Y2AxisList.Add((Y2Axis)axis);
                        break;
                }
            }
            //add axis text and label
            for (int i = 0; i < 3; i++)
            {
                BoxObj box = new BoxObj();
                box.Fill.Color = new Color[] { Color.Black, Color.Blue, Color.Red }[i];
                box.Fill.Type = FillType.Solid;
                box.Location.Width = 0.04f;
                box.Location.Height = 0.026f;
                box.Border.IsAntiAlias = true;
                box.Location.X = new float[] { -0.03F, 1.03F, 1.105F }[i] - box.Location.Width / 2;
                box.Location.Y = -0.05F;
                box.Location.CoordinateFrame = CoordType.ChartFraction;
                panel.GraphObjList.Add(box);
                //add axis title
                TextObj title = new TextObj(new string[] { "d", "d/dt", "d\u00b2/dt\u00b2" }[i], new float[] { -0.03F, 1.03F, 1.105F }[i], -0.11F, CoordType.ChartFraction);
                title.FontSpec = new FontSpec("Calibri", 9, Color.Black, true, false, false);
                title.FontSpec.Border.IsVisible = false;
                panel.GraphObjList.Add(title);
            }
            //add curve
            ori.IsY2Axis = false;
            d1.IsY2Axis = true;
            d2.IsY2Axis = true;
            d1.YAxisIndex = 0;
            d2.YAxisIndex = 1;
            ori.Line.Width = 2f;
            d1.Line.Width = 2f;
            d2.Line.Width = 2f;
            ori.IsSelectable = false;
            d1.IsSelectable = false;
            d2.IsSelectable = false;
            panel.CurveList.Add(ori);
            panel.CurveList.Add(d1);
            panel.CurveList.Add(d2);
            //set legend
            panel.Legend.Position = LegendPos.Float;
            panel.Legend.Alignment = ContentAlignment.MiddleCenter;
            FontSpec font = new FontSpec("微軟正黑體", 10f, Color.Black, false, false, false);
            font.Border.IsVisible = false;
            panel.Legend.FontSpec = font;
            panel.Legend.FontSpec.Border.IsVisible = false;
            panel.Legend.Location = new Location(0, -0.18, 1, 0.18, CoordType.ChartFraction, AlignH.Left, AlignV.Top);
            panel.Legend.Border.IsVisible = false;
            chtMain.AxisChange();
            chtMain.Invalidate();
        }
    }
}

