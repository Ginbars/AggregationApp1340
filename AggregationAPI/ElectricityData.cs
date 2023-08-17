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

        public bool IsValid()
        {
            return
                Region != null
                && Obj_Name != null
                && Obj_Type != null
                && Obj_Number != null
                && PPlus != null
                && Time != null
                && PMinus != null;
        }

        public ElectricityData? TryConvertToValid()
        {
            if (Region is string r
                && Obj_Name is string n
                && Obj_Type is string t
                && Obj_Number is string nr
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
