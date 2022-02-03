using AutoMapper;
using JobsityChatAPI.Data;
using JobsityChatAPI.DTO.Users;
using JobsityChatAPI.Models;
using JobsityChatAPI.Services.Users.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChatAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<LocalUser> _userManager;
        public UserService(BaseContext context, IMapper mapper, UserManager<LocalUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<GetUserDto>> AddUser(AddUserDto newUser)
        {
            LocalUser user = _mapper.Map<LocalUser>(newUser);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
        }

        public async Task<List<GetUserDto>> GetAllUsers()
        {
            List<LocalUser> users = await _context.LocalUsers.ToListAsync();
            return _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
        }

        public async Task<GetUserDto> GetUserById(string id)
        {
            LocalUser user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<GetUserDto>(user);
        }
    }
}
