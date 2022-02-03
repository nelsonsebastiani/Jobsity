using JobsityChatAPI.Data.Contracts;
using JobsityChatAPI.DTO.Users;
using JobsityChatAPI.Identity;
using JobsityChatAPI.Identity.Models;
using JobsityChatAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JobsityChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public TokenResult Login(
            [FromBody] User user,
            [FromServices] UserManager<LocalUser> userManager,
            [FromServices] SignInManager<LocalUser> signInManager,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfiguration tokenConfiguration)
        {
            bool validCredentials = false;
            LocalUser localUser;
            if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
            {
                var userIdentity = userManager
                    .FindByNameAsync(user.UserName).Result;
                localUser = userIdentity;

                if (userIdentity != null)
                {
                    var loginResult = signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if (loginResult.Succeeded)
                    {
                        validCredentials = userManager.IsInRoleAsync(
                            userIdentity, Roles.ROLE_CHAT_API).Result;
                    }
                }
            }

            if (validCredentials)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserName, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    }
                );

                DateTime creationDate = DateTime.Now;
                DateTime expDate = creationDate + TimeSpan.FromSeconds(tokenConfiguration.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfiguration.Issuer,
                    Audience = tokenConfiguration.Audience,
                    SigningCredentials = signingConfigurations.Credentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expDate
                });
                var token = handler.WriteToken(securityToken);

                return new TokenResult
                {
                    Authenticated = true,
                    Created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = expDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = token,
                    Message = "OK"
                };
            }
            else
            {
                return new TokenResult
                {
                    Authenticated = false,
                    Message = "Autentication failed!"
                };
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request,
            [FromServices] SignInManager<LocalUser> signInManager)
        {
            ServiceResponse<int> response = await _authRepository
                .Register(new LocalUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Name = request.Name
                }, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            await signInManager.SignInAsync(new LocalUser
            {
                UserName = request.UserName,
                Email = request.Email
            }, isPersistent: false);
            return Ok(response);
        }
    }
}