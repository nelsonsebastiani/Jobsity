using JobsityChatAPI.DTO.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobsityChatAPI.Services.Users.Contracts
{
    public interface IUserService
    {
        public Task<List<GetUserDto>> GetAllUsers();
        public Task<GetUserDto> GetUserById(string id);
        public Task<List<GetUserDto>> AddUser(AddUserDto user);
    }
}
