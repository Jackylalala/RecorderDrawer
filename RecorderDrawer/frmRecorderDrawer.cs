using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace RecorderDrawer
{
    public partial class frmRecorderDrawer : Form
    {

        #region | Field |
        //Reactor list
        public static string[] REACTOR_LIST = {
            "3L反應器(#2)",
            "1L & 5L反應器國華(#4, #5)",
            "2L反應器#201(#1, #3)",
            "R1-CHPPO",
            "R1-EOD",
            "R2-CHPPO",
            "R2-EOD(#7)",
            "R3-CHPPO",
            "R3-EOD","CHPPO Pilot",
            "2F遠東新反應器(#6)"
        };
        //Unit table
        public static string[] UNIT_TABLE = { "\u00b0C", "bar", "psi", "ml/min", "rpm", "Ncm", "%", "kg/cm\u00b2", "mm", "g/hr", "g", "h-1" };
        //Datetime format
        private static string[] dateTimeList = { 
                            "yyyy/M/d tt hh:mm:ss", 
                            "yyyy/MM/dd tt hh:mm:ss", 
                            "yyyy/M/d hh:mm:ss tt", 
                            "yyyy/MM/dd hh:mm:ss tt",
                            "yyyy/MM/dd HH:mm:ss", 
                            "yy/MM/dd HH:mm:ss",
                            "yy/MM/d HH:mm:ss",
                            "yyyy/M/d HH:mm:ss", 
                            "yyyy/MM/dd H:m:s", 
                            "yyyy/M/d H:m:s",
                            "yyyy/M/d tt hh:mm",
                            "yyyy/MM/dd tt hh:mm",
                            "yyyy/M/d hh:mm tt",
                            "yyyy/MM/dd hh:mm tt",
                            "yyyy/MM/dd HH:mm",
                            "yy/MM/dd HH:mm",
                            "yy/MM/d HH:mm",
                            "yyyy/M/d HH:mm",
                            "yyyy/MM/dd H:m",
                            "yyyy/M/d H:m",
                            "yyyy/M/d", 
                            "yyyy/MM/dd",
                            "M/d HH:mm:ss"
                        };
        //X position of y axis label
        private static readonly float[] yLabelXPos = new float[] { 13.3F, 84.3F, 8.3F, 89.3F, 3.3F, 94.3F };
        private static readonly float[] yLabelYPos = new float[] { 15F, 12.5F, 10F, 7.5F, 5F, 2.5F };
        private List<int> hiddenList = new List<int>(); //Hidden list of series
        private string rawFileName; //File name
        private Series[] trendSeries;
        //Recorder type, -1 means auto detect, according to REACTOR_LIST
        private static int type;
        //Y axis properties
        private static AxesProp[][] yProp = new AxesProp[11][];
        //Data information
        private List<RecordData> rawData = new List<RecordData>();
        private int dataCount;
        //Color of series
        private static readonly Color[] seriesColor =
            new Color[] { 
                Color.Red, Color.Lime, Color.Blue, Color.Gold, Color.Fuchsia, 
                Color.Aqua, Color.Purple, Color.FromArgb(128, 64, 0), Color.FromArgb(251, 114, 13), Color.FromArgb(0, 128, 0), 
                Color.FromArgb(250, 141, 171), Color.FromArgb(218, 151,240), Color.FromArgb(137,172,254), Color.FromArgb(1,201,101), Color.FromArgb(201,41,57), 
                Color.FromArgb(133,92,150), Color.FromArgb(185,80,57), Color.FromArgb(112,131,112), Color.FromArgb(121,117,125), Color.FromArgb(87,36,206), 
                Color.FromArgb(27,159,216), Color.FromArgb(186,56,56), Color.FromArgb(194,203,39), Color.FromArgb(0,242,43) };
        private static readonly Color[] seriesSelectionBoxTextColor =
            new Color[] { 
                Color.Black, Color.Black, Color.White, Color.Black, Color.Black, 
                Color.Black, Color.White, Color.White, Color.Black, Color.White,
                Color.White, Color.White, Color.White, Color.White, Color.White,
                Color.White, Color.White, Color.White, Color.White, Color.White,
                Color.White, Color.White, Color.Black, Color.Black};
        //Chart item selection box
        private CheckBox[] chkChartItem;
        //Chart data display box
        private Label[] lblDataDisplay;
        //Synchronization lock
        private static object syncHandle = new object();
        //Series map (6 axes, 6 type)
        //Axes: 內溫, 外溫, 壓力, 流速(升溫), 轉速, 扭力 (Default)
        private int[][][] seriesMap = new int[][][] { 
            new int[][] { new int[] { 0, 1, 6 }, new int[] { 0, 4 }, new int[] { 0 },   //內溫(溫度)
                new int[] { 2 }, new int[] { 2 }, new int[] { 5, 2 }, 
                new int[] { 5, 2 }, new int[] { 2, 5 }, new int[] { 2, 5 },
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 20 }, new int[] { 0, 1, 6 }}, 
            new int[][] { new int[] { 2, 3 }, new int[] { 1, 2, 3, 5 }, new int[] { 1, 4 },  //外溫(壓力)
                new int[] { 4 }, new int[] { 4 }, new int[] { 3 }, 
                new int[] { 3 }, new int[] { 3 }, new int[] { 3 },
                new int[] { 10, 11, 12, 13, 14, 15, 18, 22 }, new int[] { 2, 3 }}, 
            new int[][] { new int[] { 5 }, new int[] { 6 }, new int[] { 2 },   //壓力(液位)
                new int[] { 1 }, new int[] { 1 }, new int[] { 1 }, 
                new int[] { 1 }, new int[] { 1 }, new int[] { 1 },
                new int[] { 16 }, new int[] { 5 }}, 
            new int[][] { new int[] { 4 }, new int[] { 9 }, new int[] { 5 },   //流速(升溫)(流量)
                new int[] { 3 }, new int[] { 3 }, new int[] { 4 }, 
                new int[] { 4 }, new int[] { 4 }, new int[] { 4 },
                new int[] { 17, 19 }, new int[] { 4 }},  
            new int[][] { new int[] { 7 }, new int[] { 7 }, new int[] { 3 },  //轉速(總量)
                new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, 
                new int[] { 0 }, new int[] { 0 }, new int[] { 0 },
                new int[] { 21 }, new int[] { 7 }},  
            new int[][] { new int[] { }, new int[] { 8 }, new int[] { },  //扭力(WHSV)
                new int[] { }, new int[] { }, new int[] { }, 
                new int[] { }, new int[] { }, new int[] { },
                new int[] { }, new int[] { }} }; //WHSV: Column 23尚未建立
        //Threshold
        private float minLimit = 0;
        private float maxLimit = 0;
        //Drag&drop
        private bool validFile;
        //Cursor index
        private int cursorIndex;
        //Selection
        private bool select = false;
        private int indexSelectionStart;
        private int indexSelectionEnd;
        //Delegate function
        private delegate bool SetFrameWorkDelegate(int percentage);
        private delegate bool DrawChartDelegate(Chart chart);
        #endregion

        #region | Properties |
        public static AxesProp[] YProp
        {
            get
            {
                if (type == -1)
                    return null;
                else
                    return yProp[type];
            }
            set
            {
                yProp[type] = value;
            }
        }
        public static int XType { get; set; } //0: text, 1: DateTime
        public static int XInterval { get; set; } //X axis interval in minute
        public static int XAngle { get; set; } //X axis label angle
        public static bool LimitedTimePeriod { get; set; }
        public static DateTime StartTime { get; set; }
        public static DateTime EndTime { get; set; }
        public static string TitleText { get; set; } = "";
        public static float Density { get; set; }
        public static float CostPerHour { get; set; }
        public static float ReactorSize { get; set; } //In Liter
        public static int Percentage { get; set; }
        public static int Duration { get; set; }
        #endregion

        #region | Event |
        public frmRecorderDrawer()
        {
            InitializeComponent();
        }

        private void munAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void munOpen_Click(object sender, EventArgs e)
        {
            CustomOpenFileDialog ofd = new CustomOpenFileDialog();
            ofd.Title = "選擇數據檔";
            ofd.Filter = "逗號分隔值的文字檔案(*.csv)|*.csv";
            ofd.SchemaType = -1;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    rawFileName = ofd.FileName;
                    type = ofd.SchemaType;
                    txtFilePath.Text = "來源檔案︰" + rawFileName;
                    munAnalysis.Enabled = true;
                    LimitedTimePeriod = false;
                    munAnalysis_Click(null, new EventArgs());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("請選擇數據檔");
            }
        }

        private void munAnalysis_Click(object sender, EventArgs e)
        {
            hiddenList.Clear();
            LimitedTimePeriod = false;
            if (!float.TryParse(txtMinLimit.Text, out minLimit))
            {
                txtMinLimit.Text = "-32767";
                minLimit = -32767;
            }
            if (!float.TryParse(txtMaxLimit.Text, out maxLimit))
            {
                txtMaxLimit.Text = "32767";
                maxLimit = 32767;
            }
            bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
        }

        private void munTitle_Click(object sender, EventArgs e)
        {
            string input = TitleText;
            if (InputBox("圖表標題", "請輸入圖表標題", ref input) == DialogResult.OK)
            {
                TitleText = input;
                //Draw chart
                bgdWorkerDraw.RunWorkerAsync(new object[] { 0, chtMain });
            }
        }

        private void chtMain_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (e.ChartElement is Legend)
            {
                Chart c = (Chart)sender;
                Graphics g = e.ChartGraphics.Graphics;

                //The legend
                Legend l = c.Legends[0];
                Font legendFont = l.Font;

                //Absolute dimensions of the legend (New legend will be based on this.. won't be exact.)
                RectangleF pos = e.ChartGraphics.GetAbsoluteRectangle(l.Position.ToRectangleF());

                //Padding between line and text (horizontal) and each item (vertical)
                float horizontalPadding = 10;
                float verticalPadding = 1;

                //Absolute dimensions of one legend "cell"
                int maxItem = 8;
                switch (type)
                {
                    default:
                    case 0:
                        maxItem = 8;
                        break;
                    case 1:
                        maxItem = 10;
                        break;
                    case 2:
                        maxItem = 6;
                        break;
                    case 3:
                    case 4:
                        maxItem = 5;
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        maxItem = 6;
                        break;
                    case 9:
                        maxItem = 23; //Including WHSV is 24
                        break;
                }
                //Determine max items in one row
                float columnCount = maxItem - hiddenList.Count > 6 ? Math.Min(6, (int)(Math.Ceiling((maxItem - hiddenList.Count) / 2F))) : maxItem - hiddenList.Count;
                float rowCount = maxItem - hiddenList.Count > 6 ? 2 : 1;
                float itemHeight = (pos.Height - (columnCount - 1) * verticalPadding) / rowCount;
                //Twice the colume count(line and text)
                float itemWidth = (pos.Width - (rowCount - 1) * horizontalPadding) / (2 * columnCount);

                //Draw a white box on top of the default legend to hide it
                g.FillRectangle(Brushes.White, pos);

                int counter = 0;
                foreach (Series item in c.Series)
                {
                    if (counter > 11)
                        break;
                    if (!item.Name.Contains("Copy"))
                    {
                        //Modify font size
                        Font newLegandFont = new Font(legendFont.FontFamily, Math.Min(StringWidth("內溫PV", legendFont) / StringWidth(item.Name, legendFont) * legendFont.Size, legendFont.Size));
                        //Line no thicker than the item height
                        Pen p = new Pen(item.Color, Convert.ToSingle(Math.Min(item.BorderWidth, itemHeight)));

                        //Line
                        PointF startPoint = new PointF(
                            pos.X + (itemWidth + horizontalPadding) * (counter % columnCount * 2),
                            pos.Y + (itemHeight + verticalPadding) * (int)(counter / columnCount) + itemHeight / 3);
                        PointF endPoint = new PointF(
                            startPoint.X + itemWidth,
                            startPoint.Y);
                        g.DrawLine(p, startPoint, endPoint);

                        //Text
                        startPoint = new PointF(
                            pos.X + (itemWidth + horizontalPadding) * (counter % columnCount * 2 + 1),
                            pos.Y + (itemHeight + verticalPadding) * (int)(counter / columnCount));
                        g.DrawString(item.Name, newLegandFont, Brushes.Black, startPoint.X, startPoint.Y);
                        counter++;
                    }
                }
            }
        }

        private void chtMain_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                chtMain.ChartAreas["main"].CursorX.IntervalType = DateTimeIntervalType.Seconds;
                chtMain.ChartAreas["main"].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
                if (select)
                {
                    chtMain.ChartAreas["main"].CursorX.SelectionEnd = chtMain.ChartAreas["main"].CursorX.Position;
                    indexSelectionStart = CursorPositionToIndex(chtMain.ChartAreas["main"].CursorX.SelectionStart);
                    indexSelectionEnd = CursorPositionToIndex(chtMain.ChartAreas["main"].CursorX.SelectionEnd);
                }
                cursorIndex = CursorPositionToIndex(chtMain.ChartAreas["main"].CursorX.Position);
                ShowParameter(cursorIndex);
            }
            catch (Exception)
            { }
        }

        private void chtMain_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            LimitedTimePeriod = true;
            StartTime = rawData[indexSelectionStart].Date;
            EndTime = rawData[indexSelectionEnd].Date;
            //Draw chart
            bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
        }

        private void chtMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (!select && e.Button == MouseButtons.Left)
            {
                select = true;
                chtMain.ChartAreas["main"].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
                chtMain.ChartAreas["main"].CursorX.SelectionStart = chtMain.ChartAreas["main"].CursorX.Position;
                chtMain.ChartAreas["main"].CursorX.SelectionEnd = chtMain.ChartAreas["main"].CursorX.Position;
                indexSelectionStart = CursorPositionToIndex(chtMain.ChartAreas["main"].CursorX.SelectionStart);
                indexSelectionEnd = CursorPositionToIndex(chtMain.ChartAreas["main"].CursorX.SelectionEnd);
            }
        }

        private void chtMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (select)
                select = false;
            if (e.Button == MouseButtons.Left)
            {
                if (indexSelectionStart != indexSelectionEnd)
                {
                    LimitedTimePeriod = true;
                    if (indexSelectionEnd < indexSelectionStart)
                    {
                        int temp = indexSelectionStart;
                        indexSelectionStart = indexSelectionEnd;
                        indexSelectionEnd = temp;
                    }
                    StartTime = rawData[indexSelectionStart].Date;
                    EndTime = rawData[indexSelectionEnd].Date;
                    //Draw chart
                    bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
                }
                else
                    chtMain.ChartAreas["main"].CursorX.SetSelectionPosition(0, 0);
            }
        }

        private void chtMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                munAnalysis_Click(new object(), new EventArgs());
        }

        private void munDetailedSetting_Click(object sender, EventArgs e)
        {
            frmDetailedSetting frmDetailedSetting = new frmDetailedSetting();
            frmDetailedSetting.StartPosition = FormStartPosition.Manual;
            frmDetailedSetting.Location = new Point(Location.X + Width / 2 - frmDetailedSetting.ClientSize.Width / 2, Location.Y + Height / 2 - frmDetailedSetting.ClientSize.Height / 2);
            if (frmDetailedSetting.ShowDialog() == DialogResult.OK)
                bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
        }

        private void frmRecorderDrawer_Load(object sender, EventArgs e)
        {
            //Load the settings
            try
            {
                string[] yPropString = Properties.Settings.Default.yProp.Split(',');
                int counter = 0;
                for (int i = 0; i < yProp.Length; i++)
                {
                    yProp[i] = new AxesProp[6];
                    for (int j = 0; j < yProp[i].Length; j++)
                    {
                        yProp[i][j] = new AxesProp(
                            yPropString[counter++],
                            int.Parse(yPropString[counter++]),
                            float.Parse(yPropString[counter++]),
                            float.Parse(yPropString[counter++]),
                            float.Parse(yPropString[counter++]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace + ": " + ex.Message);
                //Default value
                yProp=new AxesProp[11][]{
                    new AxesProp[6]{  //Type 0
                        new AxesProp("內溫", 0, 0, 220, 20), 
                        new AxesProp("外溫", 0, 0, 220, 20), 
                        new AxesProp("壓力", 1, -2, 20, 2), 
                        new AxesProp("流速", 3, 0, 22, 2), 
                        new AxesProp("轉速", 4, 0, 1100, 100), 
                        new AxesProp("", 5, 0, 220, 20)}, 
                    new AxesProp[6]{  //Type 1
                        new AxesProp("內溫", 0, 0, 220, 20), 
                        new AxesProp("外溫", 0, 0, 220, 20), 
                        new AxesProp("壓力", 1, -2, 20, 2), 
                        new AxesProp("流速", 3, 0, 22, 2), 
                        new AxesProp("轉速", 4, 0, 1100, 100), 
                        new AxesProp("扭力", 5, 0, 220, 20)}, 
                    new AxesProp[6]{  //Type 2
                        new AxesProp("內溫", 0, 0, 220, 20), 
                        new AxesProp("外溫", 0, 0, 220, 20), 
                        new AxesProp("壓力", 1, -2, 20, 2), 
                        new AxesProp("流速", 3, 0, 22, 2), 
                        new AxesProp("轉速", 4, 0, 1100, 100), 
                        new AxesProp("", 5, 0, 220, 20)}, 
                    new AxesProp[6]{  //Type 3 R1-CHPPO
                        new AxesProp("內溫", 0, 0, 150, 15), 
                        new AxesProp("外溫", 0, 0, 150, 15), 
                        new AxesProp("壓力", 2, 0, 500, 50), 
                        new AxesProp("升溫", 6, 0, 10, 1), 
                        new AxesProp("轉速", 4, 0, 600, 60), 
                        new AxesProp("", 5, 0, 100, 10)}, 
                    new AxesProp[6]{  //Type 4 R1-EOD
                        new AxesProp("內溫", 0, 0, 150, 15), 
                        new AxesProp("外溫", 0, 0, 200, 20), 
                        new AxesProp("壓力", 2, 0, 150, 15), 
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 400, 40),
                        new AxesProp("", 5, 0, 100, 10)},  
                    new AxesProp[6]{  //Type 5 R2-CHPPO
                        new AxesProp("內溫", 0, 0, 150, 15), 
                        new AxesProp("外溫", 0, 0, 150, 15), 
                        new AxesProp("壓力", 2, 0, 500, 50), 
                        new AxesProp("升溫", 6, 0, 10, 1), 
                        new AxesProp("轉速", 4, 0, 600, 60), 
                        new AxesProp("", 5, 0, 100, 10)}, 
                    new AxesProp[6]{  //Type 6 R2-EOD
                        new AxesProp("內溫", 0, 0, 200, 20), 
                        new AxesProp("外溫", 0, 0, 200, 20), 
                        new AxesProp("壓力", 2, 0, 200, 20), 
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 1000, 100),
                        new AxesProp("", 5, 0, 100, 10)}, 
                    new AxesProp[6]{  //Type 7 R3-CHPPO
                        new AxesProp("內溫", 0, 0, 150, 15), 
                        new AxesProp("外溫", 0, 0, 150, 15), 
                        new AxesProp("壓力", 2, 0, 500, 50), 
                        new AxesProp("升溫", 6, 0, 10, 1), 
                        new AxesProp("轉速", 4, 0, 600, 60), 
                        new AxesProp("", 5, 0, 100, 10)}, 
                    new AxesProp[6]{  //Type 8 R3-EOD
                        new AxesProp("內溫", 0, 0, 150, 15), 
                        new AxesProp("外溫", 0, 0, 200, 20), 
                        new AxesProp("壓力", 2, 0, 150, 15), 
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 400, 40),
                        new AxesProp("", 5, 0, 100, 10)}, 
                    new AxesProp[6]{  //Type 9 CHPPO Pilot
                        new AxesProp("溫度", 0, 0, 200, 20), 
                        new AxesProp("壓力", 7, 0, 100, 10), 
                        new AxesProp("液位", 8, 0, 500, 50),
                        new AxesProp("流量", 9, 0, 250, 25),
                        new AxesProp("總量", 10, 0, 300, 30),
                        new AxesProp("WHSV", 11, 0, 15, 1.5F)},
                    new AxesProp[6]{  //Type 10 2F遠東新反應器(#7)
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                };
            }
            XType = StrToIntDef(Properties.Settings.Default.xType, 0);
            XInterval = StrToIntDef(Properties.Settings.Default.xInterval, 1000);
            XAngle = StrToIntDef(Properties.Settings.Default.xAngle, 0);
            chkXGrid.Checked = true;
            chkYGrid.Checked = true;
        }

        private void frmRecorderDrawer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save settings
            string yPropString = "";
            for (int i = 0; i < yProp.Length; i++)
            {
                for (int j = 0; j < yProp[i].Length; j++)
                {
                    yPropString += yProp[i][j].Title + ",";
                    yPropString += Convert.ToString(yProp[i][j].Unit) + ",";
                    yPropString += Convert.ToString(yProp[i][j].Min) + ",";
                    yPropString += Convert.ToString(yProp[i][j].Max) + ",";
                    yPropString += Convert.ToString(yProp[i][j].Interval) + ",";
                }
            }
            Properties.Settings.Default.yProp = yPropString;
            Properties.Settings.Default.xType = Convert.ToString(XType);
            Properties.Settings.Default.xInterval = Convert.ToString(XInterval);
            Properties.Settings.Default.xAngle = Convert.ToString(XAngle);
            Properties.Settings.Default.Save();
        }

        private void munExportImgToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialogWithResolution sfd = new SaveFileDialogWithResolution(true);
            try
            {
                sfd.Title = "匯出圖片";
                sfd.Filter = @"Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg,*.jpeg)|*.jpg;*.jpeg|GIF(*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif;*.tiff";
                sfd.FilterIndex = 2;
                sfd.ResolutionType = ResolutionType.extreme;
                sfd.BorderType = BorderType.medium;
                //使用者按下確認之後紀錄檔名
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //Get extension
                    string ext = Path.GetExtension(sfd.FileName).ToLower();
                    ImageFormat format = ImageFormat.Jpeg;
                    switch (ext)
                    {
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".gif":
                            format = ImageFormat.Gif;
                            break;
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                        case ".tif":
                        case ".tiff":
                            format = ImageFormat.Tiff;
                            break;
                        default:
                            MessageBox.Show("Invalid format.", "ImageHandler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                    //存為圖片
                    float zoom = 1;
                    switch (sfd.ResolutionType)
                    {
                        case ResolutionType.normal:
                            zoom = 1;
                            break;
                        case ResolutionType.medium:
                            zoom = 2;
                            break;
                        case ResolutionType.high:
                            zoom = 3;
                            break;
                        case ResolutionType.extreme:
                            zoom = 5;
                            break;
                        case ResolutionType.extremehigh:
                            zoom = 10;
                            break;
                    }
                    int whiteBorder = 0;
                    switch (sfd.BorderType)
                    {
                        case BorderType.none:
                            whiteBorder = 0;
                            break;
                        case BorderType.small:
                            whiteBorder = 2;
                            break;
                        case BorderType.medium:
                            whiteBorder = 5;
                            break;
                        case BorderType.large:
                            whiteBorder = 10;
                            break;
                    }
                    Bitmap image = Trim(new Bitmap(SaveExpandedImg(zoom, format)), (int)(whiteBorder * zoom));
                    image.Save(sfd.FileName);
                    MessageBox.Show("匯出圖片成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void munToClip_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            ms = SaveExpandedImg(5.0F,ImageFormat.Bmp);
            if (ms != null)
            {
                Bitmap bm = Trim(new Bitmap(ms), 25);
                Clipboard.SetImage(bm);
                MessageBox.Show("圖片已複製到剪貼簿");
            }
            else
                MessageBox.Show("圖片產生失敗");
        }

        private void txtThreshold_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allow integer (no decimal point)
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar != 8))
                e.Handled = true;
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
            // only allow sign symbol at first char
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
                e.Handled = true;
            if ((e.KeyChar == '-') && !((sender as TextBox).Text.IndexOf('-') > -1) && ((sender as TextBox).SelectionStart != 0))
                e.Handled = true;
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
                    else if (cursorIndex >= rawData.Count)
                        cursorIndex = rawData.Count;
                    chtMain.ChartAreas["main"].CursorX.Position = IndexToCursorPosition(cursorIndex);
                    ShowParameter(cursorIndex);
                }
                catch (Exception)
                { }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void frmRecorderDrawer_DragDrop(object sender, DragEventArgs e)
        {
            if (validFile)
            {
                munAnalysis.Enabled = true;
                LimitedTimePeriod = false;
                //Type select
                frmTypeSelector frmTypeSelector = new frmTypeSelector();
                frmTypeSelector.StartPosition = FormStartPosition.Manual;
                frmTypeSelector.Location = new Point(this.Location.X + this.Width / 2 - frmTypeSelector.ClientSize.Width / 2, this.Location.Y + this.Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                if (frmTypeSelector.ShowDialog() == DialogResult.OK)
                {
                    type = frmTypeSelector.Type;
                    //Draw chart
                    bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
                }
            }
        }

        private void frmRecorderDrawer_DragEnter(object sender, DragEventArgs e)
        {
            validFile = GetFilename(out rawFileName, e);
            if (validFile)
            {
                txtFilePath.Text = "來源檔案︰" + rawFileName;
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void chkThreshold_Changed(object sender, EventArgs e)
        {
            if (rawFileName != null)
                munAnalysis_Click(null, new EventArgs());
        }

        private void txtMaxLimit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                chkThreshold_Changed(null, new EventArgs());
        }

        private void txtMinLimit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                chkThreshold_Changed(null, new EventArgs());
        }

        private void munStatList_Click(object sender, EventArgs e)
        {
            if (type == 9)
            {
                MessageBox.Show("不適用此功能");
                return;
            }
            if (rawData == null)
                MessageBox.Show("沒有數據！");
            else
            {
                //Cal. & Show
                Form frmMsg = new Form();
                TextBox txtMsg = new TextBox();
                txtMsg.Multiline = true;
                txtMsg.Dock = DockStyle.Fill;
                txtMsg.ReadOnly = true;
                txtMsg.TabStop = false;
                txtMsg.Font = new Font("微軟正黑體", 10);
                txtMsg.ScrollBars = ScrollBars.Vertical;
                txtMsg.Text = calculate();
                frmMsg.Text = "統計數據";
                frmMsg.MaximizeBox = false;
                frmMsg.MinimizeBox = false;
                frmMsg.Controls.Add(txtMsg);
                frmMsg.Width = 400;
                frmMsg.Height = 400;
                frmMsg.StartPosition = FormStartPosition.Manual;
                frmMsg.Location = new Point(this.Location.X + this.Width / 2 - frmMsg.ClientSize.Width / 2, this.Location.Y + this.Height / 2 - frmMsg.ClientSize.Height / 2);
                frmMsg.ShowDialog();
            }
        }

        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (rawFileName != null)
                munAnalysis_Click(null, new EventArgs());
        }

        private void ChangeOfPressure(object sender, EventArgs e)
        {
            //Calcd. derivation
            int deltaTime = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            Series seriesPressureDeri = new Series();
            seriesPressureDeri.Name = "Change of Pressure (" + UNIT_TABLE[yProp[type][2].Unit] + "/" + deltaTime + "min)";
            seriesPressureDeri.ChartType = SeriesChartType.FastLine;
            seriesPressureDeri.Color = Color.Red;
            seriesPressureDeri.YAxisType = AxisType.Secondary;
            int pointPerPeriod = (int)(dataCount / (rawData[rawData.Count - 1].Date - rawData[0].Date).TotalMinutes) * deltaTime;
            for (int i = pointPerPeriod; i < rawData.Count; i++)
                seriesPressureDeri.Points.AddXY(
                    rawData[i].Date,
                    (double)Math.Round(rawData[i].Parameter[seriesMap[2][type][0]] - rawData[i - pointPerPeriod].Parameter[seriesMap[2][type][0]], 2));
            Series seriesPressureCopy = new Series();
            seriesPressureCopy.Name = "Pressure (" + UNIT_TABLE[yProp[type][2].Unit] + ")";
            seriesPressureCopy.ChartType = SeriesChartType.FastLine;
            seriesPressureCopy.Color = Color.Blue;
            seriesPressureCopy.XValueType = ChartValueType.DateTime;
            seriesPressureCopy.YAxisType = AxisType.Primary;
            foreach (RecordData item in rawData)
                seriesPressureCopy.Points.AddXY(item.Date, item.Parameter[seriesMap[2][type][0]]);
            //Show results
            frmAnalysis frmAnalysis = new frmAnalysis(seriesPressureCopy, seriesPressureDeri, yProp[type][2], deltaTime);
            frmAnalysis.Show();
        }

        public void ChangeChartItems(object sender, EventArgs e)
        {
            int index = int.Parse(((CheckBox)sender).Tag.ToString());
            //Modify hiddenlist
            if (!((CheckBox)sender).Checked)
            {
                if (!hiddenList.Contains(index))
                    hiddenList.Add(index);
            }
            else
            {
                if (hiddenList.Contains(index))
                    hiddenList.Remove(index);
            }
            //Reset chart
            bgdWorkerDraw.RunWorkerAsync(new object[] { 0, chtMain });
        }

        private void bgdWorkerAnimation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Load parameter
                int outputType = int.Parse((e.Argument as object[])[0].ToString());
                string fileName = (e.Argument as object[])[1].ToString();
                string mailAddress = (e.Argument as object[])[2].ToString();
                int percentage = int.Parse((e.Argument as object[])[3].ToString());
                if (percentage < 1)
                    percentage = 1;
                int duration = int.Parse((e.Argument as object[])[4].ToString());
                if (duration < 10)
                    duration = 10;
                //Draw chart
                int readLimit = 0;
                Chart chtDraw = new Chart();
                chtDraw.PostPaint += chtMain_PostPaint;
                chtDraw.Visible = false;
                chtDraw.Width = chtMain.Width;
                chtDraw.Height = chtMain.Height;
                List<MemoryStream> frameList = new List<MemoryStream>();
                bgdWorkerAnimation.ReportProgress(30 * (frameList.Count + 1) / (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)), "Generate chart(1/" + (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)) + ")");
                if (SetFrameWork(1))
                    DrawChart(chtDraw);
                frameList.Add(new MemoryStream());
                chtDraw.SaveImage(frameList[frameList.Count - 1], ImageFormat.Jpeg);
                for (readLimit = percentage; readLimit < 100; readLimit += percentage)
                {
                    bgdWorkerAnimation.ReportProgress(30 * (frameList.Count + 1) / (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)), "Generate chart(" + (frameList.Count + 1) + "/" + (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)) + ")");
                    if (SetFrameWork(readLimit))
                        DrawChart(chtDraw);
                    frameList.Add(new MemoryStream());
                    chtDraw.SaveImage(frameList[frameList.Count - 1], ImageFormat.Jpeg);
                }
                bgdWorkerAnimation.ReportProgress(30 * (frameList.Count + 1) / (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)), "Generate chart(" + (frameList.Count + 1) + "/" + (1 + (int)Math.Round(100F / percentage, 0, MidpointRounding.AwayFromZero)) + ")");
                if (SetFrameWork(100))
                    DrawChart(chtDraw);
                frameList.Add(new MemoryStream());
                chtDraw.SaveImage(frameList[frameList.Count - 1], ImageFormat.Jpeg);
                //Generate gif
                AnimatedGifEncoder aniGif = new AnimatedGifEncoder();
                MemoryStream ms = new MemoryStream();
                aniGif.Start(ms);
                aniGif.SetDelay(duration);
                aniGif.SetRepeat(0);
                for (int i = 0; i < frameList.Count; i++)
                {
                    bgdWorkerAnimation.ReportProgress(30 + 70 * i / (frameList.Count - 1), "Add frame(" + (i + 1) + "/" + frameList.Count + ")");
                    aniGif.AddFrame(Image.FromStream(frameList[i]));
                }
                aniGif.Finish();
                if (outputType == 0)
                {
                    bgdWorkerAnimation.ReportProgress(99, "Saving file");
                    new Bitmap(ms).Save(fileName);
                    MessageBox.Show("動畫匯出成功！", "Alert");
                }
                else
                {
                    bgdWorkerAnimation.ReportProgress(99, "Sending mail");
                    //Convert to byte[] and mail
                    ImageConverter ic = new ImageConverter();
                    byte[] ba = (byte[])ic.ConvertTo(new Bitmap(ms), typeof(byte[]));
                    MemoryStream msMail = new MemoryStream(ba);
                    Attachment attach = new Attachment(msMail, "Trend Animation - " + DateTime.Now.ToString("yyyy/MM/dd"));
                    attach.ContentDisposition.FileName = fileName + ".gif";
                    using (MailMessage myMail = new MailMessage())
                    {
                        myMail.Subject = "Trend Animation - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        myMail.From = new MailAddress("jackylalala9527@gmail.com", "Trend Chart Mailer");
                        myMail.To.Add(mailAddress);
                        myMail.SubjectEncoding = Encoding.UTF8;
                        myMail.IsBodyHtml = true;
                        myMail.BodyEncoding = Encoding.UTF8;
                        myMail.Body = "Trend Animation - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        myMail.Attachments.Add(attach);
                        //發送Mail
                        if (Mail(myMail))
                            MessageBox.Show("成功寄出動畫到" + mailAddress, "Alert");
                        else
                            MessageBox.Show("寄出動畫失敗！", "Alert");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bgdWorkerAnimation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 100)
                pgbProcess.Value = e.ProgressPercentage;
            else
                pgbProcess.Value = 99;
            lblStatus.Text = e.UserState.ToString();
        }

        private void bgdWorkerDraw_DoWork(object sender, DoWorkEventArgs e)
        {
            //Define stage
            //0: draw, 1: setFrameWork & draw, 2: all
            int stage = int.Parse((e.Argument as object[])[0].ToString());
            bgdWorkerDraw.ReportProgress(0, "Start");
            switch (stage)
            {
                case 0:
                    bgdWorkerDraw.ReportProgress(99, "Draw chart");
                    DrawChart((Chart)(e.Argument as object[])[1]);
                    break;
                case 1:
                    bgdWorkerDraw.ReportProgress(99, "Set framework");
                    if (SetFrameWork(100))
                    {
                        bgdWorkerDraw.ReportProgress(99, "Draw chart");
                        DrawChart((Chart)(e.Argument as object[])[1]);
                    }
                    break;
                case 2:
                    bgdWorkerDraw.ReportProgress(99, "Read data");
                    if (ReadData())
                    {
                        bgdWorkerDraw.ReportProgress(99, "Set framework");
                        if (SetFrameWork(100))
                        {
                            bgdWorkerDraw.ReportProgress(99, "Draw chart");
                            DrawChart((Chart)(e.Argument as object[])[1]);
                        }
                    }
                    break;
            }
        }

        private void bgdWorkerDraw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                lblProcessing.Visible = true;
                //Disable function
                lblTimeDisplay.Text = "";
                munExportImg.Enabled = false;
                munExportAnimation.Enabled = false;
                munToClip.Enabled = false;
                munStatList.Enabled = false;
                munSetting.Enabled = false;
            }
            lblStatus.Text = e.UserState.ToString();
        }

        private void bgdWorkerDraw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Text = "Idle";
            lblProcessing.Visible = false;
            if (!e.Cancelled && e.Error == null)
            {
                StartTime = rawData[0].Date;
                EndTime = rawData[rawData.Count - 1].Date;
                lblStatus.Text = "共 " + dataCount + " 筆資料，自 " + rawData[0].Date.ToString("MM/dd HH:mm:ss") + " 到 " + rawData[rawData.Count - 1].Date.ToString("MM/dd HH:mm:ss");
                munExportImg.Enabled = true;
                munExportAnimation.Enabled = true;
                munToClip.Enabled = true;
                munStatList.Enabled = true;
                munSetting.Enabled = true;
            }
        }

        private void bgdWorkerAnimation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Text = "Idle";
            pgbProcess.Visible = false;
        }

        private void munExportImgToMail_Click(object sender, EventArgs e)
        {
            frmMailer frmMailer = new frmMailer(true);
            frmMailer.StartPosition = FormStartPosition.Manual;
            frmMailer.Location = new Point(Location.X + Width / 2 - frmMailer.ClientSize.Width / 2, Location.Y + Height / 2 - frmMailer.ClientSize.Height / 2);
            try
            {
                //使用者按下確認之後紀錄檔名
                if (frmMailer.ShowDialog() == DialogResult.OK)
                {
                    //存為圖片
                    float zoom = 1;
                    switch (frmMailer.ResolutionType)
                    {
                        case 0:
                            zoom = 1;
                            break;
                        case 1:
                            zoom = 2;
                            break;
                        case 2:
                            zoom = 3;
                            break;
                        case 3:
                            zoom = 5;
                            break;
                        case 4:
                            zoom = 10;
                            break;
                    }
                    int whiteBorder = 0;
                    switch (frmMailer.BorderType)
                    {
                        case 0:
                            whiteBorder = 0;
                            break;
                        case 1:
                            whiteBorder = 2;
                            break;
                        case 2:
                            whiteBorder = 5;
                            break;
                        case 3:
                            whiteBorder = 10;
                            break;
                    }
                    Bitmap image = Trim(new Bitmap(SaveExpandedImg(zoom, frmMailer.Format)), (int)(whiteBorder * zoom));
                    //Convert to byte[] and mail
                    ImageConverter ic = new ImageConverter();
                    byte[] ba = (byte[])ic.ConvertTo(image, typeof(byte[]));
                    MemoryStream ms = new MemoryStream(ba);
                    Attachment attach = new Attachment(ms, "Trend Chart - " + DateTime.Now.ToString("yyyy/MM/dd"));
                    attach.ContentDisposition.FileName = frmMailer.FileName + "." + frmMailer.Format.ToString().ToLower();
                    MailMessage myMail = new MailMessage();
                    myMail.Subject = "Trend Chart - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    myMail.From = new MailAddress("jackylalala9527@gmail.com", "Trend Chart Mailer");
                    myMail.To.Add(frmMailer.MailAddress);
                    myMail.SubjectEncoding = Encoding.UTF8;
                    myMail.IsBodyHtml = true;
                    myMail.BodyEncoding = Encoding.UTF8;
                    myMail.Body = "Trend Chart - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    myMail.Attachments.Add(attach);
                    //發送Mail
                    bgdWorkerMail.RunWorkerAsync(myMail);
                    /*
                    if (Mail(myMail))
                        MessageBox.Show("成功寄出圖片到" + frmMailer.MailAddress, "Alert");
                    else
                        MessageBox.Show("寄出圖片失敗！", "Alert");
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void munExportAnimationToFile_Click(object sender, EventArgs e)
        {
            frmAnimation frmAnimation = new frmAnimation();
            frmAnimation.StartPosition = FormStartPosition.Manual;
            frmAnimation.Location = new Point(Location.X + Width / 2 - frmAnimation.ClientSize.Width / 2, Location.Y + Height / 2 - frmAnimation.ClientSize.Height / 2);
            if (frmAnimation.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialogWithResolution sfd = new SaveFileDialogWithResolution(false);
                try
                {
                    sfd.Title = "匯出圖片";
                    sfd.Filter = @"GIF(*.gif)|*.gif";
                    //使用者按下確認之後紀錄檔名
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        pgbProcess.Visible = true;
                        pgbProcess.Style = ProgressBarStyle.Continuous;
                        pgbProcess.Value = 0;
                        bgdWorkerAnimation.RunWorkerAsync(new object[] { 0, sfd.FileName, "", Percentage, Duration });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void munExportAnimationToMail_Click(object sender, EventArgs e)
        {
            frmAnimation frmAnimation = new frmAnimation();
            frmAnimation.StartPosition = FormStartPosition.Manual;
            frmAnimation.Location = new Point(Location.X + Width / 2 - frmAnimation.ClientSize.Width / 2, Location.Y + Height / 2 - frmAnimation.ClientSize.Height / 2);
            if (frmAnimation.ShowDialog() == DialogResult.OK)
            {
                frmMailer frmMailer = new frmMailer(false);
                try
                {
                    //使用者按下確認之後紀錄檔名
                    if (frmMailer.ShowDialog() == DialogResult.OK)
                    {
                        pgbProcess.Visible = true;
                        pgbProcess.Style = ProgressBarStyle.Continuous;
                        pgbProcess.Value = 0;
                        bgdWorkerAnimation.RunWorkerAsync(new object[] { 1, frmMailer.FileName, frmMailer.MailAddress, Percentage, Duration });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void bgdWorkerMail_DoWork(object sender, DoWorkEventArgs e)
        {
            bgdWorkerMail.ReportProgress(99, "Sending mail");
            MailMessage myMail = (MailMessage)(e.Argument as object);
            try
            {
                using (SmtpClient mySmtp = new SmtpClient())
                {
                    mySmtp.Port = 587;
                    mySmtp.Credentials = new NetworkCredential("jackylalala9527@gmail.com", "7qvt6t2738");
                    mySmtp.Host = "smtp.gmail.com";
                    mySmtp.EnableSsl = true;
                    mySmtp.Send(myMail);
                }
                MessageBox.Show("成功寄出圖片到" + myMail.To[0].Address, "Alert");
            }
            catch (Exception)
            {
                MessageBox.Show("寄出圖片失敗！", "Alert");
            }
            finally
            {
                bgdWorkerMail.ReportProgress(99, "Idle");
                myMail.Dispose();
            }
        }

        private void bgdWorkerMail_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbProcess.Style = ProgressBarStyle.Marquee;
            pgbProcess.Visible = true;
            lblStatus.Text = e.UserState.ToString();
        }

        private void bgdWorkerMail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pgbProcess.Visible = false;
        }
        #endregion

        #region | Methods |

        /// <summary>
        /// Mail to specified mailbox
        /// </summary>
        /// <param name="myMail">content</param>
        /// <remarks>Reference: "http://www.dotblogs.com.tw/joysdw12/archive/2010/10/28/18656.aspx"</remarks>
        private bool Mail(MailMessage myMail)
        {
            try
            {
                using (SmtpClient mySmtp = new SmtpClient())
                {
                    mySmtp.Port = 587;
                    mySmtp.Credentials = new NetworkCredential("jackylalala9527@gmail.com", "7qvt6t2738");
                    mySmtp.Host = "smtp.gmail.com";
                    mySmtp.EnableSsl = true;
                    mySmtp.Send(myMail);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private double IndexToCursorPosition(int index)
        {
            if (XType == 0)
                return index;
            else
                return rawData[index].Date.ToOADate();
        }

        private int CursorPositionToIndex(double position)
        {
            if (XType == 0)
            {
                int index = (int)position;
                if (index < 0)
                    index = 0;
                if (index >= rawData.Count)
                    index = rawData.Count - 1;
                return index;
            }
            else
            {
                DateTime date = DateTime.FromOADate(position);
                int index = -1;
                //Try to find the closet index
                TimeSpan diff = rawData[rawData.Count - 1].Date - rawData[0].Date;
                Parallel.For(0, rawData.Count, i =>
                {
                    TimeSpan temp = date - rawData[i].Date;
                    temp = temp.Duration();
                    if (diff.CompareTo(temp) >= 0)
                    {
                        diff = temp;
                        index = i;
                    }
                });
                chtMain.ChartAreas["main"].CursorX.Position = IndexToCursorPosition(index);
                return index;
            }
        }

        private void ShowParameter(int index)
        {
            if (index < 0 || index >= rawData.Count)
            {
                lblTimeDisplay.Text = "";
                for (int i = 0; i < trendSeries.Length; i++)
                    lblDataDisplay[i].Text = "";
            }
            else
            {
                lblTimeDisplay.Text = rawData[index].Date.ToString("MM/dd HH:mm:ss");
                for (int i = 0; i < trendSeries.Length; i++)
                    lblDataDisplay[i].Text = rawData[index].Parameter[i].ToString("0.00");
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "確定";
            buttonCancel.Text = "取消";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        /// <summary>
        /// Create extra y axis
        /// </summary>
        /// <param name="chart">Chart control</param>
        /// <param name="area">Original chart area</param>
        /// <param name="series">Series</param>
        /// <param name="axisIndex">Y axis index in selected side(0 for native axis)</param>
        /// <param name="side">Left side:0, right side: 1</param>
        /// <param name="minimum">Minimum of y axis</param>
        /// <param name="maximum">Maximum of y axis</param>
        /// <param name="interval">Interval of y axis</param>
        public void CreateYAxis(Chart chart, ChartArea area, Series[] series, int axisIndex, int side, float minimum, float maximum, float interval)
        {
            //Axis offset
            float[] leftSideOffset = new float[] { 5F, 10F };
            float[] rightSideOffset = new float[] { 40F, 60F, 80F };
            // Create new chart area for original series
            ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series[0].Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.LineWidth = 0;
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY2.MajorGrid.Enabled = false;
            areaSeries.AxisY2.MajorTickMark.Enabled = false;
            areaSeries.AxisY2.LabelStyle.Enabled = false;
            areaSeries.AxisY.Minimum = minimum;
            areaSeries.AxisY.Maximum = maximum;
            areaSeries.AxisY.Interval = interval;
            areaSeries.AxisY2.Minimum = minimum;
            areaSeries.AxisY2.Maximum = maximum;
            areaSeries.AxisY2.Interval = interval;
            foreach (Series item in series)
                item.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series[0].ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series[0].ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series[0].ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series[0].Name + "_Copy");
            seriesCopy.ChartType = series[0].ChartType;
            foreach (DataPoint point in series[0].Points)
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;
            seriesCopy.YAxisType = (side == 0 ? AxisType.Primary : AxisType.Secondary);

            // Disable drid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            if (side == 0) //Left side
            {
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.Minimum = minimum;
                areaAxis.AxisY.Maximum = maximum;
                areaAxis.AxisY.Interval = interval;
            }
            else //Right side
            {
                areaAxis.AxisY.LineWidth = 0;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.MajorTickMark.Enabled = false;
                areaAxis.AxisY.LabelStyle.Enabled = false;
                areaAxis.AxisY2.MajorGrid.Enabled = false;
                areaAxis.AxisY2.Minimum = minimum;
                areaAxis.AxisY2.Maximum = maximum;
                areaAxis.AxisY2.Interval = interval;
            }

            // Adjust area position
            areaAxis.Position.X += (side==0?-leftSideOffset[axisIndex-1]:rightSideOffset[axisIndex-1]);

        }

        public void CreateYAxis(Chart chart, ChartArea area, Series[] series, int axisIndex, int side)
        {
            //Axis offset
            float[] leftSideOffset = new float[] { 5F, 10F };
            float[] rightSideOffset = new float[] { 40F, 60F, 80F };
            // Create new chart area for original series
            ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series[0].Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.LineWidth = 0;
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY2.MajorGrid.Enabled = false;
            areaSeries.AxisY2.MajorTickMark.Enabled = false;
            areaSeries.AxisY2.LabelStyle.Enabled = false;
            foreach (Series item in series)
                item.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series[0].ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series[0].ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series[0].ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series[0].Name + "_Copy");
            seriesCopy.ChartType = series[0].ChartType;
            foreach (DataPoint point in series[0].Points)
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;
            seriesCopy.YAxisType = (side == 0 ? AxisType.Primary : AxisType.Secondary);

            // Disable drid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            if (side == 0) //Left side
                areaAxis.AxisY.MajorGrid.Enabled = false;
            else //Right side
            {
                areaAxis.AxisY.LineWidth = 0;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.MajorTickMark.Enabled = false;
                areaAxis.AxisY.LabelStyle.Enabled = false;
                areaAxis.AxisY2.MajorGrid.Enabled = false;
            }

            // Adjust area position
            areaAxis.Position.X += (side == 0 ? -leftSideOffset[axisIndex - 1] : rightSideOffset[axisIndex - 1]);

        }

        public MemoryStream SaveExpandedImg(float zoom, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                lblProcessing.Visible = true;
                //Create new chart
                Chart chtDraw = new Chart();
                chtDraw.PostPaint += chtMain_PostPaint;
                chtDraw.Width = (int)(chtMain.Width * zoom);
                chtDraw.Height = (int)(chtMain.Height * zoom);
                lblStatus.Text = "Read data";
                Application.DoEvents();
                if (ReadData())
                {
                    lblStatus.Text = "Set framework";
                    Application.DoEvents();
                    if (SetFrameWork(100))
                    {
                        lblStatus.Text = "Draw chart";
                        Application.DoEvents();
                        DrawChart(chtDraw);
                    }
                }
                foreach (Title item in chtDraw.Titles)
                {
                    item.Font = new Font(item.Font.FontFamily, item.Font.Size * zoom, item.Font.Style);
                    item.BorderWidth = (int)(item.BorderWidth * zoom);
                }
                foreach (Legend item in chtDraw.Legends)
                    item.Font = new Font(item.Font.FontFamily, item.Font.Size * zoom, item.Font.Style);
                foreach (ChartArea item in chtDraw.ChartAreas)
                {
                    foreach (Axis axes in item.Axes)
                    {
                        axes.LabelStyle.Font = new Font(axes.LabelStyle.Font.FontFamily, axes.LabelStyle.Font.Size * zoom, axes.LabelStyle.Font.Style);
                        axes.LineWidth = (int)(axes.LineWidth * zoom);
                        axes.MajorGrid.LineWidth = (int)(axes.MajorGrid.LineWidth * zoom);
                        axes.MinorGrid.LineWidth = (int)(axes.MinorGrid.LineWidth * zoom);
                        axes.MajorTickMark.LineWidth = (int)(axes.MajorTickMark.LineWidth * zoom);
                        axes.MinorTickMark.LineWidth = (int)(axes.MinorTickMark.LineWidth * zoom);
                    }
                }
                int lineWidth = trendSeries[0].BorderWidth;
                foreach (Series item in chtDraw.Series)
                    item.BorderWidth = (int)(item.BorderWidth * zoom);
                //Save image
                chtDraw.SaveImage(ms, format);
                //Reset width of series, because they don't create new copy when add them to new chart
                for (int i = 0; i < trendSeries.Length; i++)
                    trendSeries[i].BorderWidth = lineWidth;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.StackTrace + ": " + ex.Message);
            }
            finally
            {
                //Remove render message
                lblStatus.Text = "Idle";
                lblProcessing.Visible = false;
            }
            return ms;
        }

        public static int StrToIntDef(string s, int @default)
        {
            int number;
            if (int.TryParse(s, out number))
                return number;
            return @default;
        }

        public static float StringWidth(string str, Font font)
        {
            string target = str;
            Control control = new Control();
            Graphics g = control.CreateGraphics();
            SizeF sizef = g.MeasureString(target, font);
            g.Dispose(); // Necesary to destroy the graphics object
            return sizef.Width; // gets the width of the size object.
        } 

        /// <summary>
        /// Remove bmp's white border
        /// </summary>
        /// <param name="bitmap">Source bmp</param>
        /// <param name="whiteBorder">Retaining white border</param>
        /// <returns>Trimed bmp</returns>
        public static unsafe Bitmap Trim(Bitmap bitmap, int whiteBorder = 0)
        {
            Bitmap resultBmp = null;
            BitmapData bData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int newx = 0, newy = 0, newHeight = 0, newWidth = 0;
            bool isbreak = false;

            //得到x坐标
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte* color = (byte*)bData.Scan0 + x * 3 + y * bData.Stride;
                    int R = *(color + 2);
                    int G = *(color + 1);
                    int B = *color;
                    if (R != 255 || G != 255 || B != 255)
                    {
                        newx = x;
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                    break;
            }
            isbreak = false;
            //得到y坐标
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* color = (byte*)bData.Scan0 + x * 3 + y * bData.Stride;
                    int R = *(color + 2);
                    int G = *(color + 1);
                    int B = *color;
                    if (R != 255 || G != 255 || B != 255)
                    {
                        newy = y;
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                    break;
            }
            isbreak = false;
            int tmpy = 0;
            //得到height
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* color = (byte*)bData.Scan0 + x * 3 + y * bData.Stride;
                    int R = *(color + 2);
                    int G = *(color + 1);
                    int B = *color;
                    if (R != 255 || G != 255 || B != 255)
                    {
                        tmpy = y;
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                    break;
            }
            isbreak = false;
            newHeight = tmpy - newy + 1;
            int tmpx = 0;
            //得到width
            //得到x坐标
            for (int x = width - 1; x >= 0; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    byte* color = (byte*)bData.Scan0 + x * 3 + y * bData.Stride;
                    int R = *(color + 2);
                    int G = *(color + 1);
                    int B = *color;
                    if (R != 255 || G != 255 || B != 255)
                    {
                        Color newColor = Color.FromArgb(R, G, B);
                        tmpx = x;
                        isbreak = true;
                        break;
                    }
                }
                if (isbreak)
                    break;
            }
            bitmap.UnlockBits(bData);
            newWidth = tmpx - newx + 1;
            Rectangle srcRect = new Rectangle(newx, newy, newWidth, newHeight);
            resultBmp = new Bitmap(newWidth + whiteBorder * 2, newHeight + whiteBorder * 2);
            using (Graphics g = Graphics.FromImage(resultBmp))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, resultBmp.Width, resultBmp.Height));
                g.DrawImage(bitmap, new Rectangle(whiteBorder, whiteBorder, newWidth, newHeight), srcRect, GraphicsUnit.Pixel);
            }
            return resultBmp;
        }

        protected bool GetFilename(out string filename, DragEventArgs e)
        {
            filename = String.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileNameW") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = System.IO.Path.GetExtension(filename).ToLower();
                        if (ext.Equals(".csv")) //Only allow *.csv
                            return true;
                    }
                }
            }
            return false;
        }

        private bool ReadData()
        {
            try
            {
                if (!File.Exists(rawFileName))
                    throw new Exception("來源數據檔不存在！");
                rawData.Clear();
                //Read all lines from file
                List<string> allLines = new List<string>();
                FileStream fs = new FileStream(rawFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while (!sr.EndOfStream)
                    allLines.Add(sr.ReadLine());
                //Check the type of data
                string[] secondRow = allLines[1].Split(',', '\t');
                //Determine schema type
                if (type == -1)
                {
                    if (secondRow.Length == 9 && !secondRow[1].Equals(""))
                        type = 0; //3L(#2)
                    else if (secondRow.Length == 11)
                        type = 1; //1L & 5L國華(#4 #5)
                    else if (secondRow.Length == 8 && !secondRow[1].Equals(""))
                        type = 2; //2L (#201)(#1 #3)
                    else if (secondRow.Length == 9 && secondRow[1].Equals(""))
                        type = 6; //R2-EOD (Default for type 3-8)
                    else if (secondRow.Length > 20)
                        type = 9; //CHPPO Pilot
                    else
                        throw new Exception("資料格式不符");
                }
                //Data end flag, only for string X axis (because DateTime format will automatically sort)
                //Parse data string, remember skip first row(no data)
                int firstDataRow = (type >= 3 && type <= 8) ? 3 : 1; //The 1st data row for type 3~8 is 3rd row
                Parallel.For(firstDataRow, allLines.Count, i =>
                {
                    string[] data = allLines[i].Split(',', '\t');
                    if (data.Length > 7)
                    {
                        //Convert to number
                        List<float> dataNum = new List<float>();
                        DateTime date = new DateTime();
                        //First column is date, following column(s) are parameter
                        string dateString;
                        if (type >= 2 && type <= 9)
                            dateString = data[0] + " " + data[1];
                        else
                            dateString = data[0];
                        dateString = dateString.Replace("上午", "AM");
                        dateString = dateString.Replace("下午", "PM");
                        //Try to parse date
                        if (DateTime.TryParseExact(dateString, dateTimeList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date))
                        {
                            //Determine limit range of datetime
                            if (!LimitedTimePeriod ||
                                (LimitedTimePeriod && date.CompareTo(StartTime) >= 0 && date.CompareTo(EndTime) <= 0))
                            {
                                //Process numeric data
                                for (int j = 1; j < data.Length; j++)
                                {
                                    if ((type >= 2 && type <= 9) && j == 1)
                                        continue; //When type = 2 & 3, 2nd column is time
                                    float num = 0;
                                    if (float.TryParse(data[j], out num))
                                    {
                                        //Some column need to process with a factor
                                        if (type == 0)
                                        {
                                            if (j == 5 || j == 6)
                                                num /= 10;
                                            else if (j == 8)
                                                num *= 10;
                                        }
                                        if (chkThreshold.Checked)
                                        {
                                            if (num < minLimit || num > maxLimit)
                                                dataNum.Add(0);
                                            else
                                                dataNum.Add(num);
                                        }
                                        else
                                            dataNum.Add(num);
                                    }
                                    else //Not valid number
                                        dataNum.Add(0);
                                }
                                lock (syncHandle)
                                {
                                    rawData.Add(new RecordData(date, dataNum));
                                }
                            }
                        }
                    }
                });
                if (rawData.Count == 0)
                    throw new Exception("資料讀取錯誤，請檢查格式");
                //Sort
                rawData.Sort();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("資料格式不符"))
                {
                    if (MessageBox.Show(ex.Message + "，是否手動選擇資料格式？", "資料格式錯誤", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        munAnalysis.Enabled = true;
                        LimitedTimePeriod = false;
                        //Type select
                        frmTypeSelector frmTypeSelector = new frmTypeSelector();
                        frmTypeSelector.StartPosition = FormStartPosition.Manual;
                        frmTypeSelector.Location = new Point(Location.X + Width / 2 - frmTypeSelector.ClientSize.Width / 2, this.Location.Y + this.Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                        if (frmTypeSelector.ShowDialog() == DialogResult.OK)
                        {
                            type = frmTypeSelector.Type;
                            //Draw chart
                            return ReadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.StackTrace + ": " + ex.Message);
                }
            }
            return false;
        }

        private bool SetFrameWork(int percentage)
        {
            if (InvokeRequired)
                //Must be sync. invoke (BeginInvoke is unsync.)
                return (bool)Invoke(new SetFrameWorkDelegate(SetFrameWork), percentage);
            else
            {
                try
                {
                    string[] paraTitle = new string[] { };
                    ToolTip tooltip = new ToolTip();
                    tooltip.ToolTipTitle = "參數說明";
                    tooltip.UseAnimation = true;
                    tooltip.UseFading = true;
                    tooltip.IsBalloon = true;
                    bool showTooltip = false;
                    string[] tooltipString = new string[] { };
                    //Set series parameter
                    switch (type)
                    {
                        case 0:
                            trendSeries = new Series[8];
                            paraTitle = new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "流速PV", "壓力PV", "程控SV", "轉速PV", };
                            chkChartItem = new CheckBox[8];
                            lblDataDisplay = new Label[8];
                            break;
                        case 1:
                            trendSeries = new Series[10];
                            paraTitle = new string[] { "釜溫PV", "油溫PV", "油出口PV", "油入口PV", "內溫SV", "油上限SV", "壓力PV", "轉速PV", "扭力PV", "流速PV", };
                            chkChartItem = new CheckBox[10];
                            lblDataDisplay = new Label[10];
                            break;
                        case 2:
                            trendSeries = new Series[6];
                            paraTitle = new string[] { "內溫PV", "外溫PV", "壓力PV", "轉速PV", "外溫SV", "流速PV", };
                            chkChartItem = new CheckBox[6];
                            lblDataDisplay = new Label[6];
                            break;
                        case 3:
                            trendSeries = new Series[5];
                            paraTitle = new string[] { "轉速", "壓力", "內溫", "升溫", "外溫" };
                            chkChartItem = new CheckBox[5];
                            lblDataDisplay = new Label[5];
                            break;
                        case 4:
                            trendSeries = new Series[5];
                            paraTitle = new string[] { "轉速", "壓力", "內溫", "流速", "外溫" };
                            chkChartItem = new CheckBox[5];
                            lblDataDisplay = new Label[5];
                            break;
                        case 5:
                            trendSeries = new Series[6];
                            paraTitle = new string[] { "轉速", "壓力", "內溫1", "外溫", "升溫", "內溫2" };
                            chkChartItem = new CheckBox[6];
                            lblDataDisplay = new Label[6];
                            break;
                        case 6:
                            trendSeries = new Series[6];
                            paraTitle = new string[] { "轉速", "壓力", "內溫1", "外溫", "流速", "內溫2" };
                            chkChartItem = new CheckBox[6];
                            lblDataDisplay = new Label[6];
                            break;
                        case 7:
                            trendSeries = new Series[6];
                            paraTitle = new string[] { "轉速", "壓力", "內溫1", "外溫", "升溫", "內溫2" };
                            chkChartItem = new CheckBox[6];
                            lblDataDisplay = new Label[6];
                            break;
                        case 8:
                            trendSeries = new Series[6];
                            paraTitle = new string[] { "轉速", "壓力", "內溫1", "外溫", "流速", "內溫2" };
                            chkChartItem = new CheckBox[6];
                            lblDataDisplay = new Label[6];
                            break;
                        case 9:
                            showTooltip = true;
                            trendSeries = new Series[23]; //WHSV尚未建立，建立後則為24項目
                            paraTitle = new string[] { "TI-101", "TI-102", "TI-103", "TI-104", "TI-105", "TI-106", "TI-107", "TI-108", "TIC-101", "TI-301", "PI-201", "PI-101", "PI-102", "PI-103", "PI-301", "PI-104", "LI-301", "FIC-201", "PIC-101", "FI-101", "TIC-102", "FI-101 TR", "DPI-101" };
                            tooltipString = new string[] { "丙烯儲槽溫度", "反應器溫度(底)", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度(頂)", "反應器出口冷卻溫度", "反應器CHP25%入口加熱帶溫度", "出料加熱帶溫度", "PI-201", "丙烯儲槽壓力", "丙烯pump出口壓力", "丙烯pump出口至反應器管線壓力", "CHP25% 儲槽壓力", "氣液分離槽壓力", "CHP25%儲槽液位", "氮氣質量流量計", "背壓閥壓力", "氣體質量流量計", "反應器入口丙烯加熱帶", "體質量流量計總量", "反應器壓力差" };
                            chkChartItem = new CheckBox[23];
                            lblDataDisplay = new Label[23];
                            break;
                        case 10:
                            trendSeries = new Series[8];
                            paraTitle = new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "流速PV", "壓力PV", "程控SV", "轉速PV", };
                            chkChartItem = new CheckBox[8];
                            lblDataDisplay = new Label[8];
                            break;
                        default:
                            throw new Exception("資料格式不符");
                    }
                    //Set chart item selection box and corresponding data display box
                    int boxWidth = 80;
                    int padding = Math.Max((pnlChartItems.Width - boxWidth * chkChartItem.Length) / (chkChartItem.Length + 1), 5);
                    pnlChartItems.Controls.Clear();
                    if (chkChartItem.Length > 10)
                        pnlChartItems.HorizontalScroll.Visible = true;
                    int panelExactHeight = pnlChartItems.Height - (chkChartItem.Length > 10 ? SystemInformation.HorizontalScrollBarHeight : 0);
                    for (int i = 0; i < chkChartItem.Length; i++)
                    {
                        chkChartItem[i] = new CheckBox();
                        chkChartItem[i].Text = paraTitle[i];
                        chkChartItem[i].Width = boxWidth;
                        chkChartItem[i].Top = ((panelExactHeight - (chkChartItem[i].Height * 2 + 5)) / 2);
                        chkChartItem[i].Left = padding + i * (boxWidth + padding);
                        chkChartItem[i].Font = new Font("微軟正黑體", Math.Min(StringWidth("內溫PV", chkChartItem[i].Font) / StringWidth(paraTitle[i], chkChartItem[i].Font) * 10F, 10F));
                        chkChartItem[i].BackColor = seriesColor[i];
                        chkChartItem[i].ForeColor = seriesSelectionBoxTextColor[i];
                        if (showTooltip)
                            tooltip.SetToolTip(chkChartItem[i], tooltipString[i]);
                        chkChartItem[i].Tag = i;
                        //Disable checking of check state, avoid dead loop
                        chkChartItem[i].CheckedChanged -= ChangeChartItems;
                        if (!hiddenList.Contains(i))
                            chkChartItem[i].Checked = true;
                        chkChartItem[i].CheckedChanged += ChangeChartItems;
                        pnlChartItems.Controls.Add(chkChartItem[i]);
                        lblDataDisplay[i] = new Label();
                        lblDataDisplay[i].BorderStyle = BorderStyle.Fixed3D;
                        lblDataDisplay[i].AutoSize = false;
                        lblDataDisplay[i].TextAlign = ContentAlignment.MiddleCenter;
                        lblDataDisplay[i].BackColor = Color.White;
                        lblDataDisplay[i].Width = boxWidth;
                        lblDataDisplay[i].Top = chkChartItem[i].Top + chkChartItem[i].Height + 5;
                        lblDataDisplay[i].Left = padding + i * (boxWidth + padding);
                        lblDataDisplay[i].Font = new Font("微軟正黑體", 10F);
                        pnlChartItems.Controls.Add(lblDataDisplay[i]);
                    }
                    //Set series
                    Parallel.For(0, trendSeries.Length, i =>
                    {
                        trendSeries[i] = new Series();
                        trendSeries[i].Name = paraTitle[i];
                        trendSeries[i].Color = seriesColor[i];
                        trendSeries[i].ChartType = SeriesChartType.FastLine;
                        if (XType == 0)
                            trendSeries[i].XValueType = ChartValueType.String;
                        else
                            trendSeries[i].XValueType = ChartValueType.DateTime;
                        trendSeries[i].YValueType = ChartValueType.Double;
                        trendSeries[i].BorderWidth = 2;
                    });
                    //Fill in data
                    dataCount = 0;
                    Parallel.For(0, rawData.Count * percentage / 100, i =>
                    {
                        if (rawData[i] != null)
                        {
                            lock (syncHandle)
                            {
                                for (int j = 0; j < trendSeries.Length; j++)
                                    trendSeries[j].Points.AddXY(rawData[i].Date, rawData[i].Parameter[j]);
                            }
                            dataCount++;
                        }
                    });
                    //Sort and give name (if need)
                    Parallel.For(0, trendSeries.Length, i =>
                    {
                        trendSeries[i].Sort(PointSortOrder.Ascending, "X");
                        //Change date to string (must after sorting, beacuse string CAN NOT be sort)
                        if (XType == 0)
                        {
                            Parallel.For(0, trendSeries[i].Points.Count, j =>
                            {
                                trendSeries[i].Points[j].AxisLabel = DateTime.FromOADate(trendSeries[i].Points[j].XValue).ToString("MM/dd HH:mm:ss");
                                trendSeries[i].Points[j].XValue = 0;
                            });
                        }
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.StackTrace + ": " + ex.Message);
                }
                return false;
            }
        }

        private bool DrawChart(Chart targetChart)
        {
            if (InvokeRequired)
                return (bool)Invoke(new DrawChartDelegate(DrawChart), targetChart);
            else
            {
                cursorIndex = 0;
                try
                {
                    //Clear series
                    targetChart.Series.Clear();
                    //Clear chart area
                    targetChart.ChartAreas.Clear();
                    targetChart.Legends.Clear();
                    targetChart.Titles.Clear();
                    //Create title
                    Title chartTitle = new Title(TitleText);
                    chartTitle.Name = "Main";
                    chartTitle.Font = new Font("微軟正黑體", 18F);
                    targetChart.Titles.Add(chartTitle);
                    //Set chart area
                    targetChart.ChartAreas.Add("main");
                    targetChart.ChartAreas["main"].Position = new ElementPosition(10, 11, 95, 89);
                    targetChart.ChartAreas["main"].InnerPlotPosition = new ElementPosition(7.5F, 10, 70, 70);
                    targetChart.ChartAreas["main"].AxisX.LabelStyle.IsEndLabelVisible = true;
                    targetChart.ChartAreas["main"].AxisX.LabelStyle.Angle = XAngle;
                    targetChart.ChartAreas["main"].AxisX.MajorGrid.LineColor = Color.Gray;
                    targetChart.ChartAreas["main"].AxisY.MajorGrid.LineColor = Color.Gray;
                    targetChart.ChartAreas["main"].AxisX.MinorTickMark.Enabled = true;
                    targetChart.ChartAreas["main"].AxisX.MajorTickMark.Size = 2;
                    targetChart.ChartAreas["main"].AxisX.LabelStyle.Enabled = true;
                    targetChart.ChartAreas["main"].AxisX.Interval = XInterval;
                    targetChart.ChartAreas["main"].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Number;
                    targetChart.ChartAreas["main"].AxisX.LabelStyle.Interval = XInterval;
                    if (chkXGrid.Checked)
                        targetChart.ChartAreas["main"].AxisX.MajorGrid.Enabled = true;
                    else
                        targetChart.ChartAreas["main"].AxisX.MajorGrid.Enabled = false;
                    if (chkYGrid.Checked)
                    {
                        targetChart.ChartAreas["main"].AxisY.MajorGrid.Enabled = true;
                        targetChart.ChartAreas["main"].AxisY2.MajorGrid.Enabled = true;
                    }
                    else
                    {
                        targetChart.ChartAreas["main"].AxisY.MajorGrid.Enabled = false;
                        targetChart.ChartAreas["main"].AxisY2.MajorGrid.Enabled = false;
                    }
                    targetChart.ChartAreas["main"].CursorX.SelectionColor = Color.FromArgb(244, 157, 157);
                    if (XType == 1)
                    {
                        targetChart.ChartAreas["main"].AxisX.LabelStyle.Format = "MM/dd HH:mm:ss";
                        if (XInterval > 0)
                        {
                            targetChart.ChartAreas["main"].AxisX.LabelStyle.Interval = XInterval;
                            targetChart.ChartAreas["main"].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
                            targetChart.ChartAreas["main"].AxisX.MinorTickMark.Interval = XInterval;
                            targetChart.ChartAreas["main"].AxisX.MinorTickMark.IntervalType = DateTimeIntervalType.Minutes;
                        }
                        else
                            targetChart.ChartAreas["main"].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Auto;
                    }
                    targetChart.Legends.Add("");
                    targetChart.Legends[0].IsTextAutoFit = true;
                    targetChart.Legends[0].Position = new ElementPosition(20F, 8.75F, 50F, 11F);
                    if ((type == 0 && hiddenList.Count == 8) || (type == 1 && hiddenList.Count == 10)) //All hide
                        throw new Exception("未選取任何參數");
                    //Add series
                    for (int i = 0; i < trendSeries.Length; i++)
                    {
                        if (!hiddenList.Contains(i))
                            targetChart.Series.Add(trendSeries[i]);
                    }
                    //Set y axis scale (create new if needed)
                    int yAxesCount = 0; //Total y axes (Y axes order:420135)
                    for (int i = 0; i < seriesMap.Length; i++)
                    {
                        //Define series for this axis
                        List<Series> seriesList = new List<Series>();
                        for (int k = 0; k < seriesMap[i][type].Length; k++)
                        {
                            if (!hiddenList.Contains(seriesMap[i][type][k]))
                                seriesList.Add(trendSeries[seriesMap[i][type][k]]);
                        }
                        if (seriesList.Count == 0)
                            continue;
                        //Set Y axis
                        switch (yAxesCount)
                        {
                            case 0:
                                for (int k = 0; k < seriesList.Count; k++)
                                {
                                    seriesList[k].ChartArea = "main";
                                    seriesList[k].YAxisType = AxisType.Primary;
                                }
                                targetChart.ChartAreas["main"].AxisY.Minimum = yProp[type][i].Min;
                                targetChart.ChartAreas["main"].AxisY.Maximum = yProp[type][i].Max;
                                targetChart.ChartAreas["main"].AxisY.Interval = yProp[type][i].Interval;
                                break;
                            case 1:
                                for (int k = 0; k < seriesList.Count; k++)
                                {
                                    seriesList[k].ChartArea = "main";
                                    seriesList[k].YAxisType = AxisType.Secondary;
                                }
                                targetChart.ChartAreas["main"].AxisY2.Enabled = AxisEnabled.True;
                                targetChart.ChartAreas["main"].AxisY2.Minimum = yProp[type][i].Min;
                                targetChart.ChartAreas["main"].AxisY2.Maximum = yProp[type][i].Max;
                                targetChart.ChartAreas["main"].AxisY2.Interval = yProp[type][i].Interval;
                                break;
                            case 2:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 1, 0, yProp[type][i].Min, yProp[type][i].Max, yProp[type][i].Interval);
                                break;
                            case 3:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 1, 1, yProp[type][i].Min, yProp[type][i].Max, yProp[type][i].Interval);
                                break;
                            case 4:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 2, 0, yProp[type][i].Min, yProp[type][i].Max, yProp[type][i].Interval);
                                break;
                            case 5:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 2, 1, yProp[type][i].Min, yProp[type][i].Max, yProp[type][i].Interval);
                                break;
                        }
                        //Set Y label
                        List<Title> yLabel = new List<Title>();
                        Title labelTemp;
                        labelTemp = new Title(UNIT_TABLE[yProp[type][i].Unit]);
                        float labelWidth = Math.Max(3F, 3F * StringWidth(UNIT_TABLE[yProp[type][i].Unit], labelTemp.Font) / StringWidth("bar", labelTemp.Font));
                        labelTemp.Position = new ElementPosition(yLabelXPos[yAxesCount] + (3 - labelWidth) / 2, yLabelYPos[yLabel.Count], labelWidth, 2.5F);
                        yLabel.Add(labelTemp);
                        for (int j = 0; j < seriesList.Count; j++)
                        {
                            labelTemp = new Title("");
                            labelTemp.BackColor = seriesList[j].Color;
                            if (seriesList.Count <= 4) //Max label are 4
                                labelTemp.Position = new ElementPosition(yLabelXPos[yAxesCount], yLabelYPos[yLabel.Count], 3, 2);
                            else
                            {
                                int colSplit = (int)Math.Ceiling(seriesList.Count / 4F);
                                labelTemp.Position = new ElementPosition(yLabelXPos[yAxesCount] + ((int)(j / 4)) * (3F / colSplit + 0.2F), yLabelYPos[j % 4 + 1], 2.8F / colSplit, 2);
                            }
                            yLabel.Add(labelTemp);
                        }
                        labelTemp = new Title(yProp[type][i].Title);
                        labelWidth = Math.Max(3F, 3F * StringWidth(yProp[type][i].Title, labelTemp.Font) / StringWidth("bar", labelTemp.Font));
                        //Max label are 4
                        labelTemp.Position = new ElementPosition(yLabelXPos[yAxesCount] + (3 - labelWidth) / 2, yLabelYPos[Math.Min(yLabel.Count, 5)], labelWidth, 2.5F);
                        yLabel.Add(labelTemp);
                        foreach (Title item in yLabel)
                            targetChart.Titles.Add(item);
                        yAxesCount++;
                    }
                    //Set borader of label
                    foreach (Title item in targetChart.Titles)
                    {
                        if (!item.Name.Equals("Main"))
                            item.Font = new Font("微軟正黑體", 9, FontStyle.Bold);
                        if (item.Text.Equals("") && !item.Name.Equals("Main"))
                        {
                            item.BorderWidth = 1;
                            item.BorderColor = Color.Black;
                            item.ShadowColor = Color.Gray;
                            item.ShadowOffset = 1;
                        }
                    }
                    //Add reactor name
                    Title reactorName = new Title(REACTOR_LIST[type]);
                    reactorName.ForeColor = Color.DimGray;
                    reactorName.Font = new Font("微軟正黑體", 9, FontStyle.Italic | FontStyle.Bold);
                    reactorName.Position = new ElementPosition(80, 90, 20, 5);
                    targetChart.Titles.Add(reactorName);
                    //Set font sytle
                    foreach (Legend item in targetChart.Legends)
                        item.Font = new Font("微軟正黑體", 10, FontStyle.Bold);
                    foreach (ChartArea item in targetChart.ChartAreas)
                    {
                        foreach (Axis axes in item.Axes)
                            axes.LabelStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
                    }
                    //Result
                    targetChart.Visible = true;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace + ": " + ex.Message);
                    MessageBox.Show(ex.Message);
                }
                return false;
            }
        }

        private string calculate()
        {
            //Integration
            double totalTime = 0;
            List<double> blankTime = new List<double>();
            bool startFlag = false;
            bool pauseFlag = false;
            double totalFlow = 0;
            double totalPressure = 0;
            List<double> totalPressureDuringBlank = new List<double>();
            double totalInnerPV = 0;
            List<double> totalInnerPVDuringBlank = new List<double>();
            double maxP = 0;
            DateTime maxPTime = new DateTime();
            double maxFlow = 0;
            int lastFlowPoint = 0; //Last point with flow large than 0.1
            for (int i = 0; i < rawData.Count - 1; i++)
            {
                if (rawData[i + 1].Parameter[seriesMap[3][type][0]] >= 0.1)
                {
                    lastFlowPoint = i + 1;
                    startFlag = true;
                    pauseFlag = false;
                    totalFlow +=
                        (rawData[i + 1].Parameter[seriesMap[3][type][0]]) *
                        (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    totalPressure +=
                        (rawData[i + 1].Parameter[seriesMap[2][type][0]]) *
                        (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    totalInnerPV +=
                        (rawData[i + 1].Parameter[seriesMap[0][type][type == 0 ? 1 : 0]]) *
                        (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    totalTime += (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    if (rawData[i].Parameter[seriesMap[2][type][0]] > maxP)
                    {
                        maxP = rawData[i].Parameter[seriesMap[2][type][0]];
                        maxPTime = rawData[i].Date;
                    }
                    if (rawData[i].Parameter[seriesMap[3][type][0]] > maxFlow)
                        maxFlow = rawData[i].Parameter[seriesMap[3][type][0]];
                }
                else if (startFlag)
                {
                    if (!pauseFlag)
                    {
                        blankTime.Add(0);
                        totalInnerPVDuringBlank.Add(0);
                        totalPressureDuringBlank.Add(0);
                    }
                    pauseFlag = true;
                    blankTime[blankTime.Count - 1] += (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    totalPressureDuringBlank[blankTime.Count - 1] +=
                        (rawData[i + 1].Parameter[seriesMap[2][type][0]]) *
                        (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                    totalInnerPVDuringBlank[blankTime.Count - 1] +=
                        (rawData[i + 1].Parameter[seriesMap[0][type][type == 0 ? 1 : 0]]) *
                        (new TimeSpan((rawData[i + 1].Date - rawData[i].Date).Ticks)).TotalMinutes;
                }
            }
            //Final record
            if (rawData[rawData.Count - 1].Parameter[seriesMap[3][type][0]] >= 0.1)
            {
                if (rawData[rawData.Count - 1].Parameter[seriesMap[2][type][0]] > maxP)
                {
                    maxP = rawData[rawData.Count - 1].Parameter[seriesMap[2][type][0]];
                    maxPTime = rawData[rawData.Count - 1].Date;
                }
                if (rawData[rawData.Count - 1].Parameter[seriesMap[3][type][0]] > maxFlow)
                    maxFlow = rawData[rawData.Count - 1].Parameter[seriesMap[3][type][0]];
            }
            double avgFlow = totalFlow / totalTime;
            double avgPressure = totalPressure / totalTime;
            double avgInnerPV = totalInnerPV / totalTime;
            double totalTimeWithBlank = totalTime;
            double totalPressureWithBlank = totalPressure;
            double totalInnPVWithBlank = totalInnerPV;
            for (int i = 0; i < blankTime.Count - 1; i++)
            {
                if (blankTime[i] < 480)
                {
                    totalTimeWithBlank += blankTime[i];
                    totalPressureWithBlank += totalPressureDuringBlank[i];
                    totalInnPVWithBlank += totalInnerPVDuringBlank[i];
                }
            }
            double avgFlowWithBlank = totalFlow / totalTimeWithBlank;
            double avgPressureWithBlank = totalPressureWithBlank / totalTimeWithBlank;
            double avgInnerPVWithBlank = totalInnPVWithBlank / totalTimeWithBlank;
            //Calcd. aging time
            int pointPerPeriod = 0;
            for (int i = 0; i < rawData.Count - 1; i++)
            {
                if ((int)(20 / (rawData[i + 1].Date - rawData[i].Date).TotalMinutes) > 0)
                {
                    pointPerPeriod = (int)(20 / (rawData[i + 1].Date - rawData[i].Date).TotalMinutes);
                    break;
                }
            }
            bool agingDone = false;
            double agingTime = 0;
            DateTime agingStart = new DateTime();
            DateTime agingEnd = new DateTime();
            for (int i = lastFlowPoint + pointPerPeriod; i < rawData.Count; i++)
            {
                if (rawData[i].Parameter[seriesMap[2][type][0]] == rawData[i - pointPerPeriod].Parameter[seriesMap[2][type][0]])
                {
                    agingDone = true;
                    agingStart = rawData[lastFlowPoint].Date;
                    agingEnd = rawData[i - pointPerPeriod].Date;
                    agingTime = (rawData[i - pointPerPeriod].Date - rawData[lastFlowPoint].Date).TotalMinutes;
                    break;
                }
            }
            //Return
            string result = "*****僅計算流速大於0.1 ml/min(含)部分*****" + Environment.NewLine +
                "總時間：" + string.Format("{0:N2}", totalTime) + "分" + Environment.NewLine +
                "總流量：" + string.Format("{0:N2}", totalFlow) + " ml" + Environment.NewLine +
                "最高流速：" + string.Format("{0:N2}", maxFlow) + " ml/min" + Environment.NewLine +
                "平均流速：" + string.Format("{0:N2}", avgFlow) + " ml/min" + Environment.NewLine +
                "最高壓力：" + string.Format("{0:N2}", maxP) + " " + UNIT_TABLE[yProp[type][2].Unit] + " at " + maxPTime.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                "平均壓力：" + string.Format("{0:N2}", avgPressure) + " " + UNIT_TABLE[yProp[type][2].Unit] + Environment.NewLine +
                "平均內溫：" + string.Format("{0:N2}", avgInnerPV) + " \u00B0C" + Environment.NewLine + Environment.NewLine +
                "*****中途暫停時間超過8小時不列入計算*****" + Environment.NewLine +
                "暫停時間：" + string.Format("{0:N2}", totalTimeWithBlank - totalTime) + "分" + Environment.NewLine +
                "總時間(含暫停時間)：" + string.Format("{0:N2}", totalTimeWithBlank) + "分" + Environment.NewLine +
                "平均流速(含暫停時間)：" + string.Format("{0:N2}", avgFlowWithBlank) + " ml/min" + Environment.NewLine +
                "平均壓力(含暫停時間)：" + string.Format("{0:N2}", avgPressureWithBlank) + " " + UNIT_TABLE[yProp[type][2].Unit] + Environment.NewLine +
                "平均內溫(含暫停時間)：" + string.Format("{0:N2}", avgInnerPVWithBlank) + " \u00B0C" + Environment.NewLine + Environment.NewLine +
                "*****熟成時間(20分鐘壓力變化等於0視為熟成完成)*****" + Environment.NewLine +
                "熟成開始：" + (agingDone ? agingStart.ToString("MM/dd HH:mm:ss") : "N/A") + Environment.NewLine +
                "熟成結束：" + (agingDone ? agingEnd.ToString("MM/dd HH:mm:ss") : "N/A") + Environment.NewLine +
                "熟成時間：" + (agingDone ? string.Format("{0:N2}", agingTime) + "分" : "熟成未完成") + Environment.NewLine +
                "*****製程放大相關數據*****" + Environment.NewLine +
                "1噸槽預估平均進料速率：" + string.Format("{0:N2}", avgFlow * 60 * Density * 1000 / ReactorSize) + " kg/hr" + Environment.NewLine +
                "10噸槽預估平均進料速率：" + string.Format("{0:N2}", avgFlow * 60 * Density * 10000 / ReactorSize) + " kg/hr" + Environment.NewLine +
                "預估生產成本：" + string.Format("{0:N2}", (totalTimeWithBlank + agingTime) / 60 * CostPerHour) + " USD";
            return result;
        }

        #endregion
    }
}
