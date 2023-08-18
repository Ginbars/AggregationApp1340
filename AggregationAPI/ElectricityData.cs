using CsvHelper.Configuration.Attributes;

namespace AggregationApp
{
    public class ElectricityData
    {
        public string Region { get; set; }
        public string Obj_Name { get; set; }
        public string Obj_Type { get; set; }
        public string Obj_Number { get; set; }
        public float PPlus { get; set; }
        public DateTime Time { get; set; }
        public float PMinus { get; set; }

        public ElectricityData(string region, string obj_Name, string obj_Type, string obj_Number, float pPlus, DateTime time, float pMinus)
        {
            Region = region;
            Obj_Name = obj_Name;
            Obj_Type = obj_Type;
            Obj_Number = obj_Number;
            PPlus = pPlus;
            Time = time;
            PMinus = pMinus;
        }

        public override bool Equals(object? obj)
        {
            return obj is ElectricityData ed
                && (ed.Region, ed.Obj_Name, ed.Obj_Type, ed.Obj_Number, ed.PPlus, ed.Time, ed.PMinus)
                .Equals((Region, Obj_Name, Obj_Type, Obj_Number, PPlus, Time, PMinus));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Region, Obj_Name, Obj_Type, Obj_Number, PPlus, Time, PMinus);
        }
    }

    public class ElectricityDataEntry
    {
        [Index(0)]
        public string? Region { get; set; }
        [Index(1)]
        public string? Obj_Name { get; set; }
        [Index(2)]
        public string? Obj_Type { get; set; }
        [Index(3)]
        public string? Obj_Number { get; set; }
        [Index(4)]
        public float? PPlus { get; set; }
        [Index(5)]
        public DateTime? Time { get; set; }
        [Index(6)]
        public float? PMinus { get; set; }

        public ElectricityData? TryConvertToValid()
        {
            if (Region is string r && r.Length > 0
                && Obj_Name is string n && n.Length > 0
                && Obj_Type is string t && t.Length > 0
                && Obj_Number is string nr && nr.Length > 0
                && PPlus is float pp
                && Time is DateTime tm
                && PMinus is float pm
                )
            {
                return new ElectricityData(r, n, t, nr, pp, tm, pm);
            }
            else
            {
                return null;
            }
        }
    }
}
