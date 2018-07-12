using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Linq;
using ZXing;
using ZXing.QrCode;
using System.Data;
using JR.Utils.GUI.Forms;
using System.Runtime.InteropServices;

namespace RecorderDrawer
{
    public partial class frmMain : Form
    {
        #region | Class |
        private class RecorderTypeErrorException : Exception
        {
            public RecorderTypeErrorException() : base() { }
            public RecorderTypeErrorException(string message) : base(message) { }
            public RecorderTypeErrorException(string message, Exception innerException) : base(message, innerException) { }
        }
        private class FileTypeErrorException : Exception
        {
            public FileTypeErrorException() : base() { }
            public FileTypeErrorException(string message) : base(message) { }
            public FileTypeErrorException(string message, Exception innerException) : base(message, innerException) { }
        }
        #endregion

        #region | Readonly Field |
        //Reactor list
        public static readonly string[] recorderList = {
            "控制器#1", //0
            "控制器#2", //1
            "控制器#3", //2
            "控制器#4", //3
            "控制器#5", //4
            "控制器#6", //5
            "控制器#7", //6
            "控制器#8", //7
            "R1-CHPPO", //8
            "R1-EOD", //9
            "R3-CHPPO", //10
            "R3-EOD", //11
            "CHPPO Pilot", //12
            "控制器#1#2兩通道特別版", //13
            "自訂資料格式", //14
        };
        //Unit table
        public static readonly string[] unitList = { "\u00b0C", "bar", "psi", "ml/min", "rpm", "Ncm", "%", "kg/cm\u00b2", "mm", "g/hr", "g", "h-1" };
        //Fluid list
        public static readonly KeyValuePair<string, float>[] fluidList = new KeyValuePair<string, float>[] {
            new KeyValuePair<string, float> ( "Ethylene Oxide", 0.88F ),
            new KeyValuePair<string, float> ( "Propylene Oxide", 0.83F ) };
        //Reactor size
        public static readonly float[] reactorSizeList = { 1, 2, 3, 5, 100 };
        //Datetime format
        public static readonly string[] datetimeFormatList = {
                            "yyyy/MM/dd tt hh:mm:ss",
                            "yyyy/MM/dd hh:mm:ss tt",
                            "yyyy/M/d tt hh:mm:ss",
                            "yyyy/M/d hh:mm:ss tt",
                            "yyyy/MM/dd tt hh:mm",
                            "yyyy/MM/dd hh:mm tt",
                            "yyyy/M/d tt hh:mm",
                            "yyyy/M/d hh:mm tt",
                            "yyyy/MM/dd HH:mm:ss",
                            "yyyy/MM/dd H:m:s",
                            "yyyy/M/d HH:mm:ss",
                            "yyyy/M/d H:m:s",
                            "yyyy/MM/dd HH:mm",
                            "yyyy/MM/dd H:m",
                            "yyyy/M/d HH:mm",
                            "yyyy/M/d H:m",
                            "yyyy/MM/dd",
                            "yyyy/M/d",
                            "yy/MM/dd HH:mm:ss",
                            "yy/MM/d HH:mm:ss",
                            "yy/M/d HH:mm:ss",
                            "yy/MM/dd HH:mm",
                            "yy/MM/d HH:mm",
                            "yy/M/d HH:mm",
                            "M/d HH:mm:ss"
                        };
        //X position of y axis label
        private static readonly float[] yLabelXPos = new float[] { 13.3F, 84.3F, 8.3F, 89.3F, 3.3F, 94.3F };
        private static readonly float[] yLabelYPos = new float[] { 15F, 12.5F, 10F, 7.5F, 5F, 2.5F };
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
        #endregion

        #region | Field |
        //Series map (6 axes for each type)
        //Axes: 時間, 內溫, 外溫, 壓力, 流速(升溫), 轉速, 扭力 (Default)
        //new int[7][] { new int[] { }, new int[] { }, new int[] { }, new int[] {  }, new int[] {  }, new int[] {  }, new int[] {  } }
        private static List<int[][]> seriesMap = new List<int[][]>{
            new int[7][] { new int[] { 0 }, new int[] { 1, 5 }, new int[] { 2, 3, 4, 6 }, new int[] { 7 }, new int[] { 10 }, new int[] { 8 }, new int[] { 9 } }, //0
            new int[7][] { new int[] { 0 }, new int[] { 1, 5 }, new int[] { 2, 3, 4, 6 }, new int[] { 7 }, new int[] { 10 }, new int[] { 8 }, new int[] { 9 } }, //1
            new int[7][] { new int[] { 0 }, new int[] { 1, 2, 7 }, new int[] { 3, 4 }, new int[] { 6 }, new int[] { 5, 9 }, new int[] { 8 }, new int[] {  } }, //2
            new int[7][] { new int[] { 0 }, new int[] { 1, 2, 7 }, new int[] { 3, 4 }, new int[] { 6 }, new int[] { 5, 9 }, new int[] { 8 }, new int[] {  } }, //3
            new int[7][] { new int[] { 0, 1 }, new int[] { 2 }, new int[] { 3 }, new int[] { 4 }, new int[] { 5 }, new int[] { 6 }, new int[] {  } }, //4
            new int[7][] { new int[] { 0, 1 }, new int[] { 2 }, new int[] { 3, 6 }, new int[] { 4 }, new int[] { 7 }, new int[] { 5 }, new int[] {  } }, //5
            new int[7][] { new int[] { 0 }, new int[] { 1, 2, 7 }, new int[] { 3, 4 }, new int[] { 6 }, new int[] { 5, 9 }, new int[] { 8 }, new int[] {  } }, //6
            new int[7][] { new int[] { 0, 1 }, new int[] { 2 }, new int[] { 3, 6 }, new int[] { 4 }, new int[] { 7 }, new int[] { 5 }, new int[] {  } }, //7
            new int[7][] { new int[] { 0, 1 }, new int[] { 4 }, new int[] { 6 }, new int[] { 3 }, new int[] { 5 }, new int[] { 2 }, new int[] {  } }, //8
            new int[7][] { new int[] { 0, 1 }, new int[] { 4 }, new int[] { 6 }, new int[] { 3 }, new int[] { 5 }, new int[] { 2 }, new int[] {  } }, //9
            new int[7][] { new int[] { 0, 1 }, new int[] { 4, 7 }, new int[] { 5 }, new int[] { 3 }, new int[] { 6 }, new int[] { 2 }, new int[] {  } }, //10
            new int[7][] { new int[] { 0, 1 }, new int[] { 4, 7 }, new int[] { 5 }, new int[] { 3 }, new int[] { 6 }, new int[] { 2 }, new int[] {  } }, //11
            new int[7][] { new int[] { 0, 1 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 22 }, new int[] { 12, 13, 14, 15, 16, 17, 20, 24 }, new int[] { 18 }, new int[] { 19, 21 }, new int[] { 23 }, new int[] {  } }, //12
            new int[7][] { new int[] { 0 }, new int[] { 1, 5 }, new int[] { 2, 3, 4, 6 }, new int[] { 7 }, new int[] { 10, 11 }, new int[] { 8 }, new int[] { 9 } } //13
            };
        //row number of first data row
        private static List<int> firstDataRow = new List<int> { 1, 1, 1, 1, 3, 1, 1, 1, 3, 3, 3, 3, 1, 1 };
        private static List<string[]> paramTitle = new List<string[]> {
            new string[] { "釜溫PV", "油溫PV", "油出口PV", "油入口PV", "內溫SV", "油上限SV", "壓力PV", "轉速PV", "扭力PV", "流速PV" }, //0
            new string[] { "釜溫PV", "油溫PV", "油出口PV", "油入口PV", "內溫SV", "油上限SV", "壓力PV", "轉速PV", "扭力PV", "流速PV" }, //1
            new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "EO流速", "壓力PV", "程控SV", "轉速PV", "PO流速" }, //2
            new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "EO流速", "壓力PV", "程控SV", "轉速PV", "PO流速" }, //3
            new string[] { "內溫", "外溫", "壓力", "流速", "轉速" }, //4
            new string[] { "內溫PV", "外溫PV", "壓力PV", "轉速PV", "外溫SV", "流速PV" }, //5
            new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "EO流速", "壓力PV", "程控SV", "轉速PV", "PO流速" }, //6
            new string[] { "內溫PV", "外溫PV", "壓力PV", "轉速PV", "外溫SV", "流速PV" }, //7
            new string[] { "轉速", "壓力", "內溫", "升溫", "外溫" }, //8
            new string[] { "轉速", "壓力", "內溫", "流速", "外溫" }, //9
            new string[] { "轉速", "壓力", "內溫1", "外溫", "升溫", "內溫2" }, //10
            new string[] { "轉速", "壓力", "內溫1", "外溫", "流速", "內溫2" }, //11
            //WHSV尚未建立，建立後則為24項目
            new string[] { "TI-101", "TI-102", "TI-103", "TI-104", "TI-105", "TI-106", "TI-107", "TI-108", "TIC-101", "TI-301", "PI-201", "PI-101", "PI-102", "PI-103", "PI-301", "PI-104", "LI-301", "FIC-201", "PIC-101", "FI-101", "TIC-102", "FI-101 TR", "DPI-101" }, //12
            new string[] { "釜溫PV", "油溫PV", "油出口PV", "油入口PV", "內溫SV", "油上限SV", "壓力PV", "轉速PV", "扭力PV", "EO流速", "PO流速" } //13
         };
        //Data information
        private static List<RecordData> rawData = new List<RecordData>();
        private static List<RecordData> refinedData;
        private static bool dataRefined = false;
        private List<int> hiddenList = new List<int>(); //Hidden list of series
        private string rawFileName; //File name
        private int fileType; //0: csv, 1: krf, 2: dat, 3: dmt
        private Series[] trendSeries;
        //Y axis properties
        private static List<AxesProp[]> yProp = new List<AxesProp[]>();
        //Chart item selection box and title
        private CheckBox[] chkChartItem;
        //Chart data display box
        private Label[] lblDataDisplay;
        //Synchronization lock
        private static object syncHandle = new object();
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
        public static int[][] SeriesMap { get { return seriesMap[Type]; } }
        //Recorder type, -1 means auto detect, according to REACTOR_LIST
        public static int Type { get; private set; }
        public static int FirstDataRow { get { return firstDataRow[Type]; } }
        public static List<RecordData> Data { get { if (dataRefined) return refinedData; else return rawData; } }
        //ascending number of columu(s) contains parameter(s), which create from SeriesMap[Type]
        private int[] ParamColNum { get { return SeriesMap.Skip(1).SelectMany(m => m).OrderBy(s => s).ToArray(); } }
        public static string[] ParamTitle { get { return paramTitle[Type]; } }
        public static AxesProp[] YProp { get { return yProp[Type]; } }
        public static int XType { get; private set; } //0: text, 1: DateTime
        public static int XInterval { get; private set; } //X axis interval in minute
        public static int XAngle { get; private set; } //X axis label angle
        public static bool LimitedTimePeriod { get; private set; }
        public static DateTime StartTime { get; private set; }
        public static DateTime EndTime { get; private set; }
        public static string TitleText { get; private set; } = "";
        public static int FluidIndex { get; private set; }
        public static int ReactorSizeIndex { get; private set; } //In Liter
        public static int Percentage { get; private set; }
        public static int Duration { get; private set; }
        #endregion

        #region | Event |
        public frmMain()
        {
            InitializeComponent();
            //Load the settings
            try
            {
                string[] yPropString = Properties.Settings.Default.YProp.Split(',');
                if (yPropString.Length != (recorderList.Length - 1) * 30)
                    throw new Exception(); //must be 15 axes
                for (int i = 0; (i + 4) < yPropString.Length; i += 5)
                {
                    if (i % 30 == 0)
                        yProp.Add(new AxesProp[6]);
                    yProp[i / 30][(i % 30) / 5] = new AxesProp(
                        yPropString[i],
                        int.Parse(yPropString[i + 1]),
                        float.Parse(yPropString[i + 2]),
                        float.Parse(yPropString[i + 3]),
                        float.Parse(yPropString[i + 4])
                        );
                }
            }
            catch (Exception)
            {
                lblInformation.Text = "設定讀取錯誤，使用預設值";
                //Default axes setting
                yProp = new List<AxesProp[]>{
                    new AxesProp[6]{  //#1
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("扭力", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#2
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("扭力", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#3
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#4
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#5
                        new AxesProp("內溫", 0, 0, 200, 20),
                        new AxesProp("外溫", 0, 0, 200, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 1000, 100),
                        new AxesProp("", 5, 0, 100, 10)},
                    new AxesProp[6]{  //#6
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#7
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                    new AxesProp[6]{  //#8
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("", 5, 0, 220, 20)},
                    new AxesProp[6]{  //R1-CHPPO
                        new AxesProp("內溫", 0, 0, 150, 15),
                        new AxesProp("外溫", 0, 0, 150, 15),
                        new AxesProp("壓力", 2, 0, 500, 50),
                        new AxesProp("升溫", 6, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 600, 60),
                        new AxesProp("", 5, 0, 100, 10)},
                    new AxesProp[6]{  //R1-EOD
                        new AxesProp("內溫", 0, 0, 150, 15),
                        new AxesProp("外溫", 0, 0, 200, 20),
                        new AxesProp("壓力", 2, 0, 150, 15),
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 400, 40),
                        new AxesProp("", 5, 0, 100, 10)},
                    new AxesProp[6]{  //R3-CHPPO
                        new AxesProp("內溫", 0, 0, 150, 15),
                        new AxesProp("外溫", 0, 0, 150, 15),
                        new AxesProp("壓力", 2, 0, 500, 50),
                        new AxesProp("升溫", 6, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 600, 60),
                        new AxesProp("", 5, 0, 100, 10)},
                    new AxesProp[6]{  //R3-EOD
                        new AxesProp("內溫", 0, 0, 150, 15),
                        new AxesProp("外溫", 0, 0, 200, 20),
                        new AxesProp("壓力", 2, 0, 150, 15),
                        new AxesProp("流速", 3, 0, 10, 1),
                        new AxesProp("轉速", 4, 0, 400, 40),
                        new AxesProp("", 5, 0, 100, 10)},
                    new AxesProp[6]{  //CHPPO Pilot
                        new AxesProp("溫度", 0, 0, 200, 20),
                        new AxesProp("壓力", 7, 0, 100, 10),
                        new AxesProp("液位", 8, 0, 500, 50),
                        new AxesProp("流量", 9, 0, 250, 25),
                        new AxesProp("總量", 10, 0, 300, 30),
                        new AxesProp("WHSV", 11, 0, 15, 1.5F)},
                    new AxesProp[6]{  //#1 2通道特別版
                        new AxesProp("內溫", 0, 0, 220, 20),
                        new AxesProp("外溫", 0, 0, 220, 20),
                        new AxesProp("壓力", 1, -2, 20, 2),
                        new AxesProp("流速", 3, 0, 22, 2),
                        new AxesProp("轉速", 4, 0, 1100, 100),
                        new AxesProp("扭力", 5, 0, 220, 20)}
                };
            }
            XType = Properties.Settings.Default.XType;
            XInterval = Properties.Settings.Default.XInterval;
            XAngle = Properties.Settings.Default.XAngle;
            FluidIndex = Properties.Settings.Default.FluidIndex;
            ReactorSizeIndex = Properties.Settings.Default.ReactorSizeIndex;
            chkXGrid.Checked = true;
            chkYGrid.Checked = true;
        }

        private void frmRecorderDrawer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save settings
            Properties.Settings.Default.YProp = string.Join(",", yProp.Select(m => string.Join(",", m.Select(n => string.Join(",", n.ToString())))));
            Properties.Settings.Default.XType = XType;
            Properties.Settings.Default.XInterval = XInterval;
            Properties.Settings.Default.XAngle = XAngle;
            Properties.Settings.Default.FluidIndex = FluidIndex;
            Properties.Settings.Default.ReactorSizeIndex = ReactorSizeIndex;
            Properties.Settings.Default.Save();
        }

        private void frmRecorderDrawer_DragDrop(object sender, DragEventArgs e)
        {
            if (validFile)
            {
                munAnalysis.Enabled = true;
                LimitedTimePeriod = false;
                //Type select
                frmSelector frmTypeSelector = new frmSelector(new string[] { "自動選擇" }.Union(recorderList).ToArray(), "控制器編號", true);
                frmTypeSelector.StartPosition = FormStartPosition.Manual;
                frmTypeSelector.Location = new Point(Location.X + Width / 2 - frmTypeSelector.ClientSize.Width / 2, Location.Y + Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                if (frmTypeSelector.ShowDialog(this) == DialogResult.OK)
                {
                    Type = frmTypeSelector.Index - 1;
                    //Draw chart
                    bgdWorkerDraw.RunWorkerAsync(new object[] { 3, chtMain });
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
                    else if (cursorIndex >= Data.Count)
                        cursorIndex = Data.Count;
                    chtMain.ChartAreas["main"].CursorX.Position = IndexToCursorPosition(cursorIndex);
                    ShowParameter(cursorIndex);
                }
                catch (Exception)
                { }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void munAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void munOpen_Click(object sender, EventArgs e)
        {
            CustomOpenFileDialog ofd = new CustomOpenFileDialog();
            ofd.Title = "選擇數據檔";
            ofd.Filter = "所有支援的檔案格式|*.csv;*.krf;*.dat|逗號分隔值的文字檔案(*.csv)|*.csv|KR2000(*.krf)|*.krf|data file(*.dat)|*.dat";
            ofd.SchemaType = -1;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    rawFileName = ofd.FileName;
                    string ext = Path.GetExtension(ofd.FileName).ToLower();
                    switch (ext)
                    {
                        default:
                        case ".csv":
                            fileType = 0;
                            break;
                        case ".krf":
                            fileType = 1;
                            break;
                        case ".dat":
                            fileType = 2;
                            break;
                    }
                    Type = ofd.SchemaType;
                    txtFilePath.Text = "來源檔案︰" + rawFileName;
                    munAnalysis.Enabled = true;
                    LimitedTimePeriod = false;
                    munAnalysis_Click(null, new EventArgs());
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "請選擇數據檔", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            bgdWorkerDraw.RunWorkerAsync(new object[] { 3, chtMain });
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

        private void munDetailedSetting_Click(object sender, EventArgs e)
        {
            frmDetailedSetting frmDetailedSetting = new frmDetailedSetting();
            frmDetailedSetting.StartPosition = FormStartPosition.Manual;
            frmDetailedSetting.Location = new Point(Location.X + Width / 2 - frmDetailedSetting.ClientSize.Width / 2, Location.Y + Height / 2 - frmDetailedSetting.ClientSize.Height / 2);
            if (frmDetailedSetting.ShowDialog(this) == DialogResult.OK)
            {
                //General tab
                TitleText = frmDetailedSetting.Text;
                LimitedTimePeriod = frmDetailedSetting.LimitedTimePeriod;
                if (LimitedTimePeriod)
                {
                    StartTime = frmDetailedSetting.StartTime;
                    EndTime = frmDetailedSetting.EndTime;
                }
                FluidIndex = frmDetailedSetting.FluidIndex;
                ReactorSizeIndex = frmDetailedSetting.ReactorSizeIndex;
                //Axes tab
                yProp[Type] = frmDetailedSetting.YProp;
                XType = frmDetailedSetting.XType;
                XInterval = frmDetailedSetting.XInterval;
                XAngle = frmDetailedSetting.XAngle;
                //Animation tab
                Percentage = frmDetailedSetting.Percentage;
                Duration = frmDetailedSetting.Duration;
                //commit
                bgdWorkerDraw.RunWorkerAsync(new object[] { 2, chtMain });
            }

        }

        private void munStatList_Click(object sender, EventArgs e)
        {
            //Choose target fluid
            frmSelector flowChannelSelector = new frmSelector(SeriesMap[4].Select(m => ParamTitle[ParamColNum.ToList().IndexOf(m)]), "流速通道", false);
            flowChannelSelector.StartPosition = FormStartPosition.Manual;
            flowChannelSelector.Location = new Point(Location.X + Width / 2 - flowChannelSelector.ClientSize.Width / 2, Location.Y + Height / 2 - flowChannelSelector.ClientSize.Height / 2);
            if (flowChannelSelector.ShowDialog(this) == DialogResult.OK)
            {
                //Choose targer temp. channal
                frmSelector tempChannelSelector = new frmSelector(SeriesMap[1].Select(m => ParamTitle[ParamColNum.ToList().IndexOf(m)]), "內溫通道", false);
                tempChannelSelector.StartPosition = FormStartPosition.Manual;
                tempChannelSelector.Location = new Point(Location.X + Width / 2 - tempChannelSelector.ClientSize.Width / 2, Location.Y + Height / 2 - tempChannelSelector.ClientSize.Height / 2);
                if (tempChannelSelector.ShowDialog(this) == DialogResult.OK)
                    FlexibleMessageBox.Show(this, Calculate(flowChannelSelector.Index, tempChannelSelector.Index), "統計數據");
            }
        }

        private void munToClip_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            ms = SaveExpandedImg(5.0F, ImageFormat.Bmp);
            if (ms != null)
            {
                Bitmap bm = Trim(new Bitmap(ms), 25);
                Clipboard.SetImage(bm);
                MessageBox.Show(this, "圖片已複製到剪貼簿(最佳化bitmap)");
            }
            else
                MessageBox.Show(this, "圖片產生失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void munExportImgToFile_Click(object sender, EventArgs e)
        {
            lblProcessingInfo.Visible = true;
            SaveFileDialogWithResolution sfd = new SaveFileDialogWithResolution(true);
            try
            {
                sfd.Title = "匯出圖片";
                sfd.Filter = @"Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg,*.jpeg)|*.jpg;*.jpeg|GIF(*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif;*.tiff";
                sfd.FilterIndex = 2;
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
                            MessageBox.Show(this, "Invalid format.", "ImageHandler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                    //存為圖片
                    int whiteBorder = 0;
                    switch (sfd.BorderType)
                    {
                        case BorderType.none:
                            whiteBorder = 0;
                            break;
                        case BorderType.small:
                            whiteBorder = 10;
                            break;
                        case BorderType.medium:
                            whiteBorder = 25;
                            break;
                        case BorderType.large:
                            whiteBorder = 50;
                            break;
                    }
                    Bitmap image = Trim(new Bitmap(SaveExpandedImg(5.0F, format)), whiteBorder);
                    image.Save(sfd.FileName);
                    MessageBox.Show(this, "匯出圖片成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblProcessingInfo.Visible = false;
            }
        }

        private void munExportImgToMail_Click(object sender, EventArgs e)
        {
            lblProcessingInfo.Visible = true;
            MessageBox.Show(this, "將使用本機Outlook帳戶寄送郵件，若需要權限請點選[允許]", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            frmMailer frmMailer = new frmMailer(true);
            frmMailer.StartPosition = FormStartPosition.Manual;
            frmMailer.Location = new Point(Location.X + Width / 2 - frmMailer.ClientSize.Width / 2, Location.Y + Height / 2 - frmMailer.ClientSize.Height / 2);
            try
            {
                //使用者按下確認之後紀錄檔名
                if (frmMailer.ShowDialog(this) == DialogResult.OK)
                {
                    //存為圖片
                    int whiteBorder = 0;
                    switch (frmMailer.BorderType)
                    {
                        case 0:
                            whiteBorder = 0;
                            break;
                        case 1:
                            whiteBorder = 10;
                            break;
                        case 2:
                            whiteBorder = 25;
                            break;
                        case 3:
                            whiteBorder = 50;
                            break;
                    }
                    Bitmap image = Trim(new Bitmap(SaveExpandedImg(5.0F, frmMailer.Format)), whiteBorder);
                    //create temp file
                    string tempDoc = Path.GetTempPath() + Guid.NewGuid().ToString() + "." + frmMailer.Format.ToString().ToLower();
                    image.Save(tempDoc);
                    // send via outlook
                    Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
                    Microsoft.Office.Interop.Outlook.MailItem mailItem = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                    mailItem.Subject = frmMailer.FileName + " - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    mailItem.To = frmMailer.MailAddress;
                    mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                    mailItem.Body = "Sent at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    mailItem.Attachments.Add(tempDoc);
                    mailItem.Send();
                    Marshal.FinalReleaseComObject(outlook);
                    //try to del temp file
                    try
                    {
                        File.Delete(tempDoc);
                    }
                    catch (Exception) { }
                    MessageBox.Show(this, "成功寄出檔案到" + frmMailer.MailAddress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblProcessingInfo.Visible = false;
            }
        }

        private void munExportAnimationToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialogWithResolution sfd = new SaveFileDialogWithResolution(false);
            sfd.Title = "匯出圖片";
            sfd.Filter = @"GIF(*.gif)|*.gif";
            //使用者按下確認之後紀錄檔名
            if (sfd.ShowDialog() == DialogResult.OK)
                bgdWorkerAnimation.RunWorkerAsync(new object[] { 0, sfd.FileName, "", Percentage, Duration });
        }

        private void munExportAnimationToMail_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "將使用本機Outlook帳戶寄送郵件，若需要權限請點選[允許]", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            frmMailer frmMailer = new frmMailer(false);
            //使用者按下確認之後紀錄檔名
            if (frmMailer.ShowDialog(this) == DialogResult.OK)
                bgdWorkerAnimation.RunWorkerAsync(new object[] { 1, frmMailer.FileName, frmMailer.MailAddress, Percentage, Duration });
        }

        private void munRecorderFig_Click(object sender, EventArgs e)
        {
            frmRecorderFigure frmRecorderFigure = new frmRecorderFigure();
            frmRecorderFigure.StartPosition = FormStartPosition.Manual;
            frmRecorderFigure.Location = new Point(Location.X + Width / 2 - frmRecorderFigure.ClientSize.Width / 2, Location.Y + Height / 2 - frmRecorderFigure.ClientSize.Height / 2);
            frmRecorderFigure.ShowDialog(this);
        }

        private void munRawdata_Click(object sender, EventArgs e)
        {
            try
            {
                frmRawData frmRawData = new frmRawData();

                frmRawData.StartPosition = FormStartPosition.Manual;
                frmRawData.Location = new Point(Location.X + Width / 2 - frmRawData.ClientSize.Width / 2, Location.Y + Height / 2 - frmRawData.ClientSize.Height / 2);
                if (frmRawData.ShowDialog(this) == DialogResult.OK)
                    bgdWorkerDraw.RunWorkerAsync(new object[] { 1, chtMain });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int maxItem;
                switch (Type)
                {
                    case 8:
                    case 9:
                        maxItem = 5;
                        break;
                    case 4:
                    case 5:
                    case 7:
                    case 10:
                    case 11:
                        maxItem = 6;
                        break;
                    default:
                    case 13:
                        maxItem = 11;
                        break;
                    case 2:
                    case 3:
                    case 6:
                        maxItem = 9;
                        break;
                    case 0:
                    case 1:
                        maxItem = 10;
                        break;
                    case 12:
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
            StartTime = Data[indexSelectionStart].Time;
            EndTime = Data[indexSelectionEnd].Time;
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
                    StartTime = Data[indexSelectionStart].Time;
                    EndTime = Data[indexSelectionEnd].Time;
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
            {
                dataRefined = false;
                bgdWorkerDraw.RunWorkerAsync(new object[] { 1, chtMain });
            }
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

        private void chkThreshold_Changed(object sender, EventArgs e)
        {
            if (rawFileName != null)
                munAnalysis_Click(null, new EventArgs());
        }

        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (rawFileName != null)
                munAnalysis_Click(null, new EventArgs());
        }

        private void ChangeChartItems(object sender, EventArgs e)
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
            bgdWorkerAnimation.ReportProgress(0, "Now processing");
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
                    MessageBox.Show("動畫匯出成功！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    bgdWorkerAnimation.ReportProgress(99, "Sending mail");
                    //create temp file
                    string tempDoc = Path.GetTempPath() + Guid.NewGuid().ToString() + ".gif";
                    new Bitmap(ms).Save(tempDoc);
                    // send via outlook
                    Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
                    Microsoft.Office.Interop.Outlook.MailItem mailItem = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                    mailItem.Subject = fileName + " - " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    mailItem.To = mailAddress;
                    mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
                    mailItem.Body = "Sent at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    mailItem.Attachments.Add(tempDoc);
                    mailItem.Send();
                    Marshal.FinalReleaseComObject(outlook);
                    //del temp file
                    File.Delete(tempDoc);
                    MessageBox.Show("成功寄出檔案到" + mailAddress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bgdWorkerAnimation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!lblProcessingInfo.Visible)
                lblProcessingInfo.Visible = true;
            lblProcessingInfo.Text = e.UserState.ToString();
        }

        private void bgdWorkerAnimation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProcessingInfo.Visible = false;
        }

        private void bgdWorkerDraw_DoWork(object sender, DoWorkEventArgs e)
        {
            //Define stage
            //0: draw, 1: setFrameWork & draw, 2: refine, 3: all
            int stage = int.Parse((e.Argument as object[])[0].ToString());
            bgdWorkerDraw.ReportProgress(0, "Now Procssing");
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
                    refinedData = RefineData(Data, StartTime, EndTime);
                    dataRefined = true;
                    bgdWorkerDraw.ReportProgress(99, "Set framework");
                    if (SetFrameWork(100))
                    {
                        bgdWorkerDraw.ReportProgress(99, "Draw chart");
                        DrawChart((Chart)(e.Argument as object[])[1]);
                    }
                    break;
                case 3:
                    bgdWorkerDraw.ReportProgress(99, "Read data");
                    if (ReadData())
                    {
                        dataRefined = false;
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
            if (!lblProcessingInfo.Visible)
                lblProcessingInfo.Visible = true;
            lblProcessingInfo.Text = e.UserState.ToString();
        }

        private void bgdWorkerDraw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProcessingInfo.Visible = false;
            //show summary
            if (!e.Cancelled && e.Error == null && Data.Count > 0)
            {
                StartTime = Data[0].Time;
                EndTime = Data[Data.Count - 1].Time;
                lblInformation.Text = "共 " + Data.Count + " 筆資料，自 " + Data[0].Time.ToString("MM/dd HH:mm:ss") + " 到 " + Data[Data.Count - 1].Time.ToString("MM/dd HH:mm:ss");
            }
        }

        private void lblProcessingInfo_VisibleChanged(object sender, EventArgs e)
        {
            if (lblProcessingInfo.Visible)
            {
                lblProcessingInfo.Text = "Now processing";
                lblInformation.Text = "";
                FunctionSwitch(false);
            }
            else
                FunctionSwitch(true);
        }

        private void lblProcessingInfo_TextChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        #endregion

        #region | Methods |
        private double IndexToCursorPosition(int index)
        {
            if (XType == 0)
                return index;
            else
                return Data[index].Time.ToOADate();
        }

        private void FunctionSwitch(bool state)
        {
            //display render message
            lblProcessingInfo.Visible = !state;
            //clear message
            lblTimeDisplay.Text = "";
            //Disable function
            munExportImg.Enabled = state;
            munExportAnimation.Enabled = state;
            munToClip.Enabled = state;
            munStatList.Enabled = state;
            munSetting.Enabled = state;
            munRawdata.Enabled = state;
            pnlChartItems.Visible = state;
            pnlChartSetting.Visible = state;
        }

        private int CursorPositionToIndex(double position)
        {
            if (XType == 0)
            {
                int index = (int)position;
                if (index < 0)
                    index = 0;
                if (index >= Data.Count)
                    index = Data.Count - 1;
                return index;
            }
            else
            {
                DateTime date = DateTime.FromOADate(position);
                int index = -1;
                //Try to find the closet index
                TimeSpan diff = Data[Data.Count - 1].Time - Data[0].Time;
                Parallel.For(0, Data.Count, i =>
                {
                    TimeSpan temp = date - Data[i].Time;
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
            if (index < 0 || index >= Data.Count)
            {
                lblTimeDisplay.Text = "";
                for (int i = 0; i < trendSeries.Length; i++)
                    lblDataDisplay[i].Text = "";
            }
            else
            {
                lblTimeDisplay.Text = Data[index].Time.ToString("MM/dd HH:mm:ss");
                for (int i = 0; i < trendSeries.Length; i++)
                    lblDataDisplay[i].Text = Data[index].Parameter[i].ToString("0.00");
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
            areaAxis.Position.X += (side == 0 ? -leftSideOffset[axisIndex - 1] : rightSideOffset[axisIndex - 1]);

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
                lblProcessingInfo.Visible = true;
                //Create new chart
                Chart chtDraw = new Chart();
                chtDraw.PostPaint += chtMain_PostPaint;
                chtDraw.Width = (int)(chtMain.Width * zoom);
                chtDraw.Height = (int)(chtMain.Height * zoom);
                DrawChart(chtDraw);
                foreach (Title item in chtDraw.Titles)
                {
                    item.Font = new Font(item.Font.FontFamily, item.Font.Size * zoom, item.Font.Style);
                    item.BorderWidth = (int)(item.BorderWidth * zoom);
                }
                foreach (Legend item in chtDraw.Legends)
                    item.Font = new Font(item.Font.FontFamily, item.Font.Size * zoom, item.Font.Style);
                //Add QR code
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = new QrCodeEncodingOptions()
                {
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H,
                    DisableECI = true,
                    Width = (int)(80 * zoom),
                    Height = (int)(80 * zoom),
                    CharacterSet = "UTF-8",
                };
                string statInfo = "";
                //Default inner temperature channal: 0
                for (int i = 0; i < SeriesMap[3].Length; i++)
                    statInfo += Calculate(i, 0, true) + Environment.NewLine;
                NamedImage qrcode = new NamedImage("qrcode", writer.Write(statInfo));
                chtDraw.Images.Add(qrcode);
                ImageAnnotation qrImage = new ImageAnnotation()
                {
                    X = 2,
                    Y = 83,
                    Image = "qrcode"
                };
                chtDraw.Annotations.Add(qrImage);
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
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Remove render message
                lblProcessingInfo.Visible = false;
            }
            return ms;
        }

        public static int StrToNumWithDefault(string s, int @default)
        {
            int number;
            if (int.TryParse(s, out number))
                return number;
            return @default;
        }

        public static float StrToNumWithDefault(string s, float @default)
        {
            float number;
            if (float.TryParse(s, out number))
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
            filename = string.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = e.Data.GetData("FileNameW") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is string))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        //Only allow *.csv and *.krf
                        if (ext.Equals(".csv"))
                        {
                            fileType = 0;
                            return true;
                        }
                        if (ext.Equals(".krf"))
                        {
                            fileType = 1;
                            return true;
                        }
                        if (ext.Equals(".dat"))
                        {
                            fileType = 2;
                            return true;
                        }
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
                    throw new FileNotFoundException();
                rawData.Clear();
                //Variable for binary file
                byte[] binaryData;
                DateTime start;
                int interval;
                //Reading
                switch (fileType)
                {
                    case 0: //csv file
                        //Read all lines from file
                        List<string> allLines = new List<string>();
                        using (FileStream fs = new FileStream(rawFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                            {
                                while (!sr.EndOfStream)
                                    allLines.Add(sr.ReadLine());
                            }
                        }
                        if (allLines.Count == 0)
                            throw new FileNotFoundException();
                        //custom data type
                        if (Type == recorderList.Length - 1) //custom type always be the last one
                        {
                            frmFormatSelector frmFormatSelector = new frmFormatSelector(allLines.ToArray());
                            if (frmFormatSelector.ShowDialog() == DialogResult.OK)
                            {
                                seriesMap.Add(frmFormatSelector.SeriesMap);
                                firstDataRow.Add(frmFormatSelector.FirstDataRow);
                                paramTitle.Add(frmFormatSelector.ParamTitle.ToArray());
                                yProp.Add(frmFormatSelector.YProp);
                                XType = frmFormatSelector.XType;
                                XInterval = frmFormatSelector.XInterval;
                                XAngle = frmFormatSelector.XAngle;
                            }
                            else
                                return false;
                        }
                        //Check the type of data
                        string[] secondRow = allLines[1].Split(',', '\t');
                        int secondRowLength = Array.FindLastIndex(allLines[1].Split(',', '\t'), m => !m.Equals(string.Empty)) + 1;
                        //Determine schema type
                        if (Type == -1)
                        {
                            if (allLines[0].Split(',', '\t').Length > 0 && allLines[0].Split(',', '\t')[0].Contains("RecorderDrawer"))
                            {
                                //old version compatible
                                //clear custom data
                                if (seriesMap.Count == recorderList.Length)
                                {
                                    seriesMap.RemoveAt(seriesMap.Count - 1);
                                    firstDataRow.RemoveAt(firstDataRow.Count - 1);
                                    yProp.RemoveAt(yProp.Count - 1);
                                    paramTitle.RemoveAt(paramTitle.Count - 1);
                                }
                                Type = int.Parse(allLines[0].Split(',', '\t')[1]);
                                int[][] tempMap = SeriesMap;
                                //modify map, remove datetime column
                                List<List<int>> map = new List<List<int>>();
                                map.Add(new List<int>(new int[] { 0 }));
                                for (int i = 1; i < tempMap.Length; i++)
                                {
                                    map.Add(new List<int>());
                                    for (int j = 0; j < tempMap[i].Length; j++)
                                        map[i].Add(tempMap[i][j] - tempMap[0].Length + 1);
                                }
                                seriesMap.Add(map.Select(m => m.ToArray()).ToArray());
                                firstDataRow.Add(1);
                                yProp.Add(YProp);
                                paramTitle.Add(ParamTitle);
                                Type = recorderList.Length - 1;
                            }
                            else if (secondRowLength == 10)
                                Type = 2; //#3 #4 #7
                            else if (secondRowLength == 11)
                                Type = 0; //#1 #2
                            else if (secondRowLength == 8 && !secondRow[1].Equals(""))
                                Type = 5; //#6 #8
                            else if (secondRowLength == 8 && secondRow[1].Equals(""))
                                Type = 4; //#5 (Default for #5, R1, R3)
                            else if (secondRowLength > 20)
                                Type = 12; //CHPPO Pilot
                            else if (secondRowLength == 12)
                                Type = 13; //#1 #2 兩通道特別版
                            else
                                throw new RecorderTypeErrorException();
                        }
                        //Data end flag, only for string X axis (because DateTime format will automatically sort)
                        //Parse data string, remember skip first row(no data)
                        Parallel.For(FirstDataRow, allLines.Count, i =>
                        {
                            string[] data = allLines[i].Split(',', '\t');
                            //Convert to number
                            List<float> dataNum = new List<float>();
                            DateTime date = new DateTime();
                            //combine DateTime column(SeriesMap[Type][0]) into date string
                            string dateString = string.Join(" ", SeriesMap[0].Select(n => data[n]))
                                                    .Replace("上午", "AM")
                                                    .Replace("下午", "PM")
                                                    .Replace("=", "")
                                                    .Replace("\"", "");
                            //Try to parse date
                            if (DateTime.TryParseExact(dateString, datetimeFormatList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date))
                            {
                                //Determine limit range of datetime
                                if (!LimitedTimePeriod ||
                                        (LimitedTimePeriod && date.CompareTo(StartTime) >= 0 && date.CompareTo(EndTime) <= 0))
                                {
                                    //add parameter
                                    lock (syncHandle)
                                    {
                                        if (chkThreshold.Checked)
                                            rawData.Add(new RecordData(date, ParamColNum.Select(n => data[n]).ToList(), maxLimit, minLimit));
                                        else
                                            rawData.Add(new RecordData(date, ParamColNum.Select(n => data[n]).ToList()));
                                    }
                                }
                            }
                        });
                        break;
                    case 1: //krf file
                        Type = 5; //Default recorder of KR2000
                        using (FileStream fs = File.OpenRead(rawFileName))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                                binaryData = br.ReadBytes((int)fs.Length);
                        }
                        //Check file
                        if (!Encoding.ASCII.GetString(binaryData, 0, 3).Equals("KR2"))
                            throw new FileTypeErrorException();
                        //Read start datetimme and interval
                        start = new DateTime(2000, 1, 1).AddMilliseconds((long)BitConverter.ToInt32(binaryData, 0x20) * 1000);
                        interval = BitConverter.ToInt16(binaryData, 0x28) * 100 + BitConverter.ToInt16(binaryData, 0x2a) * 86400000; //In millsecond
                        List<int> list = new List<int>();
                        for (int i = 0x8bc; i <= binaryData.Length - 24; i += 24)
                            list.Add(i);
                        Parallel.ForEach(list, i =>
                        {
                            if (!LimitedTimePeriod ||
                                            (LimitedTimePeriod && start.AddMilliseconds(interval * (i - 0x8bc) / 24).CompareTo(StartTime) >= 0 && start.AddMilliseconds(interval * (i - 0x8bc) / 24).CompareTo(EndTime) <= 0))
                            {
                                List<float> dataNum = new List<float>();
                                dataNum.Add((float)BitConverter.ToInt16(binaryData, i) / 10);
                                dataNum.Add((float)BitConverter.ToInt16(binaryData, i + 4) / 100);
                                dataNum.Add((float)BitConverter.ToInt16(binaryData, i + 8) / 100);
                                if (Type == 5)
                                    dataNum.Add(BitConverter.ToInt16(binaryData, i + 12));
                                else if (Type == 7)
                                    dataNum.Add((float)BitConverter.ToInt16(binaryData, i + 12) / 10);
                                dataNum.Add((float)BitConverter.ToInt16(binaryData, i + 16) / 10);
                                dataNum.Add((float)BitConverter.ToInt16(binaryData, i + 20) / 100);
                                lock (syncHandle)
                                    rawData.Add(new RecordData(start.AddMilliseconds(interval * (i - 0x8bc) / 24), dataNum));
                            }
                        });
                        break;
                    case 2: //dat file
                        //clear custom data
                        if (seriesMap.Count == recorderList.Length)
                        {
                            seriesMap.RemoveAt(seriesMap.Count - 1);
                            firstDataRow.RemoveAt(firstDataRow.Count - 1);
                            yProp.RemoveAt(yProp.Count - 1);
                            paramTitle.RemoveAt(paramTitle.Count - 1);
                        }
                        using (FileStream fs = new FileStream(rawFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            BinaryReader reader = new BinaryReader(fs, Encoding.UTF8);
                            string version = reader.ReadString();
                            DateTime createDate = new DateTime(reader.ReadInt64());
                            Type = reader.ReadInt32();
                            if (Type == int.MaxValue) //custom type
                            {
                                Type = recorderList.Length - 1; //set to correct type
                                //read series map
                                List<List<int>> map = new List<List<int>>();
                                int axisCount = reader.ReadInt32();
                                for (int i = 0; i < axisCount; i++)
                                {
                                    map.Add(new List<int>());
                                    int channelCount = reader.ReadInt32();
                                    for (int j = 0; j < channelCount; j++)
                                        map[i].Add(reader.ReadInt32());
                                }
                                seriesMap.Add(map.Select(m => m.ToArray()).ToArray());
                                //read first row
                                firstDataRow.Add(reader.ReadInt32());
                                //read y-axis parameter
                                List<AxesProp> param = new List<AxesProp>();
                                axisCount = reader.ReadInt32();
                                for (int i = 0; i < axisCount; i++)
                                    param.Add(AxesProp.Deserialize(fs));
                                yProp.Add(param.ToArray());
                                //read parameter title
                                int paramCount = reader.ReadInt32();
                                List<string> temp = new List<string>();
                                for (int i = 0; i < paramCount; i++)
                                    temp.Add(reader.ReadString());
                                paramTitle.Add(temp.ToArray());
                            }
                            int count = reader.ReadInt32(); //data
                            for (int i = 0; i < count; i++)
                                rawData.Add(RecordData.Deserialize(fs));
                        }
                        break;
                    case 3: //dmt file
                        if (Type == -1)
                            Type = 5; //Default recorder of KR2000
                        using (FileStream fs = File.OpenRead(rawFileName))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                                binaryData = br.ReadBytes((int)fs.Length);
                        }
                        //Read start datetimme and interval
                        start = new DateTime(2000, 1, 1).AddMilliseconds((long)BitConverter.ToInt32(binaryData, 0x20) * 1000);
                        interval = BitConverter.ToInt16(binaryData, 0x28) * 100 + BitConverter.ToInt16(binaryData, 0x2a) * 86400000; //In millsecond
                        for (int i = 0x10fc; i <= binaryData.Length - 104; i += 104)
                        {
                            if (!LimitedTimePeriod ||
                                            (LimitedTimePeriod && start.AddMilliseconds(interval * (i - 0x8bc) / 24).CompareTo(StartTime) >= 0 && start.AddMilliseconds(interval * (i - 0x8bc) / 24).CompareTo(EndTime) <= 0))
                            {
                                List<float> dataNum2 = new List<float>();
                                for (int j = 8; j <= 102; j += 2)
                                    dataNum2.Add((float)BitConverter.ToInt16(new byte[] { binaryData[i + j + 1], binaryData[i + j] }, 0) / 10);
                                rawData.Add(new RecordData(
                                    new DateTime(
                                        Convert.ToInt16(binaryData[i]) + 2000,
                                        Convert.ToInt16(binaryData[i + 2]),
                                        Convert.ToInt16(binaryData[i + 1]),
                                        Convert.ToInt16(binaryData[i + 3]),
                                        Convert.ToInt16(binaryData[i + 4]),
                                        Convert.ToInt16(binaryData[i + 5])),
                                    dataNum2));
                            }
                        }
                        break;
                }
                if (rawData.Count == 0)
                    throw new RecorderTypeErrorException();
                //set process parameter
                switch (Type)
                {
                    case 0:
                    case 13://5L
                        ReactorSizeIndex = 3;
                        break;
                    case 1:
                    case 2:
                    default: //1L
                        ReactorSizeIndex = 0;
                        break;
                    case 4:
                    case 7: //2L
                        ReactorSizeIndex = 1;
                        break;
                }
                //Sort
                rawData.Sort();
                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("資料檔不存在或檔案為空", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileTypeErrorException)
            {
                MessageBox.Show("檔案類型錯誤，請重新選擇正確檔案", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RecorderTypeErrorException)
            {
                if (MessageBox.Show("資料格式錯誤，是否手動選擇資料格式？", "錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    munAnalysis.Enabled = true;
                    LimitedTimePeriod = false;
                    //Type select
                    frmSelector frmTypeSelector = new frmSelector(new string[] { "自動選擇" }.Union(recorderList).ToArray(), "控制器編號", true);
                    frmTypeSelector.StartPosition = FormStartPosition.Manual;
                    frmTypeSelector.Location = new Point(Location.X + Width / 2 - frmTypeSelector.ClientSize.Width / 2, Location.Y + Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                    if (frmTypeSelector.ShowDialog() == DialogResult.OK)
                    {
                        Type = frmTypeSelector.Index - 1;
                        //Draw chart
                        return ReadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + ": " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    //tooltip
                    ToolTip tooltip = new ToolTip();
                    tooltip.ToolTipTitle = "參數說明";
                    tooltip.UseAnimation = true;
                    tooltip.UseFading = true;
                    tooltip.IsBalloon = true;
                    bool showTooltip = false;
                    string[] tooltipString = new string[] { };
                    //Set series parameter
                    int paramCount = SeriesMap.Skip(1).SelectMany(m => m).Count();
                    if (Type==12)
                    {
                        showTooltip = true;
                        tooltipString = new string[] { "丙烯儲槽溫度", "反應器溫度(底)", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度(頂)", "反應器出口冷卻溫度", "反應器CHP25%入口加熱帶溫度", "出料加熱帶溫度", "PI-201", "丙烯儲槽壓力", "丙烯pump出口壓力", "丙烯pump出口至反應器管線壓力", "CHP25% 儲槽壓力", "氣液分離槽壓力", "CHP25%儲槽液位", "氮氣質量流量計", "背壓閥壓力", "氣體質量流量計", "反應器入口丙烯加熱帶", "體質量流量計總量", "反應器壓力差" };
                    }
                    //create object
                    trendSeries = new Series[paramCount];
                    chkChartItem = new CheckBox[paramCount];
                    lblDataDisplay = new Label[paramCount];
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
                        chkChartItem[i].Text = ParamTitle[i];
                        chkChartItem[i].Width = boxWidth;
                        chkChartItem[i].Top = ((panelExactHeight - (chkChartItem[i].Height * 2 + 5)) / 2);
                        chkChartItem[i].Left = padding + i * (boxWidth + padding);
                        chkChartItem[i].Font = new Font("微軟正黑體", Math.Min(StringWidth("內溫PV", chkChartItem[i].Font) / StringWidth(ParamTitle[i], chkChartItem[i].Font) * 10F, 10F));
                        chkChartItem[i].BackColor = seriesColor[i];
                        chkChartItem[i].ForeColor = seriesSelectionBoxTextColor[i];
                        if (showTooltip)
                            tooltip.SetToolTip(chkChartItem[i], tooltipString[i]);
                        chkChartItem[i].Tag = ParamColNum[i]; //corresponding column
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
                        trendSeries[i].Tag = ParamColNum[i]; //corresponding column
                        trendSeries[i].Name = ParamTitle[i];
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
                    for (int i = 0; i < Data.Count * percentage / 100; i++)
                    {
                        for (int j = 0; j < trendSeries.Length; j++)
                        {
                            if (XType == 1)
                                trendSeries[j].Points.AddXY(Data[i].Time, Data[i].Parameter[j]);
                            else
                                trendSeries[j].Points.AddXY(Data[i].Time.ToString("MM/dd HH:mm:ss"), Data[i].Parameter[j]);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace + ": " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    targetChart.Images.Clear();
                    targetChart.Annotations.Clear();
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
                        if (XInterval < 1)
                            XInterval = Data.Count / 10;
                        targetChart.ChartAreas["main"].AxisX.LabelStyle.Interval = XInterval;
                        targetChart.ChartAreas["main"].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
                        targetChart.ChartAreas["main"].AxisX.MinorTickMark.Interval = XInterval;
                        targetChart.ChartAreas["main"].AxisX.MinorTickMark.IntervalType = DateTimeIntervalType.Minutes;
                    }
                    targetChart.Legends.Add("");
                    targetChart.Legends[0].IsTextAutoFit = true;
                    targetChart.Legends[0].Position = new ElementPosition(20F, 8.75F, 50F, 11F);
                    //Add series
                    for (int i = 0; i < trendSeries.Length; i++)
                    {
                        if (!hiddenList.Contains(((int)trendSeries[i].Tag)))
                            targetChart.Series.Add(trendSeries[i]);
                    }
                    //Set y axis scale (create new if needed)
                    int yAxesCount = 0; //Total y axes (Y axes order:420135), first axes is time
                    for (int i = 0; (i + 1) < SeriesMap.Length; i++)
                    {
                        //Define series for this axis
                        List<Series> seriesList = new List<Series>();
                        for (int k = 0; k < SeriesMap[i + 1].Length; k++)
                        {
                            if (!hiddenList.Contains(SeriesMap[i + 1][k]))
                                seriesList.Add(trendSeries.Single(m => (int)m.Tag == SeriesMap[i + 1][k]));
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
                                targetChart.ChartAreas["main"].AxisY.Minimum = yProp[Type][i].Min;
                                targetChart.ChartAreas["main"].AxisY.Maximum = yProp[Type][i].Max;
                                targetChart.ChartAreas["main"].AxisY.Interval = yProp[Type][i].Interval;
                                break;
                            case 1:
                                for (int k = 0; k < seriesList.Count; k++)
                                {
                                    seriesList[k].ChartArea = "main";
                                    seriesList[k].YAxisType = AxisType.Secondary;
                                }
                                targetChart.ChartAreas["main"].AxisY2.Enabled = AxisEnabled.True;
                                targetChart.ChartAreas["main"].AxisY2.Minimum = yProp[Type][i].Min;
                                targetChart.ChartAreas["main"].AxisY2.Maximum = yProp[Type][i].Max;
                                targetChart.ChartAreas["main"].AxisY2.Interval = yProp[Type][i].Interval;
                                break;
                            case 2:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 1, 0, yProp[Type][i].Min, yProp[Type][i].Max, yProp[Type][i].Interval);
                                break;
                            case 3:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 1, 1, yProp[Type][i].Min, yProp[Type][i].Max, yProp[Type][i].Interval);
                                break;
                            case 4:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 2, 0, yProp[Type][i].Min, yProp[Type][i].Max, yProp[Type][i].Interval);
                                break;
                            case 5:
                                CreateYAxis(targetChart, targetChart.ChartAreas[0], seriesList.ToArray(), 2, 1, yProp[Type][i].Min, yProp[Type][i].Max, yProp[Type][i].Interval);
                                break;
                        }
                        //Set Y label
                        List<Title> yLabel = new List<Title>();
                        Title labelTemp;
                        labelTemp = new Title(unitList[yProp[Type][i].UnitIndex]);
                        float labelWidth = Math.Max(3F, 3F * StringWidth(unitList[yProp[Type][i].UnitIndex], labelTemp.Font) / StringWidth("bar", labelTemp.Font));
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
                        labelTemp = new Title(yProp[Type][i].Title);
                        labelWidth = Math.Max(3F, 3F * StringWidth(yProp[Type][i].Title, labelTemp.Font) / StringWidth("bar", labelTemp.Font));
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
                    Title reactorName = new Title(recorderList[Type]);
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
                    MessageBox.Show(ex.StackTrace + ": " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }

        private string Calculate(int flowChannel = 0, int innerTempChannel = 0, bool simple = false)
        {
            if (Type == 12)
                return "此類型不適用此功能";
            else if (Data == null)
                return "沒有數據，完全沒有數據";
            else if (SeriesMap[4].Length == 0)
                return "無可用流速資訊";
            else if (SeriesMap[1].Length == 0)
                return "無可用內溫資訊";
            else if (SeriesMap[3].Length == 0)
                return "無可用壓力資訊";
            //define target parameter
            int paramFlowIndex = ParamColNum.ToList().IndexOf(SeriesMap[4][flowChannel]);
            int paramPressureIndex = ParamColNum.ToList().IndexOf(SeriesMap[3][0]);
            int paramInnerTempIndex = ParamColNum.ToList().IndexOf(SeriesMap[1][innerTempChannel]);
            //Integration
            float totalTime = 0;
            List<float> blankTime = new List<float>();
            bool pauseFlag = false;
            float totalFlow = 0;
            float totalPressure = 0;
            List<float> totalPressureIncludingBlank = new List<float>();
            float totalInnerTemp = 0;
            List<float> totalInnerTempIncludngBlank = new List<float>();
            float maxP = 0;
            DateTime maxPTime = new DateTime();
            float maxInnerTemp = 0;
            DateTime maxInnerTempTime = new DateTime();
            float maxFlow = 0;
            int firstFlowPoint = int.MaxValue; //First point with flow large than 0.01
            int lastFlowPoint = int.MinValue; //Last point with flow large than 0.01
            //Scan for first/last flow point
            Parallel.For(1, Data.Count, i =>
            {
                if (Data[i].Parameter[paramFlowIndex] >= 0.01)
                {
                    if (i < firstFlowPoint)
                        firstFlowPoint = i;
                    if (i > lastFlowPoint)
                        lastFlowPoint = i;
                }
            });
            if (firstFlowPoint == int.MaxValue || lastFlowPoint == int.MinValue)
                return "流速皆小於0.01 ml/min，無法計算";
            //calcd. point per second
            float pointPerSecond = 0;
            for (int i = 0; i < Data.Count - 1; i++)
                pointPerSecond += (float)(1 / (Data[i + 1].Time - Data[i].Time).TotalSeconds);
            pointPerSecond /= Data.Count;
            //Calcd.
            for (int i = firstFlowPoint; i <= lastFlowPoint; i++)
            {
                if (Data[i].Parameter[paramFlowIndex] >= 0.01)
                {
                    pauseFlag = false;
                    totalFlow +=
                        (Data[i].Parameter[paramFlowIndex]) *
                        (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    totalPressure +=
                        (Data[i].Parameter[paramPressureIndex]) *
                        (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    totalInnerTemp +=
                        (Data[i].Parameter[paramInnerTempIndex]) *
                        (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    totalTime += (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    if (Data[i].Parameter[paramPressureIndex] > maxP)
                    {
                        maxP = Data[i].Parameter[paramPressureIndex];
                        maxPTime = Data[i].Time;
                    }
                    if (Data[i].Parameter[paramInnerTempIndex] > maxInnerTemp)
                    {
                        maxInnerTemp = Data[i].Parameter[paramInnerTempIndex];
                        maxInnerTempTime = Data[i].Time;
                    }
                    if (Data[i].Parameter[paramFlowIndex] > maxFlow)
                        maxFlow = Data[i].Parameter[paramFlowIndex];
                }
                else
                {
                    if (!pauseFlag)
                    {
                        blankTime.Add(0);
                        totalInnerTempIncludngBlank.Add(0);
                        totalPressureIncludingBlank.Add(0);
                    }
                    pauseFlag = true;
                    blankTime[blankTime.Count - 1] += (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    totalPressureIncludingBlank[blankTime.Count - 1] +=
                        (Data[i].Parameter[paramPressureIndex]) *
                        (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    totalInnerTempIncludngBlank[blankTime.Count - 1] +=
                        (Data[i].Parameter[paramInnerTempIndex]) *
                        (float)(Data[i].Time - Data[i - 1].Time).TotalSeconds;
                    if (Data[i].Parameter[paramPressureIndex] > maxP)
                    {
                        maxP = Data[i].Parameter[paramPressureIndex];
                        maxPTime = Data[i].Time;
                    }
                    if (Data[i].Parameter[paramInnerTempIndex] > maxInnerTemp)
                    {
                        maxInnerTemp = Data[i].Parameter[paramInnerTempIndex];
                        maxInnerTempTime = Data[i].Time;
                    }
                }
            }
            float avgFlow = totalFlow / totalTime;
            float avgPressure = totalPressure / totalTime;
            float avgInnerTemp = totalInnerTemp / totalTime;
            float totalTimeWithBlank = totalTime;
            float totalPressureWithBlank = totalPressure;
            float totalInnerTempWithBlank = totalInnerTemp;
            for (int i = 0; i < blankTime.Count; i++)
            {
                if (blankTime[i] < 1 / pointPerSecond * 60 * 60 * 8) 
                {
                    totalTimeWithBlank += blankTime[i];
                    totalPressureWithBlank += totalPressureIncludingBlank[i];
                    totalInnerTempWithBlank += totalInnerTempIncludngBlank[i];
                }
            }
            float avgFlowWithBlank = totalFlow / totalTimeWithBlank;
            float avgPressureWithBlank = totalPressureWithBlank / totalTimeWithBlank;
            float avgInnerPVWithBlank = totalInnerTempWithBlank / totalTimeWithBlank;
            //Calcd. aging time
            bool agingDone = false;
            float agingTime = 0;
            DateTime agingStart = new DateTime();
            DateTime agingEnd = new DateTime();
            float agingEndP = 0;
            float maxDifference = 0.07f; ; //max drop for pressure when aging done
            if (YProp[2].UnitIndex == 1)
                maxDifference = 0.07f;
            else if (YProp[2].UnitIndex == 2)
                maxDifference = 1;
            else
                return "壓力不知道是三小單位";
            for (int i = lastFlowPoint + (int)(pointPerSecond * 60 * 30); i < Data.Count; i++)
            {
                if (Math.Abs(Data[i].Parameter[paramPressureIndex] - Data[i - (int)(pointPerSecond * 60 * 20)].Parameter[paramPressureIndex]) < maxDifference)
                {
                    //Check pressure change
                    bool recheck = true;
                    for (int j = i - (int)(pointPerSecond * 60 * 20); j <= i; j += (int)(pointPerSecond * 60 * 20) / 10)
                    {
                        if ((Data[Math.Min(j + (int)(pointPerSecond * 60 * 20) / 10, Data.Count - 1)].Parameter[paramPressureIndex] - Data[j].Parameter[paramPressureIndex]) > (YProp[2].UnitIndex == 2 ? 0.72519 : 0.05))
                            recheck = false;
                    }
                    if (recheck)
                    {
                        agingDone = true;
                        agingStart = Data[lastFlowPoint].Time;
                        agingEnd = Data[i - (int)(pointPerSecond * 60 * 20)].Time;
                        agingEndP = Data[i - (int)(pointPerSecond * 60 * 20)].Parameter[paramPressureIndex];
                        agingTime = (float)(Data[i - (int)(pointPerSecond * 60 * 20)].Time - Data[lastFlowPoint].Time).TotalMinutes;
                        break;
                    }
                }
            }
            //Return
            if (!simple)
            {
                return
                    "計算目標通道：" + ParamTitle[paramFlowIndex] + Environment.NewLine +
                    Environment.NewLine +
                    "*****僅計算流速大於0.1 ml/min(含)部分*****" + Environment.NewLine +
                    "總時間：" + string.Format("{0:N2}", totalTime / 60) + "分" + Environment.NewLine +
                    "總流量：" + string.Format("{0:N2}", totalFlow / 60) + " ml" + Environment.NewLine +
                    "最高流速：" + string.Format("{0:N2}", maxFlow) + " ml/min" + Environment.NewLine +
                    "平均流速：" + string.Format("{0:N2}", avgFlow) + " ml/min" + Environment.NewLine +
                    "最高壓力：" + string.Format("{0:N2}", maxP) + " " + unitList[yProp[Type][2].UnitIndex] + " at " + maxPTime.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                    "最高內溫：" + string.Format("{0:N2}", maxInnerTemp) + " \u00B0C at " + maxInnerTempTime.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                    "平均壓力：" + string.Format("{0:N2}", avgPressure) + " " + unitList[yProp[Type][2].UnitIndex] + Environment.NewLine +
                    "平均內溫：" + string.Format("{0:N2}", avgInnerTemp) + " \u00B0C" + Environment.NewLine +
                    Environment.NewLine +
                    "*****中途暫停時間超過8小時不列入計算*****" + Environment.NewLine +
                    "暫停時間：" + string.Format("{0:N2}", (totalTimeWithBlank - totalTime) / 60) + "分" + Environment.NewLine +
                    "總時間(含暫停時間)：" + string.Format("{0:N2}", totalTimeWithBlank / 60) + "分" + Environment.NewLine +
                    "平均流速(含暫停時間)：" + string.Format("{0:N2}", avgFlowWithBlank) + " ml/min" + Environment.NewLine +
                    "平均壓力(含暫停時間)：" + string.Format("{0:N2}", avgPressureWithBlank) + " " + unitList[yProp[Type][2].UnitIndex] + Environment.NewLine +
                    "平均內溫(含暫停時間)：" + string.Format("{0:N2}", avgInnerPVWithBlank) + " \u00B0C" + Environment.NewLine +
                    Environment.NewLine +
                    "*****熟成時間(20分鐘壓力變化等於0視為熟成完成)*****" + Environment.NewLine +
                    "熟成開始：" + (agingDone ? agingStart.ToString("MM/dd HH:mm:ss") : "N/A") + Environment.NewLine +
                    "熟成結束：" + (agingDone ? agingEnd.ToString("MM/dd HH:mm:ss") + " at " + string.Format("{0:N2}", agingEndP) + " " + unitList[yProp[Type][2].UnitIndex] : "N/A") + Environment.NewLine +
                    "熟成時間：" + (agingDone ? string.Format("{0:N2}", agingTime) + "分" : "熟成未完成") + Environment.NewLine +
                    Environment.NewLine +
                    "*****製程放大相關數據*****" + Environment.NewLine +
                    "1噸槽預估平均進料速率：" + string.Format("{0:N0}", avgFlow * 60 * fluidList[FluidIndex].Value / 1000 * 1000 / reactorSizeList[ReactorSizeIndex]) + " kg/hr" + Environment.NewLine +
                    "10噸槽預估平均進料速率：" + string.Format("{0:N0}", avgFlow * 60 * fluidList[FluidIndex].Value / 1000 * 10000 / reactorSizeList[ReactorSizeIndex]) + " kg/hr" + Environment.NewLine;
            }
            else
            {
                return
                    "Target channel: " + ParamTitle[SeriesMap[3][flowChannel]] + Environment.NewLine +
                    "Pause time(less than 8hr): " + string.Format("{0:N2}", (totalTimeWithBlank - totalTime) / 60) + "min" + Environment.NewLine +
                    "Total time: " + string.Format("{0:N2}", totalTimeWithBlank / 60) + "min" + Environment.NewLine +
                    "Avg. Flow:" + string.Format("{0:N2}", avgFlowWithBlank) + " ml/min" + Environment.NewLine +
                    "Avg. P.:" + string.Format("{0:N2}", avgPressureWithBlank) + " " + unitList[yProp[Type][2].UnitIndex] + Environment.NewLine +
                    "Avg. Temp.:" + string.Format("{0:N2}", avgInnerPVWithBlank) + " \u00B0C" + Environment.NewLine +
                    "Max. P.:" + string.Format("{0:N2}", maxP) + " " + unitList[yProp[Type][2].UnitIndex] + " at " + maxPTime.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                    "Max. Temp.：" + string.Format("{0:N2}", maxInnerTemp) + " \u00B0C at " + maxInnerTempTime.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                    "Aging start: " + (agingDone ? agingStart.ToString("MM/dd HH:mm:ss") : "N/A") + Environment.NewLine +
                    "Aging end: " + (agingDone ? agingEnd.ToString("MM/dd HH:mm:ss") + " at " + string.Format("{0:N2}", agingEndP) + " " + unitList[yProp[Type][2].UnitIndex] : "N/A") + Environment.NewLine +
                    "Aging time: " + (agingDone ? string.Format("{0:N2}", agingTime) + "min" : "Aging doesn't finish");
            }
        }

        private List<RecordData> RefineData (List<RecordData> Data, DateTime startTime, DateTime endTime)
        {
            List<RecordData> result = new List<RecordData>();
            foreach(RecordData item in Data)
            {
                if (item.Time.CompareTo(StartTime) >= 0 && item.Time.CompareTo(EndTime) <= 0)
                    result.Add(item);
            }
            return Data.Where(m => (m.Time.CompareTo(StartTime) >= 0 && m.Time.CompareTo(EndTime) <= 0)).OfType<RecordData>().ToList();
        }

        #endregion
    }
}
