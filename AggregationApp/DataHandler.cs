using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace AggregationApp
{
    public class DataHandler
    {
        private static string[] dataPaths = new string[4]
        {
            "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv",
            "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv",
            "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv",
            "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv"
        };

        public static async Task<List<ElectricityData>> CollectData()
        {
            Console.WriteLine("Downloading data");
            List<ElectricityData> dataList = new();

            foreach (var path in dataPaths)
            {
                Stream? data = await DataFetcher.DownloadData(path);
                if (data == null)
                {
                    Console.WriteLine("Saatana");
                    throw new ArgumentException("Invalid url path: ", nameof(path));
                }

                List<ElectricityData> records = ConvertStream2List(data);
                ProcessData(records);
                dataList.AddRange(records);
                Console.WriteLine("Downloaded data");
            }

            return dataList;
        }

        public static void ProcessData(List<ElectricityData> data)
        {
            FilterInvalidData(data);
            FilterByObjName(data, "Butas");
        }

        private static List<ElectricityData> ConvertStream2List(Stream data)
        {
            using var reader = new StreamReader(data);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<ElectricityData>().ToList();
        }

        private static void FilterInvalidData(List<ElectricityData> list)
        {
            list.RemoveAll(entry => !entry.IsValid());
        }

        private static void FilterByObjName(List<ElectricityData> list, string name)
        {
            list.RemoveAll(entry => entry.Obj_Name != name);
        }
    }

    public class ElectricityData
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
    }
}
