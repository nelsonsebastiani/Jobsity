using JobsityChatAPI.Entities;

namespace JobsityChatAPI.Services.Stocks.Contracts
{
    public interface IStocksBotService
    {
        public BotResponse BotCommand(string code);
    }
}