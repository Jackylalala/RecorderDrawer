using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace RecorderDrawer
{
    public class RecordData : IComparable<RecordData>
    {
        public DateTime Time { get; private set; }
        public List<float> Parameter { get; private set; }

        public RecordData(DateTime time, List<float> parameter)
        {
            Time = time;
            Parameter = parameter;
        }

        public RecordData(DateTime time, List<string> parameter, float maxLimit = float.MaxValue, float minLimit = float.MinValue)
        {
            Time = time;
            Parameter = new List<float>();
            for (int i = 0; i < parameter.Count; i++)
            {
                float num = 0;
                float.TryParse(parameter[i], out num);
                if (frmMain.Type == 6)
                {
                    if (i == 6 || i == 5) //first flow and pressure column
                        num /= 10;
                    else if (i == 8) //rotation speed column
                        num *= 10;
                }
                if (frmMain.Type == 4 && i == 2) //pressure column
                    num /= 10;
                if (frmMain.Type == 2 && i == 7) //second flow column
                    num /= 10;
                if (num > maxLimit || num < minLimit)
                    num = 0;
                num = (float)Math.Round(num, 2, MidpointRounding.AwayFromZero);
                Parameter.Add(num);
            }
        }

        public int CompareTo(RecordData obj)
        {
            if (obj == null)
                return -1;
            else
                return Time.CompareTo(obj.Time);
        }

        public void Serialize(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(Time.Ticks);
            writer.Write(Parameter.Count);
            foreach (float item in Parameter)
                writer.Write(item);
            writer.Flush();
        }

        public static RecordData Deserialize(Stream input)
        {
            BinaryReader reader = new BinaryReader(input);
            DateTime date = new DateTime(reader.ReadInt64());
            int count = reader.ReadInt32();
            List<float> parameter = new List<float>();
            for (int i = 0; i < count; i++)
                parameter.Add(reader.ReadSingle());
            return new RecordData(date, parameter);
        }
    }
}
