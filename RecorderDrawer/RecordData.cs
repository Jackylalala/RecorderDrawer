using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace RecorderDrawer
{
    public class RecordData : IComparable<RecordData>
    {
        private DateTime date;
        private List<float> parameter; 

        public DateTime Date { get { return date; } }
        public List<float> Parameter { get { return parameter; } }

        public RecordData(DateTime date, List<float> parameter)
        {
            this.date = date;
            this.parameter = parameter.ToList<float>();
        }

        public int CompareTo(RecordData obj)
        {
            if (obj == null)
                return -1;
            else
                return date.CompareTo(obj.Date);
        }
    }
}
