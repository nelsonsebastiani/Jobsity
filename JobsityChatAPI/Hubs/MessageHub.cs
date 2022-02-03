using JobsityChatAPI.Entities;
using JobsityChatAPI.Models;
using JobsityChatAPI.Services.Stocks.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace JobsityChatAPI.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IStocksBotService _stocksBotService;
        private readonly UserManager<LocalUser> _userManager;
        public MessageHub(IStocksBotService stocksBotService, UserManager<LocalUser> userManager)
        {
            _stocksBotService = stocksBotService;
            _userManager = userManager;
        }
        public async Task NewMessage(Message msg)
        {
            var user = await _userManager.FindByIdAsync(msg.UserId);
            msg.User = user;
            await Clients.All.SendAsync("MessageReceived", msg);
            BotResponse botResponse = _stocksBotService.BotCommand(msg.Text);
            if (botResponse.Detected)
                if (botResponse.IsSuccessful)
                    await Clients.All.SendAsync("MessageReceived", BotMessage($"{botResponse.Symbol} quote is {botResponse.Close} per share."));
                else
                    await Clients.All.SendAsync("MessageReceived", BotMessage($"There was a problem with the request. {botResponse.ErrorMessage}"));
        }

        internal Message BotMessage(string msg)
        {
            return new Message
            {
                Text = msg,
                Date = DateTime.Now
            };
        }
    }
}
