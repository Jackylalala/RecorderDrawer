using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Linq;
using ZXing;
using ZXing.QrCode;
using System.Data;
using JR.Utils.GUI.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ZedGraph;
using System.Reflection;
using System.Drawing.Drawing2D;

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
        private class MemoryCardErrorException : Exception
        {
            public MemoryCardErrorException() : base() { }
            public MemoryCardErrorException(string message) : base(message) { }
            public MemoryCardErrorException(string message, Exception innerException) : base(message, innerException) { }
        }
        #endregion

        #region | Readonly Field |
        //default y properties
        private static readonly List<AxesProp[]> defaultYProp = new List<AxesProp[]>{
            new AxesProp[]{  //#1
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100),
                new AxesProp("扭力", 5, 0, 220, 20)},
            new AxesProp[]{  //#2
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100),
                new AxesProp("扭力", 5, 0, 220, 20)},
            new AxesProp[]{  //#3
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //#4
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //#5
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //#6
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //#7
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //#8
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -2, 20, 2),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //R1-CHPPO
                new AxesProp("內溫", 0, 0, 150, 15),
                new AxesProp("外溫", 0, 0, 150, 15),
                new AxesProp("壓力", 2, 0, 500, 50),
                new AxesProp("升溫", 6, 0, 10, 1),
                new AxesProp("轉速", 4, 0, 600, 60)},
            new AxesProp[]{  //R1-EOD
                new AxesProp("內溫", 0, 0, 150, 15),
                new AxesProp("外溫", 0, 0, 200, 20),
                new AxesProp("壓力", 2, 0, 150, 15),
                new AxesProp("流速", 3, 0, 10, 1),
                new AxesProp("轉速", 4, 0, 400, 40)},
            new AxesProp[]{  //R3-CHPPO
                new AxesProp("內溫", 0, 0, 150, 15),
                new AxesProp("外溫", 0, 0, 150, 15),
                new AxesProp("壓力", 2, 0, 500, 50),
                new AxesProp("升溫", 6, 0, 10, 1),
                new AxesProp("轉速", 4, 0, 600, 60)},
            new AxesProp[]{  //R3-EOD
                new AxesProp("內溫", 0, 0, 150, 15),
                new AxesProp("外溫", 0, 0, 200, 20),
                new AxesProp("壓力", 2, 0, 150, 15),
                new AxesProp("流速", 3, 0, 10, 1),
                new AxesProp("轉速", 4, 0, 400, 40)},
            new AxesProp[]{  //CHPPO Pilot
                new AxesProp("溫度", 0, 0, 200, 20),
                new AxesProp("壓力", 7, 0, 100, 10),
                new AxesProp("液位", 8, 0, 500, 50),
                new AxesProp("流量", 9, 0, 250, 25),
                new AxesProp("總量", 10, 0, 300, 30),
                new AxesProp("WHSV", 11, 0, 15, 1.5F)},
            new AxesProp[]{  //DMC CAOS
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -5, 50, 5),
                new AxesProp("流速", 3, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
            new AxesProp[]{  //DMC CHPPO
                new AxesProp("內溫", 0, 0, 220, 20),
                new AxesProp("外溫", 0, 0, 220, 20),
                new AxesProp("壓力", 1, -5, 50, 5),
                new AxesProp("升溫過程", 12, 0, 22, 2),
                new AxesProp("轉速", 4, 0, 1100, 100)},
        };
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
            "DMC CAOS", //13
            "DMC CHPPO", //14
            "自訂資料格式", //15
        };
        //Unit table
        public static readonly string[] unitList = { "\u00b0C", "bar", "psi", "ml/min", "rpm", "Ncm", "%", "kg/cm\u00b2", "mm", "g/hr", "g", "h\u207b\u00b9", "stage" };
        //Fluid list
        public static readonly KeyValuePair<string, float>[] fluidList = new KeyValuePair<string, float>[] {
            new KeyValuePair<string, float> ( "Ethylene Oxide", 0.88F ),
            new KeyValuePair<string, float> ( "Propylene Oxide", 0.83F ) };
        //Reactor size
        public static readonly float[] reactorSizeList = { 1, 2, 3, 5, 100 };
        //Datetime format
        public static readonly string[] datetimeFormatList = {
                            "yyyy-MM-dd HH:mm:ss",
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
        private static readonly float[] yLabelXPos = new float[] { -0.03F, 1.03F, -0.105F, 1.105F, -0.17F, 1.17F };
        private static readonly float[] yLabelYPos = new float[] { -0.05F, -0.11F, -0.145F, -0.18F, -0.215F, -0.25F };
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
        private static readonly int maxDataCount = 86400;
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
            new int[7][] { new int[] { 0 }, new int[] { 1, 2, 7 }, new int[] { 3, 4 }, new int[] { 6 }, new int[] { 5, 9 }, new int[] { 8 }, new int[] {  } }, //13
            new int[7][] { new int[] { 0 }, new int[] { 1, 2, 7 }, new int[] { 3, 4 }, new int[] { 6 }, new int[] { 5, 9 }, new int[] { 8 }, new int[] {  } }, //14
            };
        //row number of first data row
        private static List<int> firstDataRow = new List<int> { 1, 1, 1, 1, 3, 1, 1, 1, 3, 3, 3, 3, 1, 1, 1 };
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
            new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "EO流速", "壓力PV", "程控SV", "轉速PV", "PO流速" }, //13
            new string[] { "內溫SV", "內溫PV", "外溫SV", "外溫PV", "升溫段1", "壓力PV", "程控SV", "轉速PV", "升溫段2" }, //14
         };
        //Data information
        private static bool dataRefined = false;
        private List<int> hiddenList = new List<int>(); //Hidden list of series
        private string rawFileName; //File name
        private int fileType; //-1: folder, 0: csv, 1: krf, 2: dat, 3: dmt
        private static PointPairList[] trendSeriesRaw;
        private static PointPairList[] trendSeriesRefined;
        //Y axis properties
        private static List<AxesProp[]> yProp = new List<AxesProp[]>();
        //Chart item selection box and title
        private CheckBox[] chkChartItem;
        //Chart data display box
        private System.Windows.Forms.Label[] lblDataDisplay;
        //Chart multiple/reduce button
        private Button[] btnMultiple;
        private Button[] btnReduce;
        //Synchronization lock
        private static object syncHandle = new object();
        //Threshold
        private float minLimit = float.MinValue;
        private float maxLimit = float.MaxValue;
        //Drag&drop
        private bool validFile;
        //Cursor index
        private int cursorIndex = -1;
        private Point oldPoint = new Point(0, 0);
        //Selection
        private bool select = false;
        private int indexSelectionStart;
        private int indexSelectionEnd;
        //Delegate function
        private delegate bool SetFrameWorkDelegate();
        private delegate bool DrawChartDelegate(ZedGraphControl chart, float zoomFactor, bool qrcode = false);
        //merge count(>1 means do merge
        int mergeCount = 0;
        int dataCount = 0;
        #endregion

        #region | Properties |
        public static int[][] SeriesMap { get { return seriesMap[Type]; } }
        //Recorder type, -1 means auto detect, according to REACTOR_LIST
        public static int Type { get; private set; }
        public static int FirstDataRow { get { return firstDataRow[Type]; } }
        //note: ALL SERIES must have same count (fill 0 when null)
        public static PointPairList[] TrendSeries { get { if (dataRefined) return trendSeriesRefined; else return trendSeriesRaw; } }
        //ascending number of columu(s) contains parameter(s), which create from SeriesMap[Type]
        private int[] ParamColNum { get { return SeriesMap.Skip(1).SelectMany(m => m).OrderBy(n => n).ToArray(); } }
        public static string[] ParamTitle { get { return paramTitle[Type]; } }
        public static AxesProp[] YProp { get { return yProp[Type]; } }
        public static int XType { get; private set; } = 0;//0: text, 1: DateTime
        public static int XInterval { get; private set; } = 0;//X axis interval in minute
        public static int XAngle { get; private set; } = 0;//X axis label angle
        public static DateTime StartTime { get; private set; } = DateTime.MinValue;
        public static DateTime EndTime { get; private set; } = DateTime.MaxValue;
        public static string TitleText { get; private set; } = "";
        public static int FluidIndex { get; private set; } = 0;
        public static int ReactorSizeIndex { get; private set; } = 0; //In Liter
        public static string RecorderName { get; private set; } = "";
        public static int Percentage { get; private set; }
        public static int Duration { get; private set; }
        public static bool AgingTempFix { get; private set; } = false;
        public static float AgingPDiff { get; private set; } = 1;
        public static float AgingTimeHold { get; private set; } = 30;
        #endregion

        #region | Event |
        public frmMain()
        {
            InitializeComponent();
            //Load the settings
            try
            {
                int defaultYCount = defaultYProp.Select(m => m.Length).Sum();
                int[] yCountList = Properties.Settings.Default.YCount.Split(',').Select(m => int.Parse(m)).ToArray();
                string[] yPropString = Properties.Settings.Default.YProp.Split(',');
                if (yPropString.Length/5 != defaultYCount || yCountList.Sum() != defaultYCount)
                    throw new Exception();
                int counter = 0;
                for (int i = 0; i < yCountList.Length; i++)
                {
                    yProp.Add(new AxesProp[yCountList[i]]);
                    int counter2 = 0;
                    for (int j = counter; j < counter + yCountList[i] * 5; j += 5)
                    {
                        yProp[i][counter2] = new AxesProp(
                            yPropString[j],
                            int.Parse(yPropString[j + 1]),
                            float.Parse(yPropString[j + 2]),
                            float.Parse(yPropString[j + 3]),
                            float.Parse(yPropString[j + 4])
                            );
                        counter2++;
                    }
                    counter += yCountList[i] * 5;
                }
                XType = Properties.Settings.Default.XType;
                XInterval = Properties.Settings.Default.XInterval;
                XAngle = Properties.Settings.Default.XAngle;
                FluidIndex = Properties.Settings.Default.FluidIndex;
                ReactorSizeIndex = Properties.Settings.Default.ReactorSizeIndex;
            }
            catch (Exception)
            {
                lblInformation.Text = "設定讀取錯誤，使用預設值";
                //Default axes setting
                yProp = defaultYProp;
            }
        }

        private void frmRecorderDrawer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //clear custom data
            if (yProp.Count == recorderList.Length)
                yProp.RemoveAt(yProp.Count - 1);
            //Save settings
            Properties.Settings.Default.YCount = string.Join(",", yProp.Select(m => m.Length));
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
                //Type select
                frmSelector frmTypeSelector = new frmSelector(new string[] { "自動選擇" }.Union(recorderList).ToArray(), "控制器編號", true);
                frmTypeSelector.StartPosition = FormStartPosition.Manual;
                frmTypeSelector.Location = new Point(Location.X + Width / 2 - frmTypeSelector.ClientSize.Width / 2, Location.Y + Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                if (frmTypeSelector.ShowDialog(this) == DialogResult.OK)
                {
                    Type = frmTypeSelector.Index - 1;
                    //Draw chart
                    bgdWorkerDraw.RunWorkerAsync(new object[] { true, false, chtMain });
                }
            }
        }

        private void frmMain_DragLeave(object sender, EventArgs e)
        {
            txtFilePath.Text = "";
        }

        private void frmRecorderDrawer_DragEnter(object sender, DragEventArgs e)
        {
            validFile = GetFilename(out rawFileName, e);
            if (validFile)
            {
                txtFilePath.Text = rawFileName;
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void mnuOpen_Click(object sender, EventArgs e)
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
                    txtFilePath.Text = rawFileName;
                    bgdWorkerDraw.RunWorkerAsync(new object[] { true, false, chtMain });
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "請選擇數據檔", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuOpenFolder_Click(object sender, EventArgs e)
        {
            CustomFileBrowserDialog fbd = new CustomFileBrowserDialog();
            //fbd.Description = "選擇記憶卡";
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //Type select
                frmSelector frmTypeSelector = new frmSelector(new string[] { "自動選擇" }.Union(recorderList).ToArray(), "控制器編號", true);
                frmTypeSelector.StartPosition = FormStartPosition.Manual;
                frmTypeSelector.Location = new Point(Location.X + Width / 2 - frmTypeSelector.ClientSize.Width / 2, Location.Y + Height / 2 - frmTypeSelector.ClientSize.Height / 2);
                if (frmTypeSelector.ShowDialog(this) == DialogResult.OK)
                {
                    rawFileName = fbd.DirectoryPath;
                    txtFilePath.Text = rawFileName;
                    Type = frmTypeSelector.Index - 1;
                    fileType = -1;
                    //Draw chart
                    bgdWorkerDraw.RunWorkerAsync(new object[] { true, false, chtMain });
                }
            }
        }

        private void mnuAnalysis_Click(object sender, EventArgs e)
        {
            bgdWorkerDraw.RunWorkerAsync(new object[] { true, false, chtMain });
        }

        private void mnuTitle_Click(object sender, EventArgs e)
        {
            string input = TitleText;
            if (InputBox("圖表標題", "請輸入圖表標題", ref input) == DialogResult.OK)
            {
                TitleText = input;
                //Draw chart
                bgdWorkerDraw.RunWorkerAsync(new object[] { false, false, chtMain });
            }
        }

        private void mnuDetailedSetting_Click(object sender, EventArgs e)
        {
            frmDetailedSetting frmDetailedSetting = new frmDetailedSetting();
            frmDetailedSetting.StartPosition = FormStartPosition.Manual;
            frmDetailedSetting.Location = new Point(Location.X + Width / 2 - frmDetailedSetting.ClientSize.Width / 2, Location.Y + Height / 2 - frmDetailedSetting.ClientSize.Height / 2);
            if (frmDetailedSetting.ShowDialog(this) == DialogResult.OK)
            {
                //General tab
                TitleText = frmDetailedSetting.TitleText;
                StartTime = frmDetailedSetting.StartTime;
                EndTime = frmDetailedSetting.EndTime;
                FluidIndex = frmDetailedSetting.FluidIndex;
                ReactorSizeIndex = frmDetailedSetting.ReactorSizeIndex;
                RecorderName = frmDetailedSetting.RecorderName;
                //Axes tab
                yProp[Type] = frmDetailedSetting.YProp;
                XType = frmDetailedSetting.XType;
                XInterval = frmDetailedSetting.XInterval;
                XAngle = frmDetailedSetting.XAngle;
                //Animation tab
                Percentage = frmDetailedSetting.Percentage;
                Duration = frmDetailedSetting.Duration;
                //aging tab
                AgingTempFix = frmDetailedSetting.AgingTempFix;
                AgingPDiff = frmDetailedSetting.AgingPDiff;
                AgingTimeHold = frmDetailedSetting.AgingTimeHold;
                //commit
                bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain });
            }

        }

        private void mnuStatList_Click(object sender, EventArgs e)
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

        private void mnuToClip_Click(object sender, EventArgs e)
        {
            FunctionSwitch(false);
            MemoryStream ms = new MemoryStream();
            ZedGraphControl chtDraw = new ZedGraphControl();
            chtDraw.Width = chtMain.Width * 5;
            chtDraw.Height = chtMain.Height * 5;
            DrawChart(chtDraw, 5, true);
            chtDraw.SaveAs(ms, 300, ImageFormat.Jpeg);
            if (ms != null)
            {
                Clipboard.SetImage(new Bitmap(ms));
                MessageBox.Show(this, "圖片已複製到剪貼簿(最佳化bitmap)");
            }
            else
                MessageBox.Show(this, "圖片產生失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FunctionSwitch(true);
        }

        private void mnuExportImgToFile_Click(object sender, EventArgs e)
        {
            FunctionSwitch(false);
            SaveFileDialog sfd = new SaveFileDialog();
            try
            {
                sfd.Title = "匯出圖片";
                sfd.Filter = @"Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg,*.jpeg)|*.jpg;*.jpeg|GIF(*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif;*.tiff";
                sfd.FilterIndex = 2;
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
                    ZedGraphControl chtDraw = new ZedGraphControl();
                    chtDraw.Width = chtMain.Width * 5;
                    chtDraw.Height = chtMain.Height * 5;
                    DrawChart(chtDraw, 5, true);
                    chtDraw.SaveAs(sfd.FileName, 300, format);
                    MessageBox.Show(this, "匯出圖片成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FunctionSwitch(true);
            }
        }

        private void mnuExportImgToMail_Click(object sender, EventArgs e)
        {
            FunctionSwitch(false);
            MessageBox.Show(this, "將使用本機Outlook帳戶寄送郵件，若需要權限請點選[允許]");
            //create temp file
            string tempDoc = Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
            ZedGraphControl chtDraw = new ZedGraphControl();
            chtDraw.Width = chtMain.Width * 5;
            chtDraw.Height = chtMain.Height * 5;
            DrawChart(chtDraw, 5, true);
            chtDraw.SaveAs(tempDoc, 300, ImageFormat.Jpeg);
            // send via outlook
            Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem mailItem = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML;
            mailItem.Body = "Sent at " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            mailItem.Attachments.Add(tempDoc);
            mailItem.Display();
            Marshal.FinalReleaseComObject(outlook);
            File.Delete(tempDoc);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            FunctionSwitch(true);
        }

        private void mnuRecorderFig_Click(object sender, EventArgs e)
        {
            frmRecorderFigure frmRecorderFigure = new frmRecorderFigure();
            frmRecorderFigure.StartPosition = FormStartPosition.Manual;
            frmRecorderFigure.Location = new Point(Location.X + Width / 2 - frmRecorderFigure.ClientSize.Width / 2, Location.Y + Height / 2 - frmRecorderFigure.ClientSize.Height / 2);
            frmRecorderFigure.ShowDialog(this);
        }

        private void mnuRawdata_Click(object sender, EventArgs e)
        {
            try
            {
                if (mergeCount > 1)
                {
                    MessageBox.Show("由於資料點超過" + maxDataCount + "，目前顯示數據為平均數值，請縮小範圍後再進行編輯", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("注意！這將會修改到原始數據，需[重新分析]或重新讀取檔案才會回復。請確認後再繼續。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                dataRefined = false;
                frmRawData frmRawData = new frmRawData();
                frmRawData.StartPosition = FormStartPosition.Manual;
                frmRawData.Location = new Point(Location.X + Width / 2 - frmRawData.ClientSize.Width / 2, Location.Y + Height / 2 - frmRawData.ClientSize.Height / 2);
                if (frmRawData.ShowDialog(this) == DialogResult.OK)
                    bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuDifferential_Click(object sender, EventArgs e)
        {
            if (mergeCount > 1)
            {
                MessageBox.Show("由於資料點超過" + maxDataCount + "，目前顯示數據為平均數值，請縮小範圍後再進行計算", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Choose target fluid
            frmSelector channelSelector = new frmSelector(ParamTitle, "目標通道", false);
            channelSelector.StartPosition = FormStartPosition.Manual;
            channelSelector.Location = new Point(Location.X + Width / 2 - channelSelector.ClientSize.Width / 2, Location.Y + Height / 2 - channelSelector.ClientSize.Height / 2);
            if (channelSelector.ShowDialog(this) == DialogResult.OK)
            {
                frmSelector unitSelector = new frmSelector(new string[] { "秒", "分", "小時" }, "時間單位", false);
                unitSelector.StartPosition = FormStartPosition.Manual;
                unitSelector.Location = new Point(Location.X + Width / 2 - channelSelector.ClientSize.Width / 2, Location.Y + Height / 2 - channelSelector.ClientSize.Height / 2);
                if (unitSelector.ShowDialog() == DialogResult.OK)
                {
                    frmDifferential frmDifferential = new frmDifferential(TrendSeries[channelSelector.Index], unitSelector.Index);
                    frmDifferential.Show();
                }
            }
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
                else if (cursorIndex > TrendSeries[0].Count - 1)
                    cursorIndex = TrendSeries[0].Count - 1;
                else
                {
                    Point newPoint = chtMain.PointToScreen(new Point(
                        (int)Math.Round(chtMain.GraphPane.Chart.Rect.Width * ((float)cursorIndex / (TrendSeries[0].Count - 1)) + chtMain.GraphPane.Chart.Rect.X, MidpointRounding.AwayFromZero),
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

        private bool chtMain_MouseDown(ZedGraphControl sender, MouseEventArgs e)
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

        private bool chtMain_MouseUp(ZedGraphControl sender, MouseEventArgs e)
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
                        StartTime = new XDate(TrendSeries[0][indexSelectionStart].X);
                        EndTime = new XDate(TrendSeries[0][indexSelectionEnd].X);
                        //Draw chart
                        bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain });
                    }
                }
            }
            return false; //retuen false in order to show dash line by zedgraphcontrol itself
        }

        private void chtMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                StartTime = new XDate(trendSeriesRaw[0][0].X);
                EndTime = new XDate(trendSeriesRaw[0][trendSeriesRaw[0].Count-1].X);
                bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain });
            }
        }

        private void chtMain_MouseEnter(object sender, EventArgs e)
        {
            chtMain.Invalidate();
        }

        private void InputOnlyNumber(object sender, KeyPressEventArgs e)
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

        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            bgdWorkerDraw.RunWorkerAsync(new object[] { false, false, chtMain });
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
            bgdWorkerDraw.RunWorkerAsync(new object[] { false, false, chtMain });
        }

        /// <summary>
        /// Read data and draw chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">parameter: (bool)reload, (bool)refine, (ZedGraphControl)targetChart</param>
        private void bgdWorkerDraw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool reload = bool.Parse((e.Argument as object[])[0].ToString());
            bool refine = bool.Parse((e.Argument as object[])[1].ToString());
            ZedGraphControl targetChart = (ZedGraphControl)(e.Argument as object[])[2];
            float factor = (e.Argument as object[]).Length > 3 ? float.Parse((e.Argument as object[])[3].ToString()) : 1.0f;
            int index = (e.Argument as object[]).Length > 3 ? int.Parse((e.Argument as object[])[4].ToString()) : -1;
            if (reload)
            {
                bgdWorkerDraw.ReportProgress(99, "Read Data");
                if (ReadData())
                {
                    bgdWorkerDraw.ReportProgress(99, "Set Framework");
                    if (SetFrameWork())
                    {
                        bgdWorkerDraw.ReportProgress(99, "Draw Chart");
                        e.Result = DrawChart(targetChart);
                    }
                }
            }
            else
            {
                if (refine)
                {
                    bgdWorkerDraw.ReportProgress(99, "Refine Data");
                    RefineData(factor, index);
                }
                bgdWorkerDraw.ReportProgress(99, "Draw Chart");
                e.Result = DrawChart(targetChart);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " ms");
        }

        private void bgdWorkerDraw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Equals("Read Data")) //reset limit
            {
                txtMaxLimit.Text = "";
                txtMinLimit.Text = "";
            }
            if (!lblProcessingInfo.Visible)
                FunctionSwitch(false);
            lblProcessingInfo.Text = e.UserState.ToString();
        }

        private void bgdWorkerDraw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProcessingInfo.Visible = false;
            if ((bool)e.Result)
            {
                FunctionSwitch(true);
                //show summary
                if (!e.Cancelled && e.Error == null && TrendSeries[0].Count > 0)
                    lblInformation.Text = "共 " + dataCount + (mergeCount > 1 ? "(merged to "+ TrendSeries[0].Count + ")" : "") + " 筆資料，自 " + StartTime.ToString("MM/dd HH:mm:ss") + " 到 " + EndTime.ToString("MM/dd HH:mm:ss");
            }
        }

        private void txtLimit_TextChanged(object sender, EventArgs e)
        {
            ((Control)sender).Tag = true;
        }

        private void txtLimit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                DataLimitChage(sender, e);
        }

        private void SetDataFactor(object sender, EventArgs e)
        {
            if (MessageBox.Show("注意！這將會修改到原始數據，需[重新分析]或重新讀取檔案才會回復。請確認後再繼續。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            int index = int.Parse(((Button)sender).Tag.ToString());
            float factor = ((Button)sender).Text.Equals("x10") ? 10 : 0.1f;
            //Reset chart
            bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain, factor, index });
        }
        #endregion

        #region | Methods |
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
                index = TrendSeries[0].Count - 1;
            else
                index = (int)((TrendSeries[0].Count - 1) * ((mousePosition.X - chtMain.GraphPane.Chart.Rect.X) / chtMain.GraphPane.Chart.Rect.Width));
            return index;
        }

        private void DataLimitChage(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(TextBox))
                return;
            TextBox ctrl = sender as TextBox;
            if (bool.Parse(ctrl.Tag.ToString()))
            {
                ctrl.Tag = false;
                if (!float.TryParse(txtMinLimit.Text, out minLimit))
                    minLimit = float.MinValue;
                if (!float.TryParse(txtMaxLimit.Text, out maxLimit))
                    maxLimit = float.MaxValue;
                bgdWorkerDraw.RunWorkerAsync(new object[] { false, true, chtMain });
            }
        }

        private void FunctionSwitch(bool state)
        {
            //display render message
            lblProcessingInfo.Visible = !state;
            lblProcessingInfo.Refresh();
            //clear message
            lblTimeDisplay.Text = "";
            lblTimeDisplay.Visible = state;
            lblInformation.Text = "";
            if (lblDataDisplay != null)
            {
                foreach (System.Windows.Forms.Label item in lblDataDisplay)
                    item.Text = "";
            }
            //Disable function
            mnuAnalysis.Enabled = state;
            mnuExportImg.Enabled = state;
            mnuToClip.Enabled = state;
            mnuStatList.Enabled = state;
            mnuSetting.Enabled = state;
            mnuRawdata.Enabled = state;
            mnuCalculation.Enabled = state;
            pnlChartItems.Visible = state;
            pnlChartSetting.Visible = state;
            chtMain.Visible = state;
        }

        private void ShowParameter(int index)
        {
            if (index < 0 || index >= TrendSeries[0].Count) //out of range
            {
                lblTimeDisplay.Text = "";
                for (int i = 0; i < trendSeriesRaw.Length; i++)
                    lblDataDisplay[i].Text = "";
            }
            else
            {
                try
                {
                    lblTimeDisplay.Text = new XDate(TrendSeries[0][index].X).ToString("MM/dd HH:mm:ss");
                    for (int i = 0; i < TrendSeries.Length; i++)
                        lblDataDisplay[i].Text = TrendSeries[i][index].Y.ToString("0.00");
                }
                catch(Exception)
                {
                    lblTimeDisplay.Text = "";
                    for (int i = 0; i < trendSeriesRaw.Length; i++)
                        lblDataDisplay[i].Text = "";
                }
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
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
                if (!(File.Exists(rawFileName) || Directory.Exists(rawFileName)))
                    throw new FileNotFoundException();
                //clear data
                dataRefined = false;
                hiddenList.Clear();
                maxLimit = float.MaxValue;
                minLimit = float.MinValue;
                //clear custom data
                if (seriesMap.Count == recorderList.Length)
                    seriesMap.RemoveAt(seriesMap.Count - 1);
                if (firstDataRow.Count == recorderList.Length)
                    firstDataRow.RemoveAt(firstDataRow.Count - 1);
                if (yProp.Count == recorderList.Length)
                    yProp.RemoveAt(yProp.Count - 1);
                if (paramTitle.Count == recorderList.Length)
                    paramTitle.RemoveAt(paramTitle.Count - 1);
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
                                RecorderName = frmFormatSelector.RecorderName;
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
                            else
                                throw new RecorderTypeErrorException();
                        }
                        //initiate series
                        trendSeriesRaw = new PointPairList[SeriesMap.Skip(1).SelectMany(m => m).Count()];
                        for (int i = 0; i < trendSeriesRaw.Length; i++)
                            trendSeriesRaw[i] = new PointPairList();
                        //Data end flag, only for string X axis (because DateTime format will automatically sort)
                        //Parse data string, remember skip first row(no data)
                        for (int i = FirstDataRow; i < allLines.Count; i++)
                        {
                            string[] data = allLines[i].Split(',', '\t');
                            string[] paramString = ParamColNum.Select(n => data[n]).ToArray();
                            //Convert to number
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
                                //add parameter
                                for (int j = 0; j < trendSeriesRaw.Length; j++)
                                {
                                    double num = 0;
                                    double.TryParse(paramString[j], out num);
                                    //fix number with factor
                                    switch (Type)
                                    {
                                        case 2:
                                            if (j == 7)
                                                num /= 10;
                                            break;
                                        case 4:
                                            if (j == 2) //pressure column
                                                num /= 10;
                                            break;
                                        case 6:
                                            if (j == 4 || j == 5) //first flow and pressure column
                                                num /= 10;
                                            if (j == 7) //rotation speed column
                                                num *= 10;
                                            break;
                                        case 13:
                                            if (j == 4 || j == 5 || j == 8)
                                                num /= 10;
                                            if (j == 7)
                                                num *= 10;
                                            break;
                                    }
                                    //add to series
                                    trendSeriesRaw[j].Add(new XDate(date), num, ParamTitle[j]);
                                }
                            }
                        }
                        break;
                    case 1: //krf file
                        using (BinaryReader reader = new BinaryReader(new FileStream(rawFileName, FileMode.Open, FileAccess.Read)))
                        {
                            DateTime start;
                            int interval;
                            //Check file
                            if (reader.ReadChars(3).Equals("KR2"))
                                throw new FileTypeErrorException();
                            //Read start datetimme and interval
                            reader.BaseStream.Seek(0x20, SeekOrigin.Begin);
                            start = new DateTime(2000, 1, 1).AddSeconds(reader.ReadInt32());
                            reader.BaseStream.Seek(0x28, SeekOrigin.Begin);
                            interval = reader.ReadInt16() * 100 + reader.ReadInt16() * 86400000; //In millsecond
                            //try to get type, default is type 5
                            reader.BaseStream.Seek(0x58, SeekOrigin.Begin);
                            if (Type == -1)
                            {
                                string info = new string(reader.ReadChars(3));
                                if (info.Equals("101"))
                                    Type = 5;
                                else if (info.Equals("201"))
                                    Type = 7;
                                else
                                    Type = 5;
                            }
                            //read digit(s) of each channel
                            List<int> digits = new List<int>();
                            reader.BaseStream.Seek(0x764, SeekOrigin.Begin);
                            for (int i = 0; i < 6; i++)
                                digits.Add(reader.ReadInt16());
                            //read data
                            trendSeriesRaw = new PointPairList[6];
                            for (int i = 0; i < 6; i++)
                                trendSeriesRaw[i] = new PointPairList();
                            reader.BaseStream.Seek(0x8bc, SeekOrigin.Begin);
                            while(reader.BaseStream.Position!=reader.BaseStream.Length)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    trendSeriesRaw[i].Add(
                                        new XDate(start.AddMilliseconds(interval * trendSeriesRaw[i].Count)),
                                        reader.ReadInt16() / (float)(Math.Pow(10, digits[i])),
                                        ParamTitle[i]
                                        );
                                    reader.ReadInt16(); //skip
                                }
                            }
                        }
                        break;
                    case 2: //dat file
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
                            RecorderName = reader.ReadString(); //recorder name
                            trendSeriesRaw = new PointPairList[reader.ReadInt32()]; //trend count
                            for (int i=0;i<trendSeriesRaw.Length;i++)
                            {
                                trendSeriesRaw[i] = new PointPairList();
                                int dataCount = reader.ReadInt32();
                                for(int j=0;j<dataCount;j++)
                                    trendSeriesRaw[i].Add(
                                        new XDate(reader.ReadDouble()),
                                        reader.ReadDouble(),
                                        reader.ReadString()
                                        );
                            }
                        }
                        break;
                    case -1: //folder, CHUNDE(Observer) data
                        if (Type == -1)
                            Type = 4; //default type is 4
                        //check file
                        for (int i = 0; i < 6; i++)
                        {
                            if (!(File.Exists(rawFileName + @"\P" + i + @".dat") && File.Exists(rawFileName + @"\P" + i + @".idx")))
                                throw new MemoryCardErrorException();
                        }
                        List<long[]>[] index = new List<long[]>[6];
                        //NOTE: ALL time save as seconds(ticks * 10,000,000)
                        long[] timeStart = new long[6];
                        for (int i = 0; i < 6; i++)
                        {
                            //get index
                            index[i] = new List<long[]>();
                            byte[] data = File.ReadAllBytes(rawFileName + @"\P" + i + @".idx");
                            int counter = 0;
                            unsafe
                            {
                                fixed (byte* dataPtr = &data[0])
                                {
                                    timeStart[i] = *((long*)(dataPtr + counter));
                                    counter += 24; //skip 16 bytes
                                    while (counter < data.Length)
                                    {
                                        index[i].Add(new long[4]);
                                        index[i][index[i].Count - 1][0] = *((int*)(dataPtr + counter)); //start address
                                        counter += 4;
                                        index[i][index[i].Count - 1][1] = *((int*)(dataPtr + counter)); //data count
                                        counter += 4;
                                        index[i][index[i].Count - 1][2] = *((int*)(dataPtr + counter)); //datetime offset(in 100 millisecond)
                                        counter += 8; //skip 4 bytes
                                        index[i][index[i].Count - 1][3] = *((long*)(dataPtr + counter)); ; //digit fingerprint
                                        counter += 8;
                                    }
                                }
                            }
                        }
                        Dictionary<long,double>[] tempSeries = new Dictionary<long, double>[ParamTitle.Length];
                        long timeMin = long.MaxValue;
                        long timeMax = long.MinValue;
                        Parallel.For(0, tempSeries.Length, i =>
                        {
                            tempSeries[i] = new Dictionary<long, double>();
                            byte[] data = File.ReadAllBytes(rawFileName + @"\P" + i + @".dat");
                            foreach (long[] block in index[i])
                            {
                                //digit(temp)
                                int digit = 0;
                                if (block[3] == 0x0201000AC347FD71)
                                    digit = 2;
                                else if (block[3] == 0x0101000AC4F9FCCD)
                                    digit = 1;
                                else if (block[3] == 0x0001000AC69C3E00)
                                    digit = 0;
                                if (Type == 4 && i == 2)
                                    digit += 1;
                                //get data
                                unsafe
                                {
                                    fixed (byte* dataPtr = &data[(int)block[0]])
                                    {
                                        int offset = 0;
                                        for (int k = 0; k < block[1]; k++)
                                        {
                                            //time
                                            long time = timeStart[i] + block[2] * 1000000 + k * 10000000;
                                            //set time interval
                                            if (time > timeMax)
                                                timeMax = time;
                                            if (time < timeMin)
                                                timeMin = time;
                                            //value
                                            double value = *((ushort*)dataPtr + offset);
                                            if (value == 0xffff)
                                                value = double.NaN;
                                            else
                                                value = (*((ushort*)dataPtr + offset) - 0x4e1f) / (float)(Math.Pow(10, digit));
                                            tempSeries[i].Add(time, value);
                                            offset += 2;
                                        }
                                    }
                                }
                            }
                        });
                        //align
                        trendSeriesRaw = new PointPairList[ParamTitle.Length];
                        for (int i = 0; i < trendSeriesRaw.Length; i++)
                        {
                            trendSeriesRaw[i] = new PointPairList();
                            for (long j =0;j<((timeMax-timeMin)/10000000+1);j++)
                            {
                                tempSeries[i].TryGetValue(j * 10000000 + timeMin, out double value);
                                trendSeriesRaw[i].Add(new PointPair(XDate.DateTimeToXLDate(DateTime.FromFileTimeUtc(j * 10000000 + timeMin)), value, ParamTitle[i]));
                            }
                        }
                        break;
                }
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
                //get start time and end time
                StartTime = new XDate(trendSeriesRaw.Min(m => m[0].X));
                EndTime = new XDate(trendSeriesRaw.Max(m=>m[m.Count-1].X));
                //refine data
                RefineData();
                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("資料檔不存在或檔案為空", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (MemoryCardErrorException)
            {
                MessageBox.Show("該記憶卡(資料夾)內未找到通道資訊，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileTypeErrorException)
            {
                MessageBox.Show("檔案類型錯誤，請重新選擇正確檔案", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (RecorderTypeErrorException)
            {
                if (MessageBox.Show("資料格式錯誤，是否手動選擇資料格式？", "錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
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

        private bool SetFrameWork()
        {
            if (InvokeRequired)
                //Must be sync. invoke (BeginInvoke is unsync.)
                return (bool)Invoke(new SetFrameWorkDelegate(SetFrameWork));
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
                    if (Type == 12)
                    {
                        showTooltip = true;
                        tooltipString = new string[] { "丙烯儲槽溫度", "反應器溫度(底)", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度", "反應器溫度(頂)", "反應器出口冷卻溫度", "反應器CHP25%入口加熱帶溫度", "出料加熱帶溫度", "PI-201", "丙烯儲槽壓力", "丙烯pump出口壓力", "丙烯pump出口至反應器管線壓力", "CHP25% 儲槽壓力", "氣液分離槽壓力", "CHP25%儲槽液位", "氮氣質量流量計", "背壓閥壓力", "氣體質量流量計", "反應器入口丙烯加熱帶", "體質量流量計總量", "反應器壓力差" };
                    }
                    //create object
                    chkChartItem = new CheckBox[paramCount];
                    btnMultiple = new Button[paramCount];
                    btnReduce = new Button[paramCount];
                    lblDataDisplay = new System.Windows.Forms.Label[paramCount];
                    //Set chart item selection box and corresponding data display box
                    int boxWidth = 80;
                    int padding = Math.Max((pnlChartItems.Width - boxWidth * chkChartItem.Length) / (chkChartItem.Length + 1), 5);
                    pnlChartItems.Controls.Clear();
                    if (chkChartItem.Length > 10)
                        pnlChartItems.HorizontalScroll.Visible = true;
                    int panelExactHeight = pnlChartItems.Height - (chkChartItem.Length > 10 ? SystemInformation.HorizontalScrollBarHeight : 0);
                    for (int i = 0; i < chkChartItem.Length; i++)
                    {
                        //check box
                        chkChartItem[i] = new CheckBox();
                        chkChartItem[i].Text = ParamTitle[i];
                        chkChartItem[i].Width = boxWidth;
                        chkChartItem[i].Height = 24;
                        chkChartItem[i].Top = (panelExactHeight - (24 * 3 + 10)) / 2;
                        chkChartItem[i].Left = padding + i * (boxWidth + padding);
                        chkChartItem[i].Font = new Font("微軟正黑體", Math.Min(StringWidth("內溫PV", chkChartItem[i].Font) / StringWidth(ParamTitle[i], chkChartItem[i].Font) * 10F, 10F));
                        chkChartItem[i].BackColor = seriesColor[i];
                        chkChartItem[i].ForeColor = seriesSelectionBoxTextColor[i];
                        if (showTooltip)
                            tooltip.SetToolTip(chkChartItem[i], tooltipString[i]);
                        chkChartItem[i].Tag = ParamColNum[i]; //corresponding column
                        /*
                        //Disable checking of check state, avoid dead loop
                        chkChartItem[i].CheckedChanged -= ChangeChartItems;
                        if (!hiddenList.Contains(i))
                            chkChartItem[i].Checked = true;
                        */
                        chkChartItem[i].Checked = true;
                        chkChartItem[i].CheckedChanged += ChangeChartItems;
                        pnlChartItems.Controls.Add(chkChartItem[i]);
                        //factor button
                        btnMultiple[i] = new Button();
                        btnMultiple[i].Text = "x10";
                        btnMultiple[i].Font = new Font("Calibri", 10);
                        btnMultiple[i].Width = (boxWidth - 5) / 2;
                        btnMultiple[i].Height = 24;
                        btnMultiple[i].Top= chkChartItem[i].Top + chkChartItem[i].Height + 5;
                        btnMultiple[i].Left = padding + i * (boxWidth + padding);
                        btnMultiple[i].TextAlign = ContentAlignment.MiddleCenter;
                        btnMultiple[i].Tag = i;
                        btnMultiple[i].Click += SetDataFactor;
                        pnlChartItems.Controls.Add(btnMultiple[i]);
                        btnReduce[i] = new Button();
                        btnReduce[i].Text = "÷10";
                        btnReduce[i].Font = new Font("Calibri", 10);
                        btnReduce[i].Width = (boxWidth - 5) / 2;
                        btnReduce[i].Height = 24;
                        btnReduce[i].Top = chkChartItem[i].Top + chkChartItem[i].Height + 5;
                        btnReduce[i].Left = padding + i * (boxWidth + padding) + btnMultiple[i].Width + 5;
                        btnReduce[i].TextAlign = ContentAlignment.MiddleCenter;
                        btnReduce[i].Tag = i;
                        btnReduce[i].Click += SetDataFactor;
                        pnlChartItems.Controls.Add(btnReduce[i]);
                        //display
                        lblDataDisplay[i] = new System.Windows.Forms.Label();
                        lblDataDisplay[i].BorderStyle = BorderStyle.Fixed3D;
                        lblDataDisplay[i].AutoSize = false;
                        lblDataDisplay[i].TextAlign = ContentAlignment.MiddleCenter;
                        lblDataDisplay[i].BackColor = Color.White;
                        lblDataDisplay[i].Width = boxWidth;
                        lblDataDisplay[i].Height = 24;
                        lblDataDisplay[i].Top = btnMultiple[i].Top + btnMultiple[i].Height + 5;
                        lblDataDisplay[i].Left = padding + i * (boxWidth + padding);
                        lblDataDisplay[i].Font = new Font("微軟正黑體", 10F);
                        pnlChartItems.Controls.Add(lblDataDisplay[i]);
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

        private bool DrawChart(ZedGraphControl targetChart, float zoomFactor = 1.0f, bool qrcode = false)
        {
            if (InvokeRequired)
                return (bool)Invoke(new DrawChartDelegate(DrawChart), targetChart, zoomFactor, qrcode);
            else
            {
                cursorIndex = 0;
                try
                {
                    //set graph panel
                    GraphPane panel = targetChart.GraphPane;
                    panel.Border.IsVisible = false;
                    panel.Chart.IsRectAuto = false;
                    panel.Chart.Rect = new RectangleF(156.96917f * zoomFactor, 125.655365f * zoomFactor, 680f * zoomFactor, 369.721436f * zoomFactor);
                    //Clear series
                    panel.CurveList.Clear();
                    panel.YAxisList.Clear();
                    panel.Y2AxisList.Clear();
                    panel.GraphObjList.Clear();
                    //Create title
                    panel.Title.Text = TitleText.Equals(string.Empty) ? " " : TitleText;
                    panel.Title.FontSpec = new FontSpec("微軟正黑體", Math.Min(targetChart.Width * 0.6F / StringWidth(TitleText, new Font("微軟正黑體", 18F)) * 18, 18F), Color.Black, false, false, false);
                    panel.Title.FontSpec.Border.IsVisible = false;
                    //Set x axis
                    panel.X2Axis.IsVisible = false;
                    panel.XAxis.Title.Text = "";
                    panel.XAxis.MajorGrid.IsVisible = chkXGrid.Checked;
                    panel.XAxis.MajorGrid.Color = Color.Gray;
                    panel.XAxis.MajorGrid.PenWidth = 1 * zoomFactor;
                    panel.XAxis.MajorGrid.DashOff = 0;
                    panel.XAxis.MajorTic.IsOutside = true;
                    panel.XAxis.MajorTic.IsOpposite = false;
                    panel.XAxis.MajorTic.PenWidth = 1 * zoomFactor;
                    panel.XAxis.MinorTic.IsOutside = false;
                    panel.XAxis.MinorTic.IsOpposite = false;
                    panel.XAxis.MinorTic.PenWidth = 1 * zoomFactor;
                    panel.XAxis.Scale.FontSpec = new FontSpec("Calibri", 10, Color.Black, true, false, false);
                    panel.XAxis.Scale.FontSpec.Border.IsVisible = false;
                    panel.XAxis.Scale.FontSpec.Angle = XAngle;
                    panel.XAxis.Type = XType == 0 ? AxisType.Text : AxisType.Date;
                    panel.XAxis.Scale.MaxAuto = XType == 0 ? true : false;
                    panel.XAxis.Scale.MinAuto = XType == 0 ? true : false;
                    if (XType == 0) //text
                    {
                        panel.XAxis.Scale.TextLabels = TrendSeries[0].Select(m => new XDate(m.X).ToString("MM/dd HH:mm:ss")).ToArray();
                        panel.XAxis.Scale.MajorStep = XInterval == 0 ? TrendSeries[0].Count / 5f : XInterval;
                    }
                    else //datetime
                    {
                        panel.XAxis.Scale.Format = "MM/dd HH:mm:ss";
                        panel.XAxis.Scale.MajorUnit = DateUnit.Hour;
                        panel.XAxis.Scale.MinorUnit = DateUnit.Hour;
                        panel.XAxis.Scale.Min = new XDate(StartTime);
                        panel.XAxis.Scale.Max = new XDate(EndTime);
                        panel.XAxis.Scale.MajorStep = XInterval == 0 ? Math.Ceiling((EndTime - StartTime).TotalMinutes / (5f * 15f)) * 15f / 60f : XInterval / 60f;
                    }
                    panel.XAxis.Scale.MinorStep = panel.XAxis.Scale.MajorStep / 5f;
                    //add series (Y axis order:420135)
                    int yAxisCount = 0;
                    for (int i = 0; (i + 1) < SeriesMap.Length; i++)
                    {
                        //get series in this axis
                        List<PointPairList> seriesList = new List<PointPairList>();
                        for (int j = 0; j < SeriesMap[i + 1].Length; j++)
                        {
                            //use Tag in chkChartItem to group series
                            if (!hiddenList.Contains(SeriesMap[i + 1][j]))
                                seriesList.Add(TrendSeries.Where((m, n) => (int)(chkChartItem[n].Tag) == SeriesMap[i + 1][j]).ElementAt(0));
                        }
                        if (seriesList.Count == 0)
                            continue;
                        //create axis
                        Axis axis;
                        if (yAxisCount % 2 == 0)
                            axis = new YAxis();
                        else
                            axis = new Y2Axis();
                        axis.Title.Text = "";
                        axis.IsVisible = true;
                        axis.AxisGap = 10;
                        axis.MajorGrid.IsVisible = chkYGrid.Checked;
                        axis.MajorGrid.Color = Color.Gray;
                        axis.MajorGrid.PenWidth = 1 * zoomFactor;
                        axis.MajorGrid.DashOff = 0;
                        axis.MajorTic.IsInside = false;
                        axis.MajorTic.IsOpposite = false;
                        axis.MajorTic.PenWidth = 1 * zoomFactor;
                        axis.Scale.FontSpec = new FontSpec("Calibri", 10, Color.Black, true, false, false);
                        axis.Scale.FontSpec.Border.IsVisible = false;
                        axis.Scale.FontSpec.Angle = yAxisCount % 2 == 0 ? 90 : -90;
                        axis.Scale.LabelGap = 0f;
                        axis.Scale.Max = YProp[i].Max;
                        axis.Scale.Min = YProp[i].Min;
                        axis.Scale.MajorStep = YProp[i].Interval;
                        axis.MinorTic.IsAllTics = false;
                        if (yAxisCount % 2 == 0)
                            panel.YAxisList.Add((YAxis)axis);
                        else
                            panel.Y2AxisList.Add((Y2Axis)axis);
                        //add unit label
                        int labelCount = 0;
                        TextObj unit = new TextObj(unitList[YProp[i].UnitIndex], yLabelXPos[yAxisCount], yLabelYPos[labelCount++], CoordType.ChartFraction);
                        unit.FontSpec = new FontSpec("Calibri", 9, Color.Black, true, false, false);
                        unit.FontSpec.Border.IsVisible = false;
                        panel.GraphObjList.Add(unit);
                        //add series label(colored box)
                        for (int j = 0; j < seriesList.Count; j++)
                        {
                            BoxObj box = new BoxObj();
                            box.Fill.Color = chkChartItem.Single(m => m.Text.Equals(seriesList[j][0].Tag.ToString())).BackColor;
                            box.Fill.Type = FillType.Solid;
                            if (seriesList.Count <= 4) //add column if more than 4 label
                            {
                                box.Location.Width = 0.04f;
                                box.Location.Height = 0.026f;
                                box.Border.IsAntiAlias = true;
                                box.Border.Width = 1 * zoomFactor;
                                box.Location.X = yLabelXPos[yAxisCount] - box.Location.Width / 2;
                                box.Location.Y = yLabelYPos[labelCount++];
                            }
                            else
                            {
                                int columnCount = (int)Math.Ceiling(seriesList.Count / 4F); //column count
                                box.Location.Width = 0.04f / columnCount - 0.002f;
                                box.Location.Height = 0.026f;
                                box.Location.X = yLabelXPos[yAxisCount] - 0.02f + j / 4 * (box.Location.Width + 0.002f * columnCount / (columnCount - 1));
                                box.Location.Y = yLabelYPos[j % 4 + 1];
                                labelCount = 5; //skip to the toppest position
                            }
                            box.Location.CoordinateFrame = CoordType.ChartFraction;
                            panel.GraphObjList.Add(box);
                        }
                        //add axis title
                        TextObj title = new TextObj(YProp[i].Title, yLabelXPos[yAxisCount], yLabelYPos[labelCount], CoordType.ChartFraction);
                        title.FontSpec = new FontSpec("微軟正黑體", 9, Color.Black, true, false, false);
                        title.FontSpec.Border.IsVisible = false;
                        panel.GraphObjList.Add(title);
                        //add curve
                        foreach (PointPairList series in seriesList)
                        {
                            LineItem item = new LineItem(series[0].Tag.ToString(), series, chkChartItem.Single(m => m.Text.Equals(series[0].Tag.ToString())).BackColor, SymbolType.None);
                            item.IsY2Axis = yAxisCount % 2 == 0 ? false : true;
                            item.YAxisIndex = (int)Math.Floor((double)yAxisCount / 2);
                            item.Line.Width = 2f * zoomFactor;
                            if (item.Points.Count > 100000)
                                item.Line.IsAntiAlias = false;
                            else
                                item.Line.IsAntiAlias = true;
                            item.IsSelectable = false;
                            panel.CurveList.Add(item);
                        }
                        yAxisCount++;
                    }
                    //add recorder name
                    TextObj recorderName = new TextObj(
                        RecorderName.Equals(string.Empty) ? recorderList[Type] : RecorderName,
                        0.9f,
                        0.95f,
                        CoordType.PaneFraction);
                    recorderName.FontSpec = new FontSpec("微軟正黑體", 8, Color.DimGray, true, true, false);
                    recorderName.FontSpec.Border.IsVisible = false;
                    panel.GraphObjList.Add(recorderName);
                    //Add QR code
                    if (qrcode)
                    {
                        BarcodeWriter writer = new BarcodeWriter();
                        writer.Format = BarcodeFormat.QR_CODE;
                        writer.Options = new QrCodeEncodingOptions()
                        {
                            ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H,
                            DisableECI = true,
                            Width = (int)(80 * zoomFactor),
                            Height = (int)(80 * zoomFactor),
                            CharacterSet = "UTF-8",
                        };
                        string statInfo = "";
                        //Default inner temperature channal: 0
                        for (int i = 0; i < SeriesMap[3].Length; i++)
                            statInfo += Calculate(i, 0, true) + Environment.NewLine;
                        ImageObj qrImage = new ImageObj();
                        qrImage.Image = writer.Write(statInfo);
                        qrImage.Location = new Location(0.02f, 0.862f, CoordType.PaneFraction);
                        qrImage.Location.Width = (double)writer.Options.Width / targetChart.Width;
                        qrImage.Location.Height = (double)writer.Options.Height / targetChart.Height;
                        panel.GraphObjList.Add(qrImage);
                    }
                    //set legend
                    panel.Legend.Position = LegendPos.Float;
                    panel.Legend.Alignment = ContentAlignment.MiddleCenter;
                    FontSpec font = new FontSpec("微軟正黑體", 10f, Color.Black, false, false, false);
                    font.Border.IsVisible = false;
                    panel.Legend.FontSpec = font;
                    panel.Legend.FontSpec.Border.IsVisible = false;
                    panel.Legend.Location = new Location(0, -0.18, 1, 0.18, CoordType.ChartFraction, AlignH.Left, AlignV.Top);
                    panel.Legend.Border.IsVisible = false;
                    //commit
                    targetChart.AxisChange();
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
            else if (TrendSeries == null)
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
            double totalTime = 0;
            List<double> blankTime = new List<double>();
            bool pauseFlag = false;
            double totalFlow = 0;
            double totalPressure = 0;
            List<double> totalPressureIncludingBlank = new List<double>();
            double totalInnerTemp = 0;
            List<double> totalInnerTempIncludngBlank = new List<double>();
            double maxP = 0;
            DateTime maxPTime = new DateTime();
            double maxInnerTemp = 0;
            DateTime maxInnerTempTime = new DateTime();
            double maxFlow = 0;
            int firstFlowPoint = int.MaxValue; //First point with flow large than 0.01
            int lastFlowPoint = int.MinValue; //Last point with flow large than 0.01
            //Scan for first/last flow point
            Parallel.For(1, TrendSeries[paramFlowIndex].Count, i =>
            {
                if (TrendSeries[paramFlowIndex][i].Y >= 0.01)
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
            double pointPerSecond = 0;
            for (int i = 0; i < TrendSeries[0].Count - 1; i++)
                pointPerSecond += 1 / (new XDate(TrendSeries[0][i + 1].X).DateTime - new XDate(TrendSeries[0][i].X).DateTime).TotalSeconds;
            pointPerSecond /= TrendSeries[0].Count;
            //Calcd.
            for (int i = firstFlowPoint; i <= lastFlowPoint; i++)
            {
                if (TrendSeries[paramFlowIndex][i].Y >= 0.01)
                {
                    pauseFlag = false;
                    double timePeriod = (new XDate(TrendSeries[0][i].X).DateTime - new XDate(TrendSeries[0][i - 1].X).DateTime).TotalSeconds;
                    totalTime += timePeriod;
                    totalFlow += TrendSeries[paramFlowIndex][i].Y * timePeriod;
                    totalPressure += TrendSeries[paramPressureIndex][i].Y * timePeriod;
                    totalInnerTemp += TrendSeries[paramInnerTempIndex][i].Y * timePeriod;
                    if (TrendSeries[paramPressureIndex][i].Y > maxP)
                    {
                        maxP = TrendSeries[paramPressureIndex][i].Y;
                        maxPTime = new XDate(TrendSeries[0][i].X).DateTime;
                    }
                    if (TrendSeries[paramInnerTempIndex][i].Y > maxInnerTemp)
                    {
                        maxInnerTemp = TrendSeries[paramInnerTempIndex][i].Y;
                        maxInnerTempTime = new XDate(TrendSeries[0][i].X).DateTime;
                    }
                    if (TrendSeries[paramFlowIndex][i].Y > maxFlow)
                        maxFlow = TrendSeries[paramFlowIndex][i].Y;
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
                    double timePeriod = (new XDate(TrendSeries[0][i].X).DateTime - new XDate(TrendSeries[0][i - 1].X).DateTime).TotalSeconds;
                    blankTime[blankTime.Count - 1] += timePeriod;
                    totalPressureIncludingBlank[blankTime.Count - 1] += TrendSeries[paramPressureIndex][i].Y * timePeriod;
                    totalInnerTempIncludngBlank[blankTime.Count - 1] += TrendSeries[paramInnerTempIndex][i].Y * timePeriod;
                    if (TrendSeries[paramPressureIndex][i].Y > maxP)
                    {
                        maxP = TrendSeries[paramPressureIndex][i].Y;
                        maxPTime = new XDate(TrendSeries[0][i].X).DateTime;
                    }
                    if (TrendSeries[paramInnerTempIndex][i].Y > maxInnerTemp)
                    {
                        maxInnerTemp = TrendSeries[paramInnerTempIndex][i].Y;
                        maxInnerTempTime = new XDate(TrendSeries[0][i].X).DateTime;
                    }
                }
            }
            double avgFlow = totalFlow / totalTime;
            double avgPressure = totalPressure / totalTime;
            double avgInnerTemp = totalInnerTemp / totalTime;
            double totalTimeWithBlank = totalTime;
            double totalPressureWithBlank = totalPressure;
            double totalInnerTempWithBlank = totalInnerTemp;
            for (int i = 0; i < blankTime.Count; i++)
            {
                if (blankTime[i] < 1 / pointPerSecond * 60 * 60 * 8) 
                {
                    totalTimeWithBlank += blankTime[i];
                    totalPressureWithBlank += totalPressureIncludingBlank[i];
                    totalInnerTempWithBlank += totalInnerTempIncludngBlank[i];
                }
            }
            double avgFlowWithBlank = totalFlow / totalTimeWithBlank;
            double avgPressureWithBlank = totalPressureWithBlank / totalTimeWithBlank;
            double avgInnerPVWithBlank = totalInnerTempWithBlank / totalTimeWithBlank;
            //Calcd. aging time
            bool agingDone = false;
            double agingTime = 0;
            DateTime agingStart = new XDate(TrendSeries[0][lastFlowPoint].X);
            DateTime agingEnd = new DateTime();
            double agingEndP = 0;
            double maxPDiff; //max drop for pressure when aging done
            if (YProp[2].UnitIndex == 1)
                maxPDiff = AgingPDiff * 0.07f;
            else if (YProp[2].UnitIndex == 2)
                maxPDiff = AgingPDiff;
            else
                return "壓力不知道是三小單位";
            for (int i = lastFlowPoint; i < TrendSeries[0].Count - (int)(pointPerSecond * 60 * AgingTimeHold); i++) //must before data end
            {
                agingDone = true;
                for (int j = i + (int)(pointPerSecond * 60 * AgingTimeHold) - 1; j > i - 1; j--)
                {
                    double tempFactor;
                    if (AgingTempFix)
                        tempFactor = (TrendSeries[paramPressureIndex][j].Y + 273.15f) / (TrendSeries[paramInnerTempIndex][i].Y + 273.15f);
                    else
                        tempFactor = 1;
                    if (Math.Abs(TrendSeries[paramPressureIndex][j].Y * tempFactor - TrendSeries[paramPressureIndex][i].Y) > maxPDiff)
                    {
                        agingDone = false;
                        break;
                    }
                }
                if (agingDone)
                {
                    agingEnd = new XDate(TrendSeries[0][i].X);
                    agingEndP = TrendSeries[paramPressureIndex][i].Y;
                    agingTime = (new XDate(TrendSeries[0][i].X).DateTime - agingStart).TotalMinutes;
                    break;
                }
            }
            //Return
            if (!simple)
            {
                return
                    "計算目標通道：" + ParamTitle[paramFlowIndex] + Environment.NewLine +
                    Environment.NewLine +
                    "*****僅計算流速大於0.01 ml/min(含)部分*****" + Environment.NewLine +
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
                    "*****熟成時間(" + AgingTimeHold + "分鐘壓力變化小於" + AgingPDiff + " psi(" + AgingPDiff * 0.07 + " bar)視為熟成完成)*****" + Environment.NewLine +
                    "熟成開始：" + agingStart.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
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
                    "Aging start: " + agingStart.ToString("MM/dd HH:mm:ss") + Environment.NewLine +
                    "Aging end: " + (agingDone ? agingEnd.ToString("MM/dd HH:mm:ss") + " at " + string.Format("{0:N2}", agingEndP) + " " + unitList[yProp[Type][2].UnitIndex] : "N/A") + Environment.NewLine +
                    "Aging time: " + (agingDone ? string.Format("{0:N2}", agingTime) + "min" : "Aging doesn't finish");
            }
        }

        private void RefineData(float factor = 1, int targetSeries = -1)
        {
            //set time and value limit
            if (!(StartTime == new XDate(trendSeriesRaw[0][0].X) &&
                EndTime == new XDate(trendSeriesRaw[0][trendSeriesRaw[0].Count - 1].X) &&
                maxLimit == float.MaxValue &&
                minLimit == float.MinValue &&
                factor == 1 &&
                targetSeries == -1))
            {
                trendSeriesRefined = new PointPairList[trendSeriesRaw.Length];
                Parallel.For(0, trendSeriesRaw.Length, i =>
                {
                    trendSeriesRefined[i] = new PointPairList();
                    for (int j = 0; j < trendSeriesRaw[i].Count; j++)
                    {
                        //handle with factor
                        if (i == targetSeries)
                            trendSeriesRaw[i][j].Y *= factor;
                        //filtration
                        if (new XDate(trendSeriesRaw[i][j].X).CompareTo(new XDate(StartTime)) >= 0 && new XDate(trendSeriesRaw[i][j].X).CompareTo(new XDate(EndTime)) <= 0)
                            trendSeriesRefined[i].Add(
                                trendSeriesRaw[i][j].X,
                                (trendSeriesRaw[i][j].Y >= maxLimit || trendSeriesRaw[i][j].Y <= minLimit) ? 0 : trendSeriesRaw[i][j].Y,
                                trendSeriesRaw[i][j].Tag.ToString()
                            );
                    }
                });
                dataRefined = true;
            }
            else
                dataRefined = false; //no change to raw data
            dataCount = TrendSeries[0].Count;
            //reduce data
            mergeCount = TrendSeries[0].Count / maxDataCount;
            if (mergeCount > 1)
            {
                PointPairList[] temp = new PointPairList[TrendSeries.Length];
                Parallel.For(0, TrendSeries.Length, i =>
                {
                    temp[i] = new PointPairList();
                    double sum = 0;
                    for (int j = 0; j < TrendSeries[i].Count; j++)
                    {
                        sum += TrendSeries[i][j].Y;
                        if (j % mergeCount == 0 || j == TrendSeries[i].Count - 1)
                        {
                            temp[i].Add(new XDate(TrendSeries[i][j].X), sum / mergeCount, TrendSeries[i][j].Tag.ToString());
                            sum = 0;
                        }
                    }
                });
                trendSeriesRefined = temp;
                dataRefined = true;
            }
        }
        #endregion
    }
}

