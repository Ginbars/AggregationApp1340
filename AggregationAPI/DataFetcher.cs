
namespace AggregationApp
{
    public class DataFetcher
    {
        static readonly ILogger _logger = ApiLogger.CreateLogger<DataFetcher>();

        static readonly HttpClient client = new();

        public static async Task<Stream> DownloadData(string url)
        {
            _logger.LogInformation("Starting data download from {url}.", url);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Stream data = await response.Content.ReadAsStreamAsync();
            _logger.LogInformation("Finished data downloading {url}.", url);
            return data;
        }
    }
}
