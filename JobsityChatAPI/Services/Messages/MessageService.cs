using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using JobsityChatAPI.Data;
using JobsityChatAPI.Models;
using JobsityChatAPI.Services.Messages.Contracts;

namespace JobsityChatAPI.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<LocalUser> _userManager;
        public MessageService(IMapper mapper, BaseContext context, UserManager<LocalUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<Message>> AddMessage(Message msg)
        {
            await _context.Message.AddAsync(msg);
            await _context.SaveChangesAsync();

            return _context.Message.Select(m => _mapper.Map<Message>(m)).ToList();
        }

        public async Task<List<Message>> GetMessages()
        {
            List<Message> messages = await _context.Message.Include(p => p.User).OrderBy(m => m.Date).Take(50).ToListAsync();
            return messages;
        }

        public async void AddMessageFromHub(Message msg)
        {
            LocalUser user = await _userManager.FindByIdAsync(msg.User.Id);
            msg.User = user;
            await _context.Message.AddAsync(msg);
            await _context.SaveChangesAsync();
        }
    }
}
