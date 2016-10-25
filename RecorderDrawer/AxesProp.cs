using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecorderDrawer
{
    public class AxesProp
    {
        private string title;
        private int unit; //From frmRecorderDrawer.UNIT_TABLE
        private float max = 0;
        private float min = 0;
        private float interval = 0;

        public string Title { get { return title; } set { title = value; } }
        public int Unit { get { return unit; } set { unit = value; } }
        public float Max { get { return max; } set { max = value; } }
        public float Min { get { return min; } set { min = value; } }
        public float Interval { get { return interval; } set { interval = value; } }

        public AxesProp(string title, int unit, float min, float max, float interval)
        {
            this.title = title;
            this.unit = unit;
            this.min = min;
            this.max = max;
            this.interval = interval;
        }
    }
}
