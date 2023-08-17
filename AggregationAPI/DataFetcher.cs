using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregationApp
{
    public class DataFetcher
    {
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
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return Stream.Null;
            }
        }

    }
}
