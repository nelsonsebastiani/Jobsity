using JobsityChatAPI.Models;
using JobsityChatApp.DTO.Users;
using JobsityChatApp.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobsityChatAPI.Controllers
{
    [ApiController]
    [Authorize("Bearer")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser(
            [FromServices] UserManager<LocalUser> userManager)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return Ok(user);
        }
    }
}
