using JobsityChatAPI.Models;
using System.Threading.Tasks;

namespace JobsityChatAPI.Data.Contracts
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(LocalUser user, string password);
    }
}
