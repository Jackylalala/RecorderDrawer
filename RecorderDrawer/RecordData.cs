using System;
using System.Collections;
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
        public List<float> Parameter { get; set; }

        public RecordData(DateTime time)
        {
            Time = time;
            Parameter = new List<float>();
        }

        public RecordData(DateTime time, int paramCount)
        {
            Time = time;
            Parameter = new List<float> { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };
        }

        public RecordData(DateTime time, List<float> parameter)
        {
            Time = time;
            Parameter = parameter;
        }

        public RecordData(DateTime time, params float[] parameter)
        {
            Time = time;
            Parameter = parameter.ToList();
        }

        public RecordData(DateTime time, List<string> parameter, float maxLimit = float.MaxValue, float minLimit = float.MinValue)
        {
            Time = time;
            Parameter = new List<float>();
            for (int i = 0; i < parameter.Count; i++)
            {
                float num = 0;
                float.TryParse(parameter[i], out num);
                if (FrmMain.Type == 6)
                {
                    if (i == 6 || i == 5) //first flow and pressure column
                        num /= 10;
                    else if (i == 8) //rotation speed column
                        num *= 10;
                }
                if (FrmMain.Type == 4 && i == 2) //pressure column
                    num /= 10;
                if (FrmMain.Type == 2 && i == 7) //second flow column
                    num /= 10;
                if (FrmMain.Type == 13 && i == 4)
                    num /= 10;
                if (FrmMain.Type == 13 && i == 5)
                    num /= 10;
                if (FrmMain.Type == 13 && i == 7)
                    num *= 10;
                if (FrmMain.Type == 13 && i == 8)
                    num /= 10;
                num = (float)Math.Round(num, 2, MidpointRounding.AwayFromZero);
                Parameter.Add(num);
            }
        }

        public void AddParameter(float num)
        {
            Parameter.Add(num);
        }

        public RecordData Refine(float minLimit, float maxLimit)
        {
            List<float> param = new List<float>();
            for (int i = 0; i < Parameter.Count; i++)
            {
                if (Parameter[i] < minLimit || Parameter[i] > maxLimit)
                    param.Add(0);
                else
                    param.Add(Parameter[i]);
            }
            return new RecordData(Time, param);
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
