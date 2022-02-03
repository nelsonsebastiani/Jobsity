using JobsityChatAPI.Data.Contracts;
using JobsityChatAPI.Identity.Models;
using JobsityChatAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JobsityChatAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BaseContext _context;
        private readonly UserManager<LocalUser> _userManager;

        public AuthRepository(BaseContext context, UserManager<LocalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<int>> Register(LocalUser user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var results = _userManager
                    .CreateAsync(user, password).Result;

                if (results.Succeeded &&
                    !string.IsNullOrWhiteSpace(Roles.ROLE_CHAT_API))
                {
                    await _userManager.AddToRoleAsync(user, Roles.ROLE_CHAT_API);
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = $"Failed to add role {Roles.ROLE_CHAT_API}!";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = $"Failed to add role {Roles.ROLE_CHAT_API}!";

                return response;
            }
        }
    }
}
