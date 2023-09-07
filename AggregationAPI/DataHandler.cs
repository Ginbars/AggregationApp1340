using System.Globalization;
using CsvHelper;

namespace AggregationApp
{
    public class DataHandler
    {
        static readonly ILogger _logger = ApiLogger.CreateLogger<DataHandler>();

        public static async Task<Stream> DownloadData(string url, HttpClient client)
        {
            _logger.LogInformation("Starting data download from {url}.", url);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Stream data = await response.Content.ReadAsStreamAsync();
            _logger.LogInformation("Finished data downloading {url}.", url);
            return data;
        }

        public static List<ElectricityData> ProcessData(Stream data)
        {
            var entries = ConvertStream2List(data);
            var filtered = FilterInvalidData(entries);

            return filtered;
        }

        public static void FilterByObjName(List<ElectricityData> list, string name)
        {
            list.RemoveAll(entry => entry.Obj_Name != name);
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

        private static List<ElectricityDataEntry> ConvertStream2List(Stream data)
        {
            using var reader = new StreamReader(data);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            return csv.GetRecords<ElectricityDataEntry>().ToList();
        }

        private static List<ElectricityData> FilterInvalidData(List<ElectricityDataEntry> list)
        {
            List<ElectricityData> filtered = new();
            int invalid = 0;

            foreach (var entry in list)
            {
                ElectricityData? data = entry.TryConvertToValid();
                if (data is ElectricityData valid)
                {
                    filtered.Add(valid);
                }
                else
                {
                    invalid += 1;
                }
            }

            _logger.LogInformation("Finished filtering data with {count} invalid entries found.", invalid);

            return filtered;
        }
    }
}
