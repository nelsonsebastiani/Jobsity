using JobsityChatAPI.Entities;
using JobsityChatAPI.Models;
using JobsityChatAPI.Services.Stocks.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace JobsityChatAPI.Services.Stocks
{
    public class StocksBotService : IStocksBotService
    {
        private HttpClient _client;
        public StocksBotService(HttpClient client)
        {
            _client = client;
        }
        public BotResponse BotCommand(string code)
        {
            try
            {
                if (code.ToLower().Contains("/stock="))
                {
                    string stockCode = code.Replace("/stock=", "");
                    using (HttpResponseMessage response = _client.GetAsync($"https://localhost:44326/StocksBot/GetStock?stockCode={stockCode}").Result)
                    using (HttpContent stockContent = response.Content)
                    {
                        var stockResponse = stockContent.ReadAsStringAsync().Result;
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                            return new BotResponse { Detected = true, IsSuccessful = false, ErrorMessage = response.StatusCode.ToString() };
                        var stock = JsonConvert.DeserializeObject<Stock>(stockResponse);
                        return new BotResponse { Detected = true, IsSuccessful = true, Symbol = stock.Symbol, Close = stock.Close.ToString() };
                    }

                }
                return new BotResponse { Detected = false };
            }
            catch (Exception e)
            {
                return new BotResponse { Detected = true, IsSuccessful = false, ErrorMessage = e.Message };
            }
        }
    }
}
