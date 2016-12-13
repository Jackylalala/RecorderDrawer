
namespace RecorderDrawer
{
    public class AxesProp
    {
        public string Title { get; set; }
        public int Unit { get; set; } //From frmRecorderDrawer.UNIT_TABLE
        public float Max { get; set; }
        public float Min { get; set; }
        public float Interval { get; set; }

        public AxesProp(string title, int unit, float min, float max, float interval)
        {
            Title = title;
            Unit = unit;
            Min = min;
            Max = max;
            Interval = interval;
        }
    }
}
