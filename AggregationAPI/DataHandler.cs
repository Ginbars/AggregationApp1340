using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

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
            List<ElectricityData> dataList = new();

            foreach (var path in dataPaths)
            {
                Console.WriteLine("Downloading data");
                Stream? data = await DataFetcher.DownloadData(path);
                if (data == null)
                {
                    Console.WriteLine("Saatana");
                    throw new ArgumentException("Invalid url path: ", nameof(path));
                }

                List<ElectricityDataEntry> records = ConvertStream2List(data);
                List<ElectricityData> processed = ProcessData(records);
                dataList.AddRange(processed);
                Console.WriteLine("Downloaded data");
            }

            return dataList;
        }

        public static List<AggregatedData> AggregateData(List<ElectricityData> data)
        {
            List<AggregatedData> aData = new();
            List<List<ElectricityData>> groupedData = data.GroupBy(entry => entry.Region).Select(group => group.ToList()).ToList();

            foreach (var group in groupedData)
            {
                string region = group[0].Region;
                aData.Add(group.Aggregate(new AggregatedData() { Region = region }, (agg, data) => agg.AddAggregation(data)));
            }

            return aData;
        }

        private static List<ElectricityData> ProcessData(List<ElectricityDataEntry> data)
        {
            List<ElectricityData> validated = FilterInvalidData(data);
            FilterByObjName(validated, "Butas");
            
            return validated;
        }

        private static List<ElectricityDataEntry> ConvertStream2List(Stream data)
        {
            using var reader = new StreamReader(data);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<ElectricityDataEntry>().ToList();
        }

        private static List<ElectricityData> FilterInvalidData(List<ElectricityDataEntry> list)
        {
            List<ElectricityData> filtered = new();

            foreach (var entry in list)
            {
                ElectricityData? data = entry.TryConvertToValid();
                if (data is ElectricityData valid)
                {
                    filtered.Add(valid);
                }
            }

            return filtered;
        }

        private static void FilterByObjName(List<ElectricityData> list, string name)
        {
            list.RemoveAll(entry => entry.Obj_Name != name);
        }
    }
}
