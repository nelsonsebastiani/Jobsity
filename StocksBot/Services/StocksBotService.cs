using StocksBotAPI.Models;
using StocksBotAPI.Services.Contracts;
using System;
using System.Net;
using System.Net.Http;

namespace StocksBotAPI.Services
{
    public class StocksBotService : IStocksBotService
    {
        private HttpClient _client { get; }

        public StocksBotService(HttpClient client)
        {
            _client = client;
        }
        public Stock GetStock(string stockCode)
        {
            using (HttpResponseMessage response = _client.GetAsync($"https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv").Result)
            using (HttpContent stockContent = response.Content)
            {
                var stockResponse = stockContent.ReadAsStringAsync().Result;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ArgumentException(stockResponse);
                var data = stockResponse.Substring(stockResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                var processedArray = data.Split(',');
                return new Stock()
                {
                    Symbol = processedArray[0],
                    Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                    Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]) : default,
                    Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3]) : default,
                    High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4]) : default,
                    Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5]) : default,
                    Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6]) : default,
                    Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7]) : default,
                };
            }
        }
    }
}
