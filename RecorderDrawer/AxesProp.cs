
using System.IO;

namespace RecorderDrawer
{
    public class AxesProp
    {
        public string Title { get; set; }
        public int UnitIndex { get; set; } //From frmRecorderDrawer.UNIT_TABLE
        public float Max { get; set; }
        public float Min { get; set; }
        public float Interval { get; set; }

        public AxesProp(string title, int unitIndex, float min, float max, float interval)
        {
            Title = title;
            UnitIndex = unitIndex;
            Min = min;
            Max = max;
            Interval = interval;
        }

        public override string ToString()
        {
            return Title + "," + UnitIndex + "," + Min + "," + Max + "," + Interval;
        }

        public void Serialize(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(Title);
            writer.Write(UnitIndex);
            writer.Write(Min);
            writer.Write(Max);
            writer.Write(Interval);
            writer.Flush();
        }

        public static AxesProp Deserialize(Stream input)
        {
            BinaryReader reader = new BinaryReader(input);
            return new AxesProp(reader.ReadString(), reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
