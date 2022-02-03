using StocksBotAPI.Models;

namespace StocksBotAPI.Services.Contracts
{
    public interface IStocksBotService
    {
        Stock GetStock(string code);
    }
}