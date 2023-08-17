using AggregationAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregationApp
{
    public class DataFetcher
    {
        static readonly ILogger _logger = ApiLogger.CreateLogger<DataFetcher>();

        static readonly HttpClient client = new();

        public static async Task<Stream> DownloadData(string url)
        {
            try
            {
                Stream data = await client.GetStreamAsync(url);
                return data;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Exception caught while trying to download data from {url}", url);
                return Stream.Null;
            }
        }

    }
}
