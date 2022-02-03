using JobsityChatAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobsityChatAPI.Services.Messages.Contracts
{
    public interface IMessageService
    {
        public Task<List<Message>> GetMessages();
        public Task<List<Message>> AddMessage(Message msg);
        public void AddMessageFromHub(Message msg);
    }
}
